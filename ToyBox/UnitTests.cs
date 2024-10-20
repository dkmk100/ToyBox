using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.Intrinsics;
namespace ToyBox
{
    internal class UnitTests
    {
        public void RunTests(ComponentsRegistry registry)
        {
            Debug.WriteLine("running unit tests!");
            //RunBasicTests(registry);
            RunAdvancedTests(registry);
        }

        private void RunBasicTests(ComponentsRegistry registry)
        {
            string[] components = { "not", "and", "or", "xor" };

            foreach(var component in components)
            {
                ComponentInstance inst = new ComponentInstance(registry.Get(component), Vector2.Zero);
                TestComponent(inst, component);
            }
        }

        private void RunAdvancedTests(ComponentsRegistry registry)
        {
            GameState state = new GameState();

            int b1 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int b2 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int xor = state.AddComponent(registry.Get("xor"), Vector2.Zero);
            int and = state.AddComponent(registry.Get("and"), Vector2.Zero);

            state.AddConnection(b1, xor);
            state.AddConnection(b2, xor);
            state.AddConnection(b1, and);
            state.AddConnection(b2, and);

            Debug.WriteLine("adder b1: " + state.GetValue(b1));
            Debug.WriteLine("adder b2: " + state.GetValue(b2));
            Debug.WriteLine("adder xor: " + state.GetValue(xor));
            Debug.WriteLine("adder and: " + state.GetValue(and));

            state.ToggleComponent(b1);
            Debug.WriteLine("toggled b1");

            Debug.WriteLine("adder b1: " + state.GetValue(b1));
            Debug.WriteLine("adder b2: " + state.GetValue(b2));
            Debug.WriteLine("adder xor: " + state.GetValue(xor));
            Debug.WriteLine("adder and: " + state.GetValue(and));

            state.ToggleComponent(b2);
            Debug.WriteLine("toggled b2");

            Debug.WriteLine("adder b1: " + state.GetValue(b1));
            Debug.WriteLine("adder b2: " + state.GetValue(b2));
            Debug.WriteLine("adder xor: " + state.GetValue(xor));
            Debug.WriteLine("adder and: " + state.GetValue(and));

            state.ToggleComponent(b1);
            Debug.WriteLine("toggled b1");

            Debug.WriteLine("adder b1: " + state.GetValue(b1));
            Debug.WriteLine("adder b2: " + state.GetValue(b2));
            Debug.WriteLine("adder xor: " + state.GetValue(xor));
            Debug.WriteLine("adder and: " + state.GetValue(and));

            Debug.WriteLine(state.ToJson(registry).ToString());
        }


        private void TestComponent(in ComponentInstance instance, string name)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < TruthTable.GetPackedCount(i); j++)
                {
                    TriState[] input = TruthTable.UnpackValues(j, i);
                    TriState val = instance.Update(input);
                    Debug.Write(name);
                    Debug.Write(": (");
                    foreach (var v in input)
                    {
                        Debug.Write(v.ToString());
                        Debug.Write(", ");
                    }
                    Debug.Write(") -> ");
                    Debug.WriteLine(val.ToString());
                }
            }
        }
    }
}
