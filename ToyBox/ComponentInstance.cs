using Microsoft.Xna.Framework;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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

        public ComponentInstance Clone()
        {
            ComponentInstance inst = new ComponentInstance(type, data, pos);
            inst.previous = previous;
            inst.previousIn = new TriState[previousIn.Length];
            previousIn.CopyTo(inst.previousIn, 0);
            return inst;
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
                { "type", registry.GetName(type) },
                { "x", pos.X },
                { "y", pos.Y }
            };
            return obj;
        }

        public static ComponentInstance Load(ComponentsRegistry registry, JsonNode data)
        {

            string typeName = data["type"].ToString();
            ComponentType type = registry.Get(typeName);
            Vector2 pos = new Vector2(data["x"].GetValue<int>(), data["y"].GetValue<int>());

            return new ComponentInstance(type, type.Load(data), pos);
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
