using System.Data.SqlClient;

namespace ZTxLib.NETCore.Database.SqlServer
{
    public partial class SqlServer
    {
        private class Reader : IReader
        {
            private readonly SqlDataReader _reader;
            public Reader(SqlDataReader reader) => _reader = reader;
            public bool Read() => _reader.Read();
            public void Close() => _reader.Close();
            public bool IsClosed => _reader.IsClosed;
            public object this[int i] => _reader[i];
            public object this[string s] => _reader[s];

            public override string ToString() => string.Join(",", _reader);
        }
    }
}