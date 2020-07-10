using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
    public partial class SelectGrids
    {
        /// <summary>
        /// Property to bind allow multi select checkbox.
        /// </summary>
        bool AllowMultiSelect { get; set; } = true;

        // List of user to use in "Inline Grid" and "In Modal Grid".
        GridList<User> InlineUsersList, InModalUserList;

        // References to both Grid Lists.
        GenericList<User> InlineGridObj, InModalGridObj;

        /// <summary>
        /// Main function to get, sort and filter the grid list. This function will be used by "Inline Grid".
        /// </summary>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        async Task InlineAction(int PageNumber, int SortBy, string FilterString = "")
        {
            // Assign InlineUsersList to null, this will make the layout displaying lazy content component.
            InlineUsersList = null;

            // Force layout to refreshing component again.
            StateHasChanged();

            // Get result from server.
            InlineUsersList = await Controller.GetUsers(PageNumber, AppSettings.ItemsPerPage, SortBy, FilterString);

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        // List of selected users in "Inline Grid".
        List<User> InlineGridSelectedUsers;

        /// <summary>
        /// Function to get selected users from "Inline Grid".
        /// </summary>
        void GetInlineGridSelectedUsers()
        {
            InlineGridSelectedUsers = InlineGridObj.GetSelectedItems();
        }

        /// <summary>
        /// Main function to get, sort and filter the grid list. This function will be used by "In Modal Grid".
        /// </summary>
        /// <param name="PageNumber">Requested page number.</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result. This field is Optional, default value is an empty string.</param>
        async Task InModalAction(int PageNumber, int SortBy, string FilterString = "")
        {
            // Assign InModalUserList to null, this will make the layout displaying lazy content component.
            InModalUserList = null;

            // Force layout to refreshing component again.
            StateHasChanged();

            // Get result from server.
            InModalUserList = await Controller.GetUsers(PageNumber, AppSettings.ItemsPerPage, SortBy, FilterString);

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        // Reference to "In Modal Grid" modal.
        Modal ModalRef;

        // List of selected users in "In Modal Grid".
        List<User> InModalGridSelectedUsers;

        /// <summary>
        /// Show the modal and get selected users if confirmed;
        /// </summary>
        public async void ShowModal()
        {
            // Show the modal and get the result.
            var Result = await ModalRef.ShowModal("In modal Grid", ModalConfirmButton.Default);

            // empty seleced users list if modal has canceled.
            if (Result == DialogResult.Cancel)
            {
                InModalGridObj.ClearSelectedList();
            }

            // Get selected users.
            InModalGridSelectedUsers = InModalGridObj.GetSelectedItems();

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        // Reference to "AutoComplete" component.
        AutoComplete<User> AutoCompleteObj;

        // To view selected users in AutoComplete.
        List<AutoCompleteItem> AutoCompleteSelectedUsers;

        /// <summary>
        /// Main function to get and filter autoComplete list.
        /// </summary>
        /// <param name="FilterString">A string to filter the result.</param>
        /// <returns>A filtered user list.</returns>
        async Task<List<User>> AutoCompleteFilterList(string FilterString)
        {
            // Get result from server.
            List<User> result = await Controller.GetUsers(1, AppSettings.ItemsPerPage, User.DefaultSortedBy, FilterString);

            return result;
        }

        /// <summary>
        /// Function to convert from User to AutoCompleteItem object.
        /// </summary>
        /// <param name="User">User to be converted.</param>
        /// <returns>AutoCompleteItem that represent the result.</returns>
        AutoCompleteItem UserToAutoCompleteConverter(User User)
        {
            return new AutoCompleteItem() { Key = User.Id.ToString(), Value = User.FullName };
        }

        /// <summary>
        /// Function to get selected users from autocomplete.
        /// </summary>
        void GetAutoCompleteSelectedUsers()
        {
            AutoCompleteSelectedUsers = AutoCompleteObj.GetSelectedItems();
        }
    }
}
