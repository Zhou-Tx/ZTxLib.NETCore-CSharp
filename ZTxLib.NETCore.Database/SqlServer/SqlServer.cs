using System.Data.SqlClient;

namespace ZTxLib.Database
{
    /// <summary>
    /// SQL Server 数据库服务器操作类
    /// </summary>
    public partial class SqlServer : IDatabase
    {
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
        
        /// <summary>
        /// 断开与数据库的连接
        /// </summary>
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
            Close();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql.SqlStr, conn);
            for (int i = 0; i < sql.Args.Length; i++)
                cmd.Parameters.Add(new SqlParameter($"param_{i}", sql.Args[i]));
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
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (SqlCmd sql in sqls)
                {
                    using (SqlCommand cmd = new SqlCommand(sql.SqlStr, conn, trans))
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
