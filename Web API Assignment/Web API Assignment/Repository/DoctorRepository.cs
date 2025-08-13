using System.Data;
using Web_API_Assignment.DBScripts;
using Web_API_Assignment.Models;
using Web_API_Assignment.Repository;

namespace Web_API_Assignment.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DbHelper _db;

        public DoctorRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            var list = new List<Doctor>();
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetDoctors");
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapDoctor(reader));
            }
            return list;
        }

        public async Task<Doctor?> GetById(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_GetDoctorById", new Dictionary<string, object>
            {
                { "@DoctorID", id }
            });
            await conn.OpenAsync();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapDoctor(reader);
            }
            return null;
        }

        public async Task<int> Add(Doctor doctor)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_AddDoctor", new Dictionary<string, object>
            {
                { "@FirstName", doctor.FirstName },
                { "@LastName", doctor.LastName },
                { "@PhoneNumber", doctor.PhoneNumber ?? (object)DBNull.Value },
                { "@Email", doctor.Email ?? (object)DBNull.Value }
            });
            await conn.OpenAsync();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public async Task<bool> Update(Doctor doctor)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_UpdateDoctor", new Dictionary<string, object>
            {
                { "@DoctorID", doctor.DoctorID },
                { "@FirstName", doctor.FirstName },
                { "@LastName", doctor.LastName },
                { "@PhoneNumber", doctor.PhoneNumber ?? (object)DBNull.Value },
                { "@Email", doctor.Email ?? (object)DBNull.Value }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var conn = _db.GetConnection();
            using var cmd = _db.CreateCommand(conn, "stp_DeleteDoctor", new Dictionary<string, object>
            {
                { "@DoctorID", id }
            });
            await conn.OpenAsync();
            return cmd.ExecuteNonQuery() > 0;
        }

        private Doctor MapDoctor(IDataReader reader)
        {
            return new Doctor
            {
                DoctorID = Convert.ToInt32(reader["DoctorID"]),
                FirstName = reader["FirstName"].ToString() ?? "",
                LastName = reader["LastName"].ToString() ?? "",
                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                Email = reader["Email"]?.ToString()
            };
        }
    }
}


  

