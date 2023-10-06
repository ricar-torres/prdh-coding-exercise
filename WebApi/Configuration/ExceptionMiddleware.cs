using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Configuration;

public static class ExceptionMiddleware {
	public static void ConfigureExceptionHandler(this IApplicationBuilder app) {
		app.UseExceptionHandler(appError => {
			appError.Run(async context => {
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (contextFeature != null) {
					// logger.LogError($"Something went wrong: {contextFeature.Error}");
					string? errorResponse = new {
						Status = context.Response.StatusCode,
						Error = "Internal Server Error."
					}.ToString();
					await context.Response.WriteAsync(errorResponse ?? string.Empty);
				}
			});
		});
	}
}
