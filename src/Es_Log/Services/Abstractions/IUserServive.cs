using Es_Log.Models;

namespace Es_Log.Services.Abstractions
{
    public interface IUserServive
    {
        Task<UserResponseModel> UserRegisterAsync(RegisterViewModel registerViewModel);

        Task<UserResponseModel> UserLoginAsync(LoginViewModel loginViewModel);

        Task<bool> AddRoleToUserAsync(Guid userId, string[] roles);

        Task<List<string>> GetRolesToUserAsync(Guid userId);

        Task<List<string>> GetRolesToUserAsync(string email);
    }
}
