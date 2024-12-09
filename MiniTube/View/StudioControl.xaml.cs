using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for StudioControl.xaml
    /// This control represents a video item in the studio view.
    /// </summary>
    public partial class StudioControl : UserControl
    {
        // ----- Backing field for VideoId -----
        private int _videoId;

        /// <summary>
        /// Gets or sets the ID of the video associated with this control.
        /// </summary>
        public int VideoId
        {
            get => _videoId;
            set => _videoId = value; // You can add additional logic here if needed
        }

        /// <summary>
        /// Event triggered when the video is clicked.
        /// </summary>
        public event Action<object, int> VideoClicked;

        // ----- Constructor -----
        public StudioControl()
        {
            InitializeComponent();
        }

        // ----- Mouse Left Button Down Event -----
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Invoke the VideoClicked event if there are any subscribers
            VideoClicked?.Invoke(this, VideoId);
        }
    }
}