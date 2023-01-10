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
        #region Useless

        public static readonly DependencyProperty IsGamePausedProperty =
            DependencyProperty.Register("IsGamePaused", typeof(bool), typeof(FreeModePage), new PropertyMetadata(false));

        public FreeModePage()
        {
            InitializeComponent();
        }

        #endregion

        //private 
        KeyboardLayout USKeyboard = new KeyboardLayout();
        StatisticsCalculator Statistics;
        TypingStream TypingStream;

        public bool IsGamePaused
        {
            get { return (bool)GetValue(IsGamePausedProperty); }
            set { SetValue(IsGamePausedProperty, value);
                Statistics.IsCollecting = !value;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += FreeModePage_KeyDown;
            InitTypingStram();
            Statistics = new StatisticsCalculator();
            InitTimer();
            IsGamePaused = false;
        }

        private void InitTimer()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick += (s, e) =>
            {
                txtTimer.Text = Statistics.ElapsedTime.ToString("mm':'ss");
                txtWPM.Text = string.Format("{0:0.00}", Statistics.CorrectWordPerMinute);
            };
        }

        //TypingStream
        private void InitTypingStram()
        {
            CleanAll();
            TypingStream = new TypingStream(File.OpenText($"{Environment.CurrentDirectory}\\Texts\\FreeModeTypingPractice.txt"));
            TypingStream.OnCharTyped += TypingStream_OnCharTyped;
            TypingStream.GoToFirstLine();
        }
        private void CleanAll()
        {
            txtCurrentTypedLine.Text = "";
            txtTyped.Text = "";
        }
        private void TypingStream_OnCharTyped(string caretChar, string remainedText)
        {
            txtCaretChar.Text = caretChar;
            txtPreview.Text = remainedText;
        }

        //Key Down
        private void FreeModePage_KeyDown(object sender, KeyEventArgs e)
        {
            KeyData key = new KeyData(e.Key, USKeyboard);

            if (key.Key != Key.Escape && IsGamePaused)
                return;

            switch (e.Key)
            {
                case Key.Enter:
                    EnterKeyDown(key);
                    break;

                case Key.Escape:
                    IsGamePaused = !IsGamePaused;
                    return;

                default:
                    if(key.HasKeyChar)
                        KeyHasCharDown(key);
                    break;
            }

            Statistics.AddKey(key);
        }

        //Enter Key
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
                    AddCurrentLineToTypedText();
                    txtCurrentTypedLine.Text = "";
                }
                key.IsCorrect = true;
            }
        }
        private void AddCurrentLineToTypedText()
        {
            Inline[] inlineList = new Inline[500];
            txtCurrentTypedLine.Inlines.CopyTo(inlineList, 0);

            if(txtTyped.Text != "")
                txtTyped.Inlines.Add(new LineBreak());

            foreach (Inline inline in inlineList)
            {
                if (inline != null)
                    txtTyped.Inlines.Add(inline);
                else
                    break;
            }
            
        }

        // Any Key Has Keychar
        private void KeyHasCharDown(KeyData key)
        {
            if (!TypingStream.IsEndOfLine)
            {
                key.IsCorrect = TypingStream.IsCorrectChar(key.KeyChar);

                if (key.IsCorrect)
                    AddCorrectChar(key.KeyChar);
                else
                    AddInCorrectChar(key.KeyChar);

                TypingStream.GoToNextChar();
            }
        }
        private void AddInCorrectChar(char keyChar)
        {
            object lastInline = txtCurrentTypedLine.Inlines.LastInline;
            //Run is used for correct Piece and Underline is used for InCorrect Piece
            if (lastInline is Underline)
            {
                Underline lastInCorrectPiece = (Underline)lastInline;
                Run LastRunInUnderline = (Run)lastInCorrectPiece.Inlines.LastInline;
                LastRunInUnderline.Text += keyChar;
            }
            else
            {
                Underline newIncorrectPiece = new Underline()
                {
                    Style = (Style)App.Current.FindResource("FreeMode.MainLine.Mistake")
                };
                Run TextPiece = new Run(keyChar.ToString());
                newIncorrectPiece.Inlines.Add(TextPiece);
                txtCurrentTypedLine.Inlines.Add(newIncorrectPiece);
            }
        }
        private void AddCorrectChar(char keyChar)
        {
            object lastInline = txtCurrentTypedLine.Inlines.LastInline;
            //Run is used for correct Piece and Underline is used for InCorrect Piece
            if (lastInline is Run)
            {
                Run lastTextPiece = (Run)lastInline;
                lastTextPiece.Text += keyChar;
            }
            else
            {
                Run newTextPiece = new Run()
                {
                    Text = keyChar.ToString(),
                    Style = (Style)App.Current.FindResource("FreeMode.MainLine.Correct")
                };
                txtCurrentTypedLine.Inlines.Add(newTextPiece);
            }
        }
    }
}
