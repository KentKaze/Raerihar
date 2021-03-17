using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public sealed class ArNumberLong : ArNumber
    {
        private long _Number;        
        public const long MaxValue = 9223372036854775807;
        public const long MinValue = -9223372036854775808;
        public ArNumberLong(long value)
            => _Number = value;
        public static ArNumberLong Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberLong Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberLong Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberLong Parse(string s, NumberStyles style, IFormatProvider provider)
            => new ArNumberLong(long.Parse(s, style, provider));
        public static bool TryParse(string s, out ArNumberLong result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberLong result)
        {
            result = null;
            if (!long.TryParse(s, style, provider, out long b))
                return false;
            result = new ArNumberLong(b);
            return true;
        }
        public int CompareTo(object value)
        {
            //To Do
            return 1;
        }
        public int CompareTo(long value)
            => _Number.CompareTo(value);
        public int CompareTo(ArNumberLong value)
            => _Number.CompareTo(value._Number);
        public bool Equals(ArNumberLong value)
            => _Number.Equals(value._Number);
        public override bool Equals(object value)
        {
            //To Do
            return true;
        }
        public override int GetHashCode()
            => _Number.GetHashCode();
        public TypeCode GetTypeCode()
            => TypeCode.Int32; // Research
        public override string ToString()
            => ToString(null, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(string format, IFormatProvider provider)
            => _Number.ToString(format, provider);

        private static ArNumber AddMinus(ArNumberLong a, ArNumberLong b)
        {
            throw new NotImplementedException();
            //a._Number + b._Number
        }
        public ArNumber Add(ArNumberLong b)
        {
            return this;
        }

        public static implicit operator ArNumberLong(long a)
            => new ArNumberLong(a);
        public static implicit operator long(ArNumberLong a)
            => a._Number;
    }
}
