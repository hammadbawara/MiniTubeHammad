using MiniTube.ModelsEAD;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UploadingView.xaml
    /// This view allows users to upload videos along with thumbnails and metadata.
    /// </summary>
    public partial class UploadingView : MetroWindow
    {
        private byte[]? _thumbnail;
        private byte[]? _video;
        private string? _videoPath;
        private int Id;

        // ----- Constructor -----
        public UploadingView()
        {
            InitializeComponent();
        }

        public UploadingView(int userId)
        {
            InitializeComponent();
            Id = userId;
        }

        // ----- Window Mouse Down Event -----
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // ----- Minimize Button Click -----
        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // ----- Close Button Click -----
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // ----- Upload Button Click -----
        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            // Validate input and upload video
            if (ValidateInput())
            {
                try
                {
                    UploadVideo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during upload: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all data.");
            }
        }

        // ----- Validate Input -----
        private bool ValidateInput()
        {
            return _thumbnail != null && _video != null &&
                   !string.IsNullOrEmpty(txt_title.Text) &&
                   !string.IsNullOrEmpty(txt_description.Text);
        }

        // ----- Upload Video to Database -----
        private void UploadVideo()
        {
            string title = txt_title.Text;
            string description = txt_description.Text;

            using (var context = new MiniTubeContext())
            {
                // Check if user exists
                if (!context.Users.Any(u => u.UserId == Id))
                {
                    MessageBox.Show("The specified UserId does not exist. Please check the user ID.");
                    return;
                }

                // Create and save video
                var video = new Video
                {
                    UserId = Id,
                    Title = title,
                    Description = description,
                    Thumbnail = _thumbnail,
                    VideoFile = _video,
                    Keyword1 = "",
                    Keyword2 = "",
                    Keyword3 = "",
                    UploadDate = DateTime.Now
                };

                context.Videos.Add(video);
                context.SaveChanges();
                MessageBox.Show("Video uploaded successfully!");

                ResetForm();
                NavigateToUploadingView();
            }
        }

        // ----- Reset Form Fields -----
        private void ResetForm()
        {
            txt_title.Clear();
            txt_description.Clear();
            _thumbnail = null;
            _video = null;
            _videoPath = string.Empty;
        }

        // ----- Navigate to Uploading View -----
        private void NavigateToUploadingView()
        {
            UploadingView uv = new UploadingView(Id);
            uv.Show();
            this.Close();
        }

        // ----- Back Button Click -----
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the Studio View
            StudioView studioView = new StudioView(Id);
            studioView.Show();
            this.Close();
        }

        // ----- Thumbnail Upload Button Click -----
        private void btn_thumbupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg ;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _thumbnail = File.ReadAllBytes(openFileDialog.FileName);
                    //img_thumb.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    btn_thumbupload.Opacity = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while uploading the thumbnail: {ex.Message}");
                }
            }
        }

        // ----- Video Upload Button Click -----
        private void btn_vidupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video files (*.mp4;*.mov;*.avi)|*.mp4;*.mov;*.avi"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _video = File.ReadAllBytes(openFileDialog.FileName);
                    _videoPath = openFileDialog.FileName;
                    
                    btn_vidupload.Opacity = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while uploading the video: {ex.Message}");
                }

                InitialState.Visibility = Visibility.Hidden;
                UploadForm.Visibility = Visibility.Visible;
            }
        }

        // ----- Play Button Click -----
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // ----- Pause Button Click -----
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
           
        }

        // ----- Stop Button Click -----
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}