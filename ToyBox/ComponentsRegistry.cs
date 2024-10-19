using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyBox
{
    public class ComponentsRegistry
    {
        Dictionary<string, ComponentType> types;
        Dictionary<ComponentType, string> names;

        public ComponentType Get(string name)
        {
            return types.GetValueOrDefault(name);
        }
        public string GetName(ComponentType type)
        {
            return names.GetValueOrDefault(type);
        }

        public void Register(ComponentType type, string name)
        {
            types.Add(name, type);
            names.Add(type, name);
        }
    }
}
