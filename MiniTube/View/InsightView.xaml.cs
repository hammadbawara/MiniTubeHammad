using MiniTube.ModelsEAD;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for InsightView.xaml
    /// Displays insights for a specific video, including title, description, likes, and comments.
    /// </summary>
    public partial class InsightView : Window
    {
        private int Id; // Video ID for insights
        private int uId; // User ID
        private string? tempThumbnailPath; // Variable to hold thumbnail path

        // ----- Default constructor -----
        public InsightView()
        {
            InitializeComponent();
        }

        // ----- Constructor with parameters for User ID and Video ID -----
        public InsightView(int ui, int vi)
        {
            InitializeComponent();
            Id = vi;
            uId = ui;
            LoadInsights(vi); // Load insights for the specified video 
        }

        // ----- Asynchronously loads insights for the specified video ID -----
        private async void LoadInsights(int videoId)
        {
            using (var context = new MiniTubeContext())
            {
                try
                {
                    // ----- Fetch video details -----
                    var video = await context.Videos.FirstOrDefaultAsync(x => x.VideoId == videoId);
                    if (video != null)
                    {
                        txt_title.Text = video.Title; // Set title 
                        txt_des.Text = video.Description; // Set description 

                        // ----- Load thumbnail if available -----
                        if (video.Thumbnail != null)
                        {
                            tempThumbnailPath = SaveToTempFile(video.Thumbnail, "png");
                            thumb.Source = new BitmapImage(new Uri(tempThumbnailPath)); // Set thumbnail source 
                        }
                        else
                        {
                            MessageBox.Show("Thumbnail is not available.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Video not found.");
                        return;
                    }

                    // ----- Fetch likes for the video -----
                    var likes = await context.Likes
                        .Where(like => like.VideoId == videoId)
                        .Select(like => new
                        {
                            Username = like.User.Username,
                            LikedDate = like.LikedDate // Use the LikedDate property from the Like model
                        }).ToListAsync();

                    // ----- Fetch comments for the video -----
                    var comments = await context.Comments
                        .Where(comment => comment.VideoId == videoId)
                        .Select(comment => new
                        {
                            CommentId = comment.CommentId, // Ensure CommentId is included
                            Username = comment.User.Username,
                            CommentText = comment.CommentText,
                            CommentDate = comment.CommentDate // Use the CommentDate property from the Comment model
                        }).ToListAsync();

                    // ----- Bind the likes and comments to their respective DataGrids -----
                    LikesDataGrid.ItemsSource = likes; // Bind likes 
                    CommentsDataGrid.ItemsSource = comments; // Bind comments 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading insights: {ex.Message}"); // Handle exceptions 
                }
            }
        }

        // ----- Saves byte array data to a temporary file and returns the file path -----
        private static string SaveToTempFile(byte[] data, string extension)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            }

            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{extension}");
            File.WriteAllBytes(tempPath, data); // Write data to temp file 
            return tempPath;
        }

        // ----- Deletes the video and navigates back to the StudioView -----
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new MiniTubeContext())
                {
                    Video? vid = db.Videos.FirstOrDefault(x => x.VideoId == Id);
                    if (vid != null)
                    {
                        db.Videos.Remove(vid); // Remove video from context db.SaveChanges(); // Save changes to the database 
                    }
                    else
                    {
                        MessageBox.Show("Video not found."); // Handle case where video is not found
                    }
                }
                StudioView studioView = new StudioView(uId); // Navigate back to StudioView
                studioView.Show();
                this.Close(); // Close the current window 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting video: {ex.Message}"); // Handle exceptions 
            }
        }

        // ----- Allows the window to be dragged -----
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // ----- Minimizes the window -----
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized; // Minimize the window
        }

        // ----- Closes the application -----
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Shutdown the application 
        }

        // ----- Navigates back to the StudioView -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudioView studioView = new StudioView(uId); // Pass the UserId
                studioView.Show(); // Show the StudioView
                this.Close(); // Close the current window 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}"); // Handle exceptions 
            }
        }

        // ----- Event handler for grid selection change -----
        private void grid_insights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Currently not implemented 
        }

        // ----- Asynchronously loads comments for the specified video ID -----
        private async Task LoadComments(int videoId)
        {
            using (var context = new MiniTubeContext())
            {
                try
                {
                    var comments = await context.Comments
                        .Where(comment => comment.VideoId == videoId)
                        .Select(comment => new
                        {
                            CommentId = comment.CommentId, // Ensure CommentId is included
                            Username = comment.User.Username,
                            CommentText = comment.CommentText,
                            CommentDate = comment.CommentDate
                        }).ToListAsync();

                    CommentsDataGrid.ItemsSource = comments; // Bind comments to DataGrid 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading comments: {ex.Message}"); // Handle exceptions 
                }
            }
        }

        // ----- Deletes a comment when the delete button is clicked -----
        private async void btn_delete_cmt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the button that was clicked
                Button deleteButton = sender as Button;
                if (deleteButton != null)
                {
                    // Check the Tag property
                    if (deleteButton.Tag != null)
                    {
                        // Get the CommentId from the Tag property
                        if (deleteButton.Tag is int commentId)
                        {
                            using (var context = new MiniTubeContext())
                            {
                                // Find the comment in the database
                                var commentToDelete = await context.Comments.FindAsync(commentId);
                                if (commentToDelete != null)
                                {
                                    // Remove the comment from the context
                                    context.Comments.Remove(commentToDelete); // Remove comment 
                                    await context.SaveChangesAsync(); // Save changes asynchronously 

                                    // Refresh the comments DataGrid
                                    await LoadComments(Id); // Use the Id field to load comments for the current video 
                                    MessageBox.Show("Comment Deleted"); // Notify user of deletion 
                                }
                                else
                                {
                                    MessageBox.Show("Comment not found.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid comment ID.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tag is null.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid button.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting comment: {ex.Message}"); // Handle exceptions 
            }
        }
    }
}