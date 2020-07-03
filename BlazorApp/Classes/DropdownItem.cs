using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    ///  Represent DropdownItem Item object.
    /// </summary>
    public struct DropdownItem
    {
        /// <summary>
        /// DropdownItem Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// DropdownItem Type.
        /// </summary>
        public DropdownType Type { get; set; }

        /// <summary>
        /// DropdownItem Value.
        /// </summary>
        public string Value { get; set; }

        public DropdownItem(string Key, string Value, DropdownType Type)
        {
            this.Key = Key;
            this.Value = Value;
            this.Type = Type;
        }
    }
}
