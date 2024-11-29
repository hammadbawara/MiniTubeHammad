using System.Windows;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        private int Id;
        public UserView()
        {
            InitializeComponent();
        }
        public UserView(int i)
        {
            InitializeComponent();
            Id = i;
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewIn loginViewIn = new LoginViewIn();
            loginViewIn.Show();
            this.Close();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_studio_Click(object sender, RoutedEventArgs e)
        {
            StudioView studioView = new StudioView(Id);
            studioView.Show();
            this.Close();
        }
    }
}
