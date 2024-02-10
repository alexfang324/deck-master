using PayPalDemo.Data;
using PayPalDemo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PayPalDemo.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _context;

        public RoleRepo(ApplicationDbContext context)
        {
            this._context = context;
            CreateInitialRole();
        }

        public IEnumerable<RoleVM> GetAllRoles()
        {
            var roles =
                _context.Roles.Select(r => new RoleVM
                {
                    Id = r.Id,
                    RoleName = r.Name
                });

            return roles;
        }

        public RoleVM GetRole(string roleName)
        {


            var role = _context.Roles.Where(r => r.Name == roleName)
                                .FirstOrDefault();

            if (role != null)
            {
                return new RoleVM() { Id = role.Id, RoleName = role.Name };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _context.Roles.Add(new IdentityRole
                {
                    Id = roleName.Substring(0, 2),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _context.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public SelectList GetRoleSelectList()
        {
            var roles = GetAllRoles().Select(r => new
            SelectListItem
            {
                Value = r.RoleName,
                Text = r.RoleName
            });

            var roleSelectList = new SelectList(roles,
                                               "Value",
                                               "Text");
            return roleSelectList;
        }

        public void CreateInitialRole()
        {
            const string ADMIN = "Admin";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN);
            }
        }

        public string Delete(string roleName)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                return "Role not found";
            }
            if (_context.UserRoles.Any(ur => ur.RoleId == role.Name))
            {
                return "Role is assigned to a user, cannot delete";
            }
            _context.Roles.Remove(role);
            _context.SaveChanges();
            return "Role deleted successfully";
        }

        public bool isRoleAssigned(string roleName)
        {
            return _context.UserRoles.Any(ur => ur.RoleId == roleName.ToLower());
        }
    }


}
