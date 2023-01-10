using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Typing
{
    public class TypingStream
    {
        public StreamReader TextStream;

        public delegate void LineUpdateEventHandler(string caretChar,string remainedText);
        public event LineUpdateEventHandler OnCharTyped;

        private string currentLine;

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;

                if (OnCharTyped != null)
                {
                    string caretChar = value < currentLine.Length ? currentLine[value].ToString() : "";

                    int previewIndex = value + 1;
                    string remainedText = previewIndex < currentLine.Length ? currentLine.Substring(previewIndex) : "";

                    OnCharTyped(caretChar, remainedText);
                }

               
            }
        }

        public bool IsEndOfLine => Index >= currentLine.Length;

        public TypingStream(StreamReader text)
        {
            if (text == null)
                throw new NullReferenceException();

            TextStream = text;
        }

        public void GoToFirstLine()
        {
            TextStream.BaseStream.Seek(0, SeekOrigin.Begin);
            currentLine = TextStream.ReadLine();
            Index = 0;
        }

        public bool GoToNextLine()
        {
            string line = TextStream.ReadLine();
            if (line != null)
            {
                currentLine = line;
                Index = 0;
                return true;
            }
            else
            {
                currentLine = "";
                return false;
            }

        }

        internal void GoToNextChar() => Index++;

        public bool IsCorrectChar(char keychar) => keychar == currentLine[Index];
    }
}
