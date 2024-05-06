namespace Es_Log.Services.Abstractions
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string name);

        Task<bool> DeleteRoleAsync(Guid id);

        Task<bool> UpdateRoleAsync(Guid id, string name);

        IDictionary<Guid, string> GetAllRoles();

        Task<(Guid id, string name)> GetRoleByIdAsync(Guid id);
    }
}
