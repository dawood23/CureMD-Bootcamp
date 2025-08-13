using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;

namespace Web_API_Assignment.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DbHelper _db;

        public UserRoleRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            var list = new List<UserRole>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetUserRoles");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new UserRole
                {
                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                    RoleName = reader["RoleName"].ToString() ?? "",
                    Description = reader["Description"]?.ToString()
                });
            }
            return list;
        }

        public async Task<UserRole?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetUserRoleById", new Dictionary<string, object>
            {
                { "@RoleID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserRole
                {
                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                    RoleName = reader["RoleName"].ToString() ?? "",
                    Description = reader["Description"]?.ToString()
                };
            }
            return null;
        }

        public async Task<int> Add(UserRole role)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddUserRole", new Dictionary<string, object>
            {
                { "@RoleName", role.RoleName },
                { "@Description", role.Description }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Update(UserRole role)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateUserRole", new Dictionary<string, object>
            {
                { "@RoleID", role.RoleID },
                { "@RoleName", role.RoleName },
                { "@Description", role.Description }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteUserRole", new Dictionary<string, object>
            {
                { "@RoleID", id }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}