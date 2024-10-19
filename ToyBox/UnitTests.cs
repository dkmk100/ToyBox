using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ToyBox
{
    internal class UnitTests
    {
        public void RunTests(ComponentsRegistry registry)
        {
            Debug.WriteLine("running unit tests!");
            RunBasicTest(registry);
        }

        private void RunBasicTest(ComponentsRegistry registry)
        {
            string[] components = { "not", "and", "or", "xor" };

            foreach(var component in components)
            {
                ComponentInstance inst = new ComponentInstance(registry.Get(component));
                TestComponent(inst, component);
            }
        }

        private void TestComponent(in ComponentInstance instance, string name)
        {
            for (int i = 1; i < 4; i++)
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
