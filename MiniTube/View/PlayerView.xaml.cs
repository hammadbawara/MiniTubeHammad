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
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : Window
    {
        private int Id;
        public PlayerView()
        {
            InitializeComponent();
        }
        public PlayerView(int i)
        {
            InitializeComponent();
            Id = i;
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


        private void btn_play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void like_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image? img = sender as Image;


            if (img != null)
            {

                if (img.Source.ToString().Contains("like.png"))
                {
                    img.Source = new BitmapImage(new Uri("/Images/liked.png", UriKind.Relative));
                }
                else
                {
                    img.Source = new BitmapImage(new Uri("/Images/like.png", UriKind.Relative));
                }
            }
        }

        private void txt_cmt_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_cmt_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
