using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorList
{
    public class User
    {
        static int currentId = 0;

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public User()
        {
            currentId++;
            Id = currentId;
        }

    }
}
