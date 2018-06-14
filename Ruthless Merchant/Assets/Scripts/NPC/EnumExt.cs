//---------------------------------------------------------------
// Author: Marcel Croonenbroeck
//
//---------------------------------------------------------------

using System;

namespace RuthlessMerchant
{
    public static class EnumExt
    {
        /// <summary>
        /// Check to see if a flags enumeration has a specific flag set.
        /// </summary>
        /// <param name="variable">Flags enumeration to check</param>
        /// <param name="value">Flag to check for</param>
        /// <returns>Returns true when the variable contains the value</returns>
        /// Written by: chilltemp at https://stackoverflow.com/questions/4108828/generic-extension-method-to-see-if-an-enum-contains-a-flag
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

        /// <summary>
        /// Adds a flag to a enum variable
        /// </summary>
        /// <param name="variable">Variable where the given flag should be added</param>
        /// <param name="value">Value which should be added to the variable</param>
        /// <returns>Returns the modified variable</returns>
        public static NPC.TargetState SetFlag(this NPC.TargetState variable, NPC.TargetState value)
        {
            return variable | value;
        }

        /// <summary>
        /// Removes a flag from a enum variable
        /// </summary>
        /// <param name="variable">Variable where the given flag should be removed from</param>
        /// <param name="value">Value which should be removed from the variable</param>
        /// <returns>Returns the modified variable</returns>
        public static NPC.TargetState RemoveFlag(this NPC.TargetState variable, NPC.TargetState value)
        {
            return variable & ~value;
        }
    }
}
