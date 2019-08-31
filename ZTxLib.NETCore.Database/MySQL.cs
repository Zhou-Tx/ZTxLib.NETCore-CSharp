using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace ZTxLib.NETCore.Database
{
    public class MySQL : IDatabase
    {
        internal class MySqlReader : IReader
        {
            private readonly MySqlDataReader reader;
            public MySqlReader(MySqlDataReader reader) => this.reader = reader;
            public bool Read() => reader.Read();
            public void Close() => reader.Close();
            public bool IsClosed => reader.IsClosed;
            public object this[int i] => reader[i];
            public object this[string s] => reader[s];
        }

        private readonly MySqlConnection conn;
        public MySQL(
            string server = "localhost",
            short port = 3306,
            string user = "",
            string password = "",
            string database = "",
            string charset = "utf8",
            short timeout = 5) => conn =
            new MySqlConnection(
                $"server={server};" +
                $"port={port};" +
                $"user={user};" +
                $"password={password};" +
                $"database={database};" +
                $"charset={charset};" +
                $"connect Timeout={timeout}"
            );

        public void Close()
        {
            try { conn.Close(); } catch { }
        }

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改，但不可携带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IReader Execute(string sql) => Execute(new SqlDesigner(sql));

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改
        /// </summary>
        /// <param name="sql">一条SQL语句组</param>
        /// <returns>查询结果</returns>
        public IReader Execute(SqlDesigner sql)
        {
            Close();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql.SqlStr, conn);
            foreach (SqlParameter sqlParameter in sql.Parameters)
                cmd.Parameters.Add(sqlParameter);
            return new MySqlReader(cmd.ExecuteReader());
        }

        /// <summary>
        /// 提交一项事务，该函数不可用于查询
        /// </summary>
        /// <param name="sqls">若干条SQL语句组</param>
        /// <returns>成功/失败</returns>
        public bool Execute(params SqlDesigner[] sqls)
        {
            Close();
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (var sql in sqls)
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql.SqlStr, conn, trans))
                    {
                        foreach (SqlParameter sqlParameter in sql.Parameters)
                            cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();
                    }
                }
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
            }
        }
    }
}

