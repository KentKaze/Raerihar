using System;
using System.Globalization;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public sealed class ArNumberShort : ArNumber
    {
        private short _Number;        
        public const short MaxValue = 32767;
        public const short MinValue = -32768;
        public override object Integer => _Number;
        public override object Fraction => 0;
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
        public int CompareTo(short value)
            => _Number.CompareTo(value);
        public int CompareTo(ArNumberShort value)
            => _Number.CompareTo(value._Number);
        public bool Equals(ArNumberShort value)
            => _Number.Equals(value._Number);
        public override bool Equals(object value)
        {
            if (ReferenceEquals(this, value))
                return true;
            if (value is null)
                return false;
            if (!(value is ArNumberShort b))
                return false;
            return _Number == b._Number;
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
            => ConvertToArNumber(_Number + (byte)b.Integer);
        public override ArNumber Add(ArNumberShort b)
            => ConvertToArNumber(_Number + b._Number);
        public override ArNumber Add(ArNumberInt b)
            => ConvertToArNumber(_Number + (int)b.Integer);
        public override ArNumber Add(ArNumberLong b)
            => ConvertToArNumber(_Number + (long)b.Integer);
        public override ArNumber Add(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) + b;
        public override ArNumber Minus(ArNumberByte b)
            => ConvertToArNumber(_Number - (byte)b.Integer);
        public override ArNumber Minus(ArNumberShort b)
            => ConvertToArNumber(_Number - b._Number);
        public override ArNumber Minus(ArNumberInt b)
            => ConvertToArNumber(_Number - (int)b.Integer);
        public override ArNumber Minus(ArNumberLong b)
            => ConvertToArNumber(_Number - (long)b.Integer);
        public override ArNumber Minus(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) - b;
        public override ArNumber Multiply(ArNumberByte b)
            => ConvertToArNumber(_Number * (byte)b.Integer);
        public override ArNumber Multiply(ArNumberShort b)
            => ConvertToArNumber(_Number - b._Number);
        public override ArNumber Multiply(ArNumberInt b)
            => ConvertToArNumber(_Number - (int)b.Integer);
        public override ArNumber Multiply(ArNumberLong b)
            => ConvertToArNumber(_Number - (long)b.Integer);
        public override ArNumber Multiply(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) * b;
        public override ArNumber Quotient(ArNumberByte b)
            => ConvertToArNumber(_Number / (byte)b.Integer);
        public override ArNumber Quotient(ArNumberShort b)
            => ConvertToArNumber(_Number / b._Number);
        public override ArNumber Quotient(ArNumberInt b)
            => ConvertToArNumber(_Number / (int)b.Integer);
        public override ArNumber Quotient(ArNumberLong b)
            => ConvertToArNumber(_Number / (long)b.Integer);
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
            => ConvertToArNumber(_Number / (byte)b.Integer);
        public override ArNumber Remainder(ArNumberShort b)
            => ConvertToArNumber(_Number / b._Number);
        public override ArNumber Remainder(ArNumberInt b)
            => ConvertToArNumber(_Number / (int)b.Integer);
        public override ArNumber Remainder(ArNumberLong b)
            => ConvertToArNumber(_Number / (long)b.Integer);
        public override ArNumber Remainder(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Remainder(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this).Remainder(b);

        public static implicit operator ArNumberShort(short a)
            => new ArNumberShort(a);
        public static implicit operator short(ArNumberShort a)
            => a._Number;
    }
}
