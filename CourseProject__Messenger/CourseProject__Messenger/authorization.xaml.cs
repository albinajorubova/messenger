using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using static CourseProject__Messenger.Hash;

namespace CourseProject__Messenger
{
    /// <summary>
    /// Логика взаимодействия для authorization.xaml
    /// </summary>
    public partial class authorization : Window
    {

        string connectionString = "Server=sql11.freesqldatabase.com;Database=sql11655951;User ID=sql11655951;Password=BtYZLsm6hQ;Port=3306;";
        public authorization()
        {
            InitializeComponent();
         
        }

        private bool AuthenticateUser(string email, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT pass FROM users WHERE email = @email", connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    var hashedPasswordFromDatabase = command.ExecuteScalar() as string; // Получаем хеш пароля из базы данных
                    if (hashedPasswordFromDatabase != null && PasswordHasher.VerifyPassword(hashedPasswordFromDatabase, password))
                    {
                        return true; // Пароль верный, пользователь аутентифицирован
                    }
                }
            }
            return false; // Пользователь с таким email не найден или пароль не совпадает
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLogin.Text;
            string password = txtPass.Password;

            if (AuthenticateUser(email, password))
            {
                MessageBox.Show("Авторизация успешна!");
                // Здесь вы можете перенаправить пользователя на другую страницу или выполнить другие действия
            }
            else
            {
                MessageBox.Show("Неверный email или пароль.");
            }
        }
    }
}
