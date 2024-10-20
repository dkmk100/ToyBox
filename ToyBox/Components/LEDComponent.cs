using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public class LEDComponent : ComponentType
    {
        public override ComponentData CreateData()
        {
            BasicComponentData data = new BasicComponentData();
            data.cachedValue = TriState.OFF;
            return data;
        }

        public override void OnInteract(ComponentData component)
        {
        }

        public override ComponentData Load(JsonNode obj)
        {
            return BasicComponentData.FromJson(obj);
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

        public override int GetOutputCount()
        {
            return 1;
        }
        public override TriState[] Update(ComponentData component, TriState[] input)
        {
            var data = component as BasicComponentData;
            if (input.Length == 0)
            {
                data.cachedValue = TriState.UNPOWERED;
            }
            else {
                //TODO calculate input state correctly
                data.cachedValue = input[0];
            }
            return new TriState[]{ data.cachedValue};
        }

        public override bool TrySetState(ComponentData component, TriState state)
        {
            ((BasicComponentData)component).cachedValue = state;
            return true;
        }

        public override void Render(SpriteBatch batch, ComponentData component, Vector2 pos, SpritesManager sprites, ComponentsRegistry registry)
        {
            BasicComponentData data = component as BasicComponentData;
            string name; 
            if (data.cachedValue.IsOn())
            {
                name = "light_yellow_on";
            }
            else
            {
                name = "light_off";
            }

            sprites.Render(batch, name, pos, 1.5f);
        }
    }
}
