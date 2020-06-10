using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class Property
    {
        public string Key { get; set; }

        public PropertyType Type { get; set; }

        public string Label { get; set; }

        public string DefaultValue { get; set; }

        public bool IsEditable { get; set; }

        public string ErrorMessage { get; set; }

        public Property()
        {
            IsEditable = true;
        }

        public Property(string key, PropertyType type, string label)
        {
            Key = key;
            Type = type;
            Label = label;
            IsEditable = true;
        }
    }
}
