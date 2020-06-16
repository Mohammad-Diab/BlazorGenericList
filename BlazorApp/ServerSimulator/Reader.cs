using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public static class Reader
    {
        static List<User> AllUsres { get; set; }

        public async static Task<List<User>> GetAllUsers()
        {
            if (AllUsres == null)
            {
                await Task.Delay(2000);
                List<User> All = new List<User>();
                All.Add(new User() { Username = "samer", FullName = "Samer Ali", Email = "SamerAli@mail.com", Phone = "(502) 837-8387", Birthday = new DateTime(1960, 1, 15) });
                All.Add(new User() { Username = "ahmad", FullName = "Ahmad Kareem", Email = "A.Kareem@mail.com", Phone = "(650) 627-0862", Birthday = new DateTime(2000, 5, 20) });
                All.Add(new User() { Username = "fadi", FullName = "Fadi Zaki", Email = "FadiZaki@mail.com", Phone = "(209) 822-9795", Birthday = new DateTime(1995, 12, 1) });
                All.Add(new User() { Username = "mohammad", FullName = "Mohammad Hassan", Email = "M.Hassan@mail.com", Phone = "(818) 690-5381", Birthday = new DateTime(1975, 6, 8) });
                All.Add(new User() { Username = "shadi", FullName = "Shadi Nizzar", Email = "ShadiNezzar@mail.com", Phone = "(507) 288-0307", Birthday = new DateTime(1992, 1, 1) });

                AllUsres = All;
            }
            return AllUsres;
        }
    }
}
