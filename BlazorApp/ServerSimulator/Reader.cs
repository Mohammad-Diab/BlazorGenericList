using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp
{
    public static class Reader
    {

        /// <summary>
        /// To get random deley time to simulate a data store latency. This field is read-only.
        /// </summary>
        private static Random RandomNumber { get; } = new Random();

        /// <summary>
        /// Cache All users after reading it.
        /// </summary>
        static List<User> AllUsres { get; set; }

        /// <summary>
        /// To prevent more than one request to read from data store.
        /// </summary>
        static string locker = Guid.NewGuid().ToString();

        /// <summary>
        /// Read All Users from data store then cache it.
        /// </summary>
        /// <returns>List of all users in the data store.</returns>
        public async static Task<List<User>> GetAllUsers()
        {
            // Simulate a data store read latency.
            await Task.Delay(RandomNumber.Next(100));

            // Prevent threads from accessing this block when having AllUsres in cache.
            if (AllUsres == null)
            {
                // Prevent other threads from accessing this block
                lock (locker)
                {
                    // Check again that AllUsres is not exist in cache.
                    if (AllUsres == null)
                    {
                        // Simulate reading from data store.
                        List<User> All = new List<User>();
                        All.Add(new User() { Username = "samer", FullName = "Samer Ali", Email = "SamerAli@mail.com", Phone = "(502) 837-8387", Birthday = new DateTime(1960, 1, 15) });
                        All.Add(new User() { Username = "ahmad", FullName = "Ahmad Kareem", Email = "A.Kareem@mail.com", Phone = "(650) 627-0862", Birthday = new DateTime(2000, 5, 20) });
                        All.Add(new User() { Username = "fadi", FullName = "Fadi Zaki", Email = "FadiZaki@mail.com", Phone = "(209) 822-9795", Birthday = new DateTime(1995, 12, 1) });
                        All.Add(new User() { Username = "mohammad", FullName = "Mohammad Hassan", Email = "M.Hassan@mail.com", Phone = "(818) 690-5381", Birthday = new DateTime(1975, 6, 8) });
                        All.Add(new User() { Username = "shadi", FullName = "Shadi Nizzar", Email = "ShadiNezzar@mail.com", Phone = "(507) 288-0307", Birthday = new DateTime(1992, 1, 1) });
                        AllUsres = All;
                    }
                }
            }
                
            return AllUsres;
        }

        /// <summary>
        /// Add user to data store.
        /// </summary>
        /// <param name="newUser">Object represents the user to be added.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> AddUser(User newUser)
        {
            // Simulate a data store write latency.
            await Task.Delay(RandomNumber.Next(50));

            // Adding user to store.
            AllUsres.Add(newUser);

            return (true, "");
        }

        /// <summary>
        /// Update an existing user's information.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <param name="newUser">Object represents the new user information.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal static async Task<(bool result, string errorMessage)> EditUser(int id, User newUser)
        {
            // Simulate a data store write latency.
            await Task.Delay(RandomNumber.Next(50));

            // Seraching for user with the giving id.
            int oldUserIndex = AllUsres.FindIndex((x) => x.Id == id);

            if (oldUserIndex > -1)
            {
                // Updating user information.
                AllUsres[oldUserIndex].Update(newUser);
                return (true, "");
            }
            else
            {
                return (false, "Can't find user with this id!");
            }
        }

        /// <summary>
        /// Deleting an existing user.
        /// </summary>
        /// <param name="id">Target user's id.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> DeleteUser(int id)
        {
            // Simulate a data store write latency.
            await Task.Delay(RandomNumber.Next(50));

            // Seraching for user with the giving id.
            int userIndex = AllUsres.FindIndex((x) => x.Id == id);
            if (userIndex > -1)
            {
                // Deleting the user.
                AllUsres.RemoveAt(userIndex);

                return (true, "");
            }
            else
            {
                return (false, "Can't find user with this id!");
            }
        }

        /// <summary>
        /// Deleting multi users.
        /// </summary>
        /// <param name="Ids">List of users' ids to delete.</param>
        /// <returns>
        /// <para>Tuble of bool, string.</para> 
        /// <para>Fisrt is result: represent result statue.</para>
        /// <para>Second is errorMessage: represent error message when something went wrong.</para>
        /// </returns>
        internal async static Task<(bool result, string errorMessage)> DeleteMultiUsers(List<int> Ids)
        {
            // Simulate a data store write latency.
            await Task.Delay(RandomNumber.Next(50 * Ids.Count));

            string failedUsers = "";
            for (int i = 0; i < Ids.Count; i++)
            {
                // Seraching for user with the giving id.
                int userIndex = AllUsres.FindIndex((x) => x.Id == Ids[i]);
                if (userIndex > -1)
                {
                    // Deleting the user.
                    AllUsres.RemoveAt(userIndex);
                }
                else
                {
                    failedUsers += $"'{Ids[i]}', ";
                }
            }

            // Check if all users deleted successfully or not, in this case return an error message contains failed deleted users' ids.
            if (string.IsNullOrEmpty(failedUsers))
            {
                return (true, "");
            }
            else
            {
                failedUsers = failedUsers.Substring(0, failedUsers.Length - 2);
                return (false, $"Can't find user with these ids {failedUsers}!");
            }
        }

    }
}
