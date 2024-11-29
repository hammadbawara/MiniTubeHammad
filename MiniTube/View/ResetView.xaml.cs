using System;
using System.Collections.Generic;
using System.Linq;
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
using MiniTube.ModelsEAD;


namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for ResetView.xaml
    /// </summary>
    public partial class ResetView : Window
    {
        public ResetView()
        {
            InitializeComponent();
        }

        private void txt_confirm_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_reset_Click(sender, e);
            }

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
        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_reset_Click(sender, e);
            }
        }

        private void txt_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_reset_Click(sender, e);
            }
        }

        static bool email_vlaidity(string mailAddress)
        {
            return Regex.IsMatch(mailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
           if(txt_email.Text!="" && txt_password.Password!="" && txt_confirm_password.Password!="")
            {
                if (email_vlaidity(txt_email.Text))
                {
                    if (txt_password.Password == txt_confirm_password.Password)
                    {
                        using(var context=new MiniTubeContext() )
                        {
                            User? obj=context.Users.FirstOrDefault(x=>x.Email==txt_email.Text);
                            if (obj != null)
                            {
                                obj.Password= txt_password.Password;
                                context.SaveChanges();
                                LoginViewIn loginViewIn = new LoginViewIn("Password is reset");    
                                loginViewIn.Show();
                                this.Close();
                            }
                            else
                            {
                                System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                                txt_error.Foreground = brush;
                                txt_error.TextAlignment = TextAlignment.Center;
                                txt_error.Text = "User not found";
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
                    txt_error.Text = "Email is invalid";
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

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            LoginViewIn loginViewIn = new LoginViewIn();
            loginViewIn.Show();
            this.Close();

        }
    }
}
