using Es_Log.Extensions;
using Es_Log.Models.Entities;
using Es_Log.Models.Logs;
using Es_Log.Options;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Es_Log.Filters
{
    public class ErrorLogFilter(IEndpointService _endpointService, IElasticSearchService<ErrorLogModel> _elasticErrorSearchService, IConfiguration _configuration, UserManager<User> _userManager) : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                User? user = new();
                string? email = context.HttpContext.User.Identity?.Name;
                var elasticConfig = _configuration.GetOptions<ElasticSearchOptions>("ElasticSearchOptions");
                string? action = (string)context.RouteData.Values["action"];
                string? controller = (string)context.RouteData.Values["controller"];
                string? methodType = context.HttpContext.Request.Method;
                var exception = context.Exception;

                if (email is not null)
                    user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

                ErrorLogModel errorLogModel = new();
                errorLogModel.PostDate = DateTime.Now;
                errorLogModel.HttpType = methodType;
                errorLogModel.Action = action;
                errorLogModel.Controller = controller.ReplaceControllerTag();
                errorLogModel.ErrorCode = 500;
                errorLogModel.Message = exception.Message;
                errorLogModel.Service = "ErrorLogFilter";
                errorLogModel.UserId = user.Id.ToString() ?? "NONE";

                _elasticErrorSearchService.CheckExistsAndInsertLog(errorLogModel, elasticConfig.ElasticErrorIndex);

                context.Result = new ObjectResult(new { message = "A error occured : " + exception.Message, HttpCode = 500 })
                {
                    StatusCode = 500,
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
