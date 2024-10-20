using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public class BasicComponentData : ComponentData
    {
        public TriState cachedValue;

        public static BasicComponentData FromJson(JsonNode node)
        {
            return (BasicComponentData)JsonSerializer.Deserialize(node, typeof(BasicComponentData));
        }
        public BasicComponentData()
        {
            cachedValue = TriState.ERROR;
        }
        public JsonNode ToJson()
        {
            return JsonSerializer.SerializeToNode(this);
        }
    }
}
