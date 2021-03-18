using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ArNumberDecimal : ArNumber
    {
        private int _IntegralNumber;
        private int _FractionalNumber;

        //public const ArNumberDecimal MaxValue = 2147483647.2147483647;
        //public const ArNumberDecimal MinValue = -2147483648.2147483648;

        public ArNumberDecimal()
            : this (0, 0)
        { }

        public ArNumberDecimal(int integer, int fraction)
        {
            _IntegralNumber = integer;
            _FractionalNumber = Math.Abs(fraction);
        }
        public ArNumberDecimal(ArNumberDecimal and)
        {
            _IntegralNumber = and._IntegralNumber;
            _FractionalNumber = and._FractionalNumber;
        }
        public static ArNumberDecimal Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberDecimal Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberDecimal Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberDecimal Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArNumberDecimal and = new ArNumberDecimal();
            if(s.Contains("."))
            {
                string[] splited = s.Split('.');
                and._IntegralNumber = int.Parse(splited[0]);
                and._FractionalNumber = int.Parse(splited[1]);
                return and;
            }
            else
            {
                and._IntegralNumber = int.Parse(s, style, provider);
                return and;
            }
        }
            
        public static bool TryParse(string s, out ArNumberDecimal result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberDecimal result)
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
        public int CompareTo(ArNumberDecimal value)
        {
            int c = _IntegralNumber.CompareTo(value._IntegralNumber);
            if (c != 0)
                return c;
            return _FractionalNumber.CompareTo(value._FractionalNumber;
        }
        public bool Equals(ArNumberDecimal value)
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
