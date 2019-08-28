using System.Data.SqlClient;

namespace ZTxLib.NETCore.Database
{
    /// <summary>
    /// SQL Server 数据库服务器操作类
    /// </summary>
    public partial class SqlServer : IDatabase
    {
        internal class SqlServerReader : IReader
        {
            private readonly SqlDataReader reader;
            public SqlServerReader(SqlDataReader reader) => this.reader = reader;
            public bool Read() => reader.Read();
            public void Close() => reader.Close();
            public bool IsClosed => reader.IsClosed;
            public object this[int i] => reader[i];
            public object this[string s] => reader[s];
        }

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
            SqlCommand cmd = new SqlCommand(sql.SqlStr, conn);
            foreach (SqlParameter sqlParameter in sql.Parameters)
                cmd.Parameters.Add(sqlParameter);
            return new SqlServerReader(cmd.ExecuteReader());
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
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (var sql in sqls)
                {
                    using (SqlCommand cmd = new SqlCommand(sql.SqlStr, conn, trans))
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
