using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// Simulate a server controller.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// To get random latency time to simulate a server. This field is read-only.
        /// </summary>
        private static Random RandomNumber { get; } = new Random();

        /// <summary>
        /// Return a GridList of User that matches the conditions.
        /// </summary>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="ItemsInPage">Number of item per a page.</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        /// <returns>List of User that matches the conditions</returns>
        public async static Task<GridList<User>> GetUsers(int PageNumber, int ItemsInPage, int SortBy, string FilterString = "")
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            GridList<User> result = await Logic.GetUsers(PageNumber, ItemsInPage, SortBy, FilterString);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return result;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="NewUser">object represents the user to be added.</param>
        /// <returns>An empty string when everything goes right, otherwise return the error message.</returns>
        public async static Task<string> AddUser(User NewUser)
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            (bool result, string errorMessage) addResult = await Logic.AddNewUser(NewUser);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return addResult.result ? "" : addResult.errorMessage;
        }

        /// <summary>
        /// Update an existing user's information.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <param name="NewUser">object represents the new user information.</param>
        /// <returns>An empty string when everything goes right, otherwise return the error message.</returns>
        public async static Task<string> EditUser(int id, User NewUser)
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            (bool result, string errorMessage) editResult = await Logic.EditUser(id, NewUser);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return editResult.result ? "" : editResult.errorMessage;
        }

        /// <summary>
        /// Deleting an existing user.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <returns>An empty string when everything goes right, otherwise return the error message.</returns>
        public async static Task<string> DeleteUser(int id)
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            (bool result, string errorMessage) deleteResult = await Logic.DeleteUser(id);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return deleteResult.result ? "" : deleteResult.errorMessage;
        }

        /// <summary>
        /// Deleting multi users.
        /// </summary>
        /// <param name="Ids">List of users' ids to delete.</param>
        /// <returns>An empty string when everything goes right, otherwise return the error message.</returns>
        public async static Task<string> DeleteMultiUsers(List<int> Ids)
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            (bool result, string errorMessage) deleteResult = await Logic.DeleteMultiUsers(Ids);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return deleteResult.result ? "" : deleteResult.errorMessage;
        }

        /// <summary>
        /// Exporting a list of user to a file.
        /// </summary>
        /// <param name="Type">Desiring file type, available types is JSON, XML, CSV and PDF.</param>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="ItemsInPage">Number of item per a page.</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        /// <returns>
        /// <para>Tuble of string, string.</para> 
        /// <para>Fisrt is result: represent result file.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        public async static Task<(string result, string errorMessage)> ExportUsers(string Type, int PageNumber, int ItemsInPage, int SortBy, string FilterString = "")
        {
            // Represents the requert send delay.
            await Task.Delay(RandomNumber.Next(20));

            // Hand over the request to "Logic" to be processed.
            (string result, string errorMessage) result = await Logic.Export(Type, PageNumber, ItemsInPage, SortBy, FilterString);

            // Represents the response receive delay.
            await Task.Delay(RandomNumber.Next(20));

            return result;
        }

    }
}
