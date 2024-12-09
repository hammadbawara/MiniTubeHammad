using MiniTube.Context;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MiniTube.View
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        private int UserId = -1;

        public UserView()
        {
            InitializeComponent();
            // ----- Initializing UserView without UserId -----
            ShowData();
        }

        public UserView(int id)
        {
            InitializeComponent();
            UserId = id;
            // ----- Show data for the specified UserId -----
            ShowData();
        }

        private void ShowData()
        {
            try
            {
                using (var dbContext = new MiniTubeContext())
                {
                    // ----- Fetch videos and randomize their order -----
                    var videos = dbContext.Videos
                        .OrderBy(_ => Guid.NewGuid()) // Randomize order
                        .Select(v => new
                        {
                            v.Title,
                            v.Thumbnail,
                            v.VideoId
                        })
                        .ToList();

                    // ----- Clear existing controls in the WrapPanel -----
                    wrp_front.Children.Clear();

                    foreach (var video in videos)
                    {
                        UserControl1 userControl = new UserControl1
                        {
                            VideoID = video.VideoId.ToString() // Set the VideoID explicitly
                        };

                        // ----- Convert the thumbnail to a BitmapImage -----
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

                        // ----- Bind the data to the UserControl -----
                        userControl.DataContext = new
                        {
                            Title = video.Title,
                            ImagePath = bitmapImage
                        };

                        // ----- Subscribe to the VideoClicked event -----
                        userControl.VideoClicked += UserControl_VideoClicked;

                        // ----- Add the UserControl to the WrapPanel -----
                        wrp_front.Children.Add(userControl);
                    }
                }
            }
            catch (Exception ex)
            {
                // ----- Handle exceptions gracefully -----
                MessageBox.Show($"An error occurred while loading data: {ex.Message}");
            }
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            // ----- Navigate to LoginViewIn -----
            LoginViewIn loginViewIn = new LoginViewIn();
            loginViewIn.Show();
            this.Close();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            // ----- Minimize the application -----
            WindowState = WindowState.Minimized;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            // ----- Shutdown the application -----
            Application.Current.Shutdown();
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // ----- Trigger search on Enter key -----
                btn_search_Click(sender, e);
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txt_search.Text.Trim(); // Get the search text

            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    using (var dbContext = new MiniTubeContext())
                    {
                        // ----- Search for videos matching the search text -----
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

                        // ----- Clear the WrapPanel -----
                        wrp_front.Children.Clear();

                        // ----- Add the search results to the WrapPanel -----
                        foreach (var video in searchResults)
                        {
                            UserControl1 userControl = new UserControl1
                            {
                                VideoID = video.VideoId.ToString() // Set the VideoID explicitly
                            };

                            // ----- Convert the thumbnail to a BitmapImage -----
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

                            // ----- Bind the data to the UserControl -----
                            userControl.DataContext = new
                            {
                                Title = video.Title,
                                ImagePath = bitmapImage
                            };

                            // ----- Subscribe to the VideoClicked event -----
                            userControl.VideoClicked += UserControl_VideoClicked;

                            // ----- Add the UserControl to the WrapPanel -----
                            wrp_front.Children.Add(userControl);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ----- Handle exceptions gracefully -----
                    MessageBox.Show($"An error occurred during the search: {ex.Message}");
                }
            }
            else
            {
                // ----- Clear the WrapPanel and show all data if search text is empty -----
                wrp_front.Children.Clear();
                ShowData();
            }
        }

        private void btn_studio_Click(object sender, RoutedEventArgs e)
        {
            // ----- Navigate to StudioView -----
            StudioView studioView = new StudioView(UserId);
            studioView.Show();
            this.Close();
        }

        private void UserControl_VideoClicked(object sender, string videoID)
        {
            // ----- Navigate to PlayerView with the VideoID -----
            if (int.TryParse(videoID, out int parsedVideoID))
            {
                PlayerView playerView = new PlayerView(UserId, parsedVideoID);
                playerView.Show();
                this.Close();
            }
            else
            {
                // ----- Show error message for invalid VideoID -----
                MessageBox.Show($"Invalid VideoID: {videoID}");
            }
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txt_search.Text.Trim(); // Get the search text

            if (string.IsNullOrEmpty(searchText))
            {
                // ----- Clear the WrapPanel and show all data if search text is empty -----
                wrp_front.Children.Clear();
                ShowData();
            }
            else
            {
                // ----- Trigger search when text changes -----
                btn_search_Click(sender, e);
            }
        }
    }
}