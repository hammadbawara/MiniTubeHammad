using System;
using System.Windows;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for RegisterationView.xaml
    /// </summary>
    public partial class RegisterationView : Window
    {
        // ----- Constructor -----
        public RegisterationView()
        {
            InitializeComponent();
        }

        // ----- Mouse Down Event for Dragging the Window -----
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // ----- Minimize Button Click -----
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // ----- Close Button Click -----
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // ----- Back Button Click -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginViewIn loginView = new LoginViewIn();
                loginView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}");
            }
        }

        // ----- Select Button Click -----
        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterationViewIn registerationView = new RegisterationViewIn();
                registerationView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to registration view: {ex.Message}");
            }
        }
    }
}