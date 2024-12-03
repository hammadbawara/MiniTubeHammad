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


                    var likes = context.Likes
                .Where(like => like.VideoId == videoId)
                .Select(like => new
                {
                    Username = like.User.Username,
                    Comment = (string?)null // No comment for likes-only rows
                }).ToList();

                    // Fetch comments for the video
                    var comments = context.Comments
                        .Where(comment => comment.VideoId == videoId)
                        .Select(comment => new
                        {
                            Username = comment.User.Username,
                            Comment = comment.CommentText
                        }).ToList();

                    // Combine likes and comments into one collection
                    var likesAndComments = likes.Union(comments).ToList();

                    // Bind the combined data to the DataGrid
                    grid_insights.ItemsSource = likesAndComments;
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
    }
}