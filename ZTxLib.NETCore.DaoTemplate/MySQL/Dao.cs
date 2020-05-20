using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.DaoTemplate.MySQL
{
    public class Dao
    {
        private readonly DataSource _dataSource;

        public Dao(DataSource dataSource) => _dataSource = dataSource;

        public MySqlDataReader ExecuteReader(
            string sql,
            IEnumerable<KvPair> parameter = null,
            IEnumerable<KvPair> concat = null) =>
            Prepare(sql, parameter, concat).ExecuteReader();

        public int ExecuteNonQuery(
            string sql,
            IEnumerable<KvPair> parameter = null,
            IEnumerable<KvPair> concat = null) =>
            Prepare(sql, parameter, concat).ExecuteNonQuery();

        private MySqlCommand Prepare(string sql,
            IEnumerable<KvPair> parameter,
            IEnumerable<KvPair> concat
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

            return cmd;
        }
    }
}