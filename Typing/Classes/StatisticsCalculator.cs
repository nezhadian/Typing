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
        public List<KeyData> KeyList = new List<KeyData>();
        bool isFirstKey = true;

        public TimeSpan ElapsedTime
        {
            get
            {
                if (isFirstKey)
                    return new TimeSpan(0);

                TimeSpan et = new TimeSpan();
                for (int i = 0; i < KeyList.Count; i++)
                {
                    KeyData key = KeyList[i];
                    et += key.DelayTime;
                }
                et += (DateTime.Now - LastTime);
                return et;
            }
        }

        internal void AddKey(KeyData data)
        {
            if (isFirstKey)
            {
                LastTime = DateTime.Now;
                isFirstKey = false;
            }
            data.DelayTime = DateTime.Now - LastTime;
            KeyList.Add(data);

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
