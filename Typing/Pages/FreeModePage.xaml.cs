using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Typing.Pages
{
    /// <summary>
    /// Interaction logic for FreeModePage.xaml
    /// </summary>
    public partial class FreeModePage : Page
    {
        //private 
        KeyboardLayout USKeyboard = new KeyboardLayout();
        StatisticsCalculator Statistics;
        TypingStream TypingStream;

        //Properties
        bool isGamePaused;
        public bool IsGamePaused
        {
            get => isGamePaused;
            set {
                isGamePaused = value;
                Statistics.IsCollecting = !value;
                btnPlayPause.IsChecked = value;
            }
        }

        //Initialize
        public FreeModePage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += FreeModePage_KeyDown;
            InitTypingStram();
            Statistics = new StatisticsCalculator();
            InitTimer();
            IsGamePaused = false;
        }

        //Timer
        private void InitTimer()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object s,EventArgs e)
        {
            if (Statistics.LastElapsedTime > new TimeSpan(0, 0, 12))
                IsGamePaused = true;

            txtTimer.Text = Statistics.ElapsedTime.ToString("mm':'ss");
            txtWPM.Text = string.Format("{0:0.00}", Statistics.CorrectWordPerMinute);
        }

        //TypingStream
        private void InitTypingStram()
        {
            TypingStream = new TypingStream(File.OpenText($"{Environment.CurrentDirectory}\\Texts\\FreeModeTypingPractice.txt"));
            TypingStream.OnCharTyped += TypingStream_OnCharTyped;
            TypingStream.GoToFirstLine();
        }
        private void TypingStream_OnCharTyped(string caretChar, string remainedText)
        {
            typView.CurrentTyping = caretChar;
            typView.PreviewText = remainedText;
        }

        //Key Down
        private void FreeModePage_KeyDown(object sender, KeyEventArgs e)
        {
            KeyData key = new KeyData(e.Key, USKeyboard);

            if (key.Key != Key.Escape && IsGamePaused)
                return;

            switch (e.Key)
            {
                case Key.Escape:
                    EscKeyDown();
                    return;

                case Key.Enter:
                    EnterKeyDown(key);
                    break;

                default:
                    if (key.HasKeyChar)
                        KeyHasCharDown(key);
                    break;
            }

            Statistics.AddKey(key);
        }
        // Enter Key Down
        private void EnterKeyDown(KeyData key)
        {
            if (TypingStream.IsEndOfLine)
            {
                bool noNextLine = !TypingStream.GoToNextLine();
                if (noNextLine)
                {
                    IsGamePaused = true;
                    MessageBox.Show("Congratulations!!! You typed all of text");
                }
                else
                {
                    typView.ClearLine();
                }
                key.IsCorrect = true;
            }
        }

        // Any Key Has Keychar
        private void KeyHasCharDown(KeyData key)
        {
            if (!TypingStream.IsEndOfLine)
            {
                key.IsCorrect = TypingStream.IsCorrectChar(key.KeyChar);
                typView.WriteKeyChar(key);
                TypingStream.GoToNextChar();
            }
        }

        //Esc Key
        private void EscKeyDown()
        {
            IsGamePaused = !IsGamePaused;
        }

        //Play Pause Button
        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            EscKeyDown();
        }
    }
}
