using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Typing
{
    public class StatisticsCalculator
    {
        public DateTime LastTime;
        public List<KeyData> KeyDataList = new List<KeyData>();
        public bool WaitUntilKeyPress = true;

        private bool isCollect;
        public bool IsCollecting
        {
            set
            {
                isCollect = value;
                if (isCollect)
                    WaitUntilKeyPress = true;
            }
            get => isCollect;
        }

        public TimeSpan KeysElapsedTime = new TimeSpan(0);
        public TimeSpan LastElapsedTime => DateTime.Now - LastTime;
        public TimeSpan ElapsedTime 
            => !IsCollecting || WaitUntilKeyPress ? KeysElapsedTime : KeysElapsedTime + LastElapsedTime;

        public double CorrectWordPerMinute
        {
            get
            {
                var wordsDetail = WordsDetail();

                double divisor = (WaitUntilKeyPress ? KeysElapsedTime : ElapsedTime).TotalSeconds / 60;
                return wordsDetail.CorrectWords != 0 ? wordsDetail.CorrectWords / divisor : 0;
            }
        }

        public StatisticsCalculator()
        {
            LastTime = DateTime.Now;
        }
        

        internal void AddKey(KeyData data)
        {
            if (!IsCollecting)
                return;

            if (WaitUntilKeyPress)
            {
                LastTime = DateTime.Now;
                WaitUntilKeyPress = false;
            }

            data.DelayTime = DateTime.Now - LastTime;
            KeysElapsedTime += data.DelayTime;
            KeyDataList.Add(data);

            LastTime = DateTime.Now;
        }

        internal void ClearAll()
        {
            KeyDataList.Clear();
        }

        internal (int Words,int CorrectWords,int InCorrectWords) WordsDetail()
        {
            int wordLength = 0;
            int correctWords = 0;
            int totalWords = 0;
            bool isCorrectWord = true;

            for (int i = 0; i < KeyDataList.Count; i++)
            {
                KeyData key = KeyDataList[i];
                if (key.IsWordSeperator)
                {
                    if (wordLength > 0)
                    {
                        if (isCorrectWord)
                            correctWords++;
                        else
                            isCorrectWord = true;
                        wordLength = 0;
                        totalWords++;
                    }
                }
                else if (key.HasKeyChar)
                {
                    if (!key.IsCorrect)
                        isCorrectWord = false;
                    wordLength++;
                }
            }

            return (totalWords, correctWords, totalWords - correctWords);
        }


    }
    public class KeyData
    {
        public TimeSpan DelayTime;
        public readonly Key Key;
        public readonly bool IsToggled;
        public readonly char KeyChar;
        public readonly bool HasKeyChar;
        public bool IsCorrect;
        public bool IsWordSeperator
        {
            get
            {
                switch (Key)
                {
                    case Key.Space:
                    case Key.Enter:
                    case Key.Oem1:
                    case Key.Oem2:
                    case Key.Oem7:
                    case Key.OemComma:
                    case Key.OemPeriod:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public KeyData(Key key, KeyboardLayout kl)
        {
            Key = key;
            IsToggled = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftShift) ? Console.CapsLock : !Console.CapsLock;
            char? keyChar = kl.Key2Char(key, IsToggled);
            
            HasKeyChar = keyChar != null;
            if (HasKeyChar)
                KeyChar = keyChar.Value;
        }

    }
}
