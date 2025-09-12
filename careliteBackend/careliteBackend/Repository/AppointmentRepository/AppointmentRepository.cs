using careliteBackend.DBHelper;
using careliteBackend.DTOs;
using careliteBackend.Models;
using Microsoft.Data.SqlClient;

namespace careliteBackend.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataBaseConnection _db;

        public AppointmentRepository(DataBaseConnection db)
        {
            _db = db;
        }

        public async Task<int> AddAppointment(AppointmentRequest request)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_AddAppointment", new Dictionary<string, object>
            {
                {"@PatientID", request.PatientID},
                {"@DoctorID", request.DoctorID},
                {"@CreatedBy", request.CreatedBy},
                {"@StartTime", request.StartTime},
                {"@DurationMinutes", request.DurationMinutes},
                {"@Status", request.Status ?? "Scheduled"}
            });

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<AppointmentDto>> GetAppointments()
        {
            var appointments = new List<AppointmentDto>();

            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_GetAppointments");
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                appointments.Add(new AppointmentDto
                {
                    AppointmentID = (int)reader["AppointmentID"],
                    PatientName = reader["PatientName"].ToString()!,
                    PatientID = (int)reader["PatientID"],
                    DoctorID= (int)reader["DoctorID"],
                    DoctorName = reader["DoctorName"].ToString()!,
                    StartTime = (DateTime)reader["StartTime"],
                    DurationMinutes = (int)reader["DurationMinutes"],
                    Status = reader["Status"].ToString()!,
                    CreatedAt = (DateTime)reader["CreatedAt"]
                }); 
            }

            return appointments;
        }

        public async Task<AppointmentDto?> GetAppointmentByID(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_GetAppointmentsByID", new Dictionary<string, object>
            {
                {"@AppointmentID", id}
            });
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return new AppointmentDto
                {
                    AppointmentID = (int)reader["AppointmentID"],
                    PatientName = reader["PatientName"].ToString()!,
                    PatientID = (int)reader["PatientID"],
                    DoctorID = (int)reader["DoctorID"],
                    DoctorName = reader["DoctorName"].ToString()!,
                    StartTime = (DateTime)reader["StartTime"],
                    DurationMinutes = (int)reader["DurationMinutes"],
                    Status = reader["Status"].ToString()!,
                    CreatedAt = (DateTime)reader["CreatedAt"]
                };

            return null;
        }


        public async Task<int> UpdateAppointment(AppointmentRequest request)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_UpdateAppointment", new Dictionary<string, object>
            {
                {"@AppointmentID",request.AppointmentID},
                {"@PatientID", request.PatientID},
                {"@DoctorID", request.DoctorID},
                {"@StartTime", request.StartTime},
                {"@DurationMinutes", request.DurationMinutes},
                {"@Status", request.Status ?? "Scheduled"}
            });

            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAppointment(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_DeleteAppointment", new Dictionary<string, object>
            {
                {"@AppointmentID", id}
            });

            return await cmd.ExecuteNonQueryAsync();
        }

                public async Task<IEnumerable<WeeklyCalendarDto>> GetWeeklyCalendar(int doctorId, DateTime weekStartDate)
                {
                    var results = new List<WeeklyCalendarDto>();

                    await using var conn = _db.GetConnection();
                    await conn.OpenAsync();

                    await using var cmd = _db.CreateCommand(conn, "stp_GetWeeklyCalendar", new Dictionary<string, object>
                        {
                            { "@DoctorID", doctorId },
                            { "@WeekStartDate", weekStartDate }
                        });
    
                    await using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        results.Add(new WeeklyCalendarDto
                        {
                            AppointmentID = reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                            StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                            DurationMinutes = reader.GetInt32(reader.GetOrdinal("DurationMinutes")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                            PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                            DoctorID = reader.GetInt32(reader.GetOrdinal("DoctorID")),
                            DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                            AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                            AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime")),
                            EndTime = reader.GetDateTime(reader.GetOrdinal("EndTime"))
                        });
                    }

                    return results;
                }


    }
}
