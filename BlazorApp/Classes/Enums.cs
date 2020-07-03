using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{

    public enum GridType
    {
        Edit = 0,
        MultiSelect = 1,
        SingleSelect = 2,
    }

    public enum PropertyType
    {
        Text = 0,
        Number = 1,
        Email = 2,
        Phone = 3,
        Date = 4
    }

    public enum SelectedStatus
    {
        None = 0,
        Some = 1,
        All = 2,
    }

    public enum NotificationType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3
    }

    public enum DropdownType
    {
        Button = 0,
        Header = 1,
        Divider = 2
    }

    public enum LoadingContentType
    {
        Grid = 1,
        SelectGrid = 2
    }

    #region Modals

    public enum DialogMode
    {
        Add = 0,
        Edit = 1
    }

    public enum ModalConfirmButton
    {
        Default = 0,
        Add = 1,
        Edit = 2,
        Delete = 3
    }

    public enum DialogResult
    {
        Undefiend = 0,
        Ok = 1,
        Cancel = 2
    }

    public enum ModalShowAnimation
    {
        None = 0,
        BounceIn = 1,
        BounceInDown = 2
    }

    #endregion

}
