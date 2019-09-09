using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ZTxLib.Database
{
    public partial class MySQL : IDatabase
    {
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

        private void Open()
        {
            Close();
            conn.Open();
        }

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改，但不可携带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IReader Execute(string sql, params object[] args) => Execute(new SqlCmd(sql, args));

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改
        /// </summary>
        /// <param name="sql">一条SQL语句组</param>
        /// <returns>查询结果</returns>
        public IReader Execute(SqlCmd sql)
        {
            Open();
            MySqlCommand cmd = new MySqlCommand(sql.SqlStr, conn);
            for (int i = 0; i < sql.Args.Length; i++)
                cmd.Parameters.Add(new MySqlParameter($"param_{i}", sql.Args[i]));
            return new Reader(cmd.ExecuteReader());
        }

        /// <summary>
        /// 提交一项事务，该函数不可用于查询
        /// </summary>
        /// <param name="sqls">若干条SQL语句组</param>
        /// <returns>成功/失败</returns>
        public bool Execute(params SqlCmd[] sqls)
        {
            Open();
            MySqlTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (SqlCmd sql in sqls)
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql.SqlStr, conn, trans))
                    {
                        for (int i = 0; i < sql.Args.Length; i++)
                            cmd.Parameters.Add(new SqlParameter($"param_{i}", sql.Args[i]));
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
