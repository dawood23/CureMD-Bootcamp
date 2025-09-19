using Azure.Core;
using careliteBackend.DBHelper;
using careliteBackend.DTOs;
using System.Data;

namespace careliteBackend.Repository.BillRepository
{
    public class BillRepository : IBillRepository
    {
        private readonly DataBaseConnection _db;

        public BillRepository(DataBaseConnection db)
        {
            _db = db;
        }

        public async Task<Bill?> GenerateBill(int appointmentId)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = _db.CreateCommand(conn, "stp_GenerateBill", new Dictionary<string, object>
            {
                {"@AppointmentID",appointmentId }
            });

            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                return MapBill(reader);
            }
            return null;
        }

        public async Task<List<Bill>?> GetBills()
        {
            var list=new List<Bill>();
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();
            using var cmd = _db.CreateCommand(conn, "stp_GetBills");
            await using var reader = await cmd.ExecuteReaderAsync();

            if (reader!=null)
            {
                while (reader.Read())
                {
                    list.Add(MapBill(reader));
                }

                return list;
            }
            return null;
           
        }


        public async Task<PaymentResult> RecordPayment(PaymentRequest request)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_RecordPayment", new Dictionary<string, object>
            {
                {"@BillID", request.BillID},
                {"@Amount", request.Amount},
                {"@Method", request.Method},
                {"@RecordedBy", request.RecordedBy}
            });

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new PaymentResult
                {
                    PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                    RemainingPending = reader.GetDecimal(reader.GetOrdinal("RemainingPending")),
                    BillStatus = reader.GetString(reader.GetOrdinal("BillStatus"))
                };
                return result;
            }

            throw new InvalidOperationException("Stored procedure did not return expected result.");
        }

        public async Task<Bill?> GetBillByID(int id)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_GetBillByID", new Dictionary<string, object>
            {
                {"@BillID", id}
            });
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return MapBill(reader);

            return null;

        }

        public async Task<PaymentDto?> GetPaymentByID(int paymentID)
        {
            await using var conn = _db.GetConnection();
            await conn.OpenAsync();

            await using var cmd = _db.CreateCommand(conn, "stp_GetPaymentByID", new Dictionary<string, object>
            {
                {"@PaymentID", paymentID}
            });
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return new PaymentDto
                {
                    PaymentID = reader.GetInt32("PaymentID"),
                    BillID = reader.GetInt32("BillID"),
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    DoctorID = reader.GetInt32("DoctorID"),
                    DoctorName = reader.GetString("DoctorName"),
                    PatientID = reader.GetInt32("PatientID"),
                    PatientName = reader.GetString("PatientName"),
                    Amount = reader.GetDecimal("Amount"),
                    Method = reader.GetString("Method"),
                    PaidAt = reader.GetDateTime("PaidAt"),
                    RecordedBy = reader.GetInt32("RecordedBy")
                };

            return null;

        }

        public async Task<List<PaymentDto>> GetPayments()
        {
            var payments = new List<PaymentDto>();

            using var connection = _db.GetConnection();
            using var cmd = _db.CreateCommand(connection, "stp_GetPayments");

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                payments.Add(new PaymentDto
                {
                    PaymentID = reader.GetInt32("PaymentID"),
                    BillID = reader.GetInt32("BillID"),
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    DoctorID = reader.GetInt32("DoctorID"),
                    DoctorName = reader.GetString("DoctorName"),
                    PatientID = reader.GetInt32("PatientID"),
                    PatientName = reader.GetString("PatientName"),
                    Amount = reader.GetDecimal("Amount"),
                    Method = reader.GetString("Method"),
                    PaidAt = reader.GetDateTime("PaidAt"),
                    RecordedBy = reader.GetInt32("RecordedBy")
                });
            }

            return payments;
        }

        private Bill MapBill(IDataReader reader)
        {
            return new Bill
            {
                BillID = reader.GetInt32(reader.GetOrdinal("BillID")),
                AppointmentID = reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                PatientID = reader.GetInt32(reader.GetOrdinal("PatientID")),
                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                DoctorName=reader.GetString(reader.GetOrdinal("DoctorName")),
                GeneratedAt = reader.GetDateTime(reader.GetOrdinal("GeneratedAt")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                PendingAmount = reader.GetDecimal(reader.GetOrdinal("PendingAmount"))
            };
        }

    }
}
