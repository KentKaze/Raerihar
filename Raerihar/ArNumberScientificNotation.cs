using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ArNumberScientificNotation : ArNumber
    {
        private byte startDigit;
        private int _Exponent;
        private byte[] _Numbers;

        public override object Integer => throw new NotSupportedException();
        public override object Fraction => throw new NotSupportedException();

        //    public ArNumber()
        //    {
        //        _Data = new byte[1];            
        //        _Numbers = new byte[1];
        //    }
        //    public ArNumber(ArNumber a)
        //    {
        //        _Data = new byte[a._Data.Length];
        //        for (int i = 0; i < a._Data.Length; i++)
        //            _Data[i] = a._Data[i];
        //        _Numbers = new byte[a._Numbers.Length];
        //        for (int i = 0; i < a._Numbers.Length; i++)
        //            _Numbers[i] = a._Numbers[i];
        //    }

        public ArNumberScientificNotation()
        {

        }

        public ArNumberScientificNotation(ArNumberScientificNotation value)
        {

        }
        public ArNumberScientificNotation(byte value)
        { }
        public ArNumberScientificNotation(short value)
        { }
        public ArNumberScientificNotation(int value)
        { }
        public ArNumberScientificNotation(long value)
        { }
        public ArNumberScientificNotation(double value)
        { }
        public ArNumberScientificNotation(decimal value)
        { }
        public ArNumberScientificNotation(ArNumberDecimal value)
        { }
        public ArNumberScientificNotation(ArNumberLongDecimal value)
        { }

        //TO DO
        public static ArNumber Divide(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => throw new NotImplementedException();
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
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberInt b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Add(ArNumberScientificNotation b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberByte b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberInt b) 
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Minus(ArNumberScientificNotation b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberByte b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberInt b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Multiply(ArNumberScientificNotation b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberByte b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberShort b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberInt b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberLong b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberLongDecimal b)
            => throw new NotImplementedException();
        public override ArNumber Quotient(ArNumberScientificNotation b)
            => throw new NotImplementedException();
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
            => throw new NotImplementedException();
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
            => throw new NotImplementedException();
        public static ArNumber operator +(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Add(b);
        public static ArNumber operator -(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Minus(b);
        public static ArNumber operator *(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Multiply(b);
        public static ArNumber operator /(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Quotient(b);
        public static ArNumber operator %(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Remainder(b);
    }
}
