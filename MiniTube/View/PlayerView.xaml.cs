using Microsoft.EntityFrameworkCore;
using MiniTube.Context;
using MiniTube.ModelsEAD;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MiniTubeContext = MiniTube.ModelsEAD.MiniTubeContext;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// Provides a user interface for video playback, comments, and related videos.
    /// </summary>
    public partial class PlayerView : Window
    {
        private int UserId;
        private int VideoId;
        private bool isFullscreen = false;
        private bool isLiked = false; // Track if the user has liked the video
        private string tempFilePath; // Temporary file path for the video

        public PlayerView()
        {
            InitializeComponent();
        }

        public PlayerView(int userId, int videoId)
        {
            InitializeComponent();
            UserId = userId;
            VideoId = videoId;
            LoadVideoDetails();
            LoadRelatedVideos();
            btn_pause.Visibility = Visibility.Visible;
            btn_play.Visibility = Visibility.Hidden;
        }

        // ----- Load video details (title, description, and video content) based on VideoId -----
        private void LoadVideoDetails()
        {
            using (var dbContext = new MiniTubeContext())
            {
                var video = dbContext.Videos.FirstOrDefault(v => v.VideoId == VideoId);

                if (video != null)
                {
                    // Set title and description
                    title.Text = video.Title;
                    description.Text = video.Description;

                    // Load video file
                    if (video.VideoFile != null)
                    {
                        try
                        {
                            // Save the video file to a temporary path
                            tempFilePath = Path.Combine(Path.GetTempPath(), $"{video.VideoId}.mp4");

                            // Write the video data to the temporary file
                            File.WriteAllBytes(tempFilePath, video.VideoFile);

                            // Set the media player's source to the temporary file
                            media_video.Source = new Uri(tempFilePath, UriKind.Absolute);
                            media_video.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to load video file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Video file not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    // Check if the user has liked the video
                    try
                    {
                        var likeEntry = dbContext.Likes.SingleOrDefault(l => l.UserId == UserId && l.VideoId == VideoId);
                        isLiked = likeEntry != null; // Set the like state
                        UpdateLikeImage(); // Update the like button image
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error checking like status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Video not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
        }

        // ----- Load related videos into the WrapPanel -----
        private void LoadRelatedVideos()
        {
            using (var dbContext = new MiniTubeContext())
            {
                var relatedVideos = dbContext.Videos
                    .Where(v => v.VideoId != VideoId) // Exclude the current video
                    .OrderBy(_ => Guid.NewGuid())    // Randomize
                    .Select(v => new
                    {
                        v.Title,
                        v.Description,
                        v.Thumbnail,
                        v.VideoId
                    })
                    .ToList();

                wrp_suggestions.Children.Clear();

                foreach (var video in relatedVideos)
                {
                    // Create a new PlayerControl
                    PlayerControl suggestion = new PlayerControl
                    {
                        VideoID = video.VideoId.ToString(),
                        DataContext = new
                        {
                            Title = video.Title,
                            Description = video.Description,
                            ImagePath = ConvertToBitmapImage(video.Thumbnail)
                        }
                    };

                    suggestion.Margin = new Thickness(0, 0, 0, 16.0603);

                    // Subscribe to the VideoClicked event
                    suggestion.VideoClicked += (s, videoId) =>
                    {
                        StopCurrentVideo(); // Stop the current video before loading a new one
                        PlayerView newPlayerView = new PlayerView(UserId, int.Parse(videoId));
                        newPlayerView.Show();
                        this.Close();
                    };

                    // Add PlayerControl to the WrapPanel
                    wrp_suggestions.Children.Add(suggestion);
                }
            }
        }

        // ----- Helper method to convert byte array to BitmapImage -----
        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var stream = new MemoryStream(imageData))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        // ----- Play button click event handler -----
        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (media_video.Source != null)
                {
                    btn_play.Visibility = Visibility.Hidden;
                    btn_pause.Visibility = Visibility.Visible;
                    media_video.Play();
                }
                else
                {
                    MessageBox.Show("Video source is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing video: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Stop the current video -----
        private void StopCurrentVideo()
        {
            if (media_video.Source != null)
            {
                media_video.Stop();
                media_video.Source = null; // Dispose of the current video source
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath); // Clean up the temporary file
                }
            }
        }

        // ----- Pause button click event handler -----
        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (media_video.Source != null)
                {
                    btn_pause.Visibility = Visibility.Hidden;
                    btn_play.Visibility = Visibility.Visible;
                    media_video.Pause();
                }
                else
                {
                    MessageBox.Show("Video source is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pausing video: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Like button click event handler -----
        private void like_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (var dbContext = new MiniTubeContext())
                {
                    var likeEntry = dbContext.Likes.SingleOrDefault(l => l.UserId == UserId && l.VideoId == VideoId);
                    var video = dbContext.Videos.SingleOrDefault(v => v.VideoId == VideoId);

                    if (video == null)
                    {
                        MessageBox.Show("Video not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (likeEntry == null)
                    {
                        // User is liking the video
                        dbContext.Likes.Add(new Like { UserId = UserId, VideoId = VideoId, LikedDate = DateTime.Now });
                        isLiked = true; // Update the like state
                        video.LikesCount = (video.LikesCount ?? 0) + 1; // Increment LikesCount
                    }
                    else
                    {
                        // User is unliking the video
                        dbContext.Likes.Remove(likeEntry);
                        isLiked = false; // Update the like state
                        video.LikesCount = (video.LikesCount ?? 0) - 1; // Decrement LikesCount
                    }

                    dbContext.SaveChanges();
                    UpdateLikeImage(); // Update the image after saving changes
                }
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Database update error: {dbEx.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error toggling like: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Update the like button image based on the like state -----
        private void UpdateLikeImage()
        {
            if (isLiked)
            {
                like.Source = new BitmapImage(new Uri("/Images/liked.png", UriKind.Relative)); // Path to the liked image
            }
            else
            {
                like.Source = new BitmapImage(new Uri("/Images/like.png", UriKind.Relative)); // Path to the unliked image
            }
        }

        // ----- Comment button click event handler -----
        private void btn_cmt_Click(object sender, RoutedEventArgs e)
        {
            string commentText = txt_cmt.Text.Trim();

            if (!string.IsNullOrEmpty(commentText))
            {
                using (var dbContext = new MiniTubeContext())
                {
                    var comment = new Comment
                    {
                        VideoId = VideoId,
                        UserId = UserId,
                        CommentText = commentText,
                        CommentDate = DateTime.Now
                    };

                    dbContext.Comments.Add(comment);
                    dbContext.SaveChanges();
                    MessageBox.Show("Comment added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear comment textbox
                    txt_cmt.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please enter a comment!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ----- Search text box key down event handler -----
        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }
        }

        // ----- Search button click event handler -----
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txt_search.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                using (var dbContext = new MiniTubeContext())
                {
                    var searchResults = dbContext.Videos
                        .Where(v => v.Title.Contains(searchText) ||
                                    v.Keyword1.Contains(searchText) ||
                                    v.Keyword2.Contains(searchText) ||
                                    v.Keyword3.Contains(searchText))
                        .Select(v => new
                        {
                            v.Title,
                            v.Thumbnail,
                            v.VideoId
                        })
                        .ToList();

                    wrp_suggestions.Children.Clear();

                    foreach (var video in searchResults)
                    {
                        PlayerControl pc = new PlayerControl
                        {
                            VideoID = video.VideoId.ToString()
                        };

                        BitmapImage bitmapImage = null;
                        if (video.Thumbnail != null)
                        {
                            using (MemoryStream ms = new MemoryStream(video.Thumbnail))
                            {
                                bitmapImage = new BitmapImage();
                                bitmapImage.BeginInit();
                                bitmapImage.StreamSource = ms;
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.EndInit();
                            }
                        }

                        pc.DataContext = new
                        {
                            Title = video.Title,
                            ImagePath = bitmapImage
                        };

                        pc.Margin = new Thickness(0, 0, 0, 16.0603);

                        pc.VideoClicked += UserControl_VideoClicked;

                        wrp_suggestions.Children.Add(pc);
                    }
                }
            }
            else
            {
                wrp_suggestions.Children.Clear();
                LoadRelatedVideos(); // Load related videos when the search box is empty
            }
        }

        // ----- Handle video click event from PlayerControl -----
        private void UserControl_VideoClicked(object sender, string videoId)
        {
            StopCurrentVideo(); // Stop the current video before loading a new one
            PlayerView newPlayerView = new PlayerView(UserId, int.Parse(videoId));
            newPlayerView.Show();
            this.Close();
        }

        // ----- Back button click event handler -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            StopCurrentVideo();

            // Navigate back to the UserView
            UserView userView = new UserView(UserId);
            userView.Show();

            // Close the current PlayerView window
            Close();
        }

        // ----- Minimize button click event handler -----
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // ----- Close button click event handler -----
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            StopCurrentVideo(); // Ensure the current video is stopped
            Application.Current.Shutdown();
        }

        // ----- Comment text box key down event handler -----
        private void txt_cmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Prevent the default behavior of the Enter key
                btn_cmt_Click(sender, e); // Call the method to add the comment
            }
        }

        // ----- Stop button click event handler -----
        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (media_video.Source != null)
                {
                    media_video.Stop();
                    btn_pause.Visibility = Visibility.Hidden;
                    btn_play.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Video source is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping video: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Window key down event handler for fullscreen toggle -----
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F) // Check if the 'F' key was pressed
            {
                ToggleFullscreen();
            }
            else if (e.Key == Key.Escape && isFullscreen) // Allow exiting fullscreen with Esc
            {
                ExitFullscreen();
            }
        }

        // ----- Toggle fullscreen mode -----
        private void ToggleFullscreen()
        {
            try
            {
                if (isFullscreen)
                {
                    ExitFullscreen();
                }
                else
                {
                    EnterFullscreen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Enter fullscreen mode -----
        private void EnterFullscreen()
        {
            try
            {
                this.WindowState = WindowState.Normal; // Ensure the window is not minimized
                this.ResizeMode = ResizeMode.NoResize; // Prevent resizing while in fullscreen
                this.WindowStyle = WindowStyle.None; // Remove window borders and title bar
                this.Topmost = true; // Keep the window on top

                // Set the window to cover the entire screen
                this.Left = 0;
                this.Top = 0;
                this.Width = SystemParameters.PrimaryScreenWidth;
                this.Height = SystemParameters.PrimaryScreenHeight;

                isFullscreen = true;
                this.Focus(); // Ensure the window remains focused
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to enter fullscreen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Exit fullscreen mode -----
        private void ExitFullscreen()
        {
            try
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow; // Restore window style
                this.ResizeMode = ResizeMode.CanResize; // Allow resizing
                this.Topmost = false; // Allow other windows to be on top

                // Restore the previous size and position
                this.WindowState = WindowState.Maximized; // Set back to maximized
                this.Width = 1920; // Set to your desired width
                this.Height = 1080; // Set to your desired height
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2; // Center the window
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2; // Center the window

                isFullscreen = false;
                this.Focus(); // Ensure the window remains focused
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to exit fullscreen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ----- Search text changed event handler -----
        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txt_search.Text.Trim(); // Get the search text

            if (string.IsNullOrEmpty(searchText))
            {
                wrp_suggestions.Children.Clear(); // Clear the current suggestions
                LoadRelatedVideos(); // Load related videos when the search box is empty
            }
            else
            {
                btn_search_Click(sender, e); // Call the existing search method
            }
        }
    }
}