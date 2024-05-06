using Es_Log.Models.Entities;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Es_Log.Services
{
    public class RoleService(RoleManager<Role> _roleManager) : IRoleService
    {
        public async Task<bool> CreateRoleAsync(string name)
        {
            IdentityResult identityResult = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid(), Name = name });
            return identityResult.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            Role? role = await _roleManager.FindByIdAsync(id.ToString());
            IdentityResult identityResult = await _roleManager.DeleteAsync(role);
            return identityResult.Succeeded;
        }

        public IDictionary<Guid, string> GetAllRoles()
        {
            return _roleManager.Roles.ToDictionary(role => role.Id, role => role.Name);
        }

        public async Task<(Guid id, string name)> GetRoleByIdAsync(Guid id)
        {
            Role? role = await _roleManager.FindByIdAsync(id.ToString());
            return (role.Id, role.Name);
        }

        public async Task<bool> UpdateRoleAsync(Guid id, string name)
        {
            Role? role = await _roleManager.FindByIdAsync(id.ToString());
            role.Name = name;
            IdentityResult identityResult = await _roleManager.UpdateAsync(role);
            return identityResult.Succeeded;
        }
    }
}
