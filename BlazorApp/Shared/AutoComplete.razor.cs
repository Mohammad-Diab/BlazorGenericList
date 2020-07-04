using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorApp.Shared
{
    public partial class AutoComplete<T>
    {
        /// <summary>
        /// A label that appears above AutoComplete field.
        /// </summary>
        [Parameter] public string Title { get; set; }

        /// <summary>
        /// To determine if multiselect is allowed or not.
        /// </summary>
        [Parameter] public bool MultiSelect { get; set; }

        /// <summary>
        /// The Function that gets data to view in AutoComplete list.
        /// </summary>
        [Parameter] public Func<string, Task<List<T>>> ActionFunc { get; set; }

        /// <summary>
        /// A Function that convert from T type to AutoCompleteItem object.
        /// </summary>
        [Parameter] public Func<T, AutoCompleteItem> ConvertToAutoCompleteItem { get; set; }

        /// <summary>
        /// List of selected Items.
        /// </summary>
        List<AutoCompleteItem> SelectedItems { get; set; }

        /// <summary>
        /// List of Item that shown in AutoComplete list.
        /// </summary>
        List<AutoCompleteItem> AutoCompleteList { get; set; }

        // Is AutoComplete list is visible or not.
        bool isShown = false;

        // To show and hide little loading spinner on right of the field.
        bool isLoading = false;

        // Current and previous filtering string.
        string lastFilterString, filterString;

        // Timer to hold back request to server by adding a few milliseconds of delay.
        Timer filteringTimer;

        
        string textInputValue = "";
        /// <summary>
        /// Current value in the input text. This allows us to do some actions when text changed and binded.
        /// </summary>
        public string TextInputValue
        {
            get
            {
                return textInputValue;
            }
            set
            {
                // Reset AutoComplete list content.
                AutoCompleteList = null;

                textInputValue = value;
                filterString = value;

                // Hide AutoComplete list.
                isShown = false;

                // Stop previous Timer.
                filteringTimer.Stop();

                // Start the timer if text typed fulfill the conditions.
                if (filterString.Length > 2)
                {
                    filteringTimer.Start();
                }
            }
        }

        /// <summary>
        /// Function trigger after Initializing the component.
        /// </summary>
        protected override void OnInitialized()
        {
            // Initializ Lists and Timers.
            SelectedItems = new List<AutoCompleteItem>();

            filteringTimer = new Timer(500);
            filteringTimer.AutoReset = false;
            filteringTimer.Elapsed += Filter;

            hideListTimer = new Timer(50);
            hideListTimer.AutoReset = false;
            hideListTimer.Elapsed += (object sender, ElapsedEventArgs args) => {
                if (isOut)
                {
                    isShown = false;
                    StateHasChanged();
                }
            };
        }

        /// <summary>
        /// Triggered after changing text and timer ticking.
        /// </summary>
        async void Filter(object sender, ElapsedEventArgs args)
        {
            // If current text matches previous text just show previous result.
            if (filterString != lastFilterString)
            {
                lastFilterString = filterString;
                filterString = "";

                // Show loading spinner.
                isLoading = true;

                // Force layout to refreshing component.
                StateHasChanged();

                // Get data from server.
                var ItemList = await ActionFunc(lastFilterString);

                // Convert the data to List of AutoCompleteItem.
                AutoCompleteList = ItemList.Select((item) => ConvertToAutoCompleteItem(item)).ToList();

                // Hide loading spinner.
                isLoading = false;
            }
            // Show AutoComplete list.
            isShown = true;

            // Force layout to refreshing component.
            StateHasChanged();
        }

        /// <summary>
        /// Add item to selected list.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        void AddSelectedItem(AutoCompleteItem item)
        {
            // Reset text field value.
            TextInputValue = "";

            // Hide the list.
            isShown = false;

            // Empty the list if MultiSelect is disabled.
            if (!MultiSelect)
                SelectedItems.Clear();

            // Add the item if its not exists.
            if (SelectedItems.Find((it) => it.Key == item.Key) == null)
                SelectedItems.Add(item);
        }

        /// <summary>
        /// Remove item from selected list.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        void RemoveItem(AutoCompleteItem item)
        {
            SelectedItems.Remove(item);
        }

        // Is the focus is outside the AutoComplete
        bool isOut = false;

        // Timer to get if AutoComplete get focused again to prevent hiding the list.
        Timer hideListTimer;

        /// <summary>
        /// Blur text feild event.
        /// </summary>
        void InputBlur()
        {
            if (isShown)
            {
                // set the focus being outside the AutoComplete.
                isOut = true;

                // Start the timer to check if the focus get back to AutoComplete.
                hideListTimer.Start();
            }
        }

        /// <summary>
        /// Show AutoComplete list when text field get focus.
        /// </summary>
        async void ShowList()
        {
            // If list is already initialized just show it.
            if (AutoCompleteList == null)
            {
                // Show the list only when there is 3 letters at least written in the field. or when the text field is empty.
                if (string.IsNullOrEmpty(TextInputValue) || TextInputValue.Length > 2)
                {
                    // Show loading spinner.
                    isLoading = true;

                    // Get data from server.
                    var ItemList = await ActionFunc(TextInputValue);

                    // Convert the data to List of AutoCompleteItem.
                    AutoCompleteList = ItemList.Select((item) => ConvertToAutoCompleteItem(item)).ToList();

                    // Hide loading spinner.
                    isLoading = false;

                    // Show AutoComplete list.
                    isShown = true;

                    // Force layout to refreshing component.
                    StateHasChanged();
                }
            }
            else
            {
                // Show AutoComplete list.
                isShown = true;
            }
        }

        /// <summary>
        /// To get selected items from outside the component.
        /// </summary>
        /// <returns></returns>
        public List<AutoCompleteItem> GetSelectedItems()
        {
            return SelectedItems;
        }

        /// <summary>
        /// Dispose timers after the component being dispose.
        /// </summary>
        void Dispose()
        {
            hideListTimer?.Dispose();
            filteringTimer?.Dispose();
        }
    }
}
