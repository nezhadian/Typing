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
        TypingStream TypingStream;

        public FreeModePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += FreeModePage_KeyDown;
            CleanAll();
            TypingStream = new TypingStream(File.OpenText($"{Environment.CurrentDirectory}\\Texts\\FreeModeTypingPractice.txt"));
            TypingStream.OnCharTyped += TypingStream_OnCharTyped;
            TypingStream.GoToFirstLine();
        }

        private void TypingStream_OnCharTyped(string caretChar, string remainedText)
        {
            txtCaretChar.Text = caretChar;
            txtPreview.Text = remainedText;
        }

        private void FreeModePage_KeyDown(object sender, KeyEventArgs e)
        {
            bool isToggled = !Console.CapsLock;
            isToggled = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftShift) ? !isToggled : isToggled;
            switch (e.Key)
            {
                case Key.Enter:
                    if (TypingStream.IsEndOfLine)
                    {
                        bool noNextLine = !TypingStream.GoToNextLine();
                        if (noNextLine)
                        {
                            MessageBox.Show("Congratulations!!! You typed all of text");
                        }
                        else
                        {
                            AddCurrentLineToTypedText();
                            txtCurrentTypedLine.Text = "";
                        }
                    }
                    break;

                default:
                    char? keyChar = USKeyboard.Key2Char(e.Key, isToggled);
                    if (keyChar != null && !TypingStream.IsEndOfLine)
                    {
                        if (TypingStream.IsCorrectChar(keyChar.Value))
                            AddCorrectChar(keyChar.Value);
                        else
                            AddInCorrectChar(keyChar.Value);

                        TypingStream.GoToNextChar();
                    }
                    break;
            }
        }

        private void CleanAll()
        {
            txtCurrentTypedLine.Text = "";
            txtTyped.Text = "";
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
