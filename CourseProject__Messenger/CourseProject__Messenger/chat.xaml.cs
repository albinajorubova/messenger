using System.Windows;
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

        private void AccountMenuButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
