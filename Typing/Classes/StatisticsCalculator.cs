using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Typing
{
    class StatisticsCalculator
    {
        public DateTime LastTime;
        public List<KeyData> KeyDataList = new List<KeyData>();
        bool WaitUntilKeyPress = true;

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

        public TimeSpan ElapsedKeysTime = new TimeSpan(0);
        public TimeSpan ElapsedTime
        {
            get
            {
                if (!IsCollecting || WaitUntilKeyPress)
                    return ElapsedKeysTime;
                return ElapsedKeysTime + (DateTime.Now - LastTime);
            }
        }

        public double CorrectWordPerMinute
        {
            get
            {
                int wordLength = 0;
                int correctWord = 0;
                bool isCorrectWord = true;
                for (int i = 0; i < KeyDataList.Count; i++)
                {
                    KeyData key = KeyDataList[i];
                    switch (key.Key)
                    {
                        case Key.Space:
                        case Key.Enter:
                        case Key.Oem1:
                        case Key.Oem2:
                        case Key.Oem7:
                        case Key.OemComma:
                        case Key.OemPeriod:
                            if(wordLength > 0)
                            {
                                if (isCorrectWord)
                                    correctWord++;
                                else
                                    isCorrectWord = true;
                                wordLength = 0;
                            }
                            break;

                        default:
                            if (key.HasKeyChar)
                            {
                                if (!key.IsCorrect)
                                    isCorrectWord = false;
                                wordLength++;
                            }
                            break;
                    }
                }

                double divisor = (WaitUntilKeyPress ? ElapsedKeysTime : ElapsedTime).TotalSeconds / 60;
                return correctWord / divisor;
            }
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
            ElapsedKeysTime += data.DelayTime;
            KeyDataList.Add(data);

            LastTime = DateTime.Now;
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

        public KeyData(Key key,KeyboardLayout kl)
        {
            Key = key;
            IsToggled = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.LeftShift) ? Console.CapsLock : !Console.CapsLock;
            char? keyChar = kl.Key2Char(key, IsToggled);
            HasKeyChar = keyChar != null;
            if(HasKeyChar)
                KeyChar = keyChar.Value;
            IsCorrect = false;
            
        }

    }
}
