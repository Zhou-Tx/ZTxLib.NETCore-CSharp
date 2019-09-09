namespace ZTxLib.Database
{
    /// <summary>
    /// 数据库接口
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// 断开与数据库的连接
        /// </summary>
        void Close();

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改，但不可携带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        IReader Execute(string sql, params object[] args);

        /// <summary>
        /// 提交一条语句，该函数可用于查询或修改
        /// </summary>
        /// <param name="cmd">一条SQL语句组</param>
        /// <returns>查询结果</returns>
        IReader Execute(SqlCmd sql);

        /// <summary>
        /// 提交一项事务，该函数不可用于查询
        /// </summary>
        /// <param name="cmds">若干条SQL语句组</param>
        /// <returns>成功/失败</returns>
        bool Execute(params SqlCmd[] sql);
    }
}
