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

namespace Typing.Pages
{
    /// <summary>
    /// Interaction logic for CompletedPage.xaml
    /// </summary>
    public partial class CompletedPage : Page
    {
        bool isBstScr;
        public bool IsBestScore
        {
            get => isBstScr;
            set
            {
                isBstScr = value;
                svgCrown.Visibility = (isBstScr) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private double lstWPM;
        public double LastWPM
        {
            get => lstWPM;
            set { lstWPM = value;
                txtLastWPM.Text = string.Format("{0:0}", value);
            }
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get => time;
            set { time = value;
                txtTime.Text = value.ToString("mm':'ss");
            }
        }

        public CompletedPage()
        {
            InitializeComponent();
        }

        private void TryMore_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public void ReadFromStatisticsCalculator(StatisticsCalculator stat)
        {
            LastWPM = stat.CorrectWordPerMinute;
            Time = stat.KeysElapsedTime;
            var wordDetails = stat.WordDetails();
            rnCorrectWords.Text = "+" + wordDetails.CorrectWords;
            rnInCorrectWords.Text = "-" + wordDetails.InCorrectWords;
            rnTotalWords.Text = "/" + wordDetails.TotalWords;
        }
    }
}
