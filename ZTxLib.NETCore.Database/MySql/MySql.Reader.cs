using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.Database.MySql
{
    public partial class MySql
    {
        private class Reader : IReader
        {
            private readonly MySqlDataReader _reader;
            public Reader(MySqlDataReader reader) => _reader = reader;
            public bool Read() => _reader.Read();
            public void Close() => _reader.Close();
            public bool IsClosed => _reader.IsClosed;
            public object this[int i] => _reader[i];
            public object this[string s] => _reader[s];
            public override string ToString() => string.Join(",", _reader);
        }
    }
}