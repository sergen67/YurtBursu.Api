using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace YurtBursu.Api.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");
				await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
			}
		}

		private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			var payload = JsonSerializer.Serialize(new { error = message });
			await context.Response.WriteAsync(payload);
		}
	}
}


