using Es_Log.Extensions;
using Es_Log.Models.Entities;
using Es_Log.Models.Logs;
using Es_Log.Options;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace Es_Logç.Api.Filters
{
    public class RequestLogFilter(IElasticSearchService<RequestLogModel> _elasticSearchService, IElasticSearchService<SpecifyRequestLogModel> _elasticSpecifySearchService, UserManager<User> _userManager, IConfiguration _configuration, IEndpointService _endpointService) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            User? user = new();
            string? email = context.HttpContext.User.Identity?.Name;
            var elasticConfig = _configuration.GetOptions<ElasticSearchOptions>("ElasticSearchOptions");

            string? action = (string)context.RouteData.Values["action"];
            string? controller = (string)context.RouteData.Values["controller"];
            string? methodType = context.HttpContext.Request.Method;

            if (email is not null)
                user = await _userManager.FindByEmailAsync(email);

            List<SpecifyRequestLogModel>? specifyRequestLogModels = _endpointService.GetSpecifyRequestAttributes(typeof(Program));
            var specifyRequestLogModel = specifyRequestLogModels.Where(s => s.Action == action && s.Controller.ReplaceControllerTag() == controller).FirstOrDefault();
            if (specifyRequestLogModel is not null)
            {
                var request = context.HttpContext.Request;
                using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    var bodyContent = await reader.ReadToEndAsync();
                    specifyRequestLogModel.UserId = user.Id.ToString() ?? "NONE";
                    specifyRequestLogModel.OldData = JsonConvert.SerializeObject(bodyContent);

                    _elasticSpecifySearchService.CheckExistsAndInsertLog(specifyRequestLogModel, elasticConfig.ElasitcSpecifyRequestIndex);
                }
                await next();
            }

            RequestLogModel logModel = new();
            if (specifyRequestLogModel is null && email is not null)
            {
                logModel.Action = action;
                logModel.Controller = controller.ReplaceControllerTag();
                logModel.PostDate = DateTime.Now;
                logModel.UserId = user.Id.ToString() ?? "NONE";
                logModel.HttpType = methodType;
                _elasticSearchService.CheckExistsAndInsertLog(logModel, elasticConfig.ElasticRequestIndex);

                await next();
            }

            if (specifyRequestLogModel is null && email is null)
            {
                logModel.Action = action;
                logModel.Controller = controller.ReplaceControllerTag();
                logModel.PostDate = DateTime.Now;
                logModel.UserId = user.Id.ToString() ?? "NONE";
                logModel.HttpType = methodType;

                _elasticSearchService.CheckExistsAndInsertLog(logModel, elasticConfig.ElasticRequestIndex);
            }

            await next();
        }
    }
}
