using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CourseProject__Messenger.usercontrols
{
    public class Usercontrols
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public Usercontrols(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }
        private List<string> SearchUsersByEmail(string searchQuery)
        {
            List<string> searchResults = new List<string>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string searchUsersQuery = "SELECT Email FROM Users WHERE Email LIKE @SearchQuery";
                    MySqlCommand searchUsersCommand = new MySqlCommand(searchUsersQuery, connection);
                    searchUsersCommand.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%");

                    using (MySqlDataReader reader = searchUsersCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string userEmail = reader.GetString(0);
                            searchResults.Add(userEmail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска пользователей: {ex.Message}");
            }

            return searchResults;
        }
        public string GetInitials(string name)
        {
            string[] parts = name.Split(' ');
            if (parts.Length > 1)
            {
                return $"{parts[0][0]}{parts[1][0]}";
            }
            else
            {
                return $"{parts[0][0]}";
            }
        }

        public List<(string, string)> GetFriendsList(string email)
        {
            List<(string, string)> friendsList = new List<(string, string)>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUserIdQuery = "SELECT UserID FROM Users WHERE Email = @Email";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@Email", email);
                    object userIdResult = getUserIdCommand.ExecuteScalar();

                    if (userIdResult != null)
                    {
                        int userId = Convert.ToInt32(userIdResult);

                        string getFriendsQuery = "SELECT u.Name FROM Friends f JOIN Users u ON f.UserID2 = u.UserID WHERE f.UserID1 = @UserID";
                        MySqlCommand getFriendsCommand = new MySqlCommand(getFriendsQuery, connection);
                        getFriendsCommand.Parameters.AddWithValue("@UserID", userId);

                        using (MySqlDataReader reader = getFriendsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string friendName = reader.GetString(0);
                                string initials = GetInitials(friendName);

                                friendsList.Add((friendName, initials));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Пользователь с почтой {email} не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return friendsList;
        }
    }
}

