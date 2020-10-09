using DotnetAcademy.BLL.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using WebHttpGet = System.Web.Http.HttpGetAttribute;
using WebHttpPost = System.Web.Http.HttpPostAttribute;
using WebAuthorize = System.Web.Http.AuthorizeAttribute;

namespace DotnetAcademy.API.Controllers {
    public class AccountController : ApiController {
        private IAccountBusinessLogic _accountBl;

        public AccountController(AccountBusinessLogic accountBl) {
            _accountBl = accountBl;
        }

        //
        // POST: api/Account/GetCurrentUserName
        [WebHttpGet]
        [ValidateAntiForgeryToken]
        [WebAuthorize]
        public string GetCurrentUserName() {
            return HttpContext.Current.User.Identity.Name;
        }

        //
        // POST: api/Account/GetUserId
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public string GetUserId([FromBody] string email) {
            return _accountBl.FindByEmail(email).Id;
        }

        //
        // POST: api/Account/Register
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public bool Register([FromBody] RegisterViewModel model) {
            return _accountBl.Register(model);
        }

        //
        // POST: api/Account/Update/5
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        [WebAuthorize(Roles = "administrator")]
        public bool Update([FromBody] UserCustomerViewModel model) {
            return _accountBl.Update(model);
        }

        //
        // POST: api/Account/Delete/5
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        [WebAuthorize(Roles = "administrator")]
        public void Delete([FromBody] UserCustomerViewModel model) {
            _accountBl.Delete(model);
        }

        // GET: api/Account/GetRoles
        [WebHttpGet]
        [ValidateAntiForgeryToken]
        [WebAuthorize]
        public string Roles() {
            return _accountBl.GetRoles(HttpContext.Current.User.Identity.GetUserId());
        }

        // GET: api/Account/GenerateConfirmationEmailAsync/5
        [WebHttpGet]
        [ValidateAntiForgeryToken]
        public async Task<string> GenerateConfirmationEmailTokenAsync(string id) {
            return await _accountBl.GenerateEmailConfirmationTokenAsync(id);
        }

        // POST: api/Account/SendEmailAsync/5
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public async Task SendEmailAsync([FromUri] string id, [FromBody] EmailConfirmationViewModel model) {
            await _accountBl.SendEmailAsync(id, model.Subject, model.Body);
        }

        // POST: api/Account/ConfirmEmailAsync/5
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> ConfirmEmailAsync([FromUri] string id, [FromBody] string code) {
            return await _accountBl.ConfirmEmailAsync(id, code);
        }


        //
        // GET: api/Account/EmailConfirmed
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> EmailConfirmed([FromBody] string username) {
            return await _accountBl.EmailConfirmed(username);
        }

        //
        // POST: api/Account/FindByEmailAsync
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ApplicationUserViewModel> FindByEmailAsync([FromBody] string email) {
            return await _accountBl.FindByEmailAsync(email);
        }

        // GET: api/Account/GeneratePasswordResetTokenAsync/5
        [WebHttpGet]
        [ValidateAntiForgeryToken]
        public async Task<string> GeneratePasswordResetTokenAsync(string id) {
            return await _accountBl.GeneratePasswordResetTokenAsync(id);
        }

        // POST: api/Account/ResetPasswordAsync/5
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> ResetPasswordAsync([FromUri] string id, [FromBody] ResetPasswordViewModel model) {
            return await _accountBl.ResetPasswordAsync(id, model);
        }

        // POST: api/Account/UserExists
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public bool UserExists([FromBody] string username) {
            return _accountBl.UserExists(username);
        }

        // POST: api/Account/EmailExists
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        [WebAuthorize(Roles = "administrator")]
        public bool EmailExists([FromBody] string email) {
            return _accountBl.EmailExists(email);
        }

        // POST: api/Account/IsDeleted
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public bool IsDeleted([FromBody] string username) {
            return _accountBl.IsDeleted(username);
        }

        #region Helpers

        private IAuthenticationManager Authentication {
            get { return Request.GetOwinContext().Authentication; }
        }

        private static class RandomOAuthStateGenerator {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits) {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0) {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}