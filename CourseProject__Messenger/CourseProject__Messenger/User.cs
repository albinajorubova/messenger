using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CourseProject__Messenger.usercontrols
{
   
    public class Usercontrols
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public class UserInfo
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            // Другие свойства пользователя
        }
        public Usercontrols(string email, string userName)
        {
            Email = email;
            UserName = userName;
            UserID = UserID;
        }

        public class DialogInfo
        {
            public int DialogID { get; set; }
            public string DialogName { get; set; }
            public List<Usercontrols.FriendInfo> Participants { get; set; }
        }

        public class FriendInfo
        {
            public string Name { get; set; }
            public string Initials { get; set; }
            public string Email { get; set; }
        }

        public class Message
        {
            public int MessageID { get; set; }
            public int DialogID { get; set; }
            public int SenderID { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
            // Другие свойства сообщения
        }

        public Message GetLastMessageForDialog(int dialogID)
        {
            Message lastMessage = null;

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                string query = "SELECT * FROM Messages WHERE DialogID = @DialogID ORDER BY Timestamp DESC LIMIT 1";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DialogID", dialogID);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lastMessage = new Message
                            {
                                MessageID = Convert.ToInt32(reader["MessageID"]),
                                DialogID = Convert.ToInt32(reader["DialogID"]),
                                SenderID = Convert.ToInt32(reader["SenderID"]),
                                Content = reader["Content"].ToString(),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                                // ... other properties ...
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting last message for dialog: {ex.Message}");
            }

            return lastMessage;
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
                MessageBox.Show($"Ошибка поиска пользователей: {ex.Message}");
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
                MessageBox.Show("Ошибка получения Id");
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
                        MessageBox.Show($"Пользователь с почтой {email} не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

            return friendsList;
        }

        public List<DialogInfo> GetDialogsListWithInfo(string email)
        {
            List<DialogInfo> dialogsList = new List<DialogInfo>();

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

                        string getDialogsQuery = "SELECT d.DialogID, d.Name FROM Dialogs d JOIN DialogParticipants dp ON d.DialogID = dp.DialogID WHERE dp.UserID = @UserID";
                        MySqlCommand getDialogsCommand = new MySqlCommand(getDialogsQuery, connection);
                        getDialogsCommand.Parameters.AddWithValue("@UserID", userId);

                        using (MySqlDataReader reader = getDialogsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int dialogID = reader.GetInt32(0);
                                string dialogName = reader.GetString(1);

                                var dialogInfo = new DialogInfo
                                {
                                    DialogID = dialogID,
                                    DialogName = dialogName,
                                    Participants = GetDialogParticipants(dialogID)
                                };

                                dialogsList.Add(dialogInfo);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Пользователь с почтой {email} не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

            return dialogsList;
        }

        public List<Message> GetMessageListForDialog(int dialogID)
        {
            List<Message> messages = new List<Message>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                string query = "SELECT * FROM Messages WHERE DialogID = @DialogID ORDER BY Timestamp ASC";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DialogID", dialogID);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Message message = new Message
                            {
                                MessageID = Convert.ToInt32(reader["MessageID"]),
                                DialogID = Convert.ToInt32(reader["DialogID"]),
                                SenderID = Convert.ToInt32(reader["SenderID"]),
                                Content = reader["Content"].ToString(),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            };
                      

                            messages.Add(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при получении сообщений из базы данных
                MessageBox.Show("Ошибка при получении сообщений для диалога: " + ex.Message);
            }

            return messages;
        }


        public List<FriendInfo> GetDialogParticipants(int dialogID)
        {
            List<FriendInfo> participants = new List<FriendInfo>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Получаем список участников диалога по его DialogID
                    string getParticipantsQuery = "SELECT u.Name, u.Email FROM DialogParticipants dp JOIN Users u ON dp.UserID = u.UserID WHERE dp.DialogID = @DialogID";
                    MySqlCommand getParticipantsCommand = new MySqlCommand(getParticipantsQuery, connection);
                    getParticipantsCommand.Parameters.AddWithValue("@DialogID", dialogID);

                    using (MySqlDataReader reader = getParticipantsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string participantName = reader.GetString(0);
                            string initials = GetInitials(participantName);
                            string participantEmail = reader.GetString(1);

                            var participantInfo = new FriendInfo
                            {
                                Name = participantName,
                                Initials = initials,
                                Email = participantEmail
                            };

                            participants.Add(participantInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

            return participants;
        }
    }
}

