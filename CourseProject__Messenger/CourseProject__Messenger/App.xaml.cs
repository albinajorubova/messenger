using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject__Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool isLoggedIn = false;

        // Метод для загрузки состояния авторизации при запуске приложения
        private bool LoadLoggedInState()
        {
            return isLoggedIn;
        }

        // Метод для сохранения состояния авторизации при закрытии приложения
        private void SaveLoggedInState(bool isLoggedIn)
        {
            // Сохранение состояния в файл или другое хранилище
            // Пример сохранения в файл:
            // File.WriteAllText("logged-in-state.txt", isLoggedIn.ToString());
        }

        // Проверка состояния авторизации при запуске приложения
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            isLoggedIn = LoadLoggedInState();

            if (isLoggedIn)
            {
                chat newWindow = new chat();
                newWindow.Show();
            }
            else
            {
                authorization newWindow = new authorization();
                newWindow.Show();
            }
        }

        // Сохранение состояния авторизации при закрытии приложения
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveLoggedInState(isLoggedIn);
        }
    }
}
