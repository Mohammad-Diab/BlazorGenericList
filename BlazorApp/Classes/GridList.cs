using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class GridList<T> : List<T>
    {
        public List<Property> ItemProperties { get; set; }

        public List<string> Header { get; set; }

        public string ItemsType { get; set; } = "Item";

        public int SortedBy { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int TotalCount { get; set; }

        public GridList(string itemType, List<string> header, List<Property> itemProperties, int sortedBy)
        {
            ItemsType = itemType;
            Header = header;
            ItemProperties = itemProperties;
            SortedBy = sortedBy;
        }

        internal void SetPageCount(int ItemsCount, int PageNumber, int ItemsPerPage)
        {
            CurrentPage = PageNumber;
            PagesCount = (int)Math.Ceiling((ItemsCount + 0.0) / ItemsPerPage);
            TotalCount = ItemsCount;
        }

        public static List<T> GetPage(int PageNumber, int ItemPage, int SortBy, string FilterString)
        {
            return null;
        }
    }
}
