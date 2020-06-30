using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        internal async static Task<(bool result, string errorMessage)> DeleteMultiUsers(List<int> ids)
        {
            return await Reader.DeleteMultiUsers(ids);
        }

        internal async static Task<(string result, string errorMessage)> Export(string type, int pageNumber, int itemsInPage, int sortBy, string filterString)
        {
            var usersList = (await GetUsers(pageNumber, itemsInPage, sortBy, filterString)).GetList();
            type = type.ToLower();
            switch (type)
            {
                case "json":
                    string json = System.Text.Json.JsonSerializer.Serialize(usersList, typeof(List<User>));
                    return (json, "");
                case "xml":
                    string xml = "";
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, usersList);
                        xml = textWriter.ToString();
                    }
                    return (xml, "");
                case "csv":
                    string csv = UsersListToCSV(usersList);
                    return (csv, "");
                case "pdf":
                    return ("", "Exporting as Pdf files is not suported right now.");
                default:
                    return ("", "Invalid request.");
            }
        }

        private static string UsersListToCSV(List<User> usersList)
        {
            StringBuilder csvStringbuilder = new StringBuilder();
            for (int i = 0; i < usersList.Count; i++)
            {
                csvStringbuilder.Append($"{usersList[i].Id},");
                csvStringbuilder.Append($"\"{usersList[i].Username.Replace("\"", "\"\"")}\",");
                csvStringbuilder.Append($"\"{usersList[i].FullName.Replace("\"", "\"\"")}\",");
                csvStringbuilder.Append($"{usersList[i].Birthday:yyyy MMM dd},");
                csvStringbuilder.Append($"\"{usersList[i].Email.Replace("\"", "\"\"")}\",");
                csvStringbuilder.Append($"\"{usersList[i].Phone.Replace("\"", "\"\"")}\"");
                csvStringbuilder.AppendLine();
            }
            return csvStringbuilder.ToString();
        }
    }
}
