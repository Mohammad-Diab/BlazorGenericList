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
            GridList<User> Result = new GridList<User>(User.ItemType, User.Headers, User.ItemProperties, SortBy, FilterString);
            string param = User.GetSortByProp(Math.Abs(SortBy));
            var propertyInfo = typeof(User).GetProperty(param);
            var filterdList = allUsers.Where(x => x.IsMatch(FilterString)).ToList();
            if (SortBy < 0)
            {
                Result.AddRange(filterdList.OrderByDescending(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }
            else
            {
                Result.AddRange(filterdList.OrderBy(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }
            Result.SetPageCount(filterdList.Count, PageNumber, ItemsInPage);

            return Result;
        }

        internal async static Task<(bool result, string errorMessage)> AddNewUser(User newUser)
        {
            return await Reader.AddUser(newUser);
        }

        internal async static Task<(bool result, string errorMessage)> EditUser(int id, User newUser)
        {
            return await Reader.EditUser(id, newUser);
        }

        internal async static Task<(bool result, string errorMessage)> DeleteUser(int id)
        {
            return await Reader.DeleteUser(id);
        }

    }
}
