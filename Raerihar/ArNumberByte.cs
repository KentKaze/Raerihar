using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public sealed class ArNumberByte : ArNumber
    {
        private byte _Number;        
        public const byte MaxValue = 255;
        public const byte MinValue = 0;
        public ArNumberByte(byte value)
            => _Number = value;
        public static ArNumberByte Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberByte Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberByte Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberByte Parse(string s, NumberStyles style, IFormatProvider provider)
            => new ArNumberByte(byte.Parse(s, style, provider));
        public static bool TryParse(string s, out ArNumberByte result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberByte result)
        {
            result = null;
            if (!byte.TryParse(s, style, provider, out byte b))
                return false;
            result = new ArNumberByte(b);
            return true;
        }
        public int CompareTo(object value)
        {
            //To Do
            return 1;
        }
        public int CompareTo(byte value)
            => _Number.CompareTo(value);
        public int CompareTo(ArNumberByte value)
            => _Number.CompareTo(value._Number);
        public bool Equals(ArNumberByte value)
            => _Number.Equals(value._Number);
        public override bool Equals(object value)
        {
            //To Do
            return 1;
        }
        public override int GetHashCode()
            => _Number.GetHashCode();
        public TypeCode GetTypeCode()
            => TypeCode.Byte; // Research
        public override string ToString()
            => ToString(null, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(string format, IFormatProvider provider)
            => _Number.ToString(format, provider);

        public static implicit operator ArNumberByte(byte a)
            => new ArNumberByte(a);
        public static implicit operator byte(ArNumberByte a)
            => a._Number;
    }
}
