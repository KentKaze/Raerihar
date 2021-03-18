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
        public override object Integer => _Number;
        public override object Fraction => 0;
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
            => ConvertToArNumber(_Number + b._Number);
        public override ArNumber Add(ArNumberInt b)
            => ConvertToArNumber(_Number + b._Number);
        public override ArNumber Add(ArNumberShort b)
            => b.Add(this);
        public override ArNumber Add(ArNumberLong b)
            => b.Add(this);
        public override ArNumber Add(ArNumberDecimal b)
            => b.Add(this);
        public override ArNumber Add(ArNumberLongDecimal b)
            => b.Add(this);
        public override ArNumber Add(ArNumberScientificNotation b)
            => new ArNumberScientificNotation(this) + b;
        public override ArNumber Minus(ArNumberByte b)
            => ConvertToArNumber(_Number - b._Number);
        public override ArNumber Minus(ArNumberShort b)
            => b.Minus(this);
        public override ArNumber Minus(ArNumberInt b)
            => b.Minus(this);
        public override ArNumber Minus(ArNumberLong b)
            => b.Minus(this);
        public override ArNumber Minus(ArNumberDecimal b) 
            => b.Minus(this);
        public override ArNumber Minus(ArNumberLongDecimal b)
            => b.Minus(this);
        public override ArNumber Minus(ArNumberScientificNotation b)
            => b.Minus(this);
        public override ArNumber Multiply(ArNumberByte b)
             => ConvertToArNumber(_Number * b._Number);
        public override ArNumber Multiply(ArNumberShort b)
            => b.Multiply(this);
        public override ArNumber Multiply(ArNumberInt b)
            => b.Multiply(this);
        public override ArNumber Multiply(ArNumberLong b)
            => b.Multiply(this);
        public override ArNumber Multiply(ArNumberDecimal b)
            => b.Multiply(this);
        public override ArNumber Multiply(ArNumberLongDecimal b)
            => b.Multiply(this);
        public override ArNumber Multiply(ArNumberScientificNotation b)
            => b.Multiply(this);
        public override ArNumber Divide(ArNumberByte b)
            => ConvertToArNumber(_Number / b._Number); // To DO
        public override ArNumber Divide(ArNumberShort b)
            => b.Divide(this);
        public override ArNumber Divide(ArNumberInt b)
            => b.Divide(this);
        public override ArNumber Divide(ArNumberLong b) 
            => b.Divide(this);
        public override ArNumber Divide(ArNumberDecimal b)
            => b.Divide(this);
        public override ArNumber Divide(ArNumberLongDecimal b)
            => b.Divide(this);
        public override ArNumber Divide(ArNumberScientificNotation b)
            => b.Divide(this);
        public override ArNumber Quotient(ArNumberByte b)
            => ConvertToArNumber(_Number / b._Number);
        public override ArNumber Quotient(ArNumberShort b)
            => b.Quotient(this);
        public override ArNumber Quotient(ArNumberInt b)
            => b.Quotient(this);
        public override ArNumber Quotient(ArNumberLong b)
            => b.Quotient(this);
        public override ArNumber Quotient(ArNumberDecimal b)
            => b.Quotient(this);
        public override ArNumber Quotient(ArNumberLongDecimal b)
            => b.Quotient(this);
        public override ArNumber Quotient(ArNumberScientificNotation b)
            => b.Quotient(this);
        public override ArNumber Remainder(ArNumberByte b)
             => ConvertToArNumber(_Number % b._Number);
        public override ArNumber Remainder(ArNumberShort b)
            => b.Remainder(this);
        public override ArNumber Remainder(ArNumberInt b)
            => b.Remainder(this);
        public override ArNumber Remainder(ArNumberLong b)
            => b.Remainder(this);
        public override ArNumber Remainder(ArNumberDecimal b)
            => b.Remainder(this);
        public override ArNumber Remainder(ArNumberLongDecimal b)
            => b.Remainder(this);
        public override ArNumber Remainder(ArNumberScientificNotation b)
            => b.Remainder(this);

        public static implicit operator ArNumberByte(byte a)
            => new ArNumberByte(a);
        public static implicit operator byte(ArNumberByte a)
            => a._Number;
    }
}
