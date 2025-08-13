using System.Data;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _dbhelper;
        public UserRepository(DbHelper helper) {
           _dbhelper= helper;
        }
        public async Task<int> Add(User user)
        {
            using var connection=_dbhelper.GetConnection();
            using var command= _dbhelper.CreateCommand(connection,"stp_AddUser",new Dictionary<string, object>
            {
                { "@Username", user.Username },
                { "@PasswordHash", user.PasswordHash },
                { "@RoleID", user.RoleID },
                { "@FirstName", user.FirstName },
                { "@LastName", user.LastName }
            });
            await connection.OpenAsync();
            return Convert.ToInt32(command.ExecuteScalar());
        }

        public async Task<bool> Delete(int id)
        {
            using var connection=_dbhelper.GetConnection();
            using var command = _dbhelper.CreateCommand(connection, "stp_DeleteUser", new Dictionary<string, object> { 
                { "@UserID",id} });

            await connection.OpenAsync();

            return command.ExecuteNonQuery()>0;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var list =new List<User>();
            using var connection=_dbhelper.GetConnection();
            using var command = _dbhelper.CreateCommand(connection, "stp_GetUsers");
            await connection.OpenAsync();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(
                    MapUser(reader)
                    );
            }

            return list;
        }

        public async Task<User?> GetById(int id)
        {
            using var connection =_dbhelper.GetConnection();
            using var command= _dbhelper.CreateCommand(connection,"stp_GetUserById",new Dictionary<string, object> { 
                { "@UserID", id } 
            } );
            await connection.OpenAsync();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            else return null;
        }

        public async Task<User?> GetByUsername(string username)
        {
            using var connection = _dbhelper.GetConnection();
            using var command = _dbhelper.CreateCommand(connection, "stp_GetUserById", new Dictionary<string, object> {
                { "@Username", username }
            });
            await connection.OpenAsync();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapUser(reader);
            }
            else return null;
        }

        public async Task<bool> Update(User user)
        {
           using var connection= _dbhelper.GetConnection();
            using var command = _dbhelper.CreateCommand(connection, "stp_UpdateUser", new Dictionary<string, object> {
                { "@UserID", user.UserID },
                { "@Username", user.Username },
                { "@PasswordHash", user.PasswordHash },
                { "@RoleID", user.RoleID },
                { "@FirstName", user.FirstName },
                { "@LastName", user.LastName }
            });
            await connection.OpenAsync();

            return command.ExecuteNonQuery() > 0;
        }

        private User MapUser(IDataReader reader)
        {
            return new User
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                Username = reader["Username"].ToString() ?? "",
                PasswordHash = reader["PasswordHash"].ToString() ?? "",
                RoleID = Convert.ToInt32(reader["RoleID"]),
                FirstName = reader["FirstName"].ToString() ?? "",
                LastName = reader["LastName"].ToString() ?? ""
            };
        }
    }

   
}
