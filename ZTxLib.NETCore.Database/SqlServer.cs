using System.Collections.Generic;
using System.Data.SqlClient;

namespace ZTxLib.NETCore.Database
{
    public partial class SqlServer : IDatabase
    {
        internal class SqlServerReader : IReader
        {
            private readonly SqlDataReader reader;
            public SqlServerReader(SqlDataReader reader) => this.reader = reader;
            public bool Read() => reader.Read();
            public object this[int i] => reader[i];
            public object this[string s] => reader[s];
        }

        private readonly List<string> sqlList = new List<string>();
        private readonly SqlConnection conn;

        public SqlServer(
            string database,
            string server = ".",
            string integrated_security = "SSPI") => conn =
            new SqlConnection(
                $"server={server};" +
                $"database={database};" +
                $"integrated security={integrated_security}"
            );

        public void Close()
        {
            try { conn.Close(); } catch { }
        }

        public void Add(string sql, params object[] args) => sqlList.Add(string.Format(sql, args));

        public bool Execute()
        {
            Close();
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (string sql in sqlList)
                    new SqlCommand(sql, conn, trans).ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
            finally
            {
                conn.Close();
                sqlList.Clear();
            }
        }

        public IReader Execute(string sql, params object[] args)
        {
            Close();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format(sql, args);
            return new SqlServerReader(cmd.ExecuteReader());
        }
    }
}
