using System.Net;
using System.Text;

namespace YurtBursu.Api.Middleware
{
	/// <summary>
	/// Basic authentication middleware to protect admin-only endpoints.
	/// Secures only:
	///  - POST /api/gallery/upload
	///  - POST /api/notification/send
	/// Public endpoints remain open:
	///  - GET /api/gallery/list
	///  - POST /api/notification/register
	/// If ADMIN credentials are not configured in environment, checks are bypassed (workaround).
	/// </summary>
	public class BasicAuthMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly string? _username;
		private readonly string? _password;

		public BasicAuthMiddleware(RequestDelegate next, IConfiguration config)
		{
			_next = next;
			_username = config["AdminUser"];
			_password = config["AdminPass"];
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Determine if the current request targets an admin-protected endpoint
			var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
			var method = context.Request.Method?.ToUpperInvariant() ?? "GET";

			var isProtectedEndpoint =
				(method == HttpMethods.Post && path == "/api/gallery/upload") ||
				(method == HttpMethods.Post && path == "/api/notification/send");

			if (!isProtectedEndpoint)
			{
				await _next(context);
				return;
			}

			// Workaround: If credentials are not configured, allow the request
			if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_password))
			{
				await _next(context);
				return;
			}

			if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
			{
				context.Response.Headers["WWW-Authenticate"] = "Basic";
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}

			var auth = authHeader.ToString();
			if (!auth.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
			{
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}

			var encoded = auth.Substring("Basic ".Length).Trim();
			string decoded;
			try
			{
				decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
			}
			catch
			{
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}

			var parts = decoded.Split(':', 2);
			if (parts.Length != 2 || parts[0] != _username || parts[1] != _password)
			{
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				return;
			}

			await _next(context);
		}
	}
}
