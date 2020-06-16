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

        public GridList(string itemType, List<string> header, List<Property> itemProperties, int sortedBy, int currentPage, int pagesCount, int itemsCount)
        {
            ItemsType = itemType;
            Header = header;
            ItemProperties = itemProperties;
            SortedBy = sortedBy;
            CurrentPage = currentPage;
            PagesCount = pagesCount;
            TotalCount = itemsCount;
        }

        public static List<T> GetPage(int PageNumber, int ItemPage, int SortBy, string FilterString)
        {
            return null;
        }
    }
}
