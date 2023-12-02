using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CourseProject__Messenger.usercontrols
{
    public class Usercontrols
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public Usercontrols(string email, string userName)
        {
            Email = email;
            UserName = userName;
            UserID = UserID;
        }

        public class FriendInfo
        {
            public string Name { get; set; }
            public string Initials { get; set; }
            public string Email { get; set; }
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

        public int GetUserID()
        {
            int UserID = -1; // Возвращаемое значение по умолчанию

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUserIdQuery = "SELECT UserID FROM Users WHERE Email = @Email";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@Email", Email);

                    object result = getUserIdCommand.ExecuteScalar();

                    if (result != null)
                    {
                        UserID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если таковые возникнут
            }

            return UserID;
        }

        public List<FriendInfo> GetFriendsListWithInfo(string email)
        {
            List<FriendInfo> friendsList = new List<FriendInfo>();

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

                        string getFriendsQuery = "SELECT u.Name, u.Email FROM Friends f JOIN Users u ON f.UserID2 = u.UserID WHERE f.UserID1 = @UserID";
                        MySqlCommand getFriendsCommand = new MySqlCommand(getFriendsQuery, connection);
                        getFriendsCommand.Parameters.AddWithValue("@UserID", userId);

                        using (MySqlDataReader reader = getFriendsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string friendName = reader.GetString(0);
                                string initials = GetInitials(friendName);
                                string friendEmail = reader.GetString(1);

                                var friendInfo = new FriendInfo
                                {
                                    Name = friendName,
                                    Initials = initials,
                                    Email = friendEmail
                                };

                                friendsList.Add(friendInfo);
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

