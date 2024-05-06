using Es_Log.Extensions;
using Es_Log.Models.Entities;
using Es_Log.Models.Logs;
using Es_Log.Options;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Es_Log.Api.Filters
{
    public class SetLogFilter(IElasticSearchService<SetLogModel> _elasticSetSearchService, UserManager<User> _userManager, IConfiguration _configuration, IEndpointService _endpointService) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            User? user = new();
            string? email = context.HttpContext.User.Identity?.Name;
            var elasticConfig = _configuration.GetOptions<ElasticSearchOptions>("ElasticSearchOptions");

            string? action = (string)context.RouteData.Values["action"];
            string? controller = (string)context.RouteData.Values["controller"];
            string? methodType = context.HttpContext.Request.Method;

            List<SetLogModel>? setLogModels = _endpointService.GetSetAttributes(typeof(Program));
            var setLogModel = setLogModels.Where(s => s.Action == action && s.Controller.ReplaceControllerTag() == controller.ReplaceControllerTag()).FirstOrDefault();
            if (setLogModel is not null)
            {
                var request = context.HttpContext.Request;
                using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    if (email is not null)
                        user = await _userManager.FindByEmailAsync(email);

                    var bodyContent = await reader.ReadToEndAsync();
                    setLogModel.UserId = user.Id.ToString() ?? "NONE";
                    setLogModel.OldData = JsonConvert.SerializeObject(bodyContent);
                    setLogModel.Operation = $"{user.Id.ToString()} user updated in {action} endpoint datas";

                    _elasticSetSearchService.CheckExistsAndInsertLog(setLogModel, elasticConfig.ElasticSetIndex);
                }
            }
        }
    }
}
