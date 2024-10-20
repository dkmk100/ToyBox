using Microsoft.Xna.Framework;
using System.Text.Json.Nodes;

namespace ToyBox
{
    public class ComponentInstance
    {
        private ComponentType type;
        private ComponentData data;
        private Vector2 pos;
        private TriState previous;
        private TriState[] previousIn;

        public Vector2 GetPos()
        {
            return pos;
        }

        public ComponentInstance(ComponentType type, Vector2 pos)
        {
            this.type = type;
            this.data = type.CreateData();
            this.pos = pos;
            previous = TriState.ERROR;
        }

        public ComponentInstance(ComponentType type, ComponentData data, Vector2 pos)
        {
            this.type = type;
            this.data = data;
            this.pos = pos;
            previous = TriState.ERROR;
        }

        public JsonNode Save(ComponentsRegistry registry)
        {
            JsonNode node = type.Save(data);
            JsonObject obj = new JsonObject
            {
                { "data", node },
                { "type", registry.GetName(type) }
            };
            return obj;
        }

        public static ComponentInstance Load(ComponentsRegistry registry, JsonObject data, Vector2 pos)
        {
            if(data.TryGetPropertyValue("type", out JsonNode node))
            {
                string typeName = node.AsValue().ToString();
                ComponentType type = registry.Get(typeName);
                return new ComponentInstance(type, type.Load(data), pos);
            }

            return new ComponentInstance(null, null, Vector2.Zero);
        }

        public TriState Update(TriState[] inputs)
        {
            previousIn = inputs;

            TriState state = type.Update(data, inputs);
            previous = state;
            return state;
        }
        public TriState GetCached()
        {
            return previous;
        }
        public void OnInteract()
        {
            type.OnInteract(data);
        }
    }
}
