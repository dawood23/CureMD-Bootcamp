using careliteBackend.DBHelper;
using careliteBackend.Models;
using System.Data;
using System.Data.Common;

namespace careliteBackend.Repository.PatientRepository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataBaseConnection _db;
        public PatientRepository(DataBaseConnection db) => _db = db;

        public async Task<int> CreatePatient(Patient patient)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddPatient", new Dictionary<string, object>
            {
                {"@FirstName", patient.FirstName},
                {"@LastName", patient.LastName},
                {"@DOB", patient.DOB},
                {"@Gender", patient.Gender},
                {"@Phone", patient.Phone},
                {"@Email", patient.Email},
                {"@Address", patient.Address},
                {"@CNIC",patient.cnic }
            });

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<Patient?> GetPatientById(int patientId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetPatientById", new Dictionary<string, object>
            {
                {"@PatientID", patientId}
            });

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapPatient(reader);

            return null;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            var patients = new List<Patient>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetPatients");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                patients.Add(MapPatient(reader));
            }
            return patients;
        }

        public async Task<int> UpdatePatient(Patient patient)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdatePatient", new Dictionary<string, object>
            {
                {"@PatientID", patient.PatientID},
                {"@FirstName", patient.FirstName},
                {"@LastName", patient.LastName},
                {"@DOB", patient.DOB},
                {"@Gender", patient.Gender},
                {"@Phone", patient.Phone},
                {"@Email", patient.Email},
                {"@Address", patient.Address}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeletePatient(int patientId)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeletePatient", new Dictionary<string, object>
            {
                {"@PatientID", patientId}
            });

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<(List<Patient> Patients, int TotalCount)> GetPatientsPaged(int pageNumber, int pageSize, string search)
        {
            var patients = new List<Patient>();
            int totalCount = 0;

            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetPatientsPaged", new Dictionary<string, object>
    {
        {"@PageNumber", pageNumber},
        {"@PageSize", pageSize},
        {"@Search", search ?? string.Empty}
    });

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                patients.Add(MapPatient(reader));
            }

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
            }

            return (patients, totalCount);
        }



        private Patient MapPatient(IDataReader reader)
        {
            return new Patient
            {
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? null : reader.GetDateTime(reader.GetOrdinal("DOB")),
                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
     
            };
        }

    }
}
