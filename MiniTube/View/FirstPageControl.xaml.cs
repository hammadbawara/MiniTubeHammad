using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// Represents a video item that can be clicked to trigger an event.
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        // ----- Property to hold the Video ID -----
        public string VideoID { get; set; }

        // ----- Constructor -----
        public UserControl1()
        {
            InitializeComponent();
        }

        // ----- Event triggered when the video is clicked -----
        public event EventHandler<string> VideoClicked;

        // ----- Mouse down event handler for the video border -----
        private void bdr_video_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(VideoID))
                {
                    // ----- Raise the VideoClicked event if VideoID is not null or empty -----
                    VideoClicked?.Invoke(this, VideoID);
                }
                else
                {
                    // ----- Show a message box if VideoID is null or empty -----
                    MessageBox.Show("VideoID is null or empty. Please check binding.");
                }
            }
            catch (Exception ex)
            {
                // ----- Handle unexpected exceptions -----
                MessageBox.Show($"An error occurred while processing the click: {ex.Message}");
            }
        }

        // ----- Dispose method to clean up resources if necessary -----
        public void Dispose()
        {
            // ----- Unsubscribe from events and clean up resources here if needed -----
            VideoClicked = null; // Clear the event to prevent memory leaks
        }
    }
}