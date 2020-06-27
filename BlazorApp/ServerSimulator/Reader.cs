using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public static class Reader
    {
        static List<User> AllUsres { get; set; }
        static string locker = Guid.NewGuid().ToString();
        public async static Task<List<User>> GetAllUsers()
        {
            await Task.Delay(2000);
            lock (locker)
            {
                if (AllUsres == null)
                {
                    List<User> All = new List<User>();
                    All.Add(new User() { Username = "samer", FullName = "Samer Ali", Email = "SamerAli@mail.com", Phone = "(502) 837-8387", Birthday = new DateTime(1960, 1, 15) });
                    All.Add(new User() { Username = "ahmad", FullName = "Ahmad Kareem", Email = "A.Kareem@mail.com", Phone = "(650) 627-0862", Birthday = new DateTime(2000, 5, 20) });
                    All.Add(new User() { Username = "fadi", FullName = "Fadi Zaki", Email = "FadiZaki@mail.com", Phone = "(209) 822-9795", Birthday = new DateTime(1995, 12, 1) });
                    All.Add(new User() { Username = "mohammad", FullName = "Mohammad Hassan", Email = "M.Hassan@mail.com", Phone = "(818) 690-5381", Birthday = new DateTime(1975, 6, 8) });
                    All.Add(new User() { Username = "shadi", FullName = "Shadi Nizzar", Email = "ShadiNezzar@mail.com", Phone = "(507) 288-0307", Birthday = new DateTime(1992, 1, 1) });

                    AllUsres = All;
                }
            }
            return AllUsres;
        }

        internal async static Task<(bool result, string errorMessage)> AddUser(User newUser)
        {
            await Task.Delay(500);
            AllUsres.Add(newUser);
            return (true, "");
        }

        internal static async Task<(bool result, string errorMessage)> EditUser(int id, User newUser)
        {
            await Task.Delay(500);

            int oldUserIndex = AllUsres.FindIndex((x) => x.Id == id);
            if (oldUserIndex > -1)
            {
                AllUsres[oldUserIndex] = newUser;
                return (true, "");
            }
            else
            {
                return (false, "Can't find user with this id!");
            }
        }

        internal async static Task<(bool result, string errorMessage)> DeleteUser(int id)
        {
            await Task.Delay(500);

            int userIndex = AllUsres.FindIndex((x) => x.Id == id);
            if (userIndex > -1)
            {
                AllUsres.RemoveAt(userIndex);
                return (true, "");
            }
            else
            {
                return (false, "Can't find user with this id!");
            }
        }

    }
}
