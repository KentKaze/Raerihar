using System;
using System.Collections.Generic;
using System.Text;


//V4 Design

//_Data
//Exponent 4 bit 12bit 28bit 60bit
//Last 4 bit
//0~8      BigNumberHead 型別1
//9        byte          型別2
//10       short
//11       int
//12       long
//13       int.int       型別3
//14       long.long     型別4
//15(reserve)

//_Numbers
// 9 1 byte
//10 2 bytes
//11 4 bytes
//12 8 bytes
//13 8 bytes
//14 16 bytes
//0~8 first 1 sign 倒序排列
//300存為 => 300 E+2
//0.4存為 => 4   E-1

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public abstract class ArNumber
    {
        public abstract object Integer { get; }
        public abstract object Fraction { get; }
        public static ArNumber ConvertToArNumber(byte value)
            => new ArNumberByte(value);
        public static ArNumber ConvertToArNumber(short value) 
            => new ArNumberByte((byte)value);             
        public static ArNumber ConvertToArNumber(int value)
        {
            if (value <= byte.MaxValue && value >= byte.MinValue)
                return new ArNumberByte((byte)value);
            else if(value <= short.MaxValue && value >= short.MinValue)
                return new ArNumberShort((short)value);
            return new ArNumberInt(value);            
        }
            
        public static ArNumber ConvertToArNumber(long value)
        {
            if (value <= byte.MaxValue && value >= byte.MinValue)
                return new ArNumberByte((byte)value);
            else if (value <= short.MaxValue && value >= short.MinValue)
                return new ArNumberShort((short)value);
            else if (value <= int.MaxValue && value >= int.MinValue)
                return new ArNumberInt((int)value);
            return new ArNumberLong(value);
        }

        protected abstract ArNumber ReverseAdd(ArNumber b);
        protected abstract ArNumber ReverseMinus(ArNumber b);
        protected abstract ArNumber ReverseMultiply(ArNumber b);
        protected abstract ArNumber ReverseQuotient(ArNumber b);
        protected abstract ArNumber ReverseDivide(ArNumber b);
        protected abstract ArNumber ReverseRemainder(ArNumber b);
        public abstract ArNumber Add(ArNumberByte b);
        public abstract ArNumber Add(ArNumberShort b);
        public abstract ArNumber Add(ArNumberInt b);
        public abstract ArNumber Add(ArNumberLong b);
        public abstract ArNumber Add(ArNumberDecimal b);
        public abstract ArNumber Add(ArNumberLongDecimal b);
        public abstract ArNumber Add(ArNumberScientificNotation b);
        public abstract ArNumber Minus(ArNumberByte b);
        public abstract ArNumber Minus(ArNumberShort b);
        public abstract ArNumber Minus(ArNumberInt b);
        public abstract ArNumber Minus(ArNumberLong b);
        public abstract ArNumber Minus(ArNumberDecimal b);
        public abstract ArNumber Minus(ArNumberLongDecimal b);
        public abstract ArNumber Minus(ArNumberScientificNotation b);
        public abstract ArNumber Multiply(ArNumberByte b);
        public abstract ArNumber Multiply(ArNumberShort b);
        public abstract ArNumber Multiply(ArNumberInt b);
        public abstract ArNumber Multiply(ArNumberLong b);
        public abstract ArNumber Multiply(ArNumberDecimal b);
        public abstract ArNumber Multiply(ArNumberLongDecimal b);
        public abstract ArNumber Multiply(ArNumberScientificNotation b);
        public abstract ArNumber Divide(ArNumberByte b);
        public abstract ArNumber Divide(ArNumberShort b);
        public abstract ArNumber Divide(ArNumberInt b);
        public abstract ArNumber Divide(ArNumberLong b);
        public abstract ArNumber Divide(ArNumberDecimal b);
        public abstract ArNumber Divide(ArNumberLongDecimal b);
        public abstract ArNumber Divide(ArNumberScientificNotation b);
        public abstract ArNumber Quotient(ArNumberByte b);
        public abstract ArNumber Quotient(ArNumberShort b);
        public abstract ArNumber Quotient(ArNumberInt b);
        public abstract ArNumber Quotient(ArNumberLong b);
        public abstract ArNumber Quotient(ArNumberDecimal b);
        public abstract ArNumber Quotient(ArNumberLongDecimal b);
        public abstract ArNumber Quotient(ArNumberScientificNotation b);
        public abstract ArNumber Remainder(ArNumberByte b);
        public abstract ArNumber Remainder(ArNumberShort b);
        public abstract ArNumber Remainder(ArNumberInt b);
        public abstract ArNumber Remainder(ArNumberLong b);
        public abstract ArNumber Remainder(ArNumberDecimal b);
        public abstract ArNumber Remainder(ArNumberLongDecimal b);
        public abstract ArNumber Remainder(ArNumberScientificNotation b);
        public static ArNumber operator +(ArNumber a, ArNumber b)
            => b.ReverseAdd(a);
        public static ArNumber operator -(ArNumber a, ArNumber b)
            => b.ReverseMinus(a);
        public static ArNumber operator *(ArNumber a, ArNumber b)
            => b.ReverseMultiply(a);
        public static ArNumber operator /(ArNumber a, ArNumber b)
            => b.ReverseQuotient(a);
        public static ArNumber operator %(ArNumber a, ArNumber b)
            => b.ReverseRemainder(a);

        //public ArNumber ConvertToArNumber(byte value)
        //    => new ArNumberByte(value);
        //public ArNumber ConvertToArNumber(byte value)
        //    => new ArNumberByte(value);

    }
}
