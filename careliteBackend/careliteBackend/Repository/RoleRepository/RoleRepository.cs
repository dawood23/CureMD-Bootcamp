using careliteBackend.DBHelper;
using careliteBackend.Models;
using careliteBackend.Repository;

namespace careliteBackend.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataBaseConnection _db;
        public RoleRepository(DataBaseConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateRole(Role role)
        {
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_AddRole", new Dictionary<string, object>
            {
                {"@Name", role.Name},
                {"@Description", role.Description}
            });

            await connection.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_GetRoleById", new Dictionary<string, object>
            {
                {"@RoleID", roleId}
            });

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Role
                {
                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description"))
                };
            }
            return null;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            var roles = new List<Role>();
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_GetRoles");

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                roles.Add(new Role
                {
                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description"))
                });
            }
            return roles;
        }

        public async Task<int> UpdateRole(Role role)
        {
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_UpdateRole", new Dictionary<string, object>
            {
                {"@RoleID", role.RoleID},
                {"@Name", role.Name},
                {"@Description", role.Description}
            });

            await connection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteRole(int roleId)
        {
            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_DeleteRole", new Dictionary<string, object>
            {
                {"@RoleID", roleId}
            });

            await connection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
