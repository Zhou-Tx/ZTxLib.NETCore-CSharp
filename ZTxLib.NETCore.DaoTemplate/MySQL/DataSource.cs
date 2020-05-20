using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.DaoTemplate.MySQL
{
    public class DataSource
    {
        private string Host { get; }
        private short Port { get; }
        private string User { get; }
        private string Password { get; }
        private string Schema { get; }
        private string Charset { get; }
        private short Timeout { get; }

        public DataSource(
            string host,
            short port,
            string user,
            string password,
            string schema,
            string charset = "utf8mb4",
            short timeout = 5)
        {
            Host = host;
            Port = port;
            User = user;
            Password = password;
            Schema = schema;
            Charset = charset;
            Timeout = timeout;
        }

        internal MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(
                $"server={Host};" +
                $"port={Port};" +
                $"user={User};" +
                $"password={Password};" +
                $"database={Schema};" +
                $"charset={Charset};" +
                $"connect Timeout={Timeout}"
            );
            conn.Open();
            return conn;
        }
    }
}