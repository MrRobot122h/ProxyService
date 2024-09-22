using Newtonsoft.Json;
using OpenQA.Selenium;
using ProxyService.Services;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ProxyService.Middleware
{
    public class CheckMiddleware
    {

        private readonly RequestDelegate _next;
        MainService mainService;

        public CheckMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                string idHeader = string.Empty;
                int id = 0;
                mainService = new MainService();

                if (context.Request.Query.TryGetValue("reqresId", out var reqresIdQuery)) { idHeader = "reqresId"; id = Convert.ToInt32(reqresIdQuery); }
                else if (context.Request.Query.TryGetValue("NewId", out var newIdQuery)) { idHeader = "NewId"; id = Convert.ToInt32(newIdQuery); }
                else if (context.Request.Query.TryGetValue("id", out var idQuery)) { idHeader = "id"; id = Convert.ToInt32(idQuery); };

                switch (idHeader)
                {
                    case "reqresId":
                        var @bool = mainService.Exists(id);
                        if (@bool) Log.Warning($"User with id {id} already exist.\n id will be changed for recording");

                        break;

                    case "NewId":
                        @bool = mainService.Exists(id);
                        if (@bool) throw new Exception($"User with id {id} already exist.");

                        break;

                    case "id":
                        @bool = mainService.Exists(id);
                        if (!@bool) throw new Exception($"User with id {id} does not exist.");

                        break;

                    default:
                        Console.WriteLine("ID не передано в запиті");
                        break;
                }


                await _next(context);

            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new { error = validationException.Message });
                    Log.Warning("Validation error: {Message}", validationException.Message);
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(new { error = "Resource not found" });
                    Log.Information("Not found: {Message}", notFoundException.Message);
                    break;
                default:
                    result = JsonConvert.SerializeObject(new { error = exception.Message });
                    Log.Error(exception, "Unhandled exception occurred: {Message}", exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
    public static class CheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckMiddleware>();
        }
    }
}
