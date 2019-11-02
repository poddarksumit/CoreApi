using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using ZipTest.Models;

namespace ZipTest.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    context.Response.ContentType = "application/json";

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = context.Response.StatusCode;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Message = "Something wrong in processing the request."
                        }.ToString());
                    }
                });
            });
        }
    }
}
