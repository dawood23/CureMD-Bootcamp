using System.Data;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;

namespace Web_API_Assignment.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DbHelper _db;

        public PatientRepository(DbHelper db)
        {
            _db = db;
        }

        public async  Task<IEnumerable<Patient>> GetAll()
        {
            var list = new List<Patient>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetPatients");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapPatient(reader));
            }
            return list;
        }

        public async Task<Patient?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetPatientById", new Dictionary<string, object>
            {
                { "@PatientID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapPatient(reader);
            }
            return null;
        }

        public async Task<int> Add(Patient patient)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddPatient", new Dictionary<string, object>
            {
                { "@FirstName", patient.FirstName },
                { "@LastName", patient.LastName },
                { "@DateOfBirth", patient.DateOfBirth ?? (object)DBNull.Value },
                { "@PhoneNumber", patient.PhoneNumber ?? (object)DBNull.Value },
                { "@Email", patient.Email ?? (object)DBNull.Value },
                { "@Address", patient.Address ?? (object)DBNull.Value },
                { "@EmergencyContact", patient.EmergencyContact ?? (object)DBNull.Value }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Update(Patient patient)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdatePatient", new Dictionary<string, object>
            {
                { "@PatientID", patient.PatientID },
                { "@FirstName", patient.FirstName },
                { "@LastName", patient.LastName },
                { "@DateOfBirth", patient.DateOfBirth ?? (object)DBNull.Value },
                { "@PhoneNumber", patient.PhoneNumber ?? (object)DBNull.Value },
                { "@Email", patient.Email ?? (object)DBNull.Value },
                { "@Address", patient.Address ?? (object)DBNull.Value },
                { "@EmergencyContact", patient.EmergencyContact ?? (object)DBNull.Value }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeletePatient", new Dictionary<string, object>
            {
                { "@PatientID", id }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        private Patient MapPatient(IDataReader reader)
        {
            return new Patient
            {
                PatientID = Convert.ToInt32(reader["PatientID"]),
                FirstName = reader["FirstName"].ToString() ?? "",
                LastName = reader["LastName"].ToString() ?? "",
                DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                Email = reader["Email"]?.ToString(),
                Address = reader["Address"]?.ToString(),
                EmergencyContact = reader["EmergencyContact"]?.ToString()
            };
        }
    }
}

