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
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.Contains("login") || path.Contains("signup")))
            {
                await _next(context);
                return;
            }

            await _next(context);

            try
            {
                var userId = GetUserIdFromClaims(context.User);

                if (userId <= 0)
                {
                    Console.WriteLine($"Skipping log entry - Invalid or missing UserID: {userId}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataBaseConnection>();
                using var connection = db.GetConnection();
                await connection.OpenAsync();

                var actionWord = context.Request.Method switch
                {
                    "POST" => "CREATE",
                    "PUT" => "UPDATE",
                    "DELETE" => "DELETE",
                    "GET" => "READ",
                    _ => context.Request.Method
                };

                var description =
                    $"{actionWord} performed on {path} at {DateTime.UtcNow:O}"; 

                var parameters = new Dictionary<string, object?>
                {
                    { "@UserID", userId },
                    { "@Action", context.Request.Method },
                    { "@TableAffected", path ?? "Unknown" },
                    { "@Status", context.Response.StatusCode < 400 ? "Success" : "Failed" },
                    { "@Description", description }
                };

                using var cmd = db.CreateCommand(connection, "stp_AddLog", parameters!);
                await cmd.ExecuteNonQueryAsync();

                Console.WriteLine($"Successfully logged action for UserID: {userId}, Path: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
            }
        }

        private int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("userId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) && userId > 0)
            {
                return userId;
            }

            Console.WriteLine("Available claims:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"  {claim.Type}: {claim.Value}");
            }

            return 0;
        }
    }
}
