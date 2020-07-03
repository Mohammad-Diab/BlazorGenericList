using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorApp
{
    /// <summary>
    /// Simulate a server Business Logic.
    /// </summary>
    public static class Logic
    {
        /// <summary>
        /// Return a GridList of User that matches the conditions.
        /// </summary>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="ItemsInPage">Number of items per a page</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        /// <returns>List of User that matches the conditions</returns>
        public async static Task<GridList<User>> GetUsers(int PageNumber, int ItemsInPage, int SortBy, string FilterString = "")
        {
            // Read all user from source.
            var allUsers = await Reader.GetAllUsers();

            // Create new instance from GridList of User.
            GridList<User> Result = new GridList<User>(User.ItemType, User.Headers, User.ItemProperties, SortBy, FilterString);

            // Get sortby property name.
            string param = User.GetSortByProp(Math.Abs(SortBy));

            // Get propertyInfo to the sortby colume so we can use it in ordering function.
            var propertyInfo = typeof(User).GetProperty(param);

            // Get all users that match filter string.
            var filterdList = allUsers.Where(x => x.IsMatch(FilterString)).ToList();

            // Sort the list using propertyInfo and them get desired page.
            if (SortBy < 0)
            {
                Result.AddRange(filterdList.OrderByDescending(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }
            else
            {
                Result.AddRange(filterdList.OrderBy(x => propertyInfo.GetValue(x)).Skip(ItemsInPage * (PageNumber - 1)).Take(ItemsInPage).ToList());
            }

            // Set PageNumber, PagesCount, CurrentPage and TotalItems count in returned GridList.
            Result.SetPageCount(filterdList.Count, PageNumber, ItemsInPage);

            return Result;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="newUser">object represents the user to be added.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> AddNewUser(User newUser)
        {
            // Passing the request to data store.
            return await Reader.AddUser(newUser);
        }

        /// <summary>
        /// Update an existing user's information.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <param name="newUser">object represents the new user information.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> EditUser(int id, User newUser)
        {
            // Passing the request to data store.
            return await Reader.EditUser(id, newUser);
        }

        /// <summary>
        /// Deleting an existing user.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> DeleteUser(int id)
        {
            // Passing the request to data store.
            return await Reader.DeleteUser(id);
        }

        /// <summary>
        /// Deleting multi users.
        /// </summary>
        /// <param name="ids">List of users' ids to delete.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> DeleteMultiUsers(List<int> ids)
        {
            // Passing the request to data store.
            return await Reader.DeleteMultiUsers(ids);
        }

        /// <summary>
        /// Exporting a list of user to a file.
        /// </summary>
        /// <param name="type">Desiring file type, available types is JSON, XML, CSV and PDF.</param>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="itemsInPage">Number of item per a page.</param>
        /// <param name="sortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="filterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        /// <returns>
        /// <para>Tuble of string, string.</para> 
        /// <para>Fisrt is result: represent result file.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(string result, string errorMessage)> Export(string type, int pageNumber, int itemsInPage, int sortBy, string filterString)
        {
            // Get users page list that matches the conditions.
            var usersList = (await GetUsers(pageNumber, itemsInPage, sortBy, filterString)).GetList();

            type = type.ToLower();
            switch (type)
            {
                case "json":
                    // Convert object to JSON file.
                    string json = System.Text.Json.JsonSerializer.Serialize(usersList, typeof(List<User>));
                    return (json, "");
                case "xml":
                    string xml = "";
                    // Convert object to XML file.
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, usersList);
                        xml = textWriter.ToString();
                    }
                    return (xml, "");
                case "csv":
                    // Convert object to CSV file.
                    string csv = UsersListToCSV(usersList);
                    return (csv, "");
                case "pdf":
                    // PDF is not supported right now.
                    return ("", "Exporting as Pdf files is not suported right now.");
                default:
                    return ("", "Invalid request.");
            }
        }

        /// <summary>
        /// Convert List of User to CSV file.
        /// </summary>
        /// <param name="usersList">List of user to convert.</param>
        /// <returns>A string that represent the converted CSV file.</returns>
        private static string UsersListToCSV(List<User> usersList)
        {
            // Create new instance of StringBuilder.
            StringBuilder csvStringbuilder = new StringBuilder();
            for (int i = 0; i < usersList.Count; i++)
            {
                // Columns are separated by comma.
                csvStringbuilder.Append($"{usersList[i].Id},");
                csvStringbuilder.Append($"\"{usersList[i].Username.Replace("\"", "\"\"")}\",");
                csvStringbuilder.Append($"\"{usersList[i].FullName.Replace("\"", "\"\"")}\",");

                // Convert date to "2020 Jul 02" foramt.
                csvStringbuilder.Append($"{usersList[i].Birthday:yyyy MMM dd},");

                csvStringbuilder.Append($"\"{usersList[i].Email.Replace("\"", "\"\"")}\",");
                csvStringbuilder.Append($"\"{usersList[i].Phone.Replace("\"", "\"\"")}\"");

                // New Line represent new row in CSV file.
                csvStringbuilder.AppendLine();
            }
            return csvStringbuilder.ToString();
        }
    }
}
