using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public partial class Notification
    {
        /// <summary>
        /// Timeout to auto hide notification.
        /// </summary>
        [Parameter] public int Timeout { get; set; }

        // Has current notification a dismiss button.
        public bool IsDismissible { get; set; }

        // Notification title.
        public string Title { get; set; }

        // Notification body.
        public string Description { get; set; }

        // Notification extra information.
        public string ExtraInfo { get; set; }

        // Notification type.
        public NotificationType Type { get; set; }

        // Is Notification list is visible or not.
        bool isShown = false;

        // Animation class.
        string animationClass = "animate__fadeIn";

        /// <summary>
        /// Function trigger after Initializing the component.
        /// </summary>
        protected override void OnInitialized()
        {
            Timeout = Timeout < 1000 ? 1000 : Timeout;
        }

        /// <summary>
        /// Get class for notification depend on the type.
        /// </summary>
        /// <returns>Bootstrap classname depend on selected type.</returns>
        string GetNotificationTypeClass() =>
            Type switch
            {
                NotificationType.Info => "primary",
                NotificationType.Success => "success",
                NotificationType.Warning => "warning",
                NotificationType.Error => "danger",
                _ => "primary"
            };

        /// <summary>
        /// Show the notification.
        /// </summary>
        /// <param name="Type">Notification type.</param>
        /// <param name="Description">Notification body.</param>
        /// <param name="IsDismissible">Has the notification a dismiss button. This parameter is Optional.</param>
        /// <param name="Title">Notification title. This parameter is Optional.</param>
        /// <param name="ExtraInfo">Notification extra info. This parameter is Optional.</param>
        public async void Show(NotificationType Type, string Description, bool IsDismissible = true, string Title = "", string ExtraInfo = "")
        {
            // Assign parameter to reflect on the component.
            this.Type = Type;
            this.Description = Description;
            this.Title = Title;
            this.ExtraInfo = ExtraInfo;
            this.IsDismissible = IsDismissible;

            // Add fade in className.
            animationClass = "animate__fadeIn";

            // Show notification.
            isShown = true;

            // Force layout to refreshing component.
            StateHasChanged();

            // If this notification doesn't have a dismiss button auto hide it after timeout.
            if (!IsDismissible)
            {
                await Task.Delay(Timeout);
                Hide();
            }
        }

        /// <summary>
        /// Hide the notification
        /// </summary>
        public async void Hide()
        {
            // Add fade out className.
            animationClass = "animate__fadeOut";

            // Force layout to refreshing component.
            StateHasChanged();

            // Wait for animation to end.
            await Task.Delay(350);

            // Hide notification
            isShown = false;

            // Force layout to refreshing component again.
            StateHasChanged();
        }
    }
}
