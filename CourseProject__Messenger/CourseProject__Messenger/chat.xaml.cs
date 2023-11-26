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
                Usercontrols userControls = new Usercontrols(CurrentUser.Email, CurrentUser.UserName); // Передача адреса электронной почты и имени пользователя при создании экземпляра Usercontrols
                List<(string name, string initials)> friends = userControls.GetFriendsList(CurrentUser.Email); // Передача адреса электронной почты для получения списка друзей

                foreach (var friend in friends)
                {
                    var friendItem = new CourseProject__Messenger.usercontrols.Item();
                    friendItem.Title = friend.name;
                    friendItem.TagName = userControls.GetInitials(friend.name); // Получение инициалов друга
                    friendItem.ContextMenu = this.Resources["ContextMenuFriends"] as ContextMenu;
                    friendsListControl.Children.Add(friendItem);
                }
            }
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
            // Обработчик события "удалить друга"
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
 
        private List<string> SearchUsersByEmail(string searchQuery)
        {
            List<string> searchResults = new List<string>();

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

                            // Собираем информацию в один блок текста
                            string userInfo = $"{userName}\n{userEmail}";

                            searchResults.Add(userInfo); // Добавляем информацию о пользователе в список
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

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = ""; // Очищаем содержимое при фокусировке
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                List<string> searchResults = SearchUsersByEmail(searchText);

                resultsListBox.Items.Clear();

                foreach (string result in searchResults)
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
