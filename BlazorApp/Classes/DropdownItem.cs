using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public struct DropdownItem
    {
        public string Key { get; set; }
        public DropdownType Type { get; set; }
        public string Value { get; set; }

        public DropdownItem(string Key,string Value, DropdownType Type)
        {
            this.Key = Key;
            this.Value = Value;
            this.Type = Type;
        }
    }
}
