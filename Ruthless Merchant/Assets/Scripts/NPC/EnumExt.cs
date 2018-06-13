﻿using System;

namespace RuthlessMerchant
{
    public static class EnumExt
    {
        /// <summary>
        /// Check to see if a flags enumeration has a specific flag set.
        /// </summary>
        /// <param name="variable">Flags enumeration to check</param>
        /// <param name="value">Flag to check for</param>
        /// <returns></returns>
        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);

        }

        public static NPC.TargetState SetFlag(this NPC.TargetState variable, NPC.TargetState value)
        {
            return variable | value;
        }

        public static NPC.TargetState RemoveFlag(this NPC.TargetState variable, NPC.TargetState value)
        {
            return variable & ~value;
        }

        public static NPC.TargetState Clear(this NPC.TargetState variable)
        {
            return NPC.TargetState.None;
        }
    }
}
