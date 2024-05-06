using Es_Log.Api.Filters;
using Es_Log.Attributes;
using Es_Log.Constants;
using Es_Log.Models;
using Es_Log.Models.Entities;
using Es_Log.Models.Enums;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Es_Log.Api.Controllers
{
    public class AuthController(IUserServive _userServive, UserManager<User> _userManager) : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("user-register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            UserResponseModel userResponseModel = await _userServive.UserRegisterAsync(registerViewModel);

            return Ok(userResponseModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("user-login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            UserResponseModel? userResponseModel = await _userServive.UserLoginAsync(loginViewModel);

            return Ok(userResponseModel);
        }


        [HttpGet]
        [Route("get-user")]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            return Ok(await _userManager.FindByEmailAsync(email));
        }

        [HttpPost]
        [Route("update-user-password")]
        [SetLog(Controller = "Auth")]
        [TypeFilter(typeof(SetLogFilter))]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordViewModel updateUserPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(updateUserPasswordViewModel.Email);
            var identityResult = await _userManager.ChangePasswordAsync(user, updateUserPasswordViewModel.OldPassword, updateUserPasswordViewModel.NewPassword);
            return Ok(identityResult.Succeeded);
        }

        [HttpDelete]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser([FromHeader] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var identityResult = await _userManager.DeleteAsync(user);
            return Ok(identityResult.Succeeded);
        }

        [HttpGet]
        [Route("get-all-user")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _userManager.Users.ToListAsync());
        }

        [HttpPut]
        [SpecifyRequestLog(ActionType = ActionType.Relation, Controller = Constant.Controller.Auth)]
        [TypeFilter(typeof(SetLogFilter))]
        [Route("user-assign-role")]
        public async Task<IActionResult> UserAssignRole([FromBody] RoleAddToUserViewModel roleAddToUserViewModel)
        {
            return Ok(await _userServive.AddRoleToUserAsync(roleAddToUserViewModel.UserId, roleAddToUserViewModel.Roles));
        }

        [HttpGet]
        [Route("user-get-roles")]
        public async Task<IActionResult> UserGetRoles([FromHeader] Guid userId)
        {
            return Ok(await _userServive.GetRolesToUserAsync(userId));
        }
    }
}
