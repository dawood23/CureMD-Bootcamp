using System.Data;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly DbHelper _db;

        public ActivityLogRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ActivityLog>> GetAll()
        {
            var list = new List<ActivityLog>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetActivityLogs");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapLog(reader));
            }
            return list;
        }

        public async  Task<ActivityLog?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetActivityLogById", new Dictionary<string, object>
            {
                { "@LogID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapLog(reader);
            }
            return null;
        }

        public async Task<int> Add(ActivityLog log)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddActivityLog", new Dictionary<string, object>
            {
                { "@UserID", log.UserID },
                { "@Action", log.Action },
                { "@TableAffected", log.TableAffected },
                { "@RecordID", log.RecordID ?? (object)DBNull.Value },
                { "@Status", log.Status }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteActivityLog", new Dictionary<string, object>
            {
                { "@LogID", id }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        private ActivityLog MapLog(IDataReader reader)
        {
            return new ActivityLog
            {
                LogID = Convert.ToInt32(reader["LogID"]),
                UserID =  Convert.ToInt32(reader["UserID"]),
                Action = reader["Action"].ToString() ?? "",
                TableAffected = reader["TableAffected"].ToString() ?? "",
                RecordID = reader["RecordID"] == DBNull.Value ? null : Convert.ToInt32(reader["RecordID"]),
                Status = reader["Status"].ToString() ?? "",
                Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
            };
        }
    }
}



