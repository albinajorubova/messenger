using CourseProject__Messenger.usercontrols;
using System.Diagnostics;
using System;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using static CourseProject__Messenger.usercontrols.Usercontrols;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;


namespace CourseProject__Messenger
{
    /// <summary>
    /// Логика взаимодействия для chat.xaml
    /// </summary>
    public partial class chat : Window
    {
       
        private string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

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
            resultsListBox.ItemsSource = null;
            LoadDialogs();
            LoadFriends(); // Вызываем метод для загрузки списка друзей с информацией о почте
        }

        private void LoadFriends()
        {
            friendsListControl.Children.Clear(); // Очищаем предыдущий список друзей перед обновлением

            if (CurrentUser != null)
            {
                Usercontrols userControls = new Usercontrols(CurrentUser.Email, CurrentUser.UserName);
                List<Usercontrols.FriendInfo> friends = userControls.GetFriendsListWithInfo(CurrentUser.Email);

                foreach (var friend in friends)
                {
                    var friendItem = new CourseProject__Messenger.usercontrols.Item();
                    friendItem.Title = friend.Name;
                    friendItem.TagName = friend.Initials;
                    friendsListControl.Children.Add(friendItem);

                    // Сохраняем почту друга в Tag элемента управления, чтобы использовать ее в будущем
                    friendItem.Tag = friend.Email;

                    MenuItem deleteMenuItem = new MenuItem();
                    deleteMenuItem.Header = "Удалить друга";

                    MenuItem dialogMenuItem = new MenuItem();
                    dialogMenuItem.Header = "Начать диалог";

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Items.Add(deleteMenuItem);
                    contextMenu.Items.Add(dialogMenuItem);

                    friendItem.ContextMenu = contextMenu;

                    // Привязываем обработчик события клика по пункту "Удалить друга"
                    deleteMenuItem.Click += (sender, e) =>
                    {
                        if (friendItem.Tag != null && friendItem.Tag is string)
                        {
                            string emailToDelete = friendItem.Tag as string;
                            DeleteFriend(emailToDelete); // Вызываем метод удаления друга
                        }
                    };
                    dialogMenuItem.Click += (sender, e) =>
                    {
                        if (friendItem.Tag != null && friendItem.Tag is string)
                        {
                            string emailToStartDialog = friendItem.Tag as string;
                            StartDialog(emailToStartDialog); // Вызываем метод начала диалога
                        }
                    };
                }
            }
        }
        private void StartDialog(string friendEmail)
        {
            try
            {
                int currentUserId = GetUserIDByEmail(CurrentUser.Email);
                int friendUserId = GetUserIDByEmail(friendEmail);

                if (currentUserId != -1 && friendUserId != -1)
                {
                    MessageBoxResult result = MessageBox.Show($"Вы действительно хотите начать диалог с {friendEmail}?", "Подтверждение начала диалога", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            string getFriendNameQuery = "SELECT Name FROM Users WHERE Email = @FriendEmail";
                            MySqlCommand getFriendNameCommand = new MySqlCommand(getFriendNameQuery, connection);
                            getFriendNameCommand.Parameters.AddWithValue("@FriendEmail", friendEmail);
                            string friendName = getFriendNameCommand.ExecuteScalar()?.ToString();

                            if (friendName != null)
                            {
                                string checkDialogQuery = "SELECT DialogID FROM DialogParticipants WHERE (UserID = @UserID1 OR UserID = @UserID2) GROUP BY DialogID HAVING COUNT(DISTINCT UserID) = 2";
                                MySqlCommand checkDialogCommand = new MySqlCommand(checkDialogQuery, connection);
                                checkDialogCommand.Parameters.AddWithValue("@UserID1", currentUserId);
                                checkDialogCommand.Parameters.AddWithValue("@UserID2", friendUserId);

                                object existingDialogId = checkDialogCommand.ExecuteScalar();

                                if (existingDialogId == null)
                                {
                                    string createDialogQuery = "INSERT INTO Dialogs (Name) VALUES (@Name)";
                                    MySqlCommand createDialogCommand = new MySqlCommand(createDialogQuery, connection);
                                    createDialogCommand.Parameters.AddWithValue("@Name", friendName);
                                    int rowsAffected = createDialogCommand.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        int dialogId = (int)createDialogCommand.LastInsertedId;

                                        string addParticipantsQuery = "INSERT INTO DialogParticipants (DialogID, UserID) VALUES (@DialogID, @UserID1), (@DialogID, @UserID2)";
                                        MySqlCommand addParticipantsCommand = new MySqlCommand(addParticipantsQuery, connection);
                                        addParticipantsCommand.Parameters.AddWithValue("@DialogID", dialogId);
                                        addParticipantsCommand.Parameters.AddWithValue("@UserID1", currentUserId);
                                        addParticipantsCommand.Parameters.AddWithValue("@UserID2", friendUserId);

                                        int participantsAffected = addParticipantsCommand.ExecuteNonQuery();

                                        if (participantsAffected > 0)
                                        {
                                            MessageBox.Show("Диалог начат.");
                                            LoadDialogs();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Не удалось добавить участников диалога.");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Не удалось начать диалог.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Диалог уже существует.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Не удалось получить имя друга.");
                            }
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
                MessageBox.Show($"Ошибка начала диалога: {ex.Message}");
            }
        }



        private void DeleteDialog(int dialogId)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить этот диалог?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string deleteParticipantsQuery = "DELETE FROM DialogParticipants WHERE DialogID = @DialogID";
                        MySqlCommand deleteParticipantsCommand = new MySqlCommand(deleteParticipantsQuery, connection);
                        deleteParticipantsCommand.Parameters.AddWithValue("@DialogID", dialogId);
                        int participantsDeleted = deleteParticipantsCommand.ExecuteNonQuery();

                        string deleteDialogQuery = "DELETE FROM Dialogs WHERE DialogID = @DialogID";
                        MySqlCommand deleteDialogCommand = new MySqlCommand(deleteDialogQuery, connection);
                        deleteDialogCommand.Parameters.AddWithValue("@DialogID", dialogId);
                        int dialogDeleted = deleteDialogCommand.ExecuteNonQuery();

                        if (dialogDeleted > 0 && participantsDeleted > 0)
                        {
                            MessageBox.Show("Диалог успешно удален.");
                            LoadDialogs(); // Обновляем список диалогов после удаления
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить диалог.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении диалога: {ex.Message}");
            }
        }



        private void DeleteFriend(string friendEmail)
        {
            try
            {
                int currentUserId = GetUserIDByEmail(CurrentUser.Email);
                int friendUserId = GetUserIDByEmail(friendEmail);

                if (currentUserId != -1 && friendUserId != -1)
                {
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить друга с почтой {friendEmail}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            string deleteFriendQuery = "DELETE FROM Friends WHERE (UserID1 = @CurrentUser AND UserID2 = @Friend) OR (UserID1 = @Friend AND UserID2 = @CurrentUser)";
                            MySqlCommand deleteFriendCommand = new MySqlCommand(deleteFriendQuery, connection);
                            deleteFriendCommand.Parameters.AddWithValue("@CurrentUser", currentUserId);
                            deleteFriendCommand.Parameters.AddWithValue("@Friend", friendUserId);

                            int rowsAffected = deleteFriendCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Друг с почтой {friendEmail} удален из списка друзей.");
                                LoadFriends(); // Обновляем список друзей после удаления друга
                            }
                            else
                            {
                                MessageBox.Show($"Не удалось удалить друга с почтой {friendEmail}.");
                            }
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
                MessageBox.Show($"Ошибка при удалении друга: {ex.Message}");
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
                    // Здесь установите Email для текущего пользователя и получите UserID
                    CurrentUser.Email = userDataArray[0];
                    CurrentUser.UserID = GetUserIDByEmail(CurrentUser.Email).ToString();
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
            FriendsBtn.IsActive = true;
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

                    // Выводим SQL-запрос в консоль для отладки
                    Console.WriteLine($"SQL Query: {getUserIdCommand.CommandText}");

                    var result = getUserIdCommand.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show($"UserID not found for Email: {email}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении UserID: {ex.Message}");
                // Выводим информацию об ошибке в консоль для отладки
                Console.WriteLine(ex.StackTrace);
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
        bool Ismaximised = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!Ismaximised)
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


        private int GetUserIDByName(string name)
        {
            int userId = -1; // Значение по умолчанию, если не найден

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getUserIdQuery = "SELECT UserID FROM Users WHERE Name = @Name";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@Name", name);

                    var result = getUserIdCommand.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show($"UserID not found for Name: {name}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении UserID: {ex.Message}");
            }

            return userId;
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
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите добавить пользователя {friendName} в друзья?", "Подтверждение добавления друга", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
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
                                    LoadFriends(); // Обновляем список друзей после добавления друга
                                }
                            }
                            else
                            {
                                MessageBox.Show("Этот пользователь уже является вашим другом.");
                            }
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

        private int selectedDialogID;
        private void LoadDialogs()
        {
            BlockItems.Children.Clear();

            if (CurrentUser != null)
            {
                Usercontrols userControls = new Usercontrols(CurrentUser.Email, CurrentUser.UserName);
                List<Usercontrols.DialogInfo> dialogs = userControls.GetDialogsListWithInfo(CurrentUser.Email);

                foreach (var dialog in dialogs)
                {
                    var dialogItem = new CourseProject__Messenger.usercontrols.Item();
                    dialogItem.Title = dialog.DialogName;

                    Message lastMessage = userControls.GetLastMessageForDialog(dialog.DialogID);
                    if (lastMessage != null)
                    {
                        dialogItem.Message = lastMessage.Content;
                    }
                    else
                    {
                        dialogItem.Message = "No messages";
                    }

                    dialogItem.Color = new SolidColorBrush(Color.FromRgb(115, 175, 255));

                    BlockItems.Children.Add(dialogItem);

                    dialogItem.Tag = dialog.DialogID;

                    MenuItem deleteMenuItem = new MenuItem();
                    deleteMenuItem.Header = "Delete dialog";

                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.Items.Add(deleteMenuItem);

                    dialogItem.ContextMenu = contextMenu;

                    deleteMenuItem.Click += (sender, e) =>
                    {
                        if (dialogItem.Tag != null && dialogItem.Tag is int)
                        {
                            int dialogIDToDelete = (int)dialogItem.Tag;
                            DeleteDialog(dialogIDToDelete);
                        }
                    };

                    dialogItem.MouseLeftButtonDown += async (sender, e) =>
                    {
                        if (dialogItem.Tag != null && dialogItem.Tag is int)
                        {
                            int dialogIDToShowMessages = (int)dialogItem.Tag;
                            selectedDialogID = dialogIDToShowMessages;

                          

                            List<Message> messagesForDialog = userControls.GetMessageListForDialog(dialogIDToShowMessages);
                            StringBuilder messageText = new StringBuilder();
                            foreach (var message in messagesForDialog)
                            {
                                messageText.AppendLine($"SenderID: {message.SenderID}, Content: {message.Content}, Timestamp: {message.Timestamp}");
                            }

                           
                            DisplayMessagesForDialog(selectedDialogID);
                        }
                    };
                }
            }
        }


        private UserInfo GetUserInfoByUserID(int userID, MySqlConnection connection)
        {
            UserInfo userInfo = null;

            try
            {
                string getUserInfoQuery = "SELECT * FROM Users WHERE UserID = @UserID";
                MySqlCommand getUserInfoCommand = new MySqlCommand(getUserInfoQuery, connection);
                getUserInfoCommand.Parameters.AddWithValue("@UserID", userID);

                connection.Open(); // Открываем соединение

                using (MySqlDataReader reader = getUserInfoCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userInfo = new UserInfo
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            UserName = reader["UserName"].ToString(),
                            Email = reader["Email"].ToString(),
                            // Другие свойства, которые вы получаете из базы данных
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении информации о пользователе: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Закрываем соединение, если оно было открыто
                }
            }

            return userInfo;
        }

        private List<UserInfo> GetDialogParticipants(int dialogID)
        {
            List<UserInfo> participants = new List<UserInfo>();

            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getParticipantsQuery = "SELECT UserID FROM DialogParticipants WHERE DialogID = @DialogID";
                    MySqlCommand getParticipantsCommand = new MySqlCommand(getParticipantsQuery, connection);
                    getParticipantsCommand.Parameters.AddWithValue("@DialogID", dialogID);

                    using (MySqlDataReader reader = getParticipantsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userID = reader.GetInt32(0);

                            UserInfo userInfo = GetUserInfoByUserID(userID, connection);
                            if (userInfo != null)
                            {
                                participants.Add(userInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении участников диалога: {ex.Message}");
            }

            return participants;
        }


        private void DisplayMessagesForDialog(int dialogID)
        {
            try
            {
                Usercontrols userControls = new Usercontrols(CurrentUser.Email, CurrentUser.UserName);
                List<Usercontrols.DialogInfo> dialogs = userControls.GetDialogsListWithInfo(CurrentUser.Email);

                var dialog = dialogs.FirstOrDefault(d => d.DialogID == dialogID);
                if (dialog != null)
                {
                    DialogNameTextBlock.Text = dialog.DialogName;
                }
                else
                {
                    DialogNameTextBlock.Text = "Диалог не найден";
                }

                List<Message> messagesForDialog = userControls.GetMessageListForDialog(dialogID);
                int currentUserId = GetUserIDByEmail(CurrentUser.Email);

                // Очистка предыдущего содержимого чата
                ChatItems.Children.Clear();

                TextBlock dateTextBlock = new TextBlock();
                dateTextBlock.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                dateTextBlock.TextAlignment = TextAlignment.Center;
                dateTextBlock.Margin = new Thickness(0, 10, 0, 10);
                dateTextBlock.FontSize = 14;
                ChatItems.Children.Add(dateTextBlock);

                foreach (var message in messagesForDialog)
                {
                    var timeTextBlock = new TextBlock();
                    timeTextBlock.Text = message.Timestamp.ToString("hh:mm tt");

                    if (message.SenderID == currentUserId)
                    {
                        var myMessageChat = new MyMessageChat();
                        myMessageChat.Message = message.Content;

                        timeTextBlock.Style = (Style)Application.Current.Resources["timeTextRight"];

                        ChatItems.Children.Add(myMessageChat);
                        ChatItems.Children.Add(timeTextBlock);
                    }
                    else
                    {
                        var userChat = new UserChat();
                        var messageChat = new MessageChat();
                        messageChat.Color = new SolidColorBrush(Color.FromRgb(255, 130, 163));
                        messageChat.Message = message.Content;

                        timeTextBlock.Style = (Style)Application.Current.Resources["timeText"];

                        ChatItems.Children.Add(userChat);
                        ChatItems.Children.Add(messageChat);
                        ChatItems.Children.Add(timeTextBlock);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении сообщений: {ex.Message}");
            }
        }






        private void SendMessage(int dialogID, string messageContent, int senderID, int receiverID)
        {
            try
            {
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание запроса на вставку сообщения в таблицу Messages
                    string insertMessageQuery = "INSERT INTO Messages (DialogID, SenderID, ReceiverID, Content, Timestamp) " +
                                                "VALUES (@DialogID, @SenderID, @ReceiverID, @Content, NOW())";

                    MySqlCommand insertMessageCommand = new MySqlCommand(insertMessageQuery, connection);
                    insertMessageCommand.Parameters.AddWithValue("@DialogID", dialogID);
                    insertMessageCommand.Parameters.AddWithValue("@SenderID", senderID);
                    insertMessageCommand.Parameters.AddWithValue("@ReceiverID", receiverID);
                    insertMessageCommand.Parameters.AddWithValue("@Content", messageContent);

                    // Выполнение запроса на вставку сообщения
                    int rowsAffected = insertMessageCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Сообщение успешно отправлено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при отправке сообщения.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }


        private void DeleteDialogWithMessages(int dialogID)
        {
            try
            {
                // Создание подключения к базе данных
                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

                // Удаление всех сообщений для данного диалога
                string deleteMessagesQuery = "DELETE FROM Messages WHERE DialogID = @DialogID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand deleteMessagesCommand = new MySqlCommand(deleteMessagesQuery, connection);
                    deleteMessagesCommand.Parameters.AddWithValue("@DialogID", dialogID);

                    connection.Open();
                    deleteMessagesCommand.ExecuteNonQuery();
                }

                // Удаление диалога после удаления сообщений
                string deleteDialogQuery = "DELETE FROM Dialogs WHERE DialogID = @DialogID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand deleteDialogCommand = new MySqlCommand(deleteDialogQuery, connection);
                    deleteDialogCommand.Parameters.AddWithValue("@DialogID", dialogID);

                    connection.Open();
                    deleteDialogCommand.ExecuteNonQuery();
                }

                LoadDialogs();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Ошибка при удалении диалога: " + ex.Message);
            }
        }
        private List<int> GetReceiverIDsForDialog(int dialogID, int currentUserId)
        {
            List<int> receiverIDs = new List<int>();

            try
            {
                if (dialogID <= 0 || currentUserId <= 0)
                {
                    MessageBox.Show("Некорректные значения dialogID или currentUserId");
                    return receiverIDs; // Возвращаем пустой список, если dialogID или currentUserId некорректны
                }

                string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getReceiverIDsQuery = "SELECT UserID FROM DialogParticipants WHERE DialogID = @DialogID AND UserID != @CurrentUserId";
                    MySqlCommand getReceiverIDsCommand = new MySqlCommand(getReceiverIDsQuery, connection);
                    getReceiverIDsCommand.Parameters.AddWithValue("@DialogID", dialogID);
                    getReceiverIDsCommand.Parameters.AddWithValue("@CurrentUserId", currentUserId);

                    using (MySqlDataReader reader = getReceiverIDsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int receiverID = reader.GetInt32(0);
                            receiverIDs.Add(receiverID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки запроса
                MessageBox.Show($"Ошибка при получении receiverID: {ex.Message}");
            }

            return receiverIDs;
        }



        public void OpenDialog(int dialogIDToOpen)
        {

        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            int dialogID = selectedDialogID;
            int currentUserId = GetUserIDByEmail(CurrentUser.Email);

            int senderID = currentUserId;
            List<int> receiverIDs = GetReceiverIDsForDialog(dialogID, currentUserId);

            string messageContent = txtMessage.Text;

            foreach (int receiverID in receiverIDs)
            {
                SendMessage(dialogID, messageContent, senderID, receiverID);
            }
            DisplayMessagesForDialog(dialogID);
        }





    }

}

