using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ToyBox
{
    public class ComponentInstance
    {
        private ComponentType type;
        private ComponentData data;
        private Vector2 pos;
        private TriState[] previous;
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
            previous = new TriState[type.GetOutputCount()];
        }

        public ComponentInstance(ComponentType type, ComponentData data, Vector2 pos)
        {
            this.type = type;
            this.data = data;
            this.pos = pos;
            previous = new TriState[type.GetOutputCount()];
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

        public int GetOutputCount()
        {
            return type.GetOutputCount();
        }
        public TriState[] Update(TriState[] inputs)
        {
            previousIn = inputs;

            TriState[] states = type.Update(data, inputs);
            previous = states;
            return states;
        }
        public TriState GetCached(int id)
        {
            return previous[id];
        }
        public void OnInteract()
        {
            type.OnInteract(data);
        }

        public bool TrySet(TriState state)
        {
            return type.TrySetState(data, state);
        }

        public void Render(SpriteBatch batch, SpritesManager sprites, ComponentsRegistry registry)
        {
            type.Render(batch, data, pos, sprites, registry);
        }
    }
}
