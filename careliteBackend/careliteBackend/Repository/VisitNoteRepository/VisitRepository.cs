using careliteBackend.DBHelper;
using careliteBackend.DTOs;
using careliteBackend.Repository.VisitNoteRepository;
using System.Data;

namespace careliteBackend.Repository.VisitRepository
{
    public class VisitRepository : IVisitRepository
    {
        private readonly DataBaseConnection _db;
        public VisitRepository(DataBaseConnection db) => _db = db;

        public async Task<IEnumerable<VisitDto>> GetVisits()
        {
            var visits = new List<VisitDto>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisitNotes");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                visits.Add(MapVisits(reader));
            }
            return visits;
        }

        public async Task<VisitDto?> GetVisitById(int appointmentId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetVisitNotesById", new Dictionary<string, object>
            {
                {"@AppointmentID", appointmentId}
            });

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapVisits(reader);

            return null;
        }

        public async Task<int> AddVisit(int appointmentId, string content)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddVisitNote", new Dictionary<string, object>
            {
                {"@AppointmentID", appointmentId},
                {"@Content", content}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> UpdateVisit(int visitNoteId, string content)
        {
            Console.WriteLine("Content in repository: ", content);
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateVisitNote", new Dictionary<string, object>
            {
                {"@VisitNoteID", visitNoteId},
                {"@Content", content}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteVisit(int visitNoteId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteVisitNote", new Dictionary<string, object>
            {
                {"@VisitNoteID", visitNoteId}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        private VisitDto MapVisits(IDataReader reader)
        {
            return new VisitDto
            {
                VisitNoteID = reader.GetInt32(reader.GetOrdinal("VisitNoteID")),
                Content = reader.IsDBNull(reader.GetOrdinal("Content")) ? null : reader.GetString(reader.GetOrdinal("Content")),
                AppointmentID = reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                AppointmentStatus = reader.GetString(reader.GetOrdinal("AppointmentStatus")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

   
    }
}
