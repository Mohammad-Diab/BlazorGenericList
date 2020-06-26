using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class AutoCompleteItem
    {
        public string Key { get; set; }

        public string Value { get; set; }

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
