using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ArNumberLongDecimal : ArNumber
    {
        private long _Integer;
        private long _Fraction;

        public const long MaxFraction = 999999999999999999;
        public const long MinFraction = 0;
        public const long MaxInteger = 9223372036854775807;
        public const long MinInteger = -9223372036854775808;
        public override object Integer => _Integer;
        public override object Fraction => _Fraction;
        public ArNumberLongDecimal()
            : this(0, 0)
        { }

        public ArNumberLongDecimal(long integer, long fraction)
        {
            _Integer = integer;
            _Fraction = Math.Abs(fraction);
        }
        public ArNumberLongDecimal(ArNumberLongDecimal and)
        {
            _Integer = and._Integer;
            _Fraction = and._Fraction;
        }
        public static ArNumberLongDecimal Parse(string s)
            => Parse(s, NumberStyles.Number, null);
        public static ArNumberLongDecimal Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.Number, provider);
        public static ArNumberLongDecimal Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberLongDecimal Parse(string s, NumberStyles style, IFormatProvider provider)
            => Parse(new ArNumberLongDecimal(), s, style, provider);
        private static ArNumberLongDecimal Parse(ArNumberLongDecimal anld, string s)
            => Parse(anld, s, NumberStyles.Number, null);
        private static ArNumberLongDecimal Parse(ArNumberLongDecimal anld, string s, NumberStyles style, IFormatProvider provider)
        {
            anld._Integer = 0;
            anld._Fraction = 0;
            if (s.Contains("."))
            {
                string[] split = s.Split('.');
                anld._Integer = long.Parse(split[0]);
                anld._Fraction = Math.Abs(long.Parse(split[1].PadRight(18, '0').Substring(0, 18))); //To Do: Round
                return anld;
            }
            else
            {
                anld._Integer = int.Parse(s, style, provider);
                return anld;
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
        public int CompareTo(ArNumberLongDecimal value)
        {
            int c = _Integer.CompareTo(value._Integer);
            if (c != 0)
                return c;
            return _Fraction.CompareTo(value._Fraction);
        }
        public bool Equals(ArNumberLongDecimal value)
            => _Integer == value._Integer && _Fraction == value._Fraction;
        public override bool Equals(object value)
        {
            if (ReferenceEquals(this, value))
                return true;
            if (value is null)
                return false;
            if (!(value is ArNumberLongDecimal b))
                return false;
            return Equals(b);
        }
        public override int GetHashCode()
            => _Integer.GetHashCode() ^ _Fraction.GetHashCode();
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
            if (_Fraction != 0)
                return $"{_Integer.ToString(format, provider)}.{_Fraction.ToString(format, provider).PadLeft(18, '0').TrimEnd('0')}";
            else
                return _Integer.ToString(format, provider);
        }

        protected override ArNumber ReverseAdd(ArNumber b)
            => b.Add(this);
        protected override ArNumber ReverseMinus(ArNumber b)
            => b.Minus(this);
        protected override ArNumber ReverseMultiply(ArNumber b)
            => b.Multiply(this);
        protected override ArNumber ReverseQuotient(ArNumber b)
            => b.Quotient(this);
        protected override ArNumber ReverseDivide(ArNumber b)
            => b.Divide(this);
        protected override ArNumber ReverseRemainder(ArNumber b)
            => b.Remainder(this);
        public override ArNumber Add(ArNumberByte b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);
        public override ArNumber Add(ArNumberShort b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);
        public override ArNumber Add(ArNumberInt b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);
        public override ArNumber Add(ArNumberLong b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);            
        public override ArNumber Add(ArNumberDecimal b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);
        public override ArNumber Add(ArNumberLongDecimal b)
            => new ArNumberScientificNotation(this) + new ArNumberScientificNotation(b);
        public override ArNumber Add(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) + b;
        public override ArNumber Minus(ArNumberByte b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberShort b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberInt b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberLong b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberDecimal b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberLongDecimal b)
            => new ArNumberScientificNotation(this) - new ArNumberScientificNotation(b);
        public override ArNumber Minus(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) - b;
        public override ArNumber Multiply(ArNumberByte b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberShort b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberInt b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberLong b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberDecimal b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberLongDecimal b)
            => new ArNumberScientificNotation(this) * new ArNumberScientificNotation(b);
        public override ArNumber Multiply(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) * b;
        public override ArNumber Quotient(ArNumberByte b)
            => ConvertToArNumber(_Integer / b, 0);
        public override ArNumber Quotient(ArNumberShort b)
            => ConvertToArNumber(_Integer / b, 0);
        public override ArNumber Quotient(ArNumberInt b)
            => ConvertToArNumber(_Integer / b, 0);
        public override ArNumber Quotient(ArNumberLong b)
            => ConvertToArNumber(_Integer / b, 0);
        public override ArNumber Quotient(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) / b;
        public override ArNumber Divide(ArNumberByte b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberInt b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Divide(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this).Divide(b);
        public override ArNumber Remainder(ArNumberByte b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberInt b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this).Remainder(b);

        public static implicit operator ArNumberLongDecimal(float a)
            => new ArNumberLongDecimal(a);
        public static implicit operator ArNumberLongDecimal(double a)
            => new ArNumberLongDecimal(a);
        public static implicit operator float(ArNumberLongDecimal a)
            => float.Parse(a.ToString());
        public static implicit operator double(ArNumberLongDecimal a)
            => double.Parse(a.ToString());
    }
}


