using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{

    public class ArNumberLongDecimal : ArNumber
    {
        private long _IntegralNumber;
        private long _FractionalNumber;

        //public const ArNumberDecimal MaxValue = 2147483647.2147483647;
        //public const ArNumberDecimal MinValue = -2147483648.2147483648;

        public ArNumberLongDecimal()
            : this(0, 0)
        { }

        public ArNumberLongDecimal(long integer, long fraction)
        {
            _IntegralNumber = integer;
            _FractionalNumber = Math.Abs(fraction);
        }
        public ArNumberLongDecimal(ArNumberLongDecimal and)
        {
            _IntegralNumber = and._IntegralNumber;
            _FractionalNumber = and._FractionalNumber;
        }
        public static ArNumberLongDecimal Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberLongDecimal Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberLongDecimal Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberLongDecimal Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArNumberLongDecimal and = new ArNumberLongDecimal();
            if (s.Contains("."))
            {
                string[] splited = s.Split('.');
                and._IntegralNumber = long.Parse(splited[0]);
                and._FractionalNumber = long.Parse(splited[1]);
                return and;
            }
            else
            {
                and._IntegralNumber = long.Parse(s, style, provider);
                return and;
            }
        }

        public static bool TryParse(string s, out ArNumberLongDecimal result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberLongDecimal result)
        {
            try
            {
                result = Parse(s, style, provider);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        public int CompareTo(object value)
        {
            //To Do
            return 1;
        }
        public int CompareTo(ArNumberLongDecimal value)
        {
            int c = _IntegralNumber.CompareTo(value._IntegralNumber);
            if (c != 0)
                return c;
            return _FractionalNumber.CompareTo(value._FractionalNumber;
        }
        public bool Equals(ArNumberLongDecimal value)
            => _IntegralNumber == value._IntegralNumber && _FractionalNumber == value._FractionalNumber;
        public override bool Equals(object value)
        {
            //To Do
            return false;
        }
        public override int GetHashCode()
            => _IntegralNumber.GetHashCode() ^ _FractionalNumber.GetHashCode();
        public TypeCode GetTypeCode()
            => TypeCode.Object;
        public override string ToString()
            => ToString(null, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(string format, IFormatProvider provider)
        {
            if (_FractionalNumber != 0)
                return $"{_IntegralNumber.ToString(format, provider)}.{_FractionalNumber.ToString(format, provider)}";
            else
                return _IntegralNumber.ToString(format, provider);
        }
    }
}


