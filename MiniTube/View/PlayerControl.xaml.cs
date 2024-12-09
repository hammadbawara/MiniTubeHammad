using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for PlayerControl.xaml
    /// Provides a user interface for video playback controls and suggestions.
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        // Property to store Video ID (can be used in the event)
        public string VideoID { get; set; }

        // Event to notify when a suggestion is clicked
        public event Action<object, string> VideoClicked;

        public PlayerControl()
        {
            InitializeComponent();
        }

        // ----Handles mouse down event on the suggestion border----
        private void bdr_suggestion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Trigger the VideoClicked event with the VideoID
                VideoClicked?.Invoke(this, VideoID); // Invoke the event if there are subscribers
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling suggestion click: {ex.Message}"); // Handle exceptions
            }
        }

        // ----Handles mouse left button down event on the UserControl----
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Trigger the VideoClicked event with the VideoID
                VideoClicked?.Invoke(this, VideoID); // Invoke the event if there are subscribers
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling user control click: {ex.Message}"); // Handle exceptions
            }
        }

        // ----Clears resources when navigating away from the control----
        public void ClearResources()
        {
            try
            {
                VideoID = null; // Clear the VideoID to free memory
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing resources: {ex.Message}"); // Handle exceptions
            }
        }
    }
}