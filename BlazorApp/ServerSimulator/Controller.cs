using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public static class Controller
    {
        public async static Task<GridList<User>> GetUsers(int PageNumber, int ItemsInPage, int SortBy, string FilterString = "")
        {
            return await Logic.GetUsers(PageNumber, ItemsInPage, SortBy, FilterString);
        }
    }
}
