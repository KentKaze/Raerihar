using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //Exponent 最後一位Exponent值 / 9
    //Numbers 逆順 存正數
    public class ArNumberScientificNotation : ArNumber
    {
        public bool Negative { get; set; }
        public long Exponent 
        {
            get => (_Numbers.Length + _BlockE - 1) * 9 
                + _Numbers[_Numbers.Length - 1].ToString().Length - 1;            
        }
        private int _BlockE;
        private int[] _Numbers;

        public const int BlockMaxValue = 999999999;
        public const int BlockMinValue = 0;

        public override object Integer => throw new NotSupportedException();
        public override object Fraction => throw new NotSupportedException();

        public ArNumberScientificNotation()
        {
            _Numbers = new int[1];
        }

        public ArNumberScientificNotation(ArNumberScientificNotation value)
        {
            Negative = value.Negative;
            _BlockE = value._BlockE;
            _Numbers = new int[value._Numbers.LongLength];
            for (long i = 0; i < value._Numbers.Length; i++)
                _Numbers[i] = value._Numbers[i];
        }
        public ArNumberScientificNotation(byte value)
        {
            _Numbers = new int[] { value };
        }
        public ArNumberScientificNotation(short value)
        {
            Negative = value < 0;
            _Numbers = new int[] { Math.Abs(value) };
        }
        public ArNumberScientificNotation(int value)
        {
            Negative = value < 0;
            value = Math.Abs(value);
            if (value < BlockMaxValue)
                _Numbers = new int[] { value };
            else
            {
                int q = value / (BlockMaxValue + 1);
                int r = value % (BlockMaxValue + 1);
                _Numbers = new int[] { r, q };
            }
        }
        public ArNumberScientificNotation(long value)
        {
            Negative = value < 0;
            value = Math.Abs(value);
            if (value < BlockMaxValue)
                _Numbers = new int[] { (int)value };
            else
            {
                List<int> numberList = new List<int>();
                while(value != 0)
                {   
                    numberList.Add((int)(value % (BlockMaxValue + 1)));
                    value = value / (BlockMaxValue + 1);
                }
                _Numbers = numberList.ToArray();
            }
        }
        public ArNumberScientificNotation(decimal value)
           : this()
            => Parse(this, value.ToString());
        public ArNumberScientificNotation(float value)
            : this()
            => Parse(this, value.ToString());
        public ArNumberScientificNotation(double value)
            : this()
            => Parse(this, value.ToString());
        public ArNumberScientificNotation(ArNumberDecimal value)
            : this()
            => Parse(this, value.ToString());
        public ArNumberScientificNotation(ArNumberLongDecimal value)
            : this()
            => Parse(this, value.ToString());

        internal ArNumberScientificNotation(int exponent, int[] numbers)
        {
            _BlockE = exponent;
            _Numbers = numbers;
        }

        public static bool TryParse(string s, out ArNumberScientificNotation result)
           => TryParse(s, NumberStyles.None, null, out result);
        public static bool TryParse(string s, NumberStyles style, out ArNumberScientificNotation result)
            => TryParse(s, style, null, out result);
        public static bool TryParse(string s, IFormatProvider provider, out ArNumberScientificNotation result)
            => TryParse(s, NumberStyles.None, provider, out result);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumberScientificNotation result)
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

        public static ArNumberScientificNotation Parse(string s)
            => Parse(s, NumberStyles.None, null);
        public static ArNumberScientificNotation Parse(string s, NumberStyles style)
            => Parse(s, style, null);
        public static ArNumberScientificNotation Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.None, provider);
        public static ArNumberScientificNotation Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArNumberScientificNotation result = new ArNumberScientificNotation();
            Parse(result, s, style, provider);
            return result;
        }

        private static void Parse(ArNumberScientificNotation an, string s, NumberStyles style = NumberStyles.Number, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            an.Negative = false;
            long e;

            string numberString = s;
            if (style.HasFlag(NumberStyles.AllowThousands))
                numberString = numberString.Replace(",", "");
            numberString = numberString.Trim();

            int eIndex = -2, pointIndex = -1;
            if (numberString[0] == '+')
                numberString = numberString.Remove(0, 1);
            else if (numberString[0] == '-')
            {
                an.Negative = true;
                numberString = numberString.Remove(0, 1);
            }
            numberString = numberString.TrimStart('0');

            for (int i = 0; i < numberString.Length; i++)
            {
                if (i == eIndex + 1)
                {
                    if (numberString[i] != '+' && numberString[i] != '-')
                        throw new ArgumentException($"{nameof(s)}:{s}");
                }
                else if (numberString[i] == '.')
                    if (pointIndex == -1)
                        pointIndex = i;
                    else
                        throw new ArgumentException($"{nameof(s)}:{s}");
                else if (numberString[i] == 'E' || numberString[i] == 'e')
                    if (i == 0)
                        throw new ArgumentException($"{nameof(s)}:{s}");
                    else if (eIndex == -2)
                        eIndex = i;
                    else
                        throw new ArgumentException($"{nameof(s)}:{s}");
                else if (!char.IsDigit(numberString[i]))
                    throw new ArgumentException($"{nameof(s)}:{s}");
            }

            if (eIndex != -2)
            {
                e = int.Parse(numberString.Substring(eIndex + 1));
                numberString = numberString.Remove(eIndex);
            }
            else
                e = 0;

            if (pointIndex != -1)
            {
                numberString = numberString.TrimEnd('0');
                numberString = numberString.Remove(pointIndex, 1);
                if (numberString.Length == 0)
                {
                    an._Numbers = new int[1];
                    an._BlockE = 0;
                    an.Negative = false;
                    return;
                }
                e += pointIndex - 1;
                while (numberString[0] == '0')
                {
                    numberString = numberString.Remove(0, 1);
                    e--;
                }
            }
            else
                e += numberString.Length - 1;

            numberString = numberString.TrimEnd('0');
            //LoadStandardData(numberString, an, isNegative, e);
        }
        //private static void LoadInteger(string numberStringWithSign, ArNumber a)
        //{
        //    bool isNegative = false;
        //    if (numberStringWithSign[0] == '-')
        //    {
        //        isNegative = true;
        //        numberStringWithSign = numberStringWithSign.Remove(0, 1);
        //    }
        //    long e = numberStringWithSign.Length - 1;
        //    while (numberStringWithSign.Length > 9 && numberStringWithSign.Substring(numberStringWithSign.Length - 9, 9) == "000000000")
        //        numberStringWithSign = numberStringWithSign.Remove(numberStringWithSign.Length - 9, 9);
        //    if (numberStringWithSign == "0")
        //    {
        //        a = new ArNumber();
        //        return;
        //    }
        //    LoadStandardData(numberStringWithSign, a, isNegative, e);
        //}
        //private static void LoadStandardData(string numberString, ArNumber a, bool isNegative = false, long e = 0)
        //{
        //    int pr = PostiveRemainder(e + 1, 9);
        //    if (pr == 0)
        //        pr = 9;
        //    if (!IsInteger(a) || pr > numberString.Length)
        //        numberString = $"{numberString}{new string('0', pr - numberString.Length % 9)}";
        //    //if (pr > numberString.Length)
        //    //    numberString = $"{numberString}{new string('0', pr - numberString.Length)}";

        //    a.SetExponent(e);
        //    a._Numbers = new byte[(a.GetBits(numberString.Length) + 7) / 8];
        //    a.Negative = isNegative;


        //    uint v;
        //    int digitIndex = numberString.Length;
        //    int substractedDigits;
        //    for (int i = 0; digitIndex > 0; i++)
        //    {
        //        if (i == 0)
        //        {
        //            if ((numberString.Length - pr) % 9 != 0)
        //                substractedDigits = (numberString.Length - pr) % 9;
        //            else if (numberString.Length < 9)
        //                substractedDigits = numberString.Length;
        //            else
        //                substractedDigits = 9;
        //        }
        //        else if (digitIndex < 9)
        //            substractedDigits = PostiveRemainder(e + 1, 9);
        //        else
        //            substractedDigits = 9;
        //        digitIndex -= substractedDigits;
        //        v = uint.Parse(numberString.Substring(digitIndex, substractedDigits));
        //        a.SetNumberBlock(i, v, substractedDigits);
        //    }
        //}


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
