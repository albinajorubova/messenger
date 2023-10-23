using System;
using System.Collections.Generic;
using System.Configuration;
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
using CourseProject__Messenger;


namespace CourseProject__Messenger
{

    public partial class registration : Window
    {
        private DataAccess dataAccess;
        public registration()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            dataAccess = new DataAccess(connectionString);
        }

     

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtLogin.Text; // Получить имя из поля ввода
            string email = txtEmail.Text; // Получить адрес электронной почты из поля ввода
            string password = txtPass.Password; // Получить пароль из поля ввода
            try
            {
                // Вызов метода для регистрации
                bool isRegistered = dataAccess.RegisterUser(name, password, email);

            if (isRegistered)
            {
                // Регистрация прошла успешно, выполните необходимые действия
                MessageBox.Show("Регистрация успешна!");
            }
            else
            {
                // Регистрация не удалась, выполните необходимые действия
                MessageBox.Show("Ошибка регистрации. Пожалуйста, проверьте введенные данные.");
            }
            }
            catch (Exception ex)
            {
                // Обработка ошибки при взаимодействии с базой данных
                MessageBox.Show("Ошибка при регистрации: " + ex.Message);
            }
        }
    }
}
