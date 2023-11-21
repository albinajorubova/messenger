﻿using System;
using System.Windows;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using CourseProject__Messenger.usercontrols;

namespace CourseProject__Messenger
{
    public partial class authorization : Window
    {
        // Объект, хранящий информацию о текущем пользователе
        public static Usercontrols CurrentUser { get; private set; }

        string connectionString = "Server=127.0.0.1;Port=3306;Database=messenger;Uid=root;";

        public authorization()
        {
            InitializeComponent();
        }

        private bool AuthenticateUser(string email, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("SELECT Password, Name FROM Users WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var hashedPasswordFromDatabase = reader["Password"] as string;
                        var username = reader["Name"] as string;

                        if (hashedPasswordFromDatabase != null && BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDatabase))
                        {
                            // Создаем объект пользователя при успешной аутентификации
                            CurrentUser = new Usercontrols(email, username);
                            return true; // Пароль верный, пользователь аутентифицирован
                        }
                    }
                }
            }
            return false; // Пользователь с таким email не найден или пароль не совпадает
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLogin.Text;
            string password = txtPass.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите email и пароль.");
                return;
            }

            if (AuthenticateUser(email, password))
            {
                chat newWindow = new chat();
                newWindow.CurrentUser = CurrentUser; // Передача объекта CurrentUser в новое окно chat
                this.Close();
                newWindow.Show();
            }
            else
            {
                MessageBox.Show("Неверный email или пароль.");
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
