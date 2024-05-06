using Es_Log.Models.Logs;

namespace Es_Log.Services.Abstractions
{
    public interface IEndpointService
    {
        List<SpecifyRequestLogModel> GetSpecifyRequestAttributes(Type type);
        List<ErrorLogModel> GetErrorAttributes(Type type);
        List<SetLogModel> GetSetAttributes(Type type);
    }
}
