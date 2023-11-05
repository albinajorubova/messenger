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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProject__Messenger.usercontrols
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
        }
    }
    //public ImageSource image
    //{
    //    get { return (ImageSource)GetValue(ImageProperty); }
    //    set { SetValue(ImageProperty, value); }
    //}

    //public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageProperty), typeof(Item));
}
