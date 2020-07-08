using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public partial class GenericList<T>
    {
        /// <summary>
        /// Grid tilte.
        /// </summary>
        [Parameter] public string Title { get; set; }

        /// <summary>
        /// Grid Type: for Select or for Update.
        /// </summary>
        [Parameter] public GridType GridType { get; set; }

        /// <summary>
        /// Count of columns in the grid.
        /// </summary>
        [Parameter] public int GridColumnsCount { get; set; }

        /// <summary>
        /// Number of items per a page.
        /// </summary>
        [Parameter] public int ItemsPerPage { get; set; }

        /// <summary>
        /// The list to rendered in this grid.
        /// </summary>
        [Parameter] public GridList<T> ItemsList { get; set; }

        /// <summary>
        /// Component to render grid header.
        /// </summary>
        [Parameter] public RenderFragment TableHeader { get; set; }

        /// <summary>
        /// Component to render each item in the grid.
        /// </summary>
        [Parameter] public RenderFragment<T> ItemRender { get; set; }

        /// <summary>
        /// Loading panel reference to be able to show and hide loading panel.
        /// </summary>
        [CascadingParameter(Name = "LoadingPanal")] public Loading LoadingPanal { get; set; }

        /// <summary>
        /// Config - Does the current user has sufficient privilege to modify the grid.
        /// </summary>
        [CascadingParameter(Name = "EditAbility")] public bool EditAbility { get; set; }

        /// <summary>
        /// Main function to get, sort and filter the grid list.
        /// </summary>
        [Parameter] public Func<int, int, string, Task> ActionFunc { get; set; }

        /// <summary>
        /// Function to add item to the grid.
        /// </summary>
        [Parameter] public Func<T, Task> AddItem { get; set; }

        /// <summary>
        /// Function to update an item in the grid.
        /// </summary>
        [Parameter] public Func<string, T, Task> EditItem { get; set; }

        /// <summary>
        /// Function to delete an item from the grid.
        /// </summary>
        [Parameter] public Func<T, Task> DeleteItem { get; set; }

        /// <summary>
        /// Function to delete multi items from the grid.
        /// </summary>
        [Parameter] public Func<List<T>, Task> DeleteMultiItems { get; set; }

        /// <summary>
        /// Function to export current grid to a file and download it.
        /// </summary>
        [Parameter] public Func<string, Task> ExportItems { get; set; }

        /// <summary>
        /// The default SortBy value for current item type.
        /// </summary>
        [Parameter] public int DefaultSortBy { get; set; }

        // A string to filter the grid
        string FilterString { get; set; } = "";

        // Current grid page number.
        int CurrentPage { get; set; } = 1;

        // An integer represent column id that current grid is sorted based on.
        int SortBy { get; set; } = 1;

        // Total pages count.
        int PagesCount { get; set; } = 1;

        /// <summary>
        /// Function that triggers after Initializing the component.
        /// </summary>
        /// <returns></returns>
        protected override void OnInitialized()
        {
            Refresh();
        }

        /// <summary>
        /// Load grid content.
        /// </summary>
        public void Refresh()
        {
            RefreshGrid(1, DefaultSortBy, "");
        }

        // Selected items in the grid.
        HashSet<T> SelectedItems = new HashSet<T>();

        /// <summary>
        /// Add item to selected list.
        /// </summary>
        /// <param name="Item">Item to add.</param>
        public void AddItemToSelectedList(T Item)
        {
            // Empty the list if Multi item selection is disabled.
            if (GridType == GridType.SingleSelect)
            {
                SelectedItems.Clear();
            }
            SelectedItems.Add(Item);

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// Remove an item from selected items list.
        /// </summary>
        /// <param name="Item">Item to delete.</param>
        public void RemoveItemFromSelectedList(T Item)
        {
            // Remove item from selected items list.
            SelectedItems.Remove(Item);

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// Remove all items in selected items list.
        /// </summary>
        public void ClearSelectedList()
        {
            // Remove all items in selected items list.
            SelectedItems.Clear();

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// To check whether item is selected or not.
        /// </summary>
        /// <param name="Item">Item to check.</param>
        /// <returns>True if item is checked, otherwise False.</returns>
        public bool IsSelected(T Item)
        {
            return SelectedItems.Contains(Item);
        }

        /// <summary>
        /// Get selected items list.
        /// </summary>
        /// <returns>List of selected items if grid mode is on of select modes, otherwise null.</returns>
        public List<T> GetSelectedItems()
        {
            // This function works only on select grid modes.
            if (GridType != GridType.Edit)
                return SelectedItems.ToList();
            return null;
        }

        /// <summary>
        /// Select/Deselect all items in current page.
        /// </summary>
        /// <param name="value">True to select all, False to deselect all.</param>
        public void SelectAll(bool value)
        {
            if (value)
            {
                for (int i = 0; i < ItemsList.Count; i++)
                    SelectedItems.Add(ItemsList[i]);
            }
            else
            {
                for (int i = 0; i < ItemsList.Count; i++)
                    SelectedItems.Remove(ItemsList[i]);
            }

            // Force layout to refreshing component again.
            StateHasChanged();
        }

        /// <summary>
        /// To get select all checkbox status. This allows us to do some actions while status changed and binded.
        /// </summary>
        public SelectedStatus SelectStatus
        {
            get
            {
                // When SelectedItems or ItemsList is null or empty, return no items are selected.
                if ((SelectedItems?.Count ?? 0) == 0 || (ItemsList?.Count ?? 0) == 0)
                    return SelectedStatus.None;

                // Calculate how many items are selected and return the result based on it.
                int selectedCount = 0;
                for (int i = 0; i < ItemsList.Count; i++)
                {
                    if (SelectedItems.Contains(ItemsList[i]))
                        selectedCount++;
                }
                if (selectedCount == ItemsList.Count)
                    return SelectedStatus.All;
                else if (selectedCount == 0)
                    return SelectedStatus.None;
                else
                    return SelectedStatus.Some;
            }
        }

        // Reference to Add/Update modal.
        UpdateModal<T> Modal;

        // Reference to deletion confirm modal.
        ConfirmDialog ConfirmModal;

        // Export items list.
        List<DropdownItem> ExportDropdownItems = new List<DropdownItem>()
        {
            new DropdownItem("header","Human readable formats",DropdownType.Header),
            new DropdownItem("csv","CSV",DropdownType.Button),
            new DropdownItem("pdf","PDF",DropdownType.Button),
            new DropdownItem("divider","Divider",DropdownType.Divider),
            new DropdownItem("header","Machine readable formats",DropdownType.Header),
            new DropdownItem("json","JSON",DropdownType.Button),
            new DropdownItem("xml","XML",DropdownType.Button)
        };

        /// <summary>
        /// Show add item modal, and add item if it's confirmed.
        /// </summary>
        async void ShowAddModal()
        {
            // Show the modal and get the result.
            var result = await Modal.ShowModal($"Add new {ItemsList?.ItemsType?.ToLower() ?? "item"}", DialogMode.Add);

            // If user confirm, send request to the server to add the item.
            if (result.ModalResult == DialogResult.Ok && result.NewItem != null)
            {
                await AddItem(result.NewItem);
            }
        }

        /// <summary>
        /// Show update item modal and edit item if it's confirmed.
        /// </summary>
        /// <param name="Item">The original item.</param>
        /// <param name="ItemId">Item Unique Identifier.</param>
        /// <param name="itemName">Item friendly name to view in the message.</param>
        public async void ShowEditModal(T Item, string ItemId, string itemName)
        {
            // Show the modal and get the result.
            var result = await Modal.ShowModal($"Edit '{itemName}'", DialogMode.Edit, ItemId, Item);

            // If user confirm, send request to the server to update item properties.
            if (result.ModalResult == DialogResult.Ok && result.NewItem != null)
            {
                await EditItem(ItemId, result.NewItem);
            }
        }

        /// <summary>
        /// Show delete item confirm modal and delete item if it's confirmed.
        /// </summary>
        /// <param name="Item">Item to delete.</param>
        /// <param name="itemName">Item friendly name to view in the message.</param>
        public async void ShowDeleteModal(T Item, string itemName)
        {
            // Show the modal and get the result.
            DialogResult result = await ConfirmModal.ShowModal($"Deleting {itemName}", $"Do you really want to delete '{itemName}'?", ModalConfirmButton.Delete);

            // If user confirm, send request to the server to delete item.
            if (result == DialogResult.Ok)
            {
                await DeleteItem(Item);
            }
        }

        /// <summary>
        /// Show delete multi items confirm modal and delete them if it's confirmed.
        /// </summary>
        public async void ShowDeleteAllModal()
        {
            if (SelectedItems.Count == 0)
            {
                return;
            }

            // Show the modal and get the result.
            DialogResult result = await ConfirmModal.ShowModal($"Deleting {SelectedItems.Count} items", $"Do you really want to delete '{SelectedItems.Count} items'?", ModalConfirmButton.Delete);

            // If user confirm, send request to the server to delete selected items.
            if (result == DialogResult.Ok)
            {
                await DeleteMultiItems(SelectedItems.ToList());
                ClearSelectedList();
            }
        }

        /// <summary>
        /// Sort the list.
        /// </summary>
        /// <param name="SortBy">An integer represent column id to sort the list based on.</param>
        public void Sort(int SortBy)
        {
            // If the list is already sorted based on new sort integer, invert the order.
            if (SortBy == Math.Abs(this.SortBy))
            {
                SortBy = -this.SortBy;
            }

            // Reload the list base on new order.
            RefreshGrid(CurrentPage, SortBy, FilterString);
        }

        /// <summary>
        /// Go to Page.
        /// </summary>
        /// <param name="PageNumber">Requested page number</param>
        public void GoToPage(int PageNumber)
        {
            // Prevent out of range numbers.
            PageNumber = PageNumber < 1 ? 1 : PageNumber;
            PageNumber = PageNumber > PagesCount ? PagesCount : PageNumber;

            // Reload the list base on new page number.
            RefreshGrid(PageNumber, SortBy, FilterString);
        }

        /// <summary>
        /// Filter the list result.
        /// </summary>
        /// <param name="filterString"></param>
        public void Filter(string filterString)
        {
            FilterString = filterString;

            // Reload the list base on filter value.
            RefreshGrid(1, SortBy, FilterString);
        }

        /// <summary>
        /// Re requesting the grid content from the server.
        /// </summary>
        /// <param name="PageNumber">Requested page number</param>
        /// <param name="SortBy">An integer represent column id to sort the list based on, negative number means inverted order.</param>
        /// <param name="FilterString">A string to filter the result.</param>
        public async void RefreshGrid(int PageNumber, int SortBy, string FilterString)
        {
            // Empty selected items list list on Edit mode.
            if (GridType == GridType.Edit)
                ClearSelectedList();

            // Call the function to fill the grid.
            await ActionFunc(PageNumber, SortBy, FilterString);

            // Assign new page parameters.
            this.SortBy = ItemsList.SortedBy;
            CurrentPage = ItemsList.CurrentPage;
            PagesCount = ItemsList.PagesCount;
        }

        /// <summary>
        /// Export curent page items.
        /// </summary>
        /// <param name="type">Desiring file type, available types is JSON, XML, CSV and PDF.</param>
        /// <returns></returns>
        public async Task Export(string type)
        {
            await ExportItems(type);
        }
    }
}
