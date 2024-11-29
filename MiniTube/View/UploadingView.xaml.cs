using MiniTube.ModelsEAD;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;




namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UploadingView.xaml
    /// </summary>
    public partial class UploadingView : Window
    {
        private byte[]? _thumbnail;
        private byte[]? _video;
        private string? _videoPath;
        private int Id;
        public UploadingView()
        {
            InitializeComponent();
        }
        public UploadingView(int i)
        {
            InitializeComponent();
            Id = i;
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

        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            
            if (_thumbnail != null && _video != null &&
                !string.IsNullOrEmpty(txt_title.Text) &&
                !string.IsNullOrEmpty(txt_description.Text) &&
                !string.IsNullOrEmpty(txt_k1.Text) &&
                !string.IsNullOrEmpty(txt_k2.Text) &&
                !string.IsNullOrEmpty(txt_k3.Text))
            {
                string title = txt_title.Text;
                string description = txt_description.Text;
                string keyword1 = txt_k1.Text;
                string keyword2 = txt_k2.Text;
                string keyword3 = txt_k3.Text;

                try
                {
                    using (var context = new MiniTubeContext())
                    {
                        
                        var userExists = context.Users.Any(u => u.UserId == Id);
                        if (!userExists)
                        {
                            MessageBox.Show("The specified UserId does not exist. Please check the user ID.");
                            return;
                        }

                        var video = new Video
                        {
                            UserId = Id,
                            Title = title,
                            Description = description,
                            Thumbnail = _thumbnail,
                            VideoFile = _video,
                            Keyword1 = keyword1,
                            Keyword2 = keyword2,
                            Keyword3 = keyword3,
                            UploadDate = DateTime.Now
                        };

                        context.Videos.Add(video);
                        context.SaveChanges();
                        MessageBox.Show("Video uploaded successfully!");

                        ResetForm();
                        UploadingView uv = new UploadingView(Id);
                        uv.Show();
                        System.Threading.Thread.Sleep(1000);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all data.");
            }
        }


        private void ResetForm()
        {
            txt_title.Clear();
            txt_description.Clear();
            txt_k1.Clear();
            txt_k2.Clear();
            txt_k3.Clear();
            _thumbnail = Array.Empty<byte>();  
            _video = Array.Empty<byte>();      
            _videoPath = string.Empty;
            //img_thumb.Source = new BitmapImage(new Uri("/Images/placeholder.png", UriKind.Relative));
            media_video.Source = null;
            img_thumb.Source = null;
            btn_stack.Visibility = Visibility.Hidden;
            //btn_vidupload.Visibility= Visibility.Visible;
            //btn_thumbupload.Visibility = Visibility.Visible;
        }


        private void btn_back_Click(object sender, RoutedEventArgs e)   
        {
            StudioView studioView = new StudioView(Id);
            studioView.Show();
            this.Close();
        }

        private void btn_thumbupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                
                _thumbnail = File.ReadAllBytes(openFileDialog.FileName);
                img_thumb.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                btn_thumbupload.Opacity=0;
                
            }
        }

        private void btn_vidupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video files (*.mp4;*.mov;*.avi)|*.mp4;*.mov;*.avi";
            if (openFileDialog.ShowDialog() == true)
            {
                
                _video = File.ReadAllBytes(openFileDialog.FileName);

                
                _videoPath = openFileDialog.FileName;
                media_video.Source = new Uri(_videoPath);
                media_video.LoadedBehavior = MediaState.Manual;  
                media_video.Play();  
                btn_vidupload.Opacity = 0;
                btn_stack.Visibility=Visibility.Visible;
                Play.Visibility = Visibility.Hidden;
            }
        }
   
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            media_video.Play();
            Play.Visibility = Visibility.Hidden;
            Pause.Visibility= Visibility.Visible;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            media_video.Pause();
            Pause.Visibility= Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            media_video.Stop();
            Pause.Visibility=Visibility.Hidden;
            Play.Visibility=Visibility.Visible;
        }

        
    }
}
