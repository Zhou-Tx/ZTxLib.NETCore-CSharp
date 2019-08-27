namespace ZTxLib.NETCore.Database
{
    public interface IDatabase
    {
        IReader Execute(string sql, params object[] args);

        void Close();

        void Add(string sql, params object[] args);

        bool Execute();

    }
}
