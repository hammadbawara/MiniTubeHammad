using MiniTube.ModelsEAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for LoginViewIn.xaml
    /// </summary>
    public partial class LoginViewIn : Window
    {

      

        public LoginViewIn()
        {
            InitializeComponent();

        }

       
        public LoginViewIn(string s)
        {
            InitializeComponent();
            
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            txt_error.Foreground = brush;
            txt_error.TextAlignment = TextAlignment.Center;
            txt_error.Text = s;
        }

        static bool email_vlaidity(string mailAddress)
        {
            return Regex.IsMatch(mailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
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

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if(txt_email.Text!="" && txt_password.Password!="")
            {
                if(email_vlaidity(txt_email.Text))
                {
                    using (var context = new MiniTubeContext())
                    {
                        User? obj = context.Users.FirstOrDefault(x => x.Email == txt_email.Text && x.Password == txt_password.Password);
                        if (obj != null)
                        {
                            
                            UserView userView = new UserView(obj.UserId);
                            userView.Show();
                            this.Close();
                        }
                        else
                        {
                            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                            txt_error.Foreground = brush;
                            txt_error.TextAlignment = TextAlignment.Center;
                            txt_error.Text = "Invalid credentials";
                        }
                    }
                }
                else
                {
                    System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    txt_error.Foreground = brush;
                    txt_error.TextAlignment = TextAlignment.Center;
                    txt_error.Text = "Email is invalid";
                }

            }
            else
            {
                System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                txt_error.Foreground = brush;
                txt_error.TextAlignment = TextAlignment.Center;
                txt_error.Text = "Fill all boxes";
            }



        }

        private void btn_register_Click(object sender, MouseButtonEventArgs e)
        {
            RegisterationView registerationView = new RegisterationView();
            registerationView.Show();
            this.Close();
        }

        private void btn_reset_Click(object sender, MouseButtonEventArgs e)
        {
            ResetView resetView = new ResetView();
            resetView.Show();
            this.Close();
        }


        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_login_Click(sender, e);
            }
        }

        private void txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_login_Click(sender, e);
            }
        }
    }
}
