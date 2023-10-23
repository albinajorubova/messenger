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
using System.Configuration;
using CourseProject__Messenger;

namespace CourseProject__Messenger
{
  
    public partial class authorization : Window
    {
        private DataAccess dataAccess;
        public authorization()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            dataAccess = new DataAccess(connectionString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtLogin.Text; // Получить email пользователя из поля ввода
            string password = txtPass.Password; // Получить пароль пользователя из поля ввода

            // Вызов метода для аутентификации
            bool isAuthenticated = dataAccess.AuthenticateUser(email, password);

            if (isAuthenticated)
            {
                // Пользователь успешно аутентифицирован, выполните действия
                MessageBox.Show("Вы успешно вошли!");
            }
            else
            {
                // Аутентификация не удалась, выполните действия
                MessageBox.Show("Ошибка аутентификации. Пожалуйста, проверьте введенные данные.");
            }
        }
    }
}
