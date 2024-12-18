using MiniTube.Context;
using MiniTube.ModelsEAD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for StudioView.xaml
    /// </summary>
    public partial class StudioView : MetroWindow
    {
        private int UserId;

        public StudioView()
        {
            InitializeComponent();
            LoadVideos();
        }

        public StudioView(int userId)
        {
            InitializeComponent();
            UserId = userId;
            LoadVideos();
        }

        private async void LoadVideos()
        {
            using (var context = new ModelsEAD.MiniTubeContext())
            {
                try
                {
                    var videos = await context.Videos
                        .Where(v => v.UserId == UserId)
                        .Select(v => new
                        {
                            v.VideoId,
                            v.Title,
                            v.Description,
                            v.LikesCount,
                            v.CommentsCount, // Include CommentsCount
                            Thumbnail = ConvertToBitmapImage(v.Thumbnail)
                        })
                        .ToListAsync();

                    if (videos.Count == 0)
                    {
                        MessageBox.Show("You have not uploaded any videos yet.");
                    }

                    foreach (var video in videos)
                    {
                        var studioControl = new StudioControl
                        {
                            VideoId = video.VideoId,
                            DataContext = video
                        };
                        studioControl.Margin = new Thickness(0, 0, 0, 14.9661);
                        studioControl.VideoClicked += StudioControl_VideoClicked;
                        wrp_front.Children.Add(studioControl);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading videos: {ex.Message}");
                }
            }
        }
        private static BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null) return null;

            using (var ms = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freeze the bitmap to make it cross-thread accessible
                return bitmapImage;
            }
        }
        private void StudioControl_VideoClicked(object sender, int videoId)
        {
            // Handle the video click event, e.g., navigate to a video detail page
            var insightView = new InsightView(UserId, videoId);
            insightView.Show();
            this.Close();
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

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }
        }

        private async void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txt_search.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadVideos(); // Load all videos if the search text is empty
                return;
            }

            using (var context = new ModelsEAD.MiniTubeContext())
            {
                try
                {
                    var videos = await context.Videos
                        .Where(v => v.UserId == UserId &&
                                    (v.Title.Contains(searchText) ||
                                     v.Keyword1.Contains(searchText) ||
                                     v.Keyword2.Contains(searchText) ||
                                     v.Keyword3.Contains(searchText)))
                        .Select(v => new
                        {
                            v.VideoId,
                            v.Title,
                            v.Description,
                            v.LikesCount,
                            Thumbnail = ConvertToBitmapImage(v.Thumbnail)
                        })
                        .ToListAsync();
                    DisplayVideos(videos);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching videos: {ex.Message}");
                }
            }
        }
        private void DisplayVideos(IEnumerable<dynamic> videos)
        {
            wrp_front.Children.Clear(); // Clear previous results

            foreach (var video in videos)
            {
                var studioControl = new StudioControl
                {
                    VideoId = video.VideoId,
                    DataContext = video
                };
                studioControl.Margin = new Thickness(0, 0, 0, 14.9661);
                studioControl.VideoClicked += StudioControl_VideoClicked;
                wrp_front.Children.Add(studioControl);
            }
        }
        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            LoginViewIn loginViewIn = new LoginViewIn();
            loginViewIn.Show();
            this.Close();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            UserView userView = new UserView(UserId);
            userView.Show();
            this.Close();
        }

        private async void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            // Show uploading view and close this window after completion without blocking UI
            UploadingView uploadingView = new UploadingView(UserId);
            uploadingView.Show();
            await Task.Delay(2000);  // Simulate upload delay
            this.Close();
        }
    }
}