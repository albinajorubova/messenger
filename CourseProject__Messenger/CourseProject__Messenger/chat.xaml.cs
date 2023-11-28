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
                    friendItem.TagName = userControls.GetInitials(friend.name);
                    friendsListControl.Children.Add(friendItem);

                    MenuItem deleteMenuItem = new MenuItem();
                    deleteMenuItem.Header = "Удалить друга";

                    string friendEmail = friendItem.Email; // Сохраняем адрес друга в локальной переменной

                    deleteMenuItem.Click += (sender, e) =>
                    {
                        // Отображение MessageBox для подтверждения удаления
                        MessageBoxResult result = MessageBox.Show($"Действительно хотите удалить {friend.name}?", "Удаление друга", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (RemoveFriendByEmail(friendEmail))
                            {
                                friendsListControl.Children.Remove(friendItem);
                              
                            }
                        }
                    };

                    friendItem.ContextMenu = new ContextMenu();
                    friendItem.ContextMenu.Items.Add(deleteMenuItem);
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
        private void Item_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Обработчик события
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
            if (sender is MenuItem menuItem && menuItem.DataContext is CourseProject__Messenger.usercontrols.Item friendItem)
            {
                string friendEmail = friendItem.Email;
                string friendName = friendItem.Title;

                // Всплывающее окно подтверждения удаления
                MessageBoxResult result = MessageBox.Show($"Действительно хотите удалить {friendName}?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Удаление друга
                    if (RemoveFriendByEmail(friendEmail))
                    {
                        friendsListControl.Children.Remove(friendItem);
                    }
                }
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
                    Console.WriteLine("Не удалось получить UserID друга.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления друга: {ex.Message}");
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
                Console.WriteLine($"Ошибка при удалении дружбы: {ex.Message}");
                Console.WriteLine($"Ошибка при удалении дружбы: {ex.StackTrace}"); 
                return false; // Ошибка при удалении
            }
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
                Console.WriteLine($"Ошибка получения UserID: {ex.Message}");
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
        private List<Item> SearchUsersByEmail(string searchQuery)
        {
            List<Item> searchResults = new List<Item>();

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
                            string userName = reader.GetString(0);
                            string userEmail = reader.GetString(1);

                            var friendItem = new Item();
                            friendItem.Title = userName;
                            friendItem.Email = userEmail; // Установка электронной почты в свойство Email
                                                          // Добавление элемента Item с информацией в список результатов
                            searchResults.Add(friendItem);
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

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                List<Item> searchResults = SearchUsersByEmail(searchText);

                resultsListBox.Items.Clear();

                foreach (Item result in searchResults)
                {
                    resultsListBox.Items.Add(result);
                }
            }
            else
            {
                resultsListBox.Items.Clear();
            }
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

    }

}
