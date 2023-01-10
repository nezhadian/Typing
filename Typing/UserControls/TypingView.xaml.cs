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

namespace Typing.UserControls
{
    /// <summary>
    /// Interaction logic for TypingView.xaml
    /// </summary>
    public partial class TypingView : UserControl
    {
        public string CurrentTyping
        {
            set => txtCaretChar.Text = value;
            get => txtCaretChar.Text;
        }
        public string PreviewText
        {
            set => txtPreview.Text = value;
            get => txtPreview.Text;
        }

        public TypingView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtCurrentTypedLine.Text = "";
            txtTyped.Text = "";
        }

        public void WriteKeyChar(KeyData key)
        {
            if (key.IsCorrect)
                AddCorrectChar(key.KeyChar);
            else
                AddInCorrectChar(key.KeyChar);
        }

        public void ClearLine()
        {
            AddCurrentLineToTypedText();
            txtCurrentTypedLine.Text = "";
        }

        void AddCurrentLineToTypedText()
        {
            Inline[] inlineList = new Inline[500];
            txtCurrentTypedLine.Inlines.CopyTo(inlineList, 0);

            if (txtTyped.Text != "")
                txtTyped.Inlines.Add(new LineBreak());

            foreach (Inline inline in inlineList)
            {
                if (inline != null)
                    txtTyped.Inlines.Add(inline);
                else
                    break;
            }

        }

        void AddInCorrectChar(char keyChar)
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

        void AddCorrectChar(char keyChar)
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
