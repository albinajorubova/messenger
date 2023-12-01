using CourseProject__Messenger.usercontrols;
using System.Diagnostics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CourseProject__Messenger
{
    /// <summary>
    /// Логика взаимодействия для chat.xaml
    /// </summary>
    public partial class chat : Window
    {

        public Usercontrols CurrentUser { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public chat()
        {
            InitializeComponent();
            LoadUsersFromDatabase();
            LoadUserData();
            this.DataContext = authorization.CurrentUser;
            SetTextBlockUsername(CurrentUser?.UserName);

            if (CurrentUser != null)
            {
                Usercontrols userControls = new Usercontrols(CurrentUser.Email, CurrentUser.UserName);
                List<(string name, string initials)> friends = userControls.GetFriendsList(CurrentUser.Email);

                foreach (var friend in friends)
                {
                    var friendItem = new CourseProject__Messenger.usercontrols.Item();
                    friendItem.Title = friend.name;
                    friendItem.TagName = friend.initials;
                    friendsListControl.Children.Add(friendItem);

                    MenuItem deleteMenuItem = new MenuItem();
                    deleteMenuItem.Header = "Удалить друга";

                    string friendName = friend.name; // Сохраняем имя друга в локальной переменной

                    deleteMenuItem.Click += (sender, e) =>
                    {
                        // Отображение MessageBox для подтверждения удаления
                        MessageBoxResult result = MessageBox.Show($"Действительно хотите удалить {friendName}?", "Удаление друга", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Вызываем метод RemoveFriendByName с именем друга
                            if (RemoveFriendByEmail(friendName))
                            {
                                // Удаляем друга из списка
                                friendsListControl.Children.Remove(friendItem);
                            }
                        }
                    };

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Items.Add(deleteMenuItem);

                    friendItem.ContextMenu = contextMenu;
                }
            }
        }



        private void UserData()
        {
           string currentUserName = CurrentUser?.UserName;
           string currentUserEmail = CurrentUser?.Email;      
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
      
        public void SetTextBlockUsername(string username)
        {
            if (NameTitle != null)
            {
                NameTitle.Text = username;
            }
        }


        private void LoadUserData()
        {
            string userDataFilePath = @"C:\Users\lenovo\source\repos\messenger\userdata.txt";

            if (System.IO.File.Exists(userDataFilePath))
            {
                string userData = System.IO.File.ReadAllText(userDataFilePath);
                string[] userDataArray = userData.Split(',');

                if (userDataArray.Length == 2)
                {
                    CurrentUser = new Usercontrols(userDataArray[0], userDataArray[1]);
                    // Здесь можно установить текущего пользователя в чате или других местах
                }
            }
        }
        private void MenuButton_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
        private void MenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void FriendsBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Логика для кнопки сообщений
            friendsListControl.Visibility = Visibility.Visible;
            FriendsItem2.Visibility = Visibility.Visible;
            BlockItems.Visibility = Visibility.Collapsed;
            BlockItems2.Visibility = Visibility.Collapsed;
            FriendsBtn.IsActive = true ;
            MessageBtn.IsActive = false;

        }

        private void MessageBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Логика для кнопки друзей
            friendsListControl.Visibility = Visibility.Collapsed;
            FriendsItem2.Visibility = Visibility.Collapsed;
            BlockItems.Visibility = Visibility.Visible;
            BlockItems2.Visibility = Visibility.Visible;
            FriendsBtn.IsActive = false;
            MessageBtn.IsActive = true;
         
        }
        private void MenuButton_Loaded_3(object sender, RoutedEventArgs e)
        {
        }
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события "удалить диалог"
        }
        private void DialogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события "добавить диалог"
        }
        private void DeleteFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private int GetUserIDByEmail(string email)
        {
            int userId = -1; // Значение по умолчанию, если не найден

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUserIdQuery = "SELECT UserID FROM Users WHERE Email = @Email";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@Email", email);

                    var result = getUserIdCommand.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении UserID: {ex.Message}");
            }

            return userId;
        }


        private void NewFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события "добавить друга"
        }
        private void BlockItems_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Обработчик события
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        bool Ismaximised=false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount== 2)
            {
                if(!Ismaximised)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1250;
                    this.Height = 830;
                    Ismaximised = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                   
                    Ismaximised = true;
                }

            }
        }
     
        private void FriendsBtn_Loaded(object sender, RoutedEventArgs e)
        {
      
        }
   
        private void MessageBtn_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private void Item_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFriend(string friendEmail)
        {
            int currentUserId = GetUserIDByEmail(Email); // Получаем UserID текущего пользователя по его Email

            try
            {
                int friendUserId = GetUserIDByEmail(friendEmail); // Получаем UserID друга по его Email
                if (friendUserId != -1)
                {
                    if (RemoveFriendshipByUserIDs(currentUserId, friendUserId))
                    {
                        // Удаляем друга из списка
                        var friendItem = friendsListControl.Children.OfType<Item>()
                                                        .FirstOrDefault(item => item.Email == friendEmail);
                        if (friendItem != null)
                        {
                            friendsListControl.Children.Remove(friendItem);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось получить UserID друга.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления друга: {ex.Message}");
            }
        }

        private bool RemoveFriendByEmail(string friendEmail)
        {
            int currentUserId = GetUserIDByEmail(Email); // Получаем UserID текущего пользователя по его Email

            try
            {
                int friendUserId = GetUserIDByEmail(friendEmail); // Получаем UserID друга по его Email
                if (friendUserId != -1)
                {
                    return RemoveFriendshipByUserIDs(currentUserId, friendUserId);
                }
                else
                {
                    MessageBox.Show("Не удалось получить UserID друга.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления друга: {ex.Message}");
                return false; // Ошибка при удалении
            }
        }


        private bool RemoveFriendshipByUserIDs(int userId1, int userId2)
        {
            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteFriendshipQuery = "DELETE FROM Friends WHERE (UserID1 = @UserID1 AND UserID2 = @UserID2) OR (UserID1 = @UserID2 AND UserID2 = @UserID1)";
                    Console.WriteLine($"SQL Query: {deleteFriendshipQuery}");
                    MySqlCommand deleteFriendshipCommand = new MySqlCommand(deleteFriendshipQuery, connection);
                    deleteFriendshipCommand.Parameters.AddWithValue("@UserID1", userId1);
                    deleteFriendshipCommand.Parameters.AddWithValue("@UserID2", userId2);
                    deleteFriendshipCommand.ExecuteNonQuery();

                    return true; // Успешно удалено
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении дружбы: {ex.Message}");
                MessageBox.Show($"Ошибка при удалении дружбы: {ex.StackTrace}");
                return false; // Ошибка при удалении
            }
        }


        private void SaveUserData()
        {
            // Ваш код сохранения данных о пользователе в файл
            string userDataFilePath = @"C:\Users\lenovo\source\repos\messenger\userdata.txt";
            string userData = $"{CurrentUser.Email},{CurrentUser.UserName}";

            System.IO.File.WriteAllText(userDataFilePath, userData);
        }
        private void MenuButton_Loaded_1(object sender, RoutedEventArgs e)
        {

            SaveUserData(); // Сохранение данных о пользователе при выходе
            MainWindow newWindow = new MainWindow();
            this.Close();
            newWindow.Show();

        }


        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                List<User> searchResults = SearchUsersByEmail(searchText);
                resultsListBox.ItemsSource = searchResults;
            }
            else
            {
                resultsListBox.ItemsSource = null; // Очистить список при пустом запросе
            }
        }

        private List<User> SearchUsersByEmail(string searchQuery)
        {
            List<User> searchResults = new List<User>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string searchUsersQuery = "SELECT Name, Email FROM Users WHERE Email LIKE @SearchQuery";
                    MySqlCommand searchUsersCommand = new MySqlCommand(searchUsersQuery, connection);
                    searchUsersCommand.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%");

                    using (MySqlDataReader reader = searchUsersCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string userName = reader.GetString("Name");
                            string userEmail = reader.GetString("Email");

                            var user = new User { Name = userName, Email = userEmail };
                            searchResults.Add(user);
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

        private void LoadUsersFromDatabase()
        {
            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUsersQuery = "SELECT Name, Email FROM Users";
                    MySqlCommand getUsersCommand = new MySqlCommand(getUsersQuery, connection);

                    using (MySqlDataReader reader = getUsersCommand.ExecuteReader())
                    {
                        List<User> users = new List<User>();

                        while (reader.Read())
                        {
                            string userName = reader.GetString("Name");
                            string userEmail = reader.GetString("Email");

                            var user = new User { Name = userName, Email = userEmail };
                            users.Add(user);
                        }

                        resultsListBox.ItemsSource = users;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }
        private List<User> LoadAllUsersFromDatabase()
        {
            List<User> users = new List<User>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUsersQuery = "SELECT Name, Email FROM Users";
                    MySqlCommand getUsersCommand = new MySqlCommand(getUsersQuery, connection);

                    using (MySqlDataReader reader = getUsersCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string userName = reader.GetString("Name");
                            string userEmail = reader.GetString("Email");

                            var user = new User { Name = userName, Email = userEmail };
                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }

            return users;
        }
        private void AddFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (resultsListBox.SelectedItem != null && resultsListBox.SelectedItem is User selectedUser)
            {
                AddFriend(selectedUser.Name, selectedUser.Email);
            }
        }

        private void AddFriend(string friendName, string friendEmail)
        {
            try
            {
                int currentUserId = GetUserIDByEmail(CurrentUser.Email);
                int friendUserId = GetUserIDByEmail(friendEmail);

                if (currentUserId != -1 && friendUserId != -1)
                {
                    string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string checkFriendshipQuery = "SELECT COUNT(*) FROM Friends WHERE (UserID1 = @UserID1 AND UserID2 = @UserID2) OR (UserID1 = @UserID2 AND UserID2 = @UserID1)";
                        MySqlCommand checkFriendshipCommand = new MySqlCommand(checkFriendshipQuery, connection);
                        checkFriendshipCommand.Parameters.AddWithValue("@UserID1", currentUserId);
                        checkFriendshipCommand.Parameters.AddWithValue("@UserID2", friendUserId);

                        int existingFriendshipCount = Convert.ToInt32(checkFriendshipCommand.ExecuteScalar());

                        if (existingFriendshipCount == 0)
                        {
                            string addFriendQuery = "INSERT INTO Friends (UserID1, UserID2) VALUES (@UserID1, @UserID2)";
                            MySqlCommand addFriendCommand = new MySqlCommand(addFriendQuery, connection);
                            addFriendCommand.Parameters.AddWithValue("@UserID1", currentUserId);
                            addFriendCommand.Parameters.AddWithValue("@UserID2", friendUserId);

                            int rowsAffected = addFriendCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Пользователь успешно добавлен в друзья.");
                                LoadUsersFromDatabase(); // Обновляем список после добавления друга
                            }
                        }
                        else
                        {
                            MessageBox.Show("Этот пользователь уже является вашим другом.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка при поиске пользователя.");
                }
            }   
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления друга: {ex.Message}");
            }
        }

        public class User
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        private void Item_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                // Активация контекстного меню
                ContextMenu contextMenu = ((TextBlock)sender).ContextMenu;
                contextMenu.IsOpen = true;
            }
        }


    }

}
