using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using DotnetAcademy.BLL.Interfaces;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL;
using DotnetAcademy.DAL.Models;
using DotnetAcademy.DAL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DotnetAcademy.BLL.Identity {
    public class ManageBusinessLogic : IManageBusinessLogic {
        private IUnitOfWork _uow;

        public ManageBusinessLogic(UnitOfWork uow) {
            _uow = uow;
        }

        public bool ChangePassword(ChangePasswordViewModel model, IPrincipal loggedInUser) {
            bool succeeded = _uow.ManageRepository.ChangePassword(model, loggedInUser.Identity.GetUserId());

            return succeeded;
        }

    }
}