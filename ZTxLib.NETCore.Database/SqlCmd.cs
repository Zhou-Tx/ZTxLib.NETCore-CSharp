namespace ZTxLib.Database
{
    public class SqlCmd
    {
        public string SqlStr { get; }
        public object[] Args { get; }

        public SqlCmd(string sqlStr, params object[] args)
        {
            string[] argsName = new string[args.Length];
            for (int i = 0; i < argsName.Length; i++)
                argsName[i] = $"@param_{i}";
            SqlStr = string.Format(sqlStr, argsName);
            Args = args;
        }
    }
}