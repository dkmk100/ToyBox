using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public class ButtonComponent : ComponentType
    {
        public override ComponentData CreateData()
        {
            BasicComponentData data = new BasicComponentData();
            data.cachedValue = TriState.OFF;
            return data;
        }

        public override ComponentData Load(JsonNode obj)
        {
            return BasicComponentData.FromJson(obj);
        }

        public override void OnInteract(ComponentData component)
        {
            BasicComponentData data = component as BasicComponentData;
            data.cachedValue = data.cachedValue.Invert();
        }

        public override JsonNode Save(ComponentData instance)
        {
            return ((BasicComponentData)instance).ToJson();
        }

        public override bool TryGetTruthTable(ComponentData component, int inputCount, out TruthTable? table)
        {
            table = null;
            return false;
        }

        public override TriState Update(ComponentData component, TriState[] input)
        {
            return ((BasicComponentData)component).cachedValue;
        }
    }
}
