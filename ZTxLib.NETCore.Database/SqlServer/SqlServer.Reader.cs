using System.Data.SqlClient;

namespace ZTxLib.Database
{
    public partial class SqlServer
    {
        internal class Reader : IReader
        {
            private readonly SqlDataReader reader;
            public Reader(SqlDataReader reader) => this.reader = reader;
            public bool Read() => reader.Read();
            public void Close() => reader.Close();
            public bool IsClosed => reader.IsClosed;
            public object this[int i] => reader[i];
            public object this[string s] => reader[s];
            public override string ToString()
            {
                string s = string.Empty;
                foreach (var v in reader)
                    s += ',' + v.ToString();
                return s.Substring(1);
            }
        }

    }
}
