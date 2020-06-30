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

        public async static Task<string> AddUser(User NewUser)
        {
            (bool result, string errorMessage) addResult = await Logic.AddNewUser(NewUser);
            return addResult.result ? "" : addResult.errorMessage;
        }

        public async static Task<string> EditUser(int id, User NewUser)
        {
            (bool result, string errorMessage) editResult = await Logic.EditUser(id, NewUser);
            return editResult.result ? "" : editResult.errorMessage;
        }

        public async static Task<string> DeleteUser(int id)
        {
            (bool result, string errorMessage) deleteResult = await Logic.DeleteUser(id);
            return deleteResult.result ? "" : deleteResult.errorMessage;
        }

        public async static Task<string> DeleteMultiUsers(List<int> Ids)
        {
            (bool result, string errorMessage) deleteResult = await Logic.DeleteMultiUsers(Ids);
            return deleteResult.result ? "" : deleteResult.errorMessage;
        }

        public async static Task<(string result, string errorMessage)> ExportUsers(string Type, int PageNumber, int ItemsInPage, int SortBy, string FilterString)
        {
            await Task.Delay(200);
            (string result, string errorMessage) result = await Logic.Export(Type, PageNumber, ItemsInPage, SortBy, FilterString);
            return result;
        }

    }
}
