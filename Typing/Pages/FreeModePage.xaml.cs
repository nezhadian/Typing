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

namespace Typing.Pages
{
    /// <summary>
    /// Interaction logic for FreeModePage.xaml
    /// </summary>
    public partial class FreeModePage : Page
    {
        KeyboardLayout USKeyboard = new KeyboardLayout();
        public StreamReader TextToBeTyped;
        string currecntLine;
        private int _tIndex;

        public int TypingIndex
        {
            get { return _tIndex; }
            set { 
                _tIndex = value;
                txtPreview.Text = currecntLine.Substring(value);
            }
        }

        public FreeModePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += FreeModePage_KeyDown;
            TextToBeTyped = File.OpenText($"{Environment.CurrentDirectory}\\Texts\\FreeModeTypingPractice.txt");
            GoToNextLine();
        }

        private bool GoToNextLine()
        {
            txtCurrentTypedLine.Text = "";
            currecntLine = TextToBeTyped.ReadLine();
            TypingIndex = 0;
            return currecntLine != null;

        }

        private void FreeModePage_KeyDown(object sender, KeyEventArgs e)
        {
            bool isToggled = !Console.CapsLock;
            isToggled = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftShift) ? !isToggled : isToggled;
            switch (e.Key)
            {
                case Key.Enter:
                    if(TypingIndex >= currecntLine.Length)
                    {
                        GoToNextLine();
                    }
                    break;
                default:
                    char? initChar = USKeyboard.Key2Char(e.Key, isToggled);
                    bool isEndOfLine = TypingIndex >= currecntLine.Length;
                    if (initChar != null && !isEndOfLine)
                    {
                        char keyChar = initChar.Value;
                        AddChar(keyChar);
                        TypingIndex++;
                    }
                    break;
            }
        }

        private void AddChar(char keyChar)
        {
            if (TypingIndex >= currecntLine.Length)
                return;

            char correctChar = currecntLine[TypingIndex];
            if(keyChar == correctChar)
            {
                AddCorrectChar(keyChar);
            }
            else
            {
                AddInCorrectChar(keyChar);
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
