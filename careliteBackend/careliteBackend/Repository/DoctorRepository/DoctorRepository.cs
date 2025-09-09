using careliteBackend.DBHelper;
using System.Data.Common;
using careliteBackend.Models;
using System.Data;

namespace careliteBackend.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DataBaseConnection _db;
        public DoctorRepository(DataBaseConnection db) => _db = db;


        public async Task<int> CreateDoctor(Doctor doctor)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddDoctor", new Dictionary<string, object>
            {
                {"@DoctorName", doctor.DoctorName},
                {"@Specialization", doctor.Specialization}
            });

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Doctor?> GetDoctorById(int doctorId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetDoctors");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("DoctorID") == doctorId)
                    return MapDoctor(reader);
            }
            return null;
        }

        public async Task<List<Doctor>> GetAllDoctors()
        {
            var doctors = new List<Doctor>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetDoctors");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                doctors.Add(MapDoctor(reader));
            }
            return doctors;
        }

        public async Task<int> UpdateDoctor(Doctor doctor)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateDoctor", new Dictionary<string, object>
            {
                {"@DoctorID", doctor.DoctorID},
                {"@DoctorName", doctor.DoctorName},
                {"@Specialization", doctor.Specialization}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteDoctor(int doctorId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteDoctor", new Dictionary<string, object>
            {
                {"@DoctorID", doctorId}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        private Doctor MapDoctor(IDataReader reader)
        {
            return new Doctor
            {
                DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                Specialization = reader.IsDBNull(reader.GetOrdinal("Specialization"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Specialization"))
            };
        }
    }
}
