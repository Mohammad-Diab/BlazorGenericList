using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class User
    {
        internal static List<string> Headers { get; set; } = new List<string>() { "Id", "Username", "Full Name", "Birthday", "Email", "Phone" };

        internal static string ItemType { get; set; } = "User";

        internal static int DefaultSortedBy { get; set; } = 1;

        private static List<Property> itemProperties;
        public static List<Property> ItemProperties
        {
            get
            {
                itemProperties = itemProperties ?? GetProperties();
                return itemProperties;
            }
            set
            {
                itemProperties = value;
            }
        }

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

        static int currentId = 0;

        public int Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        internal bool IsMatch(string filterString)
        {
            if (string.IsNullOrEmpty(filterString))
                return true;
            filterString = filterString.ToLower();
            return Username.ToLower().Contains(filterString) || FullName.ToLower().Contains(filterString) || Birthday.ToString("yyyy MMM dd").ToLower().Contains(filterString) ||
                Phone.ToLower().Contains(filterString) || Email.ToLower().Contains(filterString);
        }

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
            currentId++;
            Id = currentId;
        }

        public User(string username, string fullName, string birthday, string email, string phone)
        {
            currentId++;
            Id = currentId;
            Username = username;
            FullName = fullName;
            DateTime date;
            DateTime.TryParse(birthday, out date);
            Birthday = date;
            Phone = phone;
            Email = email;
        }

        public User(string id, string username, string fullName, string birthday, string email, string phone)
        {
            int IntId;
            if (int.TryParse(id, out IntId))
            {
                currentId++;
                IntId = currentId;
            }
            Id = IntId;
            Username = username;
            FullName = fullName;
            DateTime date;
            DateTime.TryParse(birthday, out date);
            Birthday = date;
            Phone = phone;
            Email = email;
        }
    }
}
