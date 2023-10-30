using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using BCrypt.Net;
using static CourseProject__Messenger.Hash;

namespace CourseProject__Messenger
{
    public partial class registration : Window
    {
        string connectionString = "Server=sql11.freesqldatabase.com;Database=sql11657900;User ID=sql11657900;Password=SC7ljfmGqs;Port=3306;";
        string insertQuery = "INSERT INTO users (nikname, email, pass) VALUES (@nikname, @email, @pass)";

        public registration()
        {
            InitializeComponent();
        }

        private bool IsEmailExists(string email)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM users WHERE email = @email", connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
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
            string password = txtPass.Password;

            if (IsEmailExists(email))
            {
                MessageBox.Show("Этот email уже зарегистрирован.");
            }
            else if (!IsStrongPassword(password))
            {
                MessageBox.Show("Пароль слишком слабый. Используйте более сложный пароль.");
            }
            else
            {
                string hashedPassword = PasswordHasher.HashPassword(password); // Хешируем пароль с использованием класса Hash

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nikname", login);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@pass", hashedPassword); // Сохраняем хеш пароля
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    MessageBox.Show("Регистрация успешно завершена!");
                }
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
