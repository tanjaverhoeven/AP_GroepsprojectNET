using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;

namespace DotnetAcademy.BLL.Interfaces {
    public interface IManageBusinessLogic {
        bool ChangePassword(ChangePasswordViewModel model, IPrincipal loggedInUser);
    }
}
