using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace careliteBackend.DBHelper
{
    public class DataBaseConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataBaseConnection(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection string is not configured");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlCommand CreateCommand(SqlConnection connection, string storedProcedure, Dictionary<string, object>? parameters = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (string.IsNullOrEmpty(storedProcedure)) throw new ArgumentException("Stored procedure name cannot be null or empty", nameof(storedProcedure));

            var cmd = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return cmd;
        }

        public bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                Console.WriteLine("Database connection successful.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}