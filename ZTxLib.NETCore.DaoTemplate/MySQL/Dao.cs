using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.DaoTemplate.MySQL
{
    public class Dao
    {
        private readonly DataSource _dataSource;
        private MySqlCommand _cmd;

        public Dao(DataSource dataSource) => _dataSource = dataSource;

        internal MySqlDataReader ExecuteReader() =>
            _cmd.ExecuteReader();

        internal int ExecuteNonQuery() =>
            _cmd.ExecuteNonQuery();

        public void Prepare(string sql,
            IEnumerable<KvPair> parameter = null,
            IEnumerable<KvPair> concat = null
        )
        {
            var concatArray = (concat == null)
                ? new KvPair[0]
                : concat.ToArray();
            var parameterArray = (parameter == null)
                ? new KvPair[0]
                : parameter.ToArray();

            sql = concatArray.Aggregate(sql,
                (current, pair) =>
                    current.Replace($"${{{pair.Key}}}", pair.Value));

            sql = parameterArray.Aggregate(sql,
                (current, pair) =>
                    current.Replace($"#{{{pair.Key}}}", $"@{pair.Key}"));

            var cmd = new MySqlCommand(sql, _dataSource.GetConnection());

            foreach (var sqlParameter in parameterArray)
                cmd.Parameters.Add(new MySqlParameter(
                    $"@{sqlParameter.Key}", sqlParameter.Value
                ));

            _cmd = cmd;
        }

        internal void Close() => _dataSource.Close();
    }
}