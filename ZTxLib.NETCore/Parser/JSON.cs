using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ZTxLib.NETCore.Parser
{
    public class JSON
    {
        private readonly JToken json;

        private JSON(JToken json) => this.json = json;

        public JSON(string json) => this.json = (JToken)JsonConvert.DeserializeObject(json);

        public override string ToString() => json.ToString();

        public JSON this[string key]
        {
            get
            {
                JToken jToken = json[key];
                if (jToken == null)
                    throw new NullReferenceException($"不存在该键值");
                return new JSON(jToken);
            }
        }

        public JSON this[int index]
        {
            get
            {
                JToken jToken = json[index];
                if (jToken == null)
                    throw new NullReferenceException($"不存在该对象");
                return new JSON(jToken);
            }
        }
    }
}
