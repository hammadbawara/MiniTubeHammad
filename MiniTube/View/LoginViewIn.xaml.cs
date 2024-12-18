using MahApps.Metro.Controls;
using MiniTube.ModelsEAD;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for LoginViewIn.xaml
    /// Provides a user interface for user login.
    /// </summary>
    public partial class LoginViewIn : MetroWindow
    {
        // ----- Default constructor -----
        public LoginViewIn()
        {
            InitializeComponent();
        }

        // ----- Constructor with error message -----
        public LoginViewIn(string errorMessage)
        {
            InitializeComponent();
            SetErrorMessage(errorMessage); // Set error message
        }

        // ----- Validates the email format -----
        static bool EmailValidity(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        // ----- Sets the error message in the UI -----
        private void SetErrorMessage(string message)
        {
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            txt_error.Foreground = brush;
            txt_error.TextAlignment = TextAlignment.Center;
            txt_error.Text = message; // Set the error message
        }

        // ----- Allows the window to be dragged -----
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // ----- Minimizes the window -----
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Minimize the window
        }

        // ----- Closes the application -----
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Shutdown the application
        }

        // ----- Handles the login button click event -----
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_email.Text) && !string.IsNullOrEmpty(txt_password.Password))
                {
                    if (EmailValidity(txt_email.Text))
                    {
                        using (var context = new MiniTubeContext())
                        {
                            User? user = context.Users.FirstOrDefault(x => x.Email == txt_email.Text && x.Password == txt_password.Password);
                            if (user != null)
                            {
                                UserView userView = new UserView(user.UserId);
                                userView.Show(); // Show user view
                                this.Close(); // Close the login window
                            }
                            else
                            {
                                SetErrorMessage("Invalid credentials"); // Set error message for invalid credentials
                            }
                        }
                    }
                    else
                    {
                        SetErrorMessage("Email is invalid"); // Set error message for invalid email
                    }
                }
                else
                {
                    SetErrorMessage("Fill all boxes"); // Set error message for empty fields
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}"); // Handle exceptions
            }
        }

        // ----- Handles the registration button click event -----
        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    RegisterationViewIn registrationView = new RegisterationViewIn();
                    registrationView.Show(); // Show registration view
                this.Close(); // Close the login window
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to registration: {ex.Message}"); // Handle exceptions
            }
        }

        // ----- Handles the reset button click event -----
        private void btn_reset_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetView resetView = new ResetView();
                resetView.Show(); // Show reset view
                this.Close(); // Close the login window
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to reset: {ex.Message}"); // Handle exceptions
            }
        }

        // ----- Handles the Enter key press in the password field -----
        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_login_Click(sender, e); // Trigger login on Enter
            }
        }

        // ----- Handles the Enter key press in the email field -----
        private void txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_login_Click(sender, e); // Trigger login on Enter
            }
        }
    }
}