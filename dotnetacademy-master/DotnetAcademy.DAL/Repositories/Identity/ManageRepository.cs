using System.Security.Principal;
using System.Threading.Tasks;
using DotnetAcademy.Common.DTO;
using DotnetAcademy.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DotnetAcademy.DAL.Repositories.Identity {
    public class ManageRepository : IManageRepository {
        private static MainDbContext _context;
        private static UserManager<ApplicationUser> _userManager;

        public ManageRepository(MainDbContext context) {
            _context = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public bool ChangePassword(ChangePasswordViewModel model, string id) {
            var result = _userManager.ChangePassword(id, model.OldPassword, model.NewPassword);

            return result.Succeeded;
        }
    }
}
