using Es_Log.Models;
using Es_Log.Models.Logs;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Es_Log.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController(IElasticSearchService<RequestLogModel> _elasticRequestSearchService) : ControllerBase
    {
        [HttpPost]
        [Route("Get-Request-Logs")]
        public async Task<IActionResult> GetRequestLogs([FromBody] GetRequestLogViewModel getRequestLogViewModel)
        {
            return Ok(_elasticRequestSearchService.SearchRequestLog(getRequestLogViewModel.userId, getRequestLogViewModel.BeginDate
                , getRequestLogViewModel.EndDate, getRequestLogViewModel.Controller, getRequestLogViewModel.Operation, getRequestLogViewModel.Action, getRequestLogViewModel.Page, getRequestLogViewModel.RowCount, getRequestLogViewModel.IndexName));
        }

        [HttpPost]
        [Route("Get-Error-Logs")]
        public async Task<IActionResult> GetErrorLogs([FromBody] GetErrorLogViewModel getErrorLogViewModel)
        {
            return Ok(_elasticRequestSearchService.SearchErrorLog(getErrorLogViewModel.userId, getErrorLogViewModel.ErrorCode, getErrorLogViewModel.BeginDate, getErrorLogViewModel.EndDate, getErrorLogViewModel.Controller, getErrorLogViewModel.Operation, getErrorLogViewModel.Action, getErrorLogViewModel.Method, getErrorLogViewModel.Services, 0, 10, getErrorLogViewModel.IndexName));
        }

        //[HttpPost]
        //[Route("Get-Set-Logs")]
        //public async Task<IActionResult> GetSetLogs([FromBody] GetSetLogViewModel getSetLogViewModel)
        //{
        //    return Ok(_elasticRequestSearchService.SearchSetLog(getSetLogViewModel.userId, getSetLogViewModel.BeginDate, getSetLogViewModel.EndDate, getSetLogViewModel.ModelType, getSetLogViewModel.Operation, 0, 10, getSetLogViewModel.IndexName));
        //}


        [HttpPost]
        [Route("Get-Set-Logs")]
        public async Task<IActionResult> GetSetLogs([FromQuery] string controllerName)
        {
            return Ok(_elasticRequestSearchService.SearchSetLog(null,null,null,controllerName,null,"Set",0,10,"set_log"));
        }
    }
}
