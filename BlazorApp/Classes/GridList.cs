using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// A list modified class to make it easier to implement a generic list component.
    /// </summary>
    /// <typeparam name="T">The Type selected should have some required properties</typeparam>
    public class GridList<T> : List<T>
    {
        /// <summary>
        /// T Type Properties list to build 'Add' and 'Edit' modal's fields.
        /// </summary>
        public List<Property> ItemProperties { get; set; }

        /// <summary>
        /// Grid column headers.
        /// </summary>
        public List<string> Header { get; set; }

        /// <summary>
        /// What should we call your type in messages.
        /// </summary>
        public string ItemsType { get; set; } = "Item";

        /// <summary>
        /// Current list is sorted by...
        /// </summary>
        public int SortedBy { get; set; }

        /// <summary>
        /// Current list contains items from page...
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total pages count.
        /// </summary>
        public int PagesCount { get; set; }

        /// <summary>
        /// Number of items per a page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Total items count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Items in this list is filtered by...
        /// </summary>
        public string FilterString { get; set; }

        public GridList(string itemType, List<string> header, List<Property> itemProperties, int sortedBy, string filterString)
        {
            ItemsType = itemType;
            Header = header;
            ItemProperties = itemProperties;
            SortedBy = sortedBy;
            FilterString = filterString;
        }

        /// <summary>
        /// Set PageNumber, PagesCount, CurrentPage and TotalItems count in current GridList.
        /// </summary>
        /// <param name="ItemsCount">Total items count</param>
        /// <param name="PageNumber">Current Page number.</param>
        /// <param name="ItemsPerPage">Number of items per a page.</param>
        internal void SetPageCount(int ItemsCount, int PageNumber, int ItemsPerPage)
        {
            CurrentPage = PageNumber;
            this.ItemsPerPage = ItemsPerPage;
            PagesCount = (int)Math.Ceiling((ItemsCount + 0.0) / ItemsPerPage);
            TotalCount = ItemsCount;
        }

        /// <summary>
        /// Get an abstract list from current Grid.
        /// </summary>
        /// <returns>A list contains all item in current grid.</returns>
        internal List<T> GetList()
        {
            List<T> result = new List<T>();
            result.AddRange(this);
            return result;
        }
    }
}
