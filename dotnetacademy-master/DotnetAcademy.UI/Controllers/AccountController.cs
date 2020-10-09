using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotnetAcademy.Common.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DotnetAcademy.UI.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Newtonsoft.Json;
using PagedList;

namespace DotnetAcademy.UI.Controllers {
    public class AccountController : Controller {

        //
        // GET: /Account/Index
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page) {
            IEnumerable<UserCustomerViewModel> users = await this.SetAccountSorting(sortOrder, searchString, currentFilter, page);

            ViewBag.CurrentUserName = await ApiService<string>.GetApi("/api/Account/GetCurrentUserName", User.Identity.Name);
            return View(users);
        }

        //
        // GET: /Account/Create
        [Authorize(Roles = "administrator")]
        public ActionResult Create() {
            return View();
        }

        //
        // POST: /Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Create(RegisterViewModel model) {
            if (ModelState.IsValid) {
                // If username already exists, don't create the user
                if (await this.UserExists(model.Username)) {
                    TempData["UsernameExists"] = "Deze gebruikersnaam is al in gebruik";
                    return View(model);
                }

                // If e-mail already exists, don't create user (administrator is allowed to see existing e-mails)
                if (await this.EmailExists(model.Email)) {
                    TempData["EmailExists"] = "Dit e-mail adres is al in gebruik";
                    return View(model);
                }

                model.EmailConfirmed = true;
                string result = await ApiService<RegisterViewModel>.PostApi("/api/Account/Register", model);
                bool succeeded = JsonConvert.DeserializeObject<bool>(result);

                if (succeeded) {
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Account/Edit/5
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserCustomerViewModel model = await ApiService<UserCustomerViewModel>.GetApi($"/api/customer/get/{id}", User.Identity.Name);

            if (model == null) {
                return HttpNotFound();
            }

            string currentUserName = await ApiService<string>.GetApi("/api/Account/GetCurrentUserName", User.Identity.Name);

            if (model.Deleted || model.Username == currentUserName) {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id, FirstName, LastName, CompanyName, City, Street, Postal, VatNumber, ApplicationUserId")]
            UserCustomerViewModel updatedUserCustomer) {
            string currentUserName = await ApiService<string>.GetApi("/api/Account/GetCurrentUserName", User.Identity.Name);

            if (ModelState.IsValid && currentUserName != updatedUserCustomer.Username) {
                await ApiService<UserCustomerViewModel>.PostApi($"/api/account/update", updatedUserCustomer, User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(updatedUserCustomer);
        }

        // GET: Account/Delete/5
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserCustomerViewModel model = await ApiService<UserCustomerViewModel>.GetApi($"/api/customer/get/{id}", User.Identity.Name);

            if (model == null) {
                return HttpNotFound();
            }

            string currentUserName = await ApiService<string>.GetApi("/api/Account/GetCurrentUserName", User.Identity.Name);

            if (model.Deleted || model.Username == currentUserName) {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: Account/Delete/5
        [Authorize(Roles = "administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(UserCustomerViewModel model) {
            string currentUserName = await ApiService<string>.GetApi("/api/Account/GetCurrentUserName", User.Identity.Name);

            // The model that gets passed to the delete function only contains Id
            UserCustomerViewModel completeModel = await ApiService<UserCustomerViewModel>.GetApi($"/api/customer/get/{model.Id}", User.Identity.Name);

            if (currentUserName != completeModel.Username && !completeModel.Deleted) {
                await ApiService<UserCustomerViewModel>.PostApi($"/api/account/delete", model, User.Identity.Name);
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            string emailConfirmedJson = await ApiService<string>.PostApi($"/api/Account/EmailConfirmed", model.Username);
            string accountDeletedJson = await ApiService<string>.PostApi($"/api/Account/IsDeleted", model.Username);

            if (emailConfirmedJson != null && accountDeletedJson != null) {
                bool emailConfirmed = JsonConvert.DeserializeObject<bool>(emailConfirmedJson);
                bool accountDeleted = JsonConvert.DeserializeObject<bool>(accountDeletedJson);

                if (emailConfirmed && !accountDeleted) {
                    AuthTokenViewModel result =
                        await ApiService<AuthTokenViewModel>.AuthenticateAsync(model.Username, model.Password,
                            "/api/Token");

                    if (result != null) {
                        string roles = await ApiService<string>.GetApi("/api/Account/Roles", result.AccessToken);

                        //Keep the user authenticated in the mvc webapp, even when window closes
                        //By using the AccessToken, we can use User.Identity.Name in the MVC controllers to make API calls.
                        CreateTicket(result.AccessToken, roles, model.RememberMe);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Ongeldige login");
            return View(model);
        }

        //
        // GET: /Account/Register
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                // If username already exists, don't do the registration
                if (await this.UserExists(model.Username)) {
                    TempData["UsernameExists"] = "Deze gebruikersnaam is al in gebruik";
                    return View(model);
                }

                model.EmailConfirmed = false;
                string result = await ApiService<RegisterViewModel>.PostApi("/api/Account/Register", model);
                bool succeeded = JsonConvert.DeserializeObject<bool>(result);

                if (succeeded) {
                    string json = await ApiService<string>.PostApi("/api/Account/GetUserId", model.Email);
                    string id = JsonConvert.DeserializeObject<string>(json);

                    await ApiService<EmailConfirmationViewModel>.PostApi($"/api/Account/SendEmailAsync/{id}",
                        await this.GenerateConfirmationEmail(model.Email, id));

                    TempData["RegistrationMessage"] =
                        "Er is een e-mail naar u verstuurd met een link om de registratie te vervolledigen";
                    return RedirectToAction("Register");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code) {
            if (userId == null || code == null) {
                return View("Error");
            }

            string result = await ApiService<string>.PostApi($"/api/Account/ConfirmEmailAsync/{userId}", code);
            bool succeeded = JsonConvert.DeserializeObject<bool>(result);

            return View(succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                string userJson = await ApiService<string>.PostApi($"/api/Account/FindByEmailAsync", model.Email);
                ApplicationUserViewModel user = new ApplicationUserViewModel();

                if (userJson != null) {
                    user = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userJson);
                    if (!user.EmailConfirmed) {
                        // Don't reveal that the user does not exist or is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code =
                    await ApiService<string>.GetApi($"/api/Account/GeneratePasswordResetTokenAsync/{user.Id}");
                string callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code},
                    Request.Url.Scheme);

                EmailConfirmationViewModel emailConfirmationViewModel = new EmailConfirmationViewModel() {
                    Subject = "dotNET Academy wachtwoord reset",
                    Body =
                        $"Gelieve uw dotNET Academy wachtwoord te resetten door op deze link te klikken: {callbackUrl}"
                };

                await ApiService<EmailConfirmationViewModel>.PostApi($"/api/Account/SendEmailAsync/{user.Id}",
                    emailConfirmationViewModel);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code) {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            string userJson = await ApiService<string>.PostApi($"/api/Account/FindByEmailAsync", model.Email);

            if (userJson == null) {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            ApplicationUserViewModel user = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userJson);

            string resultJson =
                await ApiService<ResetPasswordViewModel>.PostApi($"/api/Account/ResetPasswordAsync/{user.Id}", model);
            bool succeeded = JsonConvert.DeserializeObject<bool>(resultJson);

            if (succeeded) {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            TempData["ErrorMsg"] = "Er is iets fout gegaan met uw verzoek. Heeft u het juiste e-mail adres ingegeven?";
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() {
            return View();
        }

        //
        // GET: /Account/LogOff
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            FormsAuthentication.SignOut();

            //Clear the auth cookie
            if (Response.Cookies["AuthCookie"] != null) {
                var c = new HttpCookie("AuthCookie") {Expires = DateTime.Now.AddDays(-1)};
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Index", "Home");
        }


        #region Helpers

        private async Task<IEnumerable<UserCustomerViewModel>> SetAccountSorting(string sortOrder,
            string searchString, string currentFilter, int? page) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.UsernameSortParm = String.IsNullOrEmpty(sortOrder) ? "Username_desc" : "";
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            IEnumerable<UserCustomerViewModel> users =
                await ApiService<IEnumerable<UserCustomerViewModel>>.GetApi("/api/customer/GetAllUserCustomers", User.Identity.Name);

            if (users != null) {
                if (!String.IsNullOrEmpty(searchString)) {
                    users = users.Where(u =>
                            u.Username.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.FirstName?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.LastName?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.Email?.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                switch (sortOrder) {
                    case "Username_desc":
                        users = users.OrderByDescending(u => u.Username).ToList();
                        break;
                    case "FirstName":
                        users = users.OrderBy(u => u.FirstName).ToList();
                        break;
                    case "FirstName_desc":
                        users = users.OrderByDescending(u => u.FirstName).ToList();
                        break;
                    case "LastName":
                        users = users.OrderBy(u => u.LastName).ToList();
                        break;
                    case "LastName_desc":
                        users = users.OrderByDescending(u => u.LastName).ToList();
                        break;
                    case "Email":
                        users = users.OrderBy(u => u.Email).ToList();
                        break;
                    case "Email_desc":
                        users = users.OrderByDescending(u => u.Email).ToList();
                        break;
                    default:
                        users = users.OrderBy(i => i.Username).ToList();
                        break;
                }
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return users.ToPagedList(pageNumber, pageSize);
        }

        private void CreateTicket(string username, string roles, bool rememberme) {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: username,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(1),
                isPersistent: rememberme,
                // Don't allow roles to contain ',' character
                userData: String.Join(",", roles));

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            HttpContext.Response.Cookies.Add(cookie);
        }

        private async Task<bool> UserExists(string username) {
            string userExistsJson = await ApiService<string>.PostApi("/api/Account/UserExists", username);
            bool userExists = JsonConvert.DeserializeObject<bool>(userExistsJson);

            return userExists;
        }

        private async Task<bool> EmailExists(string email) {
            string emailExistsJson =
                await ApiService<string>.PostApi("/api/Account/EmailExists", email, User.Identity.Name);
            bool emailExists = JsonConvert.DeserializeObject<bool>(emailExistsJson);

            return emailExists;
        }

        private async Task<EmailConfirmationViewModel> GenerateConfirmationEmail(string email, string id) {
            string code = await ApiService<string>.GetApi($"/api/Account/GenerateConfirmationEmailTokenAsync/{id}");
            string callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = id, code},
                protocol: Request.Url.Scheme);

            EmailConfirmationViewModel emailConfirmationViewModel = new EmailConfirmationViewModel() {
                Subject = "dotNET Academy account bevestiging",
                Body =
                    $"Gelieve uw dotNET Academy account te bevestigen door op deze link te klikken: {callbackUrl}"
            };

            return emailConfirmationViewModel;
        }

        #endregion
    }
}