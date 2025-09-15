using Microsoft.Data.SqlClient;

namespace Lab_1_utilities.Data
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists(IConfiguration configuration, IWebHostEnvironment webHost)
        {
            string connectionString = configuration.GetConnectionString("Localhost");
            string databaseName = configuration["DatabaseName"];
            if (!DatabaseExists(connectionString, databaseName))
            {
                CreateDatabase(connectionString, databaseName);
            }

            if (!TablesExist(connectionString))
            {
                ExecuteSqlScript(connectionString, webHost);
            }
        }

        private static bool DatabaseExists(string connectionString, string databaseName)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection);
            return command.ExecuteScalar() != DBNull.Value;
        }

        private static void CreateDatabase(string connectionString, string databaseName)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
            command.ExecuteNonQuery();
        }

        private static bool TablesExist(string appConnectionString)
        {
            using var connection = new SqlConnection(appConnectionString);
            connection.Open();
            var command = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = 'BASE TABLE' 
                AND TABLE_NAME IN ('Tenant', 'Service', 'Tenant_Service')", connection);

            int count = (int)command.ExecuteScalar();
            return count == 3;
        }

        private static void ExecuteSqlScript(string appConnectionString, IWebHostEnvironment webHost)
        {
            string scriptPath = Path.Combine(webHost.ContentRootPath, "data", "DatabaseSetup.sql");
            string script = File.ReadAllText(scriptPath);
            if (!File.Exists(scriptPath))
                throw new FileNotFoundException("SQL script not found", scriptPath);

            string sqlScript = File.ReadAllText(scriptPath);
            string[] commands = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using var connection = new SqlConnection(appConnectionString);
            connection.Open();

            foreach (var commandText in commands)
            {
                if (!string.IsNullOrWhiteSpace(commandText))
                {
                    using var command = new SqlCommand(commandText, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
