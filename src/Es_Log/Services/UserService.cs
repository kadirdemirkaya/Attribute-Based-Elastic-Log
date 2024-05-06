using Es_Log.Constants;
using Es_Log.Models;
using Es_Log.Models.Entities;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Es_Log.Services
{
    public class UserService(UserManager<User> userManager, ITokenService tokenService) : IUserServive
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserResponseModel> UserLoginAsync(LoginViewModel loginViewModel)
        {
            if (loginViewModel is null)
                throw new NullReferenceException($"{nameof(LoginViewModel)} is null !");

            User? user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user is null)
                return new()
                {
                    IsSuccess = false,
                    Errors = new string[] { "User Not found" }
                };

            bool result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (!result)
                return new()
                {
                    IsSuccess = false,
                    Errors = new string[] { "Invalid user inputs" }
                };

            Models.Token token = _tokenService.GenerateToken(user.Email, user.Id.ToString());

            return new()
            {
                IsSuccess = true,
                Token = token.AccessToken
            };
        }

        public async Task<UserResponseModel> UserRegisterAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel is null)
                throw new NullReferenceException($"{nameof(RegisterViewModel)} is null !");

            User user = User.Create(registerViewModel.Name, registerViewModel.Email, registerViewModel.AboutMe, registerViewModel.PhoneNumber);

            IdentityResult result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                IdentityResult? roleResult = await _userManager.AddToRoleAsync(user, Constant.Role.User);
                return roleResult.Succeeded is true ? new() { IsSuccess = true } : new() { IsSuccess = false };
            }
            else
            {
                string[] errorMessages = result.Errors.Select(e => e.Description).ToArray();
                return new()
                {
                    IsSuccess = false,
                    Errors = errorMessages
                };
            }
        }

        public async Task<bool> AddRoleToUserAsync(Guid userId, string[] roles)
        {
            User? user = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            IdentityResult identityResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (identityResult.Succeeded)
            {
                IdentityResult idenRes = await _userManager.AddToRolesAsync(user, roles);
                return idenRes.Succeeded;
            }
            return false;
        }

        public async Task<List<string>> GetRolesToUserAsync(Guid userId)
        {
            User? user = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count() == 0)
                return null;
            return userRoles.ToList();
        }

        public async Task<List<string>> GetRolesToUserAsync(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count() == 0)
                return null;
            return userRoles.ToList();
        }
    }
}
