namespace ZTxLib.NETCore.DaoTemplate
{
    public readonly struct KvPair
    {
        public string Key { get; }
        public string Value { get; }

        public KvPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}