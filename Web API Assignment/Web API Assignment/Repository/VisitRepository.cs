using System.Data;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public class VisitRepository : IVisitRepository
    {
        private readonly DbHelper _db;

        public VisitRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Visit>> GetAll()
        {
            var list = new List<Visit>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisits");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapVisit(reader));
            }
            return list;
        }

        public async Task<Visit?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisitById", new Dictionary<string, object>
            {
                { "@VisitID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapVisit(reader);
            }
            return null;
        }

        public async Task<int> Add(Visit visit)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddVisit", new Dictionary<string, object>
            {
                { "@PatientID", visit.PatientID },
                { "@DoctorID", visit.DoctorID ?? (object)DBNull.Value },
                { "@VisitTypeID", visit.VisitTypeID },
                { "@VisitDate", visit.VisitDate },
                { "@VisitTime", visit.VisitTime },
                { "@Description", visit.Description ?? (object)DBNull.Value },
                { "@Notes", visit.Notes ?? (object)DBNull.Value },
                { "@Status", visit.Status ?? "Scheduled" },
                { "@Fee", visit.Fee ?? (object)DBNull.Value },
                { "@CreatedBy", visit.CreatedBy }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Update(Visit visit)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateVisit", new Dictionary<string, object>
            {
                { "@VisitID", visit.VisitID },
                { "@PatientID", visit.PatientID },
                { "@DoctorID", visit.DoctorID ?? (object)DBNull.Value },
                { "@VisitTypeID", visit.VisitTypeID },
                { "@VisitDate", visit.VisitDate },
                { "@VisitTime", visit.VisitTime },
                { "@Description", visit.Description ?? (object)DBNull.Value },
                { "@Notes", visit.Notes ?? (object)DBNull.Value },
                { "@Status", visit.Status ?? "Scheduled" },
                { "@Fee", visit.Fee ?? (object)DBNull.Value },
                { "@CreatedBy", visit.CreatedBy }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteVisit", new Dictionary<string, object>
            {
                { "@VisitID", id }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        private Visit MapVisit(IDataReader reader)
        {
            return new Visit
            {
                VisitID = Convert.ToInt32(reader["VisitID"]),
                PatientID = Convert.ToInt32(reader["PatientID"]),
                DoctorID = reader["DoctorID"] == DBNull.Value ? null : Convert.ToInt32(reader["DoctorID"]),
                VisitTypeID = Convert.ToInt32(reader["VisitTypeID"]),
                VisitDate = reader.GetDateTime(reader.GetOrdinal("VisitDate")),
                VisitTime = (TimeSpan)reader["VisitTime"],
                Description = reader["Description"]?.ToString(),
                Notes = reader["Notes"]?.ToString(),
                Status = reader["Status"]?.ToString(),
                Fee = reader["Fee"] == DBNull.Value ? null : reader.GetDecimal(reader.GetOrdinal("Fee")),
                CreatedBy = Convert.ToInt32(reader["CreatedBy"])
            };
        }
    }

}
