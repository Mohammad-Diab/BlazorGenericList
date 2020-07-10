using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
    public partial class EditGrid

    {
        // List of users that used for rendering in the Grid.
        GridList<User> UsersList;

        // References to LoadingPanal component.
        Loading LoadingPanal;

        // References to NotificationPanal component.
        Notification NotificationPanal;

        /// <summary>
        /// Main function to get, sort and filter the grid list.
        /// </summary>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        async Task Action(int PageNumber, int SortBy, string FilterString = "")
        {
            // Assign UsersList to null, this will make the layout displaying lazy content component.
            UsersList = null;

            // Force layout to refreshing component again.
            StateHasChanged();

            // Get result from server.
            UsersList = await Controller.GetUsers(PageNumber, AppSettings.ItemsPerPage, SortBy, FilterString);

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// Add user function.
        /// </summary>
        /// <param name="newUser">User to be added.</param>
        async Task AddUser(User newUser)
        {
            // Show loading panal.
            LoadingPanal.Show();

            // Send the request and wait for result.
            string result = await Controller.AddUser(newUser);

            // Hide loading panal.
            LoadingPanal.Hide();

            if (!string.IsNullOrEmpty(result))
            {
                // If the result is not empty the somthing went wrong in the request, So display the error.
                NotificationPanal.Show(NotificationType.Error, result, true, $"Can't add '{newUser.Username}'!");
            }
            else
            {
                // If everything goes right, Add the user to list.
                UsersList.Add(newUser);

                // Increase total users count.
                UsersList.TotalCount++;

                // Force layout to refreshing component again.
                StateHasChanged();
            }
        }

        /// <summary>
        /// Edit user function.
        /// </summary>
        /// <param name="id">Desiring User's Id.</param>
        /// <param name="newUser">New user object.</param>
        async Task EditUser(string id, User newUser)
        {
            // check if id is empty.
            if (!string.IsNullOrEmpty(id))
            {
                // Show loading panal.
                LoadingPanal.Show();

                // Send the request and wait for result.
                string result = await Controller.EditUser(id, newUser);

                // Hide loading panal.
                LoadingPanal.Hide();
                if (!string.IsNullOrEmpty(result))
                {
                    // If the result is not empty the somthing went wrong in the request, So display the error.
                    NotificationPanal.Show(NotificationType.Error, result, true, $"Can't Edit '{newUser.Username}'!");
                }
                else
                {
                    // If everything goes right, Replace old user object with the new one..
                    int oldUserIndex = UsersList.FindIndex((x) => x.Id == id);
                    if (oldUserIndex > -1)
                    {
                        UsersList[oldUserIndex] = newUser;

                        // Force layout to refreshing component again.
                        StateHasChanged();
                    }
                }
            }
            else
            {
                NotificationPanal.Show(NotificationType.Error, "Invalid Id format!", true);
            }
        }

        /// <summary>
        /// Delete user function.
        /// </summary>
        /// <param name="User">User to delete.</param>
        async Task DeleteUser(User User)
        {
            // Show loading panal.
            LoadingPanal.Show();

            // Send the request and wait for result.
            string result = await Controller.DeleteUser(User.Id);

            // Hide loading panal.
            LoadingPanal.Hide();

            if (!string.IsNullOrEmpty(result))
            {
                // If the result is not empty the somthing went wrong in the request, So display the error.
                NotificationPanal.Show(NotificationType.Error, result, true, $"Can't delete '{User.Username}'!");
            }
            else
            {
                // If everything goes right, Delete the user from local list.
                UsersList.Remove(User);

                //Decrease total users count.
                UsersList.TotalCount--;

                // If current user is the last user in the list and there is other pages, request other page from the server.
                if (UsersList.Count == 0 && UsersList.PagesCount > 1)
                {
                    // New page number will be same as current page number except if the current page is last page then we should take previous one.
                    int pageNumber = UsersList.CurrentPage == UsersList.PagesCount ? UsersList.PagesCount - 1 : UsersList.CurrentPage;

                    // Keep other properties as they are.
                    int sortBy = UsersList.SortedBy;
                    string filterstring = UsersList.FilterString;

                    // Assign UsersList to null, this will make the layout displaying lazy content component.
                    UsersList = null;

                    // Force layout to refreshing component again.
                    StateHasChanged();

                    // Get result from server.
                    UsersList = await Controller.GetUsers(pageNumber, AppSettings.ItemsPerPage, sortBy, filterstring);
                }
            }

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// Delete multi users at once.
        /// </summary>
        /// <param name="Users">List of users to delete.</param>
        async Task DeleteSelectedUsers(List<User> Users)
        {
            // Show loading panal.
            LoadingPanal.Show();

            // Send the request and wait for result.
            string result = await Controller.DeleteMultiUsers(Users.Select((x) => x.Id).ToList());

            // Hide loading panal.
            LoadingPanal.Hide();

            if (!string.IsNullOrEmpty(result))
            {
                // If the result is not empty the somthing went wrong in the request, So display the error.
                NotificationPanal.Show(NotificationType.Error, result, true, $"Can't delete '{Users.Count}' Users!");
            }
            else
            {
                // If everything goes right, Remove users from the list.
                for (int i = 0; i < Users.Count; i++)
                {
                    UsersList.Remove(Users[i]);
                }

                // Decrease total users by the number of deleted users.
                UsersList.TotalCount -= Users.Count;

                // If current page is empty and there is other pages, request other page from the server.
                if (UsersList.Count == 0 && UsersList.PagesCount > 1)
                {
                    // New page number will be same as current page number except if the current page is last page then we should take previous one.
                    int pageNumber = UsersList.CurrentPage == UsersList.PagesCount ? UsersList.PagesCount - 1 : UsersList.CurrentPage;

                    // Keep other properties as they are.
                    int sortBy = UsersList.SortedBy;
                    string filterstring = UsersList.FilterString;

                    // Assign UsersList to null, this will make the layout displaying lazy content component.
                    UsersList = null;

                    // Force layout to refreshing component again.
                    StateHasChanged();

                    // Get result from server.
                    UsersList = await Controller.GetUsers(pageNumber, AppSettings.ItemsPerPage, sortBy, filterstring);
                }
                // Force layout to refreshing component again.
                StateHasChanged();
            }
        }

        /// <summary>
        /// Exporting a users' page to a file.
        /// </summary>
        /// <param name="type">Desiring file type, available types is JSON, XML, CSV and PDF.</param>
        async Task ExportItems(string type)
        {
            // Show loading panal.
            LoadingPanal.Show();

            // Read current page properties.
            int pageNumber = UsersList.CurrentPage;
            int sortBy = UsersList.SortedBy;
            string filterstring = UsersList.FilterString;

            // Send the request and wait for result.
            var result = await Controller.ExportUsers(type, pageNumber, AppSettings.ItemsPerPage, sortBy, filterstring);

            if (!string.IsNullOrEmpty(result.errorMessage))
            {
                // If errorMessage is not empty the somthing went wrong in the request, So display the error.
                NotificationPanal.Show(NotificationType.Error, result.errorMessage, true);
            }
            else
            {
                // If everything goes right, Display the result in the console (for now). Downloading files will be available in next updates.
                Console.WriteLine(result.result);

                // Show a notification that the result will not be downloaded, and it'll be in the Browser 'Console'.
                NotificationPanal.Show(NotificationType.Info, "Can't download files right now, result will be found in your browser 'Console'.", true);
            }

            // Hide loading panal.
            LoadingPanal.Hide();
        }
    }
}
