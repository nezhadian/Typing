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
        bool isFirstKey = true;

        public TimeSpan ElapsedKeysTime;
        public TimeSpan ElapsedTime
        {
            get
            {
                if (isFirstKey)
                    return new TimeSpan(0);
                return ElapsedKeysTime + (DateTime.Now - LastTime);
            }
        }

        public double CorrectWordPerMinute
        {
            get
            {
                if (isFirstKey)
                    return 0;

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

                double divisor = ElapsedTime.TotalSeconds / 60;
                return correctWord / divisor;
            }
        }

        internal void AddKey(KeyData data)
        {
            if (isFirstKey)
            {
                LastTime = DateTime.Now;
                ElapsedKeysTime = new TimeSpan(0);
                isFirstKey = false;
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
