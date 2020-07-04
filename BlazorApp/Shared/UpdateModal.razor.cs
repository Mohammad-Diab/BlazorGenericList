using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public partial class UpdateModal<T>
    {
        /// <summary>
        /// List of T object's properties
        /// </summary>
        [Parameter] public List<Property> Properties { get; set; }

        /// <summary>
        /// Dialog mode either Add or Edit.
        /// </summary>
        DialogMode DialogMode { get; set; }

        // Reference to current modal.
        Modal ModalRef;

        /// <summary>
        /// Ttem to be updated in Update mode.
        /// </summary>
        T Item { get; set; }

        /// <summary>
        /// Show Add/Update Modal and return the result.
        /// </summary>
        /// <param name="ModalTitle">Modal Title.</param>
        /// <param name="Mode">Add or Update modal.</param>
        /// <param name="ItemId">Item unique identifier to send to server when confirm, In update mode only.</param>
        /// <param name="Item">Item to update, In update mode only.</param>
        /// <returns>
        /// <para>Tuble of DialogResult, T.</para> 
        /// <para>Fisrt is ModalResult: represent user selection, confirm or cancel.</para>
        /// <para>Second is NewItem: new item or updated item.</para>
        /// </returns>
        public async Task<(DialogResult ModalResult, T NewItem)> ShowModal(string ModalTitle, DialogMode Mode, string ItemId = null, T Item = default)
        {
            // Assign parameter to reflect on the component.
            DialogMode = Mode;
            this.Item = Item;
            ModalConfirmButton mode = ModalConfirmButton.Add;

            
            if (Mode == DialogMode.Edit)
            {
                // Update case:

                mode = ModalConfirmButton.Edit;

                // Create field for each property.
                for (int i = 0; i < Properties.Count; i++)
                {
                    // Read property info.
                    var prop = typeof(T).GetProperty(Properties[i].Key);

                    // Read property value from item.
                    var value = prop.GetValue(Item);

                    // Assign value to defaultValue of the property to use in component.
                    Properties[i].DefaultValue = (value is DateTime) ? ((DateTime)value).ToString("yyyy-MM-dd") : value.ToString();
                }
            }
            else
            {
                // Add case:

                // Clear default values.
                for (int i = 0; i < Properties.Count; i++)
                {
                    Properties[i].DefaultValue = "";
                }
            }

            // Show modal and wait for the result.
            var ModalResult = await ModalRef.ShowModal(ModalTitle, mode);

            // Create new instance for result.
            T updatedItem = default;

            if (ModalResult == DialogResult.Ok)
            {
                // Modal confirmed:
                var param = new List<object>();

                // Add ItemId to contractor parameters list in Update mode.
                if (Mode == DialogMode.Edit)
                    param.Add(ItemId);

                // Add all other properties.
                for (int i = 0; i < Properties.Count; i++)
                {
                    param.Add(Properties[i].DefaultValue);
                }

                // Create new instance of T object.
                updatedItem = (T)Activator.CreateInstance(typeof(T), param.ToArray());
            }

            return (ModalResult, updatedItem);

        }
    }
}
