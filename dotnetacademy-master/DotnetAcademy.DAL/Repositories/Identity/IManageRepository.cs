using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;

namespace DotnetAcademy.DAL.Repositories.Identity {
    public interface IManageRepository {
        bool ChangePassword(ChangePasswordViewModel model, string id);
    }
}
