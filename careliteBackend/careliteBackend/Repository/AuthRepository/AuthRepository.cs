using careliteBackend.DBHelper;
using careliteBackend.Models;
using careliteBackend.Repository;
using System.Data;

namespace careliteBackend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataBaseConnection _db;

        public AuthRepository(DataBaseConnection db)
        {
            _db = db;
        }

     
        public async Task<User?> GetByUsername(string username)
        {
            using var connection=_db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_GetUserByName", new Dictionary<string, object>
        {
            {"@Username", username}
        });
            await connection.OpenAsync();
            using var reader=await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }
            else return null;
        }

        public async Task<int?> CreateUser(User user)
        {
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_AddUser", new Dictionary<string, object>
                {
                    { "@Username", user.Username },
                    { "@PasswordHash", user.PasswordHash },
                    {"@Email",user.Email },
                    {"@Phone",user.Phone },
                    { "@RoleID", user.RoleID },
                    { "@Active", user.Active }
                });

            await connection.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }


        private User MapUser(IDataReader reader)
        {
            return new User
            {
                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                Active = reader.GetBoolean(reader.GetOrdinal("Active"))
            };
        }
    }
}
