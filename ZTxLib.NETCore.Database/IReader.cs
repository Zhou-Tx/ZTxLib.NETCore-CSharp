namespace ZTxLib.NETCore.Database
{
    public interface IReader
    {
        bool Read();

        object this[int i] { get; }

        object this[string s] { get; }

    }
}
