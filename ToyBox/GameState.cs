using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ToyBox
{
    public class GameState
    {

        public JsonNode ToJson(ComponentsRegistry registry)
        {
            JsonArray arr = new JsonArray();

            foreach(var item in components)
            {
                JsonObject obj = new JsonObject();
                obj.Add("component", item.Item1.Save(registry));
                obj.Add("inputs", JsonSerializer.Serialize(item.Item2.ToArray()));
                obj.Add("outputs", JsonSerializer.Serialize(item.Item3.ToArray()));
                arr.Add(obj);
            }

            return arr;
        }
        public static GameState FromJson(ComponentsRegistry registry, JsonNode node)
        {
            GameState state = new GameState();

            JsonArray arr = node.AsArray();
            foreach(var item in arr)
            {
                JsonObject obj = item.AsObject();
                ComponentInstance component = ComponentInstance.Load(registry, obj["component"]);
                List<int> inputs = new List<int>(obj["inputs"].AsArray().GetValues<int>());
                List<int> outputs = new List<int>(obj["outputs"].AsArray().GetValues<int>());
                state.components.Add((component, inputs, outputs));
            }

            return state;
        }
        public GameState Clone()
        {
            GameState state = new GameState();
            foreach(var item in components)
            {
                state.components.Add((item.Item1.Clone(), item.Item2.ToArray().ToList(), item.Item3.ToArray().ToList()));
            }
            return state;
        }
        
        List<(ComponentInstance, List<int>, List<int>)> components = new List<(ComponentInstance, List<int>, List<int>)>();

        public List<ComponentInstance> GetComponents()
        {
            List<ComponentInstance> ls = new List<ComponentInstance>();
            foreach(var item in components)
            {
                ls.Add(item.Item1);
            }
            return ls;
        }

        public TriState GetValue(int index)
        {
            return components[index].Item1.GetCached();
        }

        public int GetComponent(Vector2 pos, float range)
        {
            for(int i = 0; i < components.Count; i++)
            {
                ComponentInstance component = components[i].Item1;
                if ((component.GetPos()+pos).LengthSquared() < range*range)
                {
                    return i;
                }
            }
            return -1;//not found
        }

        public int AddComponent(ComponentType type, Vector2 pos)
        {
            ComponentInstance component = new ComponentInstance(type, pos);
            components.Add((component, new List<int>(), new List<int>()));
            UpdateComponent(components.Count - 1);//initialize component
            return components.Count - 1;
        }

        public void AddConnection(int item1, int item2)
        {
            //list of inputs is before list of outputs
            components[item2].Item2.Add(item1);
            components[item1].Item3.Add(item2);
            UpdateComponent(item2);//update component that has connection
        }

        public void ToggleComponent(int id)
        {
            components[id].Item1.OnInteract();
            UpdateComponent(id);
        }

        public void UpdateComponent(int id)
        {
            TriState[] states = new TriState[components[id].Item2.Count];
            int i = 0;
            foreach(int input in components[id].Item2)
            {
                states[i] = components[input].Item1.GetCached();
                i++;
            }

            TriState state = components[id].Item1.Update(states);
            Debug.WriteLine("updated: " + id + ", new state: " + state);
            foreach(int next in components[id].Item3)
            {
                UpdateComponent(next);
            }
        }


        //TODO: method to create a new component at a position
        //TODO: method to add connection between component
        //TODO: method to interact with component (ex. buttons)
    }
}
