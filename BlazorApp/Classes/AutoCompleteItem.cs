using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// Represent AutoComplete Item object.
    /// </summary>
    public class AutoCompleteItem
    {
        /// <summary>
        /// AutoComplete Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// AutoComplete Value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// To store additional values in AutoComplete object.
        /// </summary>
        public List<string> AdditionalParameters { get; set; }

        public AutoCompleteItem()
        {
            AdditionalParameters = new List<string>();
        }

        public AutoCompleteItem(string Key, string Value, params string[] AdditionalParameters)
        {
            this.Key = Key;
            this.Value = Value;

            this.AdditionalParameters = new List<string>();
            this.AdditionalParameters.AddRange(AdditionalParameters);
        }

    }
}
