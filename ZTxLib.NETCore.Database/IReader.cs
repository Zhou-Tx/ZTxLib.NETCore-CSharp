namespace ZTxLib.NETCore.Database
{
    public interface IReader
    {
        bool Read();

        void Close();

        bool IsClosed { get; }

        object this[int i] { get; }

        object this[string s] { get; }

    }
}
