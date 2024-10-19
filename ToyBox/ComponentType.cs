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
        public abstract JsonObject Save(ComponentInstance instance);

        public abstract ComponentInstance Load(JsonObject obj);

        public abstract TriState Activate(ComponentInstance component, TriState[] input);
    }
}
