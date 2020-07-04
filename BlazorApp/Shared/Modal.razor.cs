using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public partial class Modal
    {
        /// <summary>
        /// To set modal size and position.
        /// </summary>
        [Parameter] public string ModalClasses { get; set; }

        /// <summary>
        /// The animation that will be used when show the modal.
        /// </summary>
        [Parameter] public ModalShowAnimation ShowAnimation { get; set; }

        /// <summary>
        /// Modal body.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Modal title.
        /// </summary>
        string ModalTitle { get; set; }

        /// <summary>
        /// To set confirm button styles
        /// </summary>
        ModalConfirmButton ConfirmType { get; set; }

        // Is Modal is visible or not.
        bool IsShown = false;

        // Animation and confirm button styles
        string AnimateClassName = "animate__bounceIn animate__fast";
        string animationName = "animate__bounceIn";
        string MainButtonClass = "btn-success";
        string MainButtonText = "Ok";
        string FocusClassName = "";

        // Modal result
        DialogResult Result = DialogResult.Undefiend;

        // To prevent multi shaking animation on modal.
        bool IsAnimating = false;

        /// <summary>
        /// Show the modal.
        /// </summary>
        /// <param name="ModalTitle">Modal title.</param>
        /// <param name="Mode">To determine confirm button style and content.</param>
        /// <returns>Modal result, Confirm or cancel depend on user selection.</returns>
        public async Task<DialogResult> ShowModal(string ModalTitle, ModalConfirmButton Mode)
        {
            // Assign parameter to reflect on the component.
            // set animation class.
            animationName = ShowAnimation switch
            {
                ModalShowAnimation.BounceIn => "animate__bounceIn",
                ModalShowAnimation.BounceInDown => "animate__bounceInDown",
                _ => "animate__bounceIn"
            };
            AnimateClassName = $"{animationName} animate__fast";


            ConfirmType = Mode;
            this.ModalTitle = ModalTitle;

            Result = DialogResult.Undefiend;

            // set confirm button style and content.
            switch (Mode)
            {
                case ModalConfirmButton.Add:
                    MainButtonClass = "btn-primary";
                    MainButtonText = "Add";
                    break;
                case ModalConfirmButton.Edit:
                    MainButtonClass = "btn-primary";
                    MainButtonText = "Edit";
                    break;
                case ModalConfirmButton.Delete:
                    MainButtonClass = "btn-danger";
                    MainButtonText = "Delete";
                    break;
                default:
                    MainButtonClass = "btn-success";
                    MainButtonText = "Ok";
                    break;
            }

            // Show the modal.
            IsShown = true;

            // Force layout to refreshing component.
            StateHasChanged();

            // Wait for user input.
            while (Result == DialogResult.Undefiend)
            {
                await Task.Delay(50);
            }

            return Result;
        }

        /// <summary>
        /// Animate the modal when clicking outside it.
        /// </summary>
        public async void FocusModal()
        {
            // Don't start animation if there is a running animation.
            if (IsAnimating)
                return;

            // Register a running animation.
            IsAnimating = true;

            // Set animation class.
            FocusClassName = "animate__tada";

            // Wait for animation to finish.
            await Task.Delay(600);

            // Remove animation class.
            FocusClassName = "";

            // Clear animation Register.
            IsAnimating = false;
        }

        /// <summary>
        /// Close the modal.
        /// </summary>
        public async void CloseModal()
        {
            // Set close's animation class.
            AnimateClassName = "animate__bounceOut animate__faster";

            // Wait for animation to finish.
            await Task.Delay(500);

            // Hide the modal.
            IsShown = false;

            // Force layout to refreshing component.
            StateHasChanged();

            // Reset open's animation class.
            AnimateClassName = $"{animationName} animate__fast";
        }

        /// <summary>
        /// Return confirm result and close the modal.
        /// </summary>
        public void Confirm()
        {
            Result = DialogResult.Ok;
            CloseModal();
        }

        /// <summary>
        /// Return cancel result and close the modal.
        /// </summary>
        public void Cancel()
        {
            Result = DialogResult.Cancel;
            CloseModal();
        }
    }
}
