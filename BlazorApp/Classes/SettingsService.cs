using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// Settings
    /// </summary>
    public class SettingsService
    {
        /// <summary>
        /// Number of items per a page.
        /// </summary>
        public int ItemsPerPage { get; set; } = 5;

        /// <summary>
        /// Is current user has admin privileges.
        /// </summary>
        public bool IsAdmin { get; set; } = true;
    }
}
