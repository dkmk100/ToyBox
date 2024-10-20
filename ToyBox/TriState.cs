using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToyBox
{
    public enum TriState
    {
        ERROR = 0b00,
        UNPOWERED = 0b01,
        OFF = 0b10,
        ON = 0b11
    }

    static class TriStateExtensions
    {
        public static TriState Invert(this TriState state)
        {
            switch (state)
            {
                case TriState.ON:
                    return TriState.OFF;
                case TriState.OFF:
                    return TriState.ON;
                default:
                    return state;
            }
        }
        public static bool IsValid(this TriState state)
        {
            return state != TriState.ERROR;
        }
        public static bool IsOn(this TriState state)
        {
            return state == TriState.ON;
        }
        public static bool IsOff(this TriState state)
        {
            return state == TriState.OFF;
        }
    }
}

