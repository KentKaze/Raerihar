using System;
using System.Globalization;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public sealed class ArNumberShort : ArNumber
    {
        private short _Number;        
        public const short MaxValue = 32767;
        public const short MinValue = -32768;
        public ArNumberShort(short value)
            => _Number = value;
        public static ArNumberShort Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberShort Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberShort Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberShort Parse(string s, NumberStyles style, IFormatProvider provider)
            => new ArNumberShort(short.Parse(s, style, provider));
        public static bool TryParse(string s, out ArNumberShort result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberShort result)
        {
            result = null;
            if (!short.TryParse(s, style, provider, out short b))
                return false;
            result = new ArNumberShort(b);
            return true;
        }
        public int CompareTo(object value)
        {
            //To Do
            return 1;
        }
        public int CompareTo(byte value)
            => _Number.CompareTo(value);
        public int CompareTo(ArNumberShort value)
            => _Number.CompareTo(value._Number);
        public bool Equals(ArNumberShort value)
            => _Number.Equals(value._Number);
        public override bool Equals(object value)
        {
            //To Do
            return true;
        }
        public override int GetHashCode()
            => _Number.GetHashCode();
        public TypeCode GetTypeCode()
            => TypeCode.Int16; // Research
        public override string ToString()
            => ToString(null, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(string format, IFormatProvider provider)
            => _Number.ToString(format, provider);

        public static implicit operator ArNumberShort(short a)
            => new ArNumberShort(a);
        public static implicit operator short(ArNumberShort a)
            => a._Number;
    }
}
