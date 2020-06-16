using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public static class Logic
    {
        public async static Task<GridList<User>> GetUsers(int PageNumber, int ItemsInPage, int SortBy, string FilterString = "")
        {
            var allUsers = await Reader.GetAllUsers();
            await Task.Delay(new Random().Next(500));
            GridList<User> Result = new GridList<User>(User.ItemType, User.Headers, User.ItemProperties, SortBy, PageNumber, (int)Math.Ceiling((allUsers.Count + 0.0) / ItemsInPage), allUsers.Count);
            string param = User.GetSortByProp(Math.Abs(SortBy));
            var propertyInfo = typeof(User).GetProperty(param);
            if (SortBy < 0)
            {
                Result.AddRange(allUsers.Where(x => x.IsMatch(FilterString)).OrderByDescending(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }
            else
            {
                Result.AddRange(allUsers.Where(x => x.IsMatch(FilterString)).OrderBy(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }
            
            return Result;
        }
    }
}
