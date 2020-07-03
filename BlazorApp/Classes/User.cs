using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorApp
{
    /// <summary>
    /// An example to show how to use GenericGrid. a simple user informations.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Headers to use in GenericGrid.
        /// </summary>
        internal static List<string> Headers { get; set; } = new List<string>() { "Id", "Username", "Full Name", "Birthday", "Email", "Phone" };

        /// <summary>
        /// Item Name.
        /// </summary>
        internal static string ItemType { get; set; } = "User";

        /// <summary>
        /// The default SortBy value.
        /// </summary>
        internal static int DefaultSortedBy { get; set; } = 1;

        /// <summary>
        /// Current object Properties list.
        /// </summary>
        private static List<Property> itemProperties;
        public static List<Property> ItemProperties
        {
            get
            {
                itemProperties = itemProperties ?? GetProperties();
                return itemProperties;
            }
        }

        // Return a list of properties for this object.
        static List<Property> GetProperties()
        {
            List<Property> result = new List<Property>();
            result.Add(new Property("Username", PropertyType.Text, "Username"));
            result.Add(new Property("FullName", PropertyType.Text, "Full Name"));
            result.Add(new Property("Birthday", PropertyType.Date, "Birthday"));
            result.Add(new Property("Email", PropertyType.Email, "Email"));
            result.Add(new Property("Phone", PropertyType.Phone, "Phone"));
            return result;
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// A required function to determine if current user match a given string.
        /// </summary>
        /// <param name="filterString">String to look for.</param>
        /// <returns>True if current user match the string, otherwise False.</returns>
        internal bool IsMatch(string filterString)
        {
            if (string.IsNullOrEmpty(filterString))
                return true;
            filterString = filterString.ToLower();
            return Username.ToLower().Contains(filterString) || FullName.ToLower().Contains(filterString) || Birthday.ToString("yyyy MMM dd").ToLower().Contains(filterString) ||
                Phone.ToLower().Contains(filterString) || Email.ToLower().Contains(filterString);
        }

        /// <summary>
        /// A required function to return sortby property name.
        /// </summary>
        /// <param name="SortBy">An integer represent column id to sort the list based on.</param>
        /// <returns>A property name that match the given integer.</returns>
        internal static string GetSortByProp(int SortBy) =>
            SortBy switch
            {
                1 => "Id",
                2 => "Username",
                3 => "FullName",
                4 => "Birthday",
                5 => "Email",
                6 => "Phone",
                _ => "Id"
            };

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User(string username, string fullName, string birthday, string email, string phone)
        {
            Id = Guid.NewGuid().ToString();
            Username = username;
            FullName = fullName;
            DateTime date;
            DateTime.TryParse(birthday, out date);
            Birthday = date;
            Phone = phone;
            Email = email;
        }

        /// <summary>
        /// A required function to create an instance of current class using activator. It should has all properties.
        /// </summary>
        public User(string id, string username, string fullName, string birthday, string email, string phone)
        {
            Id = id;
            Username = username;
            FullName = fullName;
            DateTime date;
            DateTime.TryParse(birthday, out date);
            Birthday = date;
            Phone = phone;
            Email = email;
        }

        /// <summary>
        /// To update user information.
        /// </summary>
        internal void Update(User newUser)
        {
            Username = newUser.Username;
            FullName = newUser.FullName;
            Birthday = newUser.Birthday;
            Phone = newUser.Phone;
            Email = newUser.Email;
        }
    }
}
