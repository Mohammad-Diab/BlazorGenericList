using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// Represent Grid Property object.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Property Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Property Type.
        /// </summary>
        public PropertyType Type { get; set; }

        /// <summary>
        /// Text shown over current Property field.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Represent the initial value of current Property, usually used in 'Edit' modal.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Is current Property editable.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Used for validation error messages. 
        /// </summary>
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
