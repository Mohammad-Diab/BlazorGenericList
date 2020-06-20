using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class SettingsService
    {
        public int ItemsPerPage { get; set; } = 5;

        public bool IsAdmin { get; set; } = true;
    }
}
