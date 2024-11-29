using MiniTube.Context;
using MiniTube.ModelsEAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for StudioView.xaml
    /// </summary>
    public partial class StudioView : Window
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
            using (var context = new Context.MiniTubeContext())
            {
                try
                {
                    var videos = await Task.Run(() =>
                        context.Videos.Select(v => new
                        {
                            v.VideoId,
                            v.Title,
                            v.Description,
                            v.LikesCount,
                            v.ViewsCount,
                            ThumbnailPath = SaveToTempFile(v.Thumbnail, "png") // Use helper method to save thumbnails
                        }).ToListAsync());

                    videoDataGrid.ItemsSource = videos;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading videos: {ex.Message}");
                }
            }
        }

        // Helper method to save byte[] data to a temporary file
        static private string SaveToTempFile(byte[] data, string extension)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.{extension}");
            File.WriteAllBytes(tempPath, data);
            return tempPath;
        }

        private void videoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedVideo = videoDataGrid.SelectedItem as dynamic;  // Cast as dynamic since we used anonymous types.
            if (selectedVideo != null)
            {
                int videoId = selectedVideo.VideoId;
                var insightView = new InsightView(UserId, videoId);
                insightView.Show();
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

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_search_Click(sender, e);
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            // Implement search functionality here.
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
            await Task.Delay(2000);  // Replaced Thread.Sleep with async Task.Delay
            this.Close();
        }
    }
}
