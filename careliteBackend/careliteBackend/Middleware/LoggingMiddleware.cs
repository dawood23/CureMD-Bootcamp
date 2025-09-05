using careliteBackend.DBHelper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace careliteBackend.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            // Skip logging for login/signup
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.Contains("login") || path.Contains("signup")))
            {
                await _next(context);
                return;
            }

            // Process request first so we know response status
            await _next(context);

            // Now log the action
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataBaseConnection>();

                using var connection = db.GetConnection();
                await connection.OpenAsync();

                var parameters = new Dictionary<string, object?>
                {
                    { "@UserID", GetUserIdFromClaims(context.User) },
                    { "@Action", context.Request.Method },
                    { "@TableAffected", path ?? "Unknown" },
                    { "@RecordID", DBNull.Value },
                    { "@Status", context.Response.StatusCode < 400 ? "Success" : "Failed" }
                };

                using var cmd = db.CreateCommand(connection, "stp_AddLog", parameters!);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Logging failed: {ex.Message}");
            }
        }

        private int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : 0;
        }
    }
}
