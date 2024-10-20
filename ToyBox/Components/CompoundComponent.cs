using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public class CompoundComponent : ComponentType
    {
        GameState template;
        ComponentsRegistry registry;
        int[] inputs;
        (int,int)[] outputs;

        public CompoundComponent(GameState template, int[] inputs, (int,int)[] outputs, ComponentsRegistry registry)
        {
            this.template = template;
            this.inputs = inputs;
            this.outputs = outputs;
            this.registry = registry;
        }
        public override ComponentData CreateData()
        {
            return new CompoundComponentData(template.Clone());
        }

        public override ComponentData Load(JsonNode obj)
        {
            return CompoundComponentData.FromJson(registry, obj);
        }

        public override void OnInteract(ComponentData component)
        {
            //nothing to do here
        }

        public override JsonNode Save(ComponentData instance)
        {
            CompoundComponentData data = instance as CompoundComponentData;

            return data.ToJson(registry);
        }

        public override bool TryGetTruthTable(ComponentData component, int inputCount, out TruthTable? table)
        {
            table = null;
            return false;
        }
        public override bool TrySetState(ComponentData component, TriState state)
        {
            return false;
        }
        public override int GetOutputCount()
        {
            return outputs.Count();
        }
        public override TriState[] Update(ComponentData component, TriState[] input)
        {
            if (input.Length != inputs.Length)
            {
                TriState[] old = input;
                input = new TriState[inputs.Length];
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (j < old.Length)
                    {
                        input[j] = old[j];
                    }
                    else
                    {
                        input[j] = TriState.UNPOWERED;
                    }
                }
            }

            Console.Write("compound input: [");
            foreach (var id in inputs)
            {
                Console.Write(id);
            }
            Console.WriteLine("]");

            CompoundComponentData data = component as CompoundComponentData;
            int i = 0;
            foreach(var id in inputs)
            {
                data.internalState.TrySetValue(id, input[i]);
                i++;
            }


            TriState[] outs = new TriState[outputs.Length];
            i = 0;
            foreach(var id in outputs)
            {
                outs[i] = data.internalState.GetValue(id.Item1, id.Item2);
                i++;
            }

            Console.Write("compound output: [");
            foreach (var id in outs)
            {
                Console.Write(id);
            }
            Console.WriteLine("]");

            return outs;
        }

        public override void Render(SpriteBatch batch, ComponentData component, Vector2 pos, SpritesManager sprites, ComponentsRegistry registry)
        {
            sprites.Render(batch, "cpu", pos, 1.5f);
        }
    }

}
