using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class User
    {
        static int currentId = 0;

        public int Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

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
            if(int.TryParse(id,out IntId))
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
