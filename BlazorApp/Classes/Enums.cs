using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public enum DialogMode
    {
        Add = 0,
        Edit = 1
    }

    public enum PropertyType
    {
        Text = 0,
        Number = 1,
        Email = 2,
        Phone = 3,
        Date = 4
    }

    public enum ConfirmType
    {
        Default = 0,
        Delete = 1
    }

    public enum DialogResult
    {
        Undefiend = 0,
        Ok = 1,
        Cancel = 2
    }

    public enum LoadingContentType
    {
        Grid = 1,
    }
}
