using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BCrypt.Net;

namespace CourseProject__Messenger
{
    internal class DataAccess
    {
        private string connectionString;

        public DataAccess(string connection)
        {
            connectionString = connection;
        }

        private SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public class MyData
        {
            public int id { get; set; }
            public string nikname { get; set; }
            public string email { get; set; }
            public string pass { get; set; }
            
        }

        public List<MyData> GetData()
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand("SELECT * FROM users", connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<MyData> data = new List<MyData>();
                while (reader.Read())
                {
                    MyData item = new MyData
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        nikname = reader.GetString(reader.GetOrdinal("nikname")),
                        email = reader.GetString(reader.GetOrdinal("email")),
                     
                    };
                    data.Add(item);
                }
                return data;
            }
        }
        private bool UserExists(string email)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM users WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        public bool AuthenticateUser(string email, string password)
        {
            try
            {
                using (SqlConnection connection = GetOpenConnection())
                using (SqlCommand command = new SqlCommand("SELECT pass FROM users WHERE email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    string hashedPassword = (string)command.ExecuteScalar();

                    if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                    {
                        return true; // Пароль совпадает, пользователь аутентифицирован
                    }
                }
            }
            catch (SqlException ex)
            {
                // Обработка ошибки базы данных
                Console.WriteLine("Ошибка базы данных: " + ex.Message);
            }

            return false; // В случае ошибки или неверного пароля, пользователь не аутентифицирован
        }

        public bool RegisterUser(string username, string password, string email )
        {
            try
            {
                // Проверка, существует ли пользователь с заданным email
                if (UserExists(email))
                {

                    return false;
                }

                // Хеширование пароля перед сохранением
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                using (SqlConnection connection = GetOpenConnection())
                using (SqlCommand command = new SqlCommand("INSERT INTO users (nikname, pass, email) VALUES (@Username, @Password, @Email)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword); // Сохраняем хешированный пароль
                    command.Parameters.AddWithValue("@Email", email);
         
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        
                catch (SqlException ex)
            {
                Console.WriteLine("Ошибка базы данных: " + ex.Message);
                Console.WriteLine("Код ошибки: " + ex.ErrorCode);
                Console.WriteLine("Дополнительные детали: " + ex.ToString());
            }
        

            return false;  // В случае ошибки, регистрация не удалась
        }

        public MyData GetUserInfo(string email)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand("SELECT * FROM users WHERE email = @Email", connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MyData user = new MyData
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            nikname = reader.GetString(reader.GetOrdinal("nikname")),
                            email = reader.GetString(reader.GetOrdinal("email")),
             
                        };
                        return user;
                    }
                }
            }

            return new MyData(); // Пользователь с заданным логином не найден
        }
    }
}
