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

        public List<string> GetFriendsList()
        {
            List<string> friendsList = new List<string>();

            try
            {
                // Здесь используйте вашу строку подключения к базе данных
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Получение списка друзей пользователя по его электронной почте
                    string getUserIdQuery = "SELECT UserID FROM Users WHERE Email = @Email";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@Email", Email);
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
                                friendsList.Add(friendName);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Пользователь с почтой {Email} не найден.");
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
