using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyBox
{
    public class ComponentsRegistry
    {
        Dictionary<string, ComponentType> types = new Dictionary<string, ComponentType>();
        Dictionary<ComponentType, string> names = new Dictionary<ComponentType, string>();

        public IEnumerable<string> GetNames()
        {
            return names.Values;
        }

        public ComponentType Get(string name)
        {
            return types.GetValueOrDefault(name);
        }
        public string GetName(ComponentType type)
        {
            return names.GetValueOrDefault(type);
        }

        public void RegisterBuiltin(ComponentType type, string name)
        {
            types.Add(name, type);
            names.Add(type, name);
        }

        public void Register(ComponentType type, string name)
        {
            types.Add(name, type);
            names.Add(type, name);
        }

        public ComponentsRegistry Clone()
        {
            ComponentsRegistry registry = new ComponentsRegistry();
            foreach (var val in types)
            {
                registry.types.Add(val.Key, val.Value);
                registry.names.Add(val.Value, val.Key);

            }
            return registry;
        }
    }
}
