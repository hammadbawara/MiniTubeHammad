using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniTube.View
{

    /// <summary>
    /// Interaction logic for StudioControl.xaml
    /// </summary>
    public partial class StudioControl : UserControl
    {
        public int VideoId { get; set; }
        public event Action<object, int> VideoClicked;
        public StudioControl()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VideoClicked?.Invoke(this, VideoId);
        }
    }
}
