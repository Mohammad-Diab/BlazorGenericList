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
    }
}
