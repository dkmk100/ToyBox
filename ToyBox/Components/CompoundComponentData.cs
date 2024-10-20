using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public class CompoundComponentData : ComponentData
    {
        public GameState internalState;

        public static CompoundComponentData FromJson(ComponentsRegistry registry, JsonNode node)
        {
            return new CompoundComponentData(GameState.FromJson(registry, node));
        }
        public CompoundComponentData(GameState state)
        {
            internalState = state;
        }
        public CompoundComponentData()
        {
            internalState = new GameState();
        }
        public JsonNode ToJson(ComponentsRegistry registry)
        {
            return internalState.ToJson(registry);
        }
    }
}
