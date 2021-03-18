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

        //TO DO
        public static ArNumber Divide(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => new ArNumberScientificNotation();
        public static ArNumber operator +(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => new ArNumberScientificNotation();
        public static ArNumber operator -(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => new ArNumberScientificNotation();
        public static ArNumber operator *(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => new ArNumberScientificNotation();
        public static ArNumber operator /(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => new ArNumberScientificNotation();
        public static ArNumber operator %(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => b.ReverseRemainder(a);
    }
}
