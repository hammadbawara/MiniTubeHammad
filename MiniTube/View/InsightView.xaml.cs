using MiniTube.ModelsEAD;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Windows.Media.Imaging;
using System.Reflection.PortableExecutable; // Add this for BitmapImage

namespace MiniTube.View
{
    public partial class InsightView : Window
    {
        private int Id;
        private int uId;// Video ID for insights
        private string? tempThumbnailPath; // Variable to hold thumbnail path (renamed to avoid ambiguity)

        public InsightView()
        {
            InitializeComponent();

        }

        public InsightView(int ui,int vi)
        {
            InitializeComponent();
            Id = vi;
            uId = ui;
            LoadInsights(vi);
        }

        private async void LoadInsights(int videoId)
        {
            using (var context = new MiniTubeContext())
            {
                try
                {
                    var video = await context.Videos.FirstOrDefaultAsync(x => x.VideoId == videoId);
                    if (video != null)
                    {
                        txt_title.Text = video.Title;
                        txt_des.Text = video.Description;

                        if (video.Thumbnail != null)
                        {
                            tempThumbnailPath = SaveToTempFile(video.Thumbnail, "png");
                            thumb.Source = new BitmapImage(new Uri(tempThumbnailPath));
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

                    // Fetch likes for the video
                    var likes = await context.Likes
                            .Where(like => like.VideoId == videoId)
                            .Select(like => new
                            {
                                Username = like.User.Username,
                                LikedDate = like.LikedDate // Use the LikedDate property from the Like model
                            }).ToListAsync();
                    // Fetch comments for the video
                    var comments = await context.Comments
                        .Where(comment => comment.VideoId == videoId)
                        .Select(comment => new
                        {
                            Username = comment.User.Username,
                            CommentText = comment.CommentText,
                            CommentDate = comment.CommentDate // Use the CommentDate property from the Comment model
                        }).ToListAsync();

                    // Bind the likes and comments to their respective DataGrids
                    LikesDataGrid.ItemsSource = likes;
                    CommentsDataGrid.ItemsSource = comments;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading insights: {ex.Message}");
                }
            }
        }
        private static string SaveToTempFile(byte[] data, string extension)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            }

            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{extension}");
            File.WriteAllBytes(tempPath, data);
            return tempPath;
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            using(var db=new MiniTubeContext())
            {
                Video? vid=db.Videos.FirstOrDefault(x=> x.VideoId==Id);
                db.Videos.Remove(vid);
                db.SaveChanges();
                StudioView studioView = new StudioView(uId);
                studioView.Show();
                this.Close();
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

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            StudioView studioView = new StudioView(uId); // Pass the UserId
            studioView.Show();
            this.Close();
        }

        private void grid_insights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Event handler for grid selection change
        }


        private async Task LoadComments(int videoId)
        {
            using (var context = new MiniTubeContext())
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

                CommentsDataGrid.ItemsSource = comments;
            }
        }
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
                            MessageBox.Show($"Attempting to delete comment with ID: {commentId}"); // Debugging line
                            using (var context = new MiniTubeContext())
                            {
                                // Find the comment in the database
                                var commentToDelete = await context.Comments.FindAsync(commentId);
                                if (commentToDelete != null)
                                {
                                    // Remove the comment from the context
                                    context.Comments.Remove(commentToDelete);
                                    await context.SaveChangesAsync();

                                    // Refresh the comments DataGrid
                                    await LoadComments(Id); // Use the Id field to load comments for the current video
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
                MessageBox.Show($"Error deleting comment: {ex.Message}");
            }
        }
    }
}