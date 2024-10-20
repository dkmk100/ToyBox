using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ToyBox.Components
{
    public enum GateType
    {
        NOT,
        OR,
        AND,
        XOR,
        NAND,
        NOR,
        XNOR
    }
    public class BasicGateComponent : ComponentType
    {
        GateType type;
        public BasicGateComponent(GateType type) 
        {
            this.type = type;
        }
        public override ComponentData Load(JsonNode obj)
        {
            return BasicComponentData.FromJson(obj);
        }
        public override ComponentData CreateData()
        {
            return new BasicComponentData();
        }
        public override JsonNode Save(ComponentData instance)
        {
            return ((BasicComponentData)instance).ToJson();
        }

        public override bool TryGetTruthTable(ComponentData component, int inputCount, out TruthTable? table)
        {
            table = new TruthTable(inputCount);

            for(int i=0; i < TruthTable.GetPackedCount(inputCount); i++)
            {
                TriState[] vals = TruthTable.UnpackValues(i, inputCount);
                table.Value.SetValue(vals, GetValue(vals, this.type));
            }

            table.Value.Lock();

            return true;
        }

        TriState GetValue(TriState[] input, GateType type)
        {
            TriState val;
            switch(type)
            {
                case GateType.NOT:
                    val = TriState.UNPOWERED;
                    foreach (TriState state in input)
                    {
                        if(state.IsOn() || state.IsOff())
                        {
                            if(val == TriState.UNPOWERED)
                            {
                                val = state;
                            }
                            else
                            {
                                return TriState.ERROR;
                            }
                        }
                        else if (!state.IsValid())
                        {
                            return TriState.ERROR;
                        }
                    }
                    return val.Invert();
                case GateType.AND:
                    val = TriState.UNPOWERED;
                    foreach(TriState state in input)
                    {
                        if (!state.IsValid())
                        {
                            return TriState.ERROR;
                        }
                        else if (state.IsOff())
                        {
                            return TriState.OFF;
                        }
                        else if (state.IsOn())
                        {
                            val = TriState.ON;
                        }
                    }
                    return val;
                case GateType.OR:
                    val = TriState.UNPOWERED;
                    foreach (TriState state in input)
                    {
                        if (!state.IsValid())
                        {
                            return TriState.ERROR;
                        }
                        else if (state.IsOn())
                        {
                            return TriState.ON;
                        }
                        else if (state.IsOff())
                        {
                            val = TriState.OFF;
                        }
                    }
                    return val;
                case GateType.XOR:
                    bool hasInput = true;
                    int count = 0;
                    foreach (TriState state in input)
                    {
                        if (!state.IsValid())
                        {
                            return TriState.ERROR;
                        }
                        else
                        {
                            if (state.IsOn())
                            {
                                hasInput = true;
                                count++;
                            }
                            else if (state.IsOff())
                            {
                                hasInput = true;
                            }
                        }
                    }
                    if(!hasInput)
                    {
                        return TriState.UNPOWERED;
                    }
                    return count % 2 == 0 ? TriState.OFF : TriState.ON;
                default:
                    return TriState.ERROR;
            }
        }

        public override int GetOutputCount()
        {
            return 1;
        }
        public override TriState[] Update(ComponentData component, TriState[] input)
        {
            return new TriState[]{GetValue(input, this.type)};
        }

        public override void OnInteract(ComponentData component)
        {
            //nothing to do here
        }

        public override bool TrySetState(ComponentData component, TriState state)
        {
            return false;
        }
    }
}
