using Es_Log.Attributes;
using Es_Log.Models.Logs;
using Es_Log.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace Es_Log.Services
{
    public class EndpointService : IEndpointService
    {
        public List<ErrorLogModel> GetErrorAttributes(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<ErrorLogModel> errorLogModels = new();

            if (controllers is not null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(ErrorLogAttribute)));
                    if (actions is not null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes is not null)
                            {
                                var authorizeDefAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(ErrorLogAttribute)) as ErrorLogAttribute;

                                var httpMethodAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;

                                errorLogModels.Add(new() { HttpType = httpMethodAttribute.HttpMethods.First(), Controller = controller.Name, Action = action.Name, PostDate = DateTime.Now });
                            }
                        }
                }
            return errorLogModels;
        }

        public List<SpecifyRequestLogModel> GetSpecifyRequestAttributes(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<SpecifyRequestLogModel> specifyRequestLogModels = new();

            if (controllers is not null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(SpecifyRequestLogAttribute)));
                    if (actions is not null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes is not null)
                            {
                                var authorizeDefAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(SpecifyRequestLogAttribute)) as SpecifyRequestLogAttribute;

                                var httpMethodAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;

                                specifyRequestLogModels.Add(new() { ActionType = authorizeDefAttribute.ActionType, HttpType = httpMethodAttribute.HttpMethods.First(), Controller = authorizeDefAttribute.Controller, Action = action.Name, PostDate = DateTime.Now });
                            }
                        }
                }
            return specifyRequestLogModels;
        }

        public List<SetLogModel> GetSetAttributes(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<SetLogModel> setLogModels = new();

            if (controllers is not null)
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(SetLogAttribute)));
                    if (actions is not null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes is not null)
                            {
                                var authorizeDefAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(SetLogAttribute)) as SetLogAttribute;

                                var httpMethodAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;

                                setLogModels.Add(new() { HttpType = httpMethodAttribute.HttpMethods.First(), Controller = controller.Name, Action = action.Name, PostDate = DateTime.Now });
                            }
                        }
                }
            return setLogModels;
        }
    }
}
