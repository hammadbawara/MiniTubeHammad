using MiniTube.ModelsEAD;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MiniTube.View
{
    public partial class RegisterationViewIn : Window
    {
        private string? person;

        // ----- Constructor -----
        public RegisterationViewIn()
        {
            InitializeComponent();
        }

        public RegisterationViewIn(string s)
        {
            InitializeComponent();
            person = s;
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
        private void txt_username_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);
        private void txt_password_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);
        private void txt_confirm_password_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);
        private void txt_email_KeyDown(object sender, KeyEventArgs e) => HandleEnterKey(e);

        // ----- Handle Enter Key Press -----
        private void HandleEnterKey(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_register_Click(this, new RoutedEventArgs());
            }
        }

        // ----- Back Button Click -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterationView registerationView = new RegisterationView();
                registerationView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}");
            }
        }

        // ----- Email Validity Check -----
        static bool EmailValidity(string mailAddress)
        {
            return Regex.IsMatch(mailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        // ----- Register Button Click -----
        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            string email = txt_email.Text;
            string uname = txt_username.Text;
            string pass = txt_password.Password;
            string cpass = txt_confirm_password.Password;

            // ----- Validate Input -----
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(uname) ||
                string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(cpass))
            {
                ShowError("Fill every box");
                return;
            }

            if (!EmailValidity(email))
            {
                ShowError("Enter valid email");
                return;
            }

            if (pass != cpass)
            {
                ShowError("Passwords don't match");
                return;
            }

            // ----- Register User -----
            try
            {
                using (var context = new MiniTubeContext())
                {
                    User? existingUser = context.Users.FirstOrDefault(x => x.Email == email);
                    if (existingUser == null)
                    {
                        User user = new User
                        {
                            Email = email,
                            Username = uname,
                            Password = pass,
                            Role = "Viewer"
                        };

                        context.Users.Add(user);
                        context.SaveChanges();

                        LoginViewIn loginViewIn = new LoginViewIn("Registration Successful");
                        loginViewIn.Show();
                        this.Close();
                    }
                    else
                    {
                        ShowError("User  already exists");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Registration failed: {ex.Message}");
            }
        }

        // ----- Show Error Message -----
        private void ShowError(string message)
        {
            txt_error.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            txt_error.TextAlignment = TextAlignment.Center;
            txt_error.Text = message;
        }
    }
}