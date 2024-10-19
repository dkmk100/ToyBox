using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ToyBox
{
    public struct TruthTable
    {
        int inputCount;
        TriState[] values;
        bool locked;
        public TruthTable() : this(2)
        {

        }
        public TruthTable(int inputCount)
        {
            this.inputCount = inputCount;

            int valueCount = 1;
            for(int i=0;i<inputCount;i++)
            {
                valueCount *= 4;//for easier indexing
            }
            values = new TriState[valueCount];
            locked = false;
        }
        
        public TriState GetValue(TriState[] input)
        {
            return values[GetId(input)];
        }
        public void SetAllValues(TriState state)
        {
            if (locked)
            {
                throw new InvalidOperationException("cannot modify finished truth table");
            }
            for(int i=0;i<values.Length; i++)
            {
                values[i] = state;
            }
        }
        public void SetValue(TriState[] input, TriState value) 
        {
            if (locked)
            {
                throw new InvalidOperationException("cannot modify finished truth table");
            }
            values[GetId(input)] = value;
        }
        public void Lock()
        {
            if (locked)
            {
                throw new InvalidOperationException("cannot modify finished truth table");
            }
            locked = true;
        }

        int GetId(TriState[] input)
        {
            if (input.Length != inputCount)
            {
                throw new InvalidOperationException("input array does not match truth table");
            }

            int id = 0;
            foreach (TriState state in input)
            {
                id = id << 2;//tristate returns number from 0 to 3
                id += (int)state;//add tristate
            }
            return id;
        }
    }
}
