using MiniTube.ModelsEAD;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniTube.View
{
    public partial class RegisterationViewIn : Window
    {
        private string? person;
        public RegisterationViewIn()
        {
            InitializeComponent();
            

        }
        public RegisterationViewIn(string s)
        {
            InitializeComponent();
            person = s;
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
        private void txt_username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_register_Click(sender, e);
            }

        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_register_Click(sender, e);
            }
        }
        private void txt_confirm_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_register_Click(sender, e);
            }
        }
        private void txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_register_Click(sender, e);
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            RegisterationView registerationView = new RegisterationView();
            registerationView.Show();
            this.Close();
        }
        static bool email_vlaidity(string mailAddress)
        {
            return Regex.IsMatch(mailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            string email=txt_email.Text;
            string uname=txt_username.Text;
            string pass=txt_password.Password;
            string cpass= txt_confirm_password.Password;

            if (txt_confirm_password.Password != "" && txt_password.Password != "" && txt_email.Text != "" && txt_username.Text != "")
            {
                if (email_vlaidity(email))
                {

                    if (pass == cpass)
                    {
                        
                        using(var context=new MiniTubeContext())
                        {
                            User? u= context.Users.FirstOrDefault(x=> x.Email==email && x.Password==pass);
                            if (u == null)
                            {
                                User user = new User();
                                user.Email = email;
                                user.Username = uname;
                                user.Password = pass;
                                user.Role = "Viewer";

                                context.Users.Add(user);
                                context.SaveChanges();

                                LoginViewIn loginViewIn = new LoginViewIn("Registeration Successful");
                                loginViewIn.Show();
                                this.Close();

                            }
                            else
                            {
                                System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                                txt_error.Foreground = brush;
                                txt_error.TextAlignment = TextAlignment.Center;
                                txt_error.Text = "User already exists";
                            }
                            
                        }

                    }
                    else
                    {
                        System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                        txt_error.Foreground = brush;
                        txt_error.TextAlignment = TextAlignment.Center;
                        txt_error.Text = "Passwords don't match";
                    }
                }
                else
                {
                    System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    txt_error.Foreground = brush;
                    txt_error.TextAlignment = TextAlignment.Center;
                    txt_error.Text = "Enter valid email";
                }
            }
            else
            {
                System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                txt_error.Foreground = brush;
                txt_error.TextAlignment = TextAlignment.Center;
                txt_error.Text = "Fill every box";
            }

        }
    }
}
