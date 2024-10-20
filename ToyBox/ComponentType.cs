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

        public abstract TriState Update(ComponentData component, TriState[] input);

        public abstract bool TryGetTruthTable(ComponentData component, int inputCount, out TruthTable table);
    }
}
