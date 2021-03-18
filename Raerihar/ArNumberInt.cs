using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public sealed class ArNumberInt : ArNumber
    {
        private int _Number;        
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;
        public override object Integer => _Number;
        public override object Fraction => 0;
        public ArNumberInt(int value)
            => _Number = value;
        public static ArNumberInt Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberInt Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberInt Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberInt Parse(string s, NumberStyles style, IFormatProvider provider)
            => new ArNumberInt(byte.Parse(s, style, provider));
        public static bool TryParse(string s, out ArNumberInt result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberInt result)
        {
            result = null;
            if (!int.TryParse(s, style, provider, out int i))
                return false;
            result = new ArNumberInt(i);
            return true;
        }
        public int CompareTo(object value)
        {
            //To Do
            return 1;
        }
        public int CompareTo(int value)
            => _Number.CompareTo(value);
        public int CompareTo(ArNumberInt value)
            => _Number.CompareTo(value._Number);
        public bool Equals(ArNumberInt value)
            => _Number.Equals(value._Number);
        public override bool Equals(object value)
        {
            //To Do
            return true;
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

        private static ArNumber Add(ArNumberInt a, ArNumberInt b)
            => ConvertToArNumber(a._Number + b._Number);
        private static ArNumber Minus(ArNumberInt a, ArNumberInt b)
            => ConvertToArNumber(a._Number + b._Number);
        public ArNumber Add(ArNumberInt b)
        {
            return this;
        }

        public override ArNumber Add(ArNumber b)
            => b.ReverseAdd(this);
    

        public static implicit operator ArNumberInt(int a)
            => new ArNumberInt(a);
        public static implicit operator int(ArNumberInt a)
            => a._Number;
    }
}
