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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public string VideoID { get; set; }
        public UserControl1()
        {
            InitializeComponent();
        }

        private void bdr_video_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(VideoID))
            {
                // Raise the event if VideoID is not null
                VideoClicked?.Invoke(this, VideoID);
            }
            else
            {
                MessageBox.Show("VideoID is null or empty. Please check binding.");
            }
        }


        public event EventHandler<string> VideoClicked;
    }
}
