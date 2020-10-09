using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using DotnetAcademy.BLL.Identity;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebHttpGet = System.Web.Http.HttpGetAttribute;
using WebHttpPost = System.Web.Http.HttpPostAttribute;
using WebAuthorize = System.Web.Http.AuthorizeAttribute;

namespace DotnetAcademy.API.Controllers {
    [WebAuthorize]
    public class ManageController: ApiController {

        private IManageBusinessLogic _manageBl;
        private IAccountBusinessLogic _accountBl;

        public ManageController(ManageBusinessLogic manageBl, AccountBusinessLogic accountBl) {
            _manageBl = manageBl;
            _accountBl = accountBl;
        }

        //
        // POST: /Manage/ChangePassword
        [WebHttpPost]
        [ValidateAntiForgeryToken]
        public bool ChangePassword([FromBody] ChangePasswordViewModel model) {
            IPrincipal principal = AuthenticationManager.User;

            return this._manageBl.ChangePassword(model, principal);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        #endregion
    }
}