using MahApps.Metro.Controls;
using MiniTube.ModelsEAD;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for ResetView.xaml
    /// </summary>
    public partial class ResetView : MetroWindow
    {
        // ----- Constructor -----
        public ResetView()
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

        // ----- Key Down Event for Text Boxes -----
        private void txt_email_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);
        private void txt_password_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);
        private void txt_confirm_password_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);

        // ----- Handle Enter Key Press -----
        private void HandleEnterKey(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_reset_Click(this, new RoutedEventArgs());
            }
        }

        // ----- Email Validity Check -----
        static bool EmailValidity(string mailAddress)
        {
            return Regex.IsMatch(mailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        // ----- Reset Button Click -----
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            // ----- Validate Input -----
            if (string.IsNullOrWhiteSpace(txt_email.Text) ||
                string.IsNullOrWhiteSpace(txt_password.Password) ||
                string.IsNullOrWhiteSpace(txt_confirm_password.Password))
            {
                ShowError("Fill every box");
                return;
            }

            if (!EmailValidity(txt_email.Text))
            {
                ShowError("Email is invalid");
                return;
            }

            if (txt_password.Password != txt_confirm_password.Password)
            {
                ShowError("Passwords don't match");
                return;
            }

            // ----- Reset Password -----
            try
            {
                using (var context = new MiniTubeContext())
                {
                    User? user = context.Users.FirstOrDefault(x => x.Email == txt_email.Text);
                    if (user != null)
                    {
                        user.Password = txt_password.Password;
                        context.SaveChanges();

                        LoginViewIn loginViewIn = new LoginViewIn("Password is reset");
                        loginViewIn.Show();
                        this.Close();
                    }
                    else
                    {
                        ShowError("User  not found");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error resetting password: {ex.Message}");
            }
        }

        // ----- Show Error Message -----
        private void ShowError(string message)
        {
            txt_error.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            txt_error.TextAlignment = TextAlignment.Center;
            txt_error.Text = message;
        }

        // ----- Back Button Click -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            LoginViewIn loginViewIn = new LoginViewIn();
            loginViewIn.Show();
            this.Close();
        }
    }
}