using System.Collections.Generic;
using System.Data.SqlClient;

namespace ZTxLib.NETCore.Database
{
    public class SqlDesigner
    {
        /// <summary>
        /// 参数名集合
        /// </summary>
        public string[] ParameterCollection { get; private set; }
        private readonly Dictionary<string, SqlParameter> ParameterPairCollection = new Dictionary<string, SqlParameter>();
        public string SqlStr { get; }

        /// <summary>
        /// 生成含参数SQL语句
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="argsName"></param>
        public SqlDesigner(string sqlStr, params string[] argsName)
        {
            ParameterCollection = argsName;
            for (int i = 0; i < argsName.Length; i++)
            {
                ParameterPairCollection.Add(argsName[i], new SqlParameter(argsName[i], ""));
                argsName[i] = '@' + argsName[i];
            }
            SqlStr = string.Format(sqlStr, argsName);
        }

        public string this[string argsName]
        {
            get => (string)ParameterPairCollection[argsName].Value;
            set => ParameterPairCollection[argsName] = new SqlParameter(argsName, value);
        }

        public Dictionary<string, SqlParameter>.ValueCollection Parameters => ParameterPairCollection.Values;
    }
}
