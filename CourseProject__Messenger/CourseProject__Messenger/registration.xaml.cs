using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using BCrypt.Net;

namespace CourseProject__Messenger
{
    public partial class registration : Window
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";
        string insertQuery = "INSERT INTO Users (Name, Email, ProfilePhoto, Password) VALUES (@Name, @Email, NULL, @Password)";

        public registration()
        {
            InitializeComponent();
        }

        private bool IsEmailExists(string email)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@(?:[a-zA-Z0-9]+\.)+[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsStrongPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score >= 3;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string email = txtEmail.Text;
            string enteredPassword = txtPass.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (IsEmailExists(email))
            {
                MessageBox.Show("Этот email уже зарегистрирован.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите действительный адрес электронной почты.");
                return;
            }

            if (!IsStrongPassword(enteredPassword))
            {
                MessageBox.Show("Пароль слишком слабый. Используйте более сложный пароль.");
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(enteredPassword);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", login);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.ExecuteNonQuery();
                }
                connection.Close();
                MessageBox.Show("Регистрация успешно завершена!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            this.Close();
            newWindow.Show();
        }
    }
}
