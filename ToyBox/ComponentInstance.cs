﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox
{
    //can be a struct because it only stores reference types
    public struct ComponentInstance
    {
        private ComponentType type;
        private ComponentData data;

        public ComponentInstance(ComponentType type, ComponentData data)
        {
            this.type = type;
            this.data = data;
        }

        public JsonObject Save(ComponentsRegistry registry)
        {
            JsonObject obj = type.Save(data);
            obj.Add("type", registry.GetName(type));
            return obj;
        }

        public static ComponentInstance Load(ComponentsRegistry registry, JsonObject data)
        {
            if(data.TryGetPropertyValue("type", out JsonNode node))
            {
                string typeName = node.AsValue().ToString();
                ComponentType type = registry.Get(typeName);
                return new ComponentInstance(type, type.Load(data));
            }

            return new ComponentInstance(null, null);
        }

        public TriState Update(TriState[] inputs)
        {
            return type.Update(data, inputs);
        }
    }
}
