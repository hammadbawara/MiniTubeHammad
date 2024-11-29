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

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for RegisterationView.xaml
    /// </summary>
    public partial class RegisterationView : Window
    {
        public RegisterationView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            LoginViewIn loginView = new LoginViewIn();
            loginView.Show();
            this.Close();

        }

        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            RegisterationViewIn registerationView = new RegisterationViewIn();
            registerationView.Show();
            this.Close();
        }
    }
}
