using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox
{
    public class ComponentInstance
    {
        private ComponentType type;

        public JsonObject Save(ComponentsRegistry registry)
        {
            JsonObject obj = type.Save(this);
            obj.Add("type", registry.GetName(type));
            return obj;
        }

        public static ComponentInstance Load(ComponentsRegistry registry, JsonObject data)
        {
            if(data.TryGetPropertyValue("type", out JsonNode node))
            {
                string type = node.AsValue().ToString();
                return registry.Get(type).Load(data);
            }

            return null;
        }

        public TriState Activate(TriState[] inputs)
        {
            return type.Activate(this, inputs);
        }
    }
}
