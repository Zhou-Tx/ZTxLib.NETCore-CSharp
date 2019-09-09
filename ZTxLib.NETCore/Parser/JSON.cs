using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ZTxLib.NETCore.Parser
{
    public class JSON
    {
        private readonly string key;
        private readonly JToken json;

        private JSON(JToken json, string key)
        {
            this.json = json;
            this.key = key;
        }

        public JSON(string json) => this.json = (JToken)JsonConvert.DeserializeObject(json);

        public JSON this[string key] => new JSON(json[key], key);

        public override string ToString()
        {
            try
            {
                return json.ToString();
            }
            catch (NullReferenceException ex)
            {
                string message = $"不存在键\"{key}\"";
                throw new NullReferenceException(message, ex.InnerException);
            }
        }
    }
}
