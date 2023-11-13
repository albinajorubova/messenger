using CourseProject__Messenger.usercontrols;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CourseProject__Messenger
{
    /// <summary>
    /// Логика взаимодействия для chat.xaml
    /// </summary>
    public partial class chat : Window
    {
       

        public chat()
        {
            InitializeComponent();

        }
        private void MenuButton_Loaded(object sender, RoutedEventArgs e)
        {
            // Обработчик события Loaded для MenuButton
          
        }
        private void MenuButton_Loaded_1(object sender, RoutedEventArgs e)
        {
        }
        private void MenuButton_Loaded_2(object sender, RoutedEventArgs e)
        {
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

        private void MenuButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // При нажатии на кнопку меню, устанавливаем активную вкладку
            if (sender is MenuButton menuButton)
            {
                switch (menuButton.Title.ToLower())
                {
                    case "friends":
                        MainTabControl.SelectedIndex = 0; // Индекс 0 соответствует вкладке "Friends"
                        break;
                    case "messsege":
                        MainTabControl.SelectedIndex = 1; // Индекс 1 соответствует вкладке "Message" (проверьте правильность названия)
                        break;
                    case "settings":
                        MainTabControl.SelectedIndex = 2; // Индекс 2 соответствует вкладке "Settings"
                        break;
                        // Добавьте обработку других кнопок, если необходимо
                }
            }
        }
    }

}
