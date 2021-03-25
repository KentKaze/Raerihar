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
        public int Exponent 
        {
            get => (_Numbers.Length + _LastBlockE - 1) * 9 
                + _Numbers[_Numbers.Length - 1].ToString().Length - 1;            
        }
        private int _LastBlockE;
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
            _LastBlockE = value._LastBlockE;
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

        internal ArNumberScientificNotation(int lastE, int[] numbers)
        {
            _LastBlockE = lastE;
            _Numbers = numbers;
        }

        private static int PostiveRemainder(long a, int b)
        {
            int result = (int)(a % b);
            return result >= 0 ? result : result + b;
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
            if(numberString.Length > 1)
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
                    an._LastBlockE = 0;
                    an.Negative = false;
                    return;
                }
                //e += pointIndex - 1;
                e -= numberString.Length - pointIndex;
                numberString = numberString.TrimStart('0');
                //while (numberString[0] == '0')
                //{
                //    numberString = numberString.Remove(0, 1);
                //    e--;
                //}
            }
            //else
            //    e += numberString.Length - 1;
            //3.698783E+103
            //3698783E+97
            //36987 830000000E+90 => LastBlockE = E+90
            //numberString = numberString.TrimEnd('0');
            Read(numberString, an, e);
            //LoadStandardData(numberString, an, isNegative, e);
        }

        private static void Read(string numberString, ArNumberScientificNotation ansn, long lastE)
        {
            int r9 = PostiveRemainder(lastE, 9);
            numberString = $"{numberString}{new string('0', r9)}";
            lastE -= r9;
            ansn._LastBlockE = (int)(lastE / 9);
            ansn._Numbers = new int[(numberString.Length + 8) / 9];
            for (int i = 0; i < ansn._Numbers.Length; i++)
                if (i == ansn._Numbers.Length - 1)
                    ansn._Numbers[i] = int.Parse(numberString.Substring(0, numberString.Length % 9 == 0 ? 9 : numberString.Length % 9));
                else
                    ansn._Numbers[i] = int.Parse(numberString.Substring(numberString.Length - 9 * i - 9, 9));
                    
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

        public string GetNumbersToString()
        {
            StringBuilder numbers = new StringBuilder();
            for (int i = _Numbers.Length - 1; i >= 0; i--)
                numbers.Append(_Numbers[i]);
            if (numbers.Length > 1)
                return numbers.ToString().TrimEnd('0');
            else
                return numbers.ToString();
            //int tail = PostiveRemainder(Exponent + 1, 9);
            //long digitsCount = DigitsCount;

            //if (tail > digitsCount)
            //    tail = (int)digitsCount;
            //else if (tail == 0)
            //    tail = 9;
            //int mid = (int)((digitsCount - tail) / 9);
            //int head = PostiveRemainder(digitsCount - tail, 9);
            //int indexCount = 1 + mid + (head > 0 ? 1 : 0);

            //numbers.Append(GetNumberBlock(indexCount - 1, tail));
            //for (int i = indexCount - 2; i >= 1; i--)
            //    numbers.Append(GetNumberBlock(i, 9).ToString().PadLeft(9, '0'));
            //if (head > 0)
            //    numbers.Append(GetNumberBlock(0, head).ToString().PadLeft(head, '0'));
            //else if (indexCount - 1 != 0)
            //    numbers.Append(GetNumberBlock(0, 9).ToString().PadLeft(9, '0'));
            //while (numbers.Length > 1 && numbers[numbers.Length - 1] == '0')
            //    numbers.Remove(numbers.Length - 1, 1);
            //return numbers.ToString();
        }
        private string ToString(int digitsDisplay, char format, IFormatProvider provider)
        {
            string numbers = GetNumbersToString();
            if (digitsDisplay < 0)
                throw new ArgumentOutOfRangeException(nameof(digitsDisplay));
            else if (digitsDisplay == 0 || digitsDisplay > numbers.Length)
                digitsDisplay = numbers.Length;            
            int e = Exponent;
            if (format == 'G')
                if (e < -30 || e > 30)
                    format = 'E';
                else if (e >= 0)
                    format = 'D';
                else
                    format = 'F';

            //if(format == 'C' && format == 'N' && format == 'P')
            //CDFNPXEG
            StringBuilder result = new StringBuilder();
            result.Append(numbers);
            if (result.Length != 1 && format == 'E')
                result.Insert(1, '.');
            if (e != 0)
            {
                if (format == 'E')
                    result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
                else if (format == 'D')
                {
                    if (e > digitsDisplay - 1)
                        result.Append(new string('0', (int)e - digitsDisplay + 1));
                    else
                    {
                        if (e > 0)
                            result.Remove((int)e + 1, result.Length - (int)e - 1);
                        else
                            return "0";
                    }
                }
                else if (format == 'F')
                {
                    if (e > digitsDisplay - 1)
                        result.Append(new string('0', (int)e - digitsDisplay + 1));
                    else if (e > 0)
                        result.Insert((int)e + 1, '.');
                    else
                        result.Insert(0, $"0.{new string('0', Math.Abs((int)e + 1))}");
                }
            }
            else if (result.Length > 1)
                result.Insert(1, '.');

            if (Negative)
                result.Insert(0, '-');
            return result.ToString();
        }

        public override string ToString()
            => ToString(null, null);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format, IFormatProvider provider)
        {
            int length = 0;
            if (string.IsNullOrEmpty(format))
                format = "G";
            format = format.Trim().ToUpperInvariant();
            if (provider == null)
                provider = NumberFormatInfo.CurrentInfo;
            if (format.Length > 1 && !int.TryParse(format.Substring(1), out length))
                throw new FormatException($"{nameof(format)}:{format}");

            switch (format[0])
            {
                // TO DO
                case 'C':
                case 'D':
                case 'F':
                case 'N':
                case 'P':
                case 'X':
                case 'E':
                case 'G':
                    return ToString(length, format[0], provider);
                default:
                    throw new FormatException(string.Format("The '{0}' format string is not supported.", format));
            }
        }

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
