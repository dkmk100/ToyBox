using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox
{
    public abstract class ComponentType
    {
        public abstract JsonNode Save(ComponentData instance);

        public abstract ComponentData Load(JsonNode obj);
        public abstract ComponentData CreateData();

        public abstract int GetOutputCount();
        public abstract TriState[] Update(ComponentData component, TriState[] input);

        public abstract bool TrySetState(ComponentData component, TriState state);

        public abstract void OnInteract(ComponentData component);
        public abstract bool TryGetTruthTable(ComponentData component, int inputCount, out TruthTable? table);

        public abstract void Render(SpriteBatch batch, ComponentData component, Vector2 pos, SpritesManager sprites, ComponentsRegistry registry);
    }
}
