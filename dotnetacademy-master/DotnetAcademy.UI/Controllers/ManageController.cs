using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.UI.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace DotnetAcademy.UI.Controllers {
    [Authorize]
    public class ManageController : Controller {
        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index() {
            ViewBag.Username = await ApiService<string>.GetApi($"/api/account/GetCurrentUserName", User.Identity.Name);

            return View();
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            string resultJson =
                await ApiService<ChangePasswordViewModel>.PostApi($"/api/Manage/ChangePassword", model,
                    User.Identity.Name);
            bool succeeded = JsonConvert.DeserializeObject<bool>(resultJson);

            if (succeeded) {
                TempData["StatusMessage"] = "Uw wachtwoord is gewijzigd";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Er is iets fout gegaan, mogelijk heeft u een verkeerd wachtwoord ingegeven");
            return View(model);
        }


        //
        // GET: /Manage/ChangeProfile
        public async Task<ActionResult> ChangeProfile() {
            UserCustomerViewModel model =
                await ApiService<UserCustomerViewModel>.GetApi("/api/Customer/GetCurrentUserCustomer",
                    User.Identity.Name);

            return View(model);
        }

        //
        // POST: /Manage/ChangeProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile(
            [Bind(Include = "Id, FirstName, LastName, CompanyName, City, Street, Postal, VatNumber, ApplicationUserId")]
            UserCustomerViewModel profile) {
            if (ModelState.IsValid) {
                string succeededJson =
                    await ApiService<UserCustomerViewModel>.PostApi($"/api/Account/Update", profile,
                        User.Identity.Name);
                bool succeeded = JsonConvert.DeserializeObject<bool>(succeededJson);

                if (succeeded) {
                    TempData["StatusMessage"] = "Uw profiel is gewijzigd";
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Er is iets fout gegaan, gelieve de administrator te contacteren");
            return View(profile);
        }

        //
        // GET: /Manage/MyPurchasedProducts
        public async Task<ActionResult> MyPurchasedProducts() {
            IEnumerable<ProductPerInvoiceViewModel> myProducts =
                await ApiService<List<ProductPerInvoiceViewModel>>.GetApi($"/api/Invoice/GetCurrentUserProductsPerInvoice", User.Identity.Name);

            return View(myProducts);
        }

        //
        // GET: /Manage/MyInvoices
        public async Task<ActionResult> MyInvoices() {
            IEnumerable<InvoiceViewModel> invoices = await ApiService<IEnumerable<InvoiceViewModel>>.GetApi($"/api/invoice/GetCurrentUserInvoices", User.Identity.Name);
            string username = await ApiService<string>.GetApi($"/api/Account/GetCurrentUserName", User.Identity.Name);

            ViewBag.Username = username;
            return View(invoices);
        }
    }
}