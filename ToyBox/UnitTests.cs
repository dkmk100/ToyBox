using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using ToyBox.Components;
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
            //TestHalfAdder(registry);
            TestFullAdder(registry);
        }

        private void TestHalfAdder(ComponentsRegistry registry)
        {
            GameState state = new GameState();

            int b1 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int b2 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int xor = state.AddComponent(registry.Get("xor"), Vector2.Zero);
            int and = state.AddComponent(registry.Get("and"), Vector2.Zero);

            state.AddConnection(b1, 0, xor);
            state.AddConnection(b2, 0, xor);
            state.AddConnection(b1, 0, and);
            state.AddConnection(b2, 0, and);

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

        private void TestFullAdder(ComponentsRegistry registry)
        {
            //unit tests shouldn't break stuff
            registry = registry.Clone();

            GameState state = new GameState();

            int b1 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int b2 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int xor = state.AddComponent(registry.Get("xor"), Vector2.Zero);
            int and = state.AddComponent(registry.Get("and"), Vector2.Zero);

            state.AddConnection(b1, 0, xor);
            state.AddConnection(b2, 0, xor);
            state.AddConnection(b1, 0, and);
            state.AddConnection(b2, 0, and);

            Debug.WriteLine("Half Adder Game State: ");
            Debug.WriteLine(state.ToJson(registry));
            CompoundComponent halfAdder = new CompoundComponent(state.Clone(), [b1, b2], [(xor, 0),(and, 0)], registry);
            registry.Register(halfAdder, "halfAdder");


            
            state.Clear();
            int i1 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int i2 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int i3 = state.AddComponent(registry.Get("button"), Vector2.Zero);
            int h1 = state.AddComponent(registry.Get("halfAdder"),Vector2.Zero);
            int h2 = state.AddComponent(registry.Get("halfAdder"), Vector2.Zero);
            int or = state.AddComponent(registry.Get("or"), Vector2.Zero);

            state.AddConnection(i1, 0, h1);
            state.AddConnection(i2, 0, h1);
            state.AddConnection(i3, 0, h2);
            state.AddConnection(h1, 0, h2);
            state.AddConnection(h1, 1, or);
            state.AddConnection(h2, 1, or);

            //Debug.WriteLine("Full Adder Game State: ");
            //Debug.WriteLine(state.ToJson(registry));

            Debug.WriteLine("adder i1: " + state.GetValue(i1));
            Debug.WriteLine("adder i2: " + state.GetValue(i2));
            Debug.WriteLine("adder i3: " + state.GetValue(i3));
            Debug.WriteLine("adder h1: " + state.GetValue(h1, 0) + ", " + state.GetValue(h1, 1));
            Debug.WriteLine("adder h2: " + state.GetValue(h2, 0) + ", " + state.GetValue(h2, 1));
            Debug.WriteLine("adder sum: " + state.GetValue(h2));
            Debug.WriteLine("adder carry: " + state.GetValue(or));

            state.ToggleComponent(i1);
            Debug.WriteLine("toggled i1");

            Debug.WriteLine("adder i1: " + state.GetValue(i1));
            Debug.WriteLine("adder i2: " + state.GetValue(i2));
            Debug.WriteLine("adder i3: " + state.GetValue(i3));
            Debug.WriteLine("adder h1: " + state.GetValue(h1, 0) + ", " + state.GetValue(h1, 1));
            Debug.WriteLine("adder h2: " + state.GetValue(h2, 0) + ", " + state.GetValue(h2, 1));
            Debug.WriteLine("adder sum: " + state.GetValue(h2));
            Debug.WriteLine("adder carry: " + state.GetValue(or));

            state.ToggleComponent(i3);
            Debug.WriteLine("toggled i3");

            Debug.WriteLine("adder i1: " + state.GetValue(i1));
            Debug.WriteLine("adder i2: " + state.GetValue(i2));
            Debug.WriteLine("adder i3: " + state.GetValue(i3));
            Debug.WriteLine("adder h1: " + state.GetValue(h1, 0) + ", " + state.GetValue(h1, 1));
            Debug.WriteLine("adder h2: " + state.GetValue(h2, 0) + ", " + state.GetValue(h2, 1));
            Debug.WriteLine("adder sum: " + state.GetValue(h2));
            Debug.WriteLine("adder carry: " + state.GetValue(or));

            state.ToggleComponent(i1);
            Debug.WriteLine("toggled i1");
            state.ToggleComponent(i2);
            Debug.WriteLine("toggled i2");

            Debug.WriteLine("adder i1: " + state.GetValue(i1));
            Debug.WriteLine("adder i2: " + state.GetValue(i2));
            Debug.WriteLine("adder i3: " + state.GetValue(i3));
            Debug.WriteLine("adder h1: " + state.GetValue(h1, 0) + ", " + state.GetValue(h1, 1));
            Debug.WriteLine("adder h2: " + state.GetValue(h2, 0) + ", " + state.GetValue(h2, 1));
            Debug.WriteLine("adder sum: " + state.GetValue(h2));
            Debug.WriteLine("adder carry: " + state.GetValue(or));

            state.ToggleComponent(i1);
            Debug.WriteLine("toggled i1");

            Debug.WriteLine("adder i1: " + state.GetValue(i1));
            Debug.WriteLine("adder i2: " + state.GetValue(i2));
            Debug.WriteLine("adder i3: " + state.GetValue(i3));
            Debug.WriteLine("adder h1: " + state.GetValue(h1, 0) + ", " + state.GetValue(h1, 1));
            Debug.WriteLine("adder h2: " + state.GetValue(h2, 0) + ", " + state.GetValue(h2, 1));
            Debug.WriteLine("adder sum: " + state.GetValue(h2));
            Debug.WriteLine("adder carry: " + state.GetValue(or));
        }

            private void TestComponent(in ComponentInstance instance, string name)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < TruthTable.GetPackedCount(i); j++)
                {
                    TriState[] input = TruthTable.UnpackValues(j, i);
                    TriState[] vals = instance.Update(input);

                    Debug.Write(name);
                    Debug.Write(": (");
                    foreach (var v in input)
                    {
                        Debug.Write(v.ToString());
                        Debug.Write(", ");
                    }
                    Debug.Write(") -> ");

                    Debug.Write("[");
                    foreach (var v in vals)
                    {
                        Debug.Write(v.ToString());
                        Debug.Write(", ");
                    }
                    Debug.Write("]");
                }
            }
        }
    }
}
