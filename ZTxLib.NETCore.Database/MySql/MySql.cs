using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.Database.MySql
{
    public partial class MySql : IDatabase
    {
        private readonly MySqlConnection _conn;

        public MySql(
            string host = "localhost",
            short port = 3306,
            string user = "",
            string password = "",
            string database = "",
            string charset = "utf8",
            short timeout = 5) => _conn =
            new MySqlConnection(
                $"server={host};" +
                $"port={port};" +
                $"user={user};" +
                $"password={password};" +
                $"database={database};" +
                $"charset={charset};" +
                $"connect Timeout={timeout}"
            );

        public void Close()
        {
            try
            {
                _conn.Close();
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改，但不可携带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IReader Execute(string sql, params object[] args) => Execute(new SqlCmd(sql, args));

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改
        /// </summary>
        /// <param name="sql">一条SQL语句组</param>
        /// <returns>查询结果</returns>
        public IReader Execute(SqlCmd sql)
        {
            Close();
            _conn.Open();
            var cmd = new MySqlCommand(sql.SqlStr, _conn);
            for (var i = 0; i < sql.Args.Length; i++)
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
            Close();
            _conn.Open();
            var trans = _conn.BeginTransaction();
            try
            {
                foreach (var sql in sqls)
                {
                    using (var cmd = new MySqlCommand(sql.SqlStr, _conn, trans))
                    {
                        for (var i = 0; i < sql.Args.Length; i++)
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
                _conn.Close();
            }
        }
    }
}