using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        // Property to store Video ID (can be used in the event)
        public string VideoID { get; set; }

        // Event to notify when a suggestion is clicked delegate
        public event Action<object, string> VideoClicked;

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void bdr_suggestion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Trigger the VideoClicked event with the VideoID
            VideoClicked?.Invoke(this, VideoID);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Trigger the VideoClicked event with the VideoID
            VideoClicked?.Invoke(this, VideoID);
        }
    }
}
