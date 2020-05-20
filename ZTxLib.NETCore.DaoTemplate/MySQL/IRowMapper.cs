using MySql.Data.MySqlClient;

namespace ZTxLib.NETCore.DaoTemplate.MySQL
{
    public interface IRowMapper<out T>
    {
        public T MapRow(MySqlDataReader reader, int index);
    }
}