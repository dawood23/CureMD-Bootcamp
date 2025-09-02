using API_Demo.Repository;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public class VisitTypeRepository : IVisitTypeRepository
    {
        private readonly DbHelper _db;

        public VisitTypeRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<VisitType>> GetAll()
        {
            var list = new List<VisitType>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisitTypes");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new VisitType
                {
                    VisitTypeID = reader.GetInt32(reader.GetOrdinal("VisitTypeID")),
                    TypeName = reader["TypeName"].ToString() ?? "",
                    BaseFee = reader.GetDecimal(reader.GetOrdinal("BaseFee")),
                    EstimatedDuration = reader.GetInt32(reader.GetOrdinal("EstimatedDuration"))
                });
            }
            return list;
        }

        public async Task<VisitType?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisitTypeById", new Dictionary<string, object>
            {
                { "@VisitTypeID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new VisitType
                {
                    VisitTypeID = reader.GetInt32(reader.GetOrdinal("VisitTypeID")),
                    TypeName = reader["TypeName"].ToString() ?? "",
                    BaseFee = reader.GetDecimal(reader.GetOrdinal("BaseFee")),
                    EstimatedDuration = reader.GetInt32(reader.GetOrdinal("EstimatedDuration"))
                };
            }
            return null;
        }

        public async Task<int> Add(VisitType type)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddVisitType", new Dictionary<string, object>
            {
                { "@TypeName", type.TypeName },
                { "@BaseFee", type.BaseFee },
                { "@EstimatedDuration", type.EstimatedDuration }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Update(VisitType type)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateVisitType", new Dictionary<string, object>
            {
                { "@VisitTypeID", type.VisitTypeID },
                { "@TypeName", type.TypeName },
                { "@BaseFee", type.BaseFee },
                { "@EstimatedDuration", type.EstimatedDuration }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteVisitType", new Dictionary<string, object>
            {
                { "@VisitTypeID", id }
            });
            await conn.OpenAsync();
            var val = await cmd.ExecuteNonQueryAsync() > 0;
            return val;
        }
    }
}

