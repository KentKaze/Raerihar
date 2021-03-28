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
                while (value != 0)
                {
                    numberList.Add((int)(value % (BlockMaxValue + 1)));
                    value = value / (BlockMaxValue + 1);
                }
                _Numbers = numberList.ToArray();
            }
        }
        public ArNumberScientificNotation(ulong value)
            : this()
            => Parse(this, value.ToString());
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
            if (numberString.Length > 1)
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

        public string GetNumbersToString()
        {
            StringBuilder numbers = new StringBuilder();
            for (int i = _Numbers.Length - 1; i >= 0; i--)
                numbers.Append(_Numbers[i].ToString().PadLeft(9, '0'));
            if (numbers.ToString() == "000000000")
                return "0";
            return numbers.ToString().Trim('0');
        }

        private string ToString(int digitsDisplay, char format, IFormatProvider provider)
        {
            string numbers = GetNumbersToString();
            if (digitsDisplay < 0)
                throw new ArgumentOutOfRangeException(nameof(digitsDisplay));
            int e = Exponent;
            if (format == 'G')
                if (e < -30 || e > 30)
                    format = 'E';
                else if (e - numbers.Length + 1 >= 0)
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
                    if (e > numbers.Length - 1)
                        result.Append(new string('0', e - numbers.Length + 1));
                    else
                    {
                        if (e > 0)
                            result.Remove(e + 1, result.Length - e - 1);
                        else
                            return "0".PadLeft(digitsDisplay, '0');
                    }
                }
                else if (format == 'F')
                {
                    if (e >= numbers.Length - 1)
                        result.Append(new string('0', e - numbers.Length + 1));
                    else if (e > 0)
                        result.Insert(e + 1, '.');
                    else
                        result.Insert(0, $"0.{new string('0', Math.Abs(e + 1))}");
                }
            }
            else if (result.Length > 1)
                result.Insert(1, '.');

            if (Negative)
                result.Insert(0, '-');

            string s = result.ToString();
            if (format == 'F' && digitsDisplay != 0)
            {
                int pIndex = s.IndexOf('.');
                if (pIndex != 0)
                    s = $"{s.Substring(0, pIndex)}{s.Substring(pIndex).PadRight(digitsDisplay + 1, '0').Substring(0, digitsDisplay + 1)}";
                else
                    s = $"{s}{new string('0', digitsDisplay)}";
            }
            else if (format == 'D' && digitsDisplay != 0)
                s = s.PadLeft(digitsDisplay, '0');

            //else if(format == 'E' && digitsDisplay != 0)

            return s;
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

        public static ArNumberScientificNotation Negate(ArNumberScientificNotation a)
        {
            ArNumberScientificNotation result = new ArNumberScientificNotation(a);
            result.Negative = !result.Negative;
            return result;
        }

        private static ArNumberScientificNotation AddMinus(ArNumberScientificNotation a, ArNumberScientificNotation b, bool isAdd = true)
        {
            if (isAdd)
            {
                if (a.Negative && !b.Negative)
                    return AddMinus(b, Negate(a), false);
                else if (!a.Negative && b.Negative)
                    return AddMinus(a, Negate(b), false);
            }
            else
            {
                if ((a.Negative && !b.Negative) || (!a.Negative && b.Negative))
                    return AddMinus(a, Negate(b), true);
                else if (b > a && !a.Negative && !b.Negative)
                    return AddMinus(Negate(b), Negate(a), false);
                else if (a > b && a.Negative && b.Negative)
                    return AddMinus(Negate(b), Negate(a), false);
                else if (a == b)
                    return new ArNumberScientificNotation();
            }

            int lastlastBlockE = a._LastBlockE <= b._LastBlockE ? a._LastBlockE : b._LastBlockE;
            int e = lastlastBlockE;
            int i = 0, j = 0;
            List<int> sumList = new List<int>();
            int plusA, plusB, sum, carry = 0;
            while (i != a._Numbers.Length || j != b._Numbers.Length)
            {
                if (e >= a._LastBlockE && i != a._Numbers.Length)
                    plusA = a._Numbers[i++];
                else
                    plusA = 0;

                if (e >= b._LastBlockE && j != b._Numbers.Length)
                    plusB = b._Numbers[j++];
                else
                    plusB = 0;

                if (isAdd)
                    sum = plusA + plusB + carry;
                else
                    sum = plusA - plusB + carry;

                if (sum > BlockMaxValue)
                {
                    sum -= 1000000000;
                    carry = 1;
                }
                else if (sum < BlockMinValue)
                {
                    sum += 1000000000;
                    carry = -1;
                }
                else
                    carry = 0;

                sumList.Add(sum);
                e++;
            }

            if (carry == 1)
                sumList.Add(1);
            //else
            //    e--;

            while (sumList[0] == 0)
            {
                sumList.RemoveAt(0);
                lastlastBlockE++;
            }

            while (sumList[sumList.Count - 1] == 0)
                sumList.RemoveAt(sumList.Count - 1);

            ArNumberScientificNotation result = new ArNumberScientificNotation();
            if (sumList.Count == 0)
                return result;

            result._Numbers = new int[sumList.Count];
            for (i = 0; i < sumList.Count; i++)
                result._Numbers[i] = sumList[i];
            result._LastBlockE = lastlastBlockE;
            result.Negative = a.Negative;
            return result;
        }

        private static ArNumberScientificNotation Multiply(ArNumberScientificNotation a, ArNumberScientificNotation b)
        {
            //Last E計算
            //Negative計算
            int lastlastBlockE = a._LastBlockE + b._LastBlockE;            
            List<long> sumList = new List<long>();
            sumList.AddRange(new long[a._Numbers.Length + b._Numbers.Length + 1]);
            for (int i = 0; i < a._Numbers.Length; i++)
            {
                for (int j = 0; j < b._Numbers.Length; j++)
                {   
                    sumList[i + j] += (long)a._Numbers[i] * b._Numbers[j];
                    if(sumList[i + j] > BlockMaxValue)
                    {
                        sumList[i + j + 1] += sumList[i + j] / 1000000000;
                        sumList[i + j] %= 1000000000;
                    }
                }
            }

            while (sumList[0] == 0)
            {
                sumList.RemoveAt(0);
                lastlastBlockE++;
            }

            while (sumList[sumList.Count - 1] == 0)
                sumList.RemoveAt(sumList.Count - 1);

            ArNumberScientificNotation result = new ArNumberScientificNotation();
            result._Numbers = new int[sumList.Count];
            for (int i = 0; i < sumList.Count; i++)
                result._Numbers[i] = (int)sumList[i];
            result._LastBlockE = lastlastBlockE;
            result.Negative = a.Negative ^ b.Negative;
            return result;
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
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberShort b)
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberInt b)
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberLong b)
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberDecimal b)
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberLongDecimal b)
            => AddMinus(this, new ArNumberScientificNotation(b));
        public override ArNumber Add(ArNumberScientificNotation b)
            => AddMinus(this, b);
        public override ArNumber Minus(ArNumberByte b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberShort b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberInt b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberLong b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberDecimal b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberLongDecimal b)
            => AddMinus(this, new ArNumberScientificNotation(b), false);
        public override ArNumber Minus(ArNumberScientificNotation b)
            => AddMinus(this, b, false);
        public override ArNumber Multiply(ArNumberByte b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberShort b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberInt b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberLong b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberDecimal b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberLongDecimal b)
            => Multiply(this, new ArNumberScientificNotation(b));
        public override ArNumber Multiply(ArNumberScientificNotation b)
            => Multiply(this, b);
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
        //public static ArNumberScientificNotation operator ++(ArNumberScientificNotation a)
        //    => new ArNumberScientificNotation(a + 1);
        public static ArNumber operator -(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Minus(b);
        //public static ArNumberScientificNotation operator --(ArNumberScientificNotation a)
        //    => new ArNumberScientificNotation(a - 1);
        public static ArNumber operator *(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Multiply(b);
        public static ArNumber operator /(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Quotient(b);
        public static ArNumber operator %(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Remainder(b);

        public int CompareTo(ArNumberScientificNotation other)
        {
            if (Equals(other))
                return 0;
            else if (!Negative && other.Negative)
                return 1;
            else if (Negative && !other.Negative)
                return -1;
            int result = Negative && other.Negative ? -1 : 1;
            if (Exponent > other.Exponent)
                return result;
            else if (Exponent < other.Exponent)
                return result * -1;
            //(To Do - Temp)
            return GetNumbersToString().CompareTo(other.GetNumbersToString()) * result;
            //int indexCount = GetIndexCount();
            //int otherIndexCount = other.GetIndexCount();
            //for (int i = 0; i < indexCount && i < otherIndexCount; i++)
            //    if (GetNumberBlock(indexCount - i, i == 0) > other.GetNumberBlock(otherIndexCount - i, i == 0))
            //        return result;
            //if (indexCount > otherIndexCount)
            //    return result;
            //else if (indexCount < otherIndexCount)
            //    return result * -1;
            //throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int result = _LastBlockE.GetHashCode() ^ Negative.GetHashCode();
            for (int i = 0; i < _Numbers.Length; i++)
                result ^= _Numbers.GetHashCode();
            return result;
        }
        public bool Equals(ArNumberScientificNotation other)
        {
            if (_LastBlockE != other._LastBlockE)
                return false;
            if (Negative != other.Negative)
                return false;
            if (_Numbers.Length != other._Numbers.Length)
                return false;
            for (int i = 0; i < _Numbers.Length; i++)
                if (_Numbers[i] != other._Numbers[i])
                    return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArNumberScientificNotation ar))
                return false;
            return Equals(ar);
        }
        public int CompareTo(object obj)
           => CompareTo((ArNumberScientificNotation)obj);
        public object Clone()
            => new ArNumberScientificNotation(this);
        public TypeCode GetTypeCode()
            => TypeCode.Object;
        public bool ToBoolean(IFormatProvider provider)
            => throw new InvalidCastException();
        public byte ToByte(IFormatProvider provider)
            => (byte)this;
        public char ToChar(IFormatProvider provider)
            => (char)this;
        public DateTime ToDateTime(IFormatProvider provider)
            => throw new InvalidCastException();
        public decimal ToDecimal(IFormatProvider provider)
            => (decimal)this;
        public double ToDouble(IFormatProvider provider)
            => (double)this;
        public short ToInt16(IFormatProvider provider)
            => (short)this;
        public int ToInt32(IFormatProvider provider)
            => (int)this;
        public long ToInt64(IFormatProvider provider)
            => (long)this;
        public sbyte ToSByte(IFormatProvider provider)
            => (sbyte)this;
        public float ToSingle(IFormatProvider provider)
            => (float)this;
        public object ToType(Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(this, conversionType, provider);
        public ushort ToUInt16(IFormatProvider provider)
            => (ushort)this;
        public uint ToUInt32(IFormatProvider provider)
            => (uint)this;
        public ulong ToUInt64(IFormatProvider provider)
            => (ulong)this;

        public static implicit operator ArNumberScientificNotation(sbyte a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(byte a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(short a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(ushort a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(char a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(int a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(uint a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(long a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(ulong a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(decimal a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(float a)
            => new ArNumberScientificNotation(a);
        public static implicit operator ArNumberScientificNotation(double a)
            => new ArNumberScientificNotation(a);
        public static explicit operator sbyte(ArNumberScientificNotation a)
            => sbyte.Parse(a.ToString());
        public static explicit operator byte(ArNumberScientificNotation a)
            => byte.Parse(a.ToString());
        public static explicit operator short(ArNumberScientificNotation a)
            => short.Parse(a.ToString());
        public static explicit operator ushort(ArNumberScientificNotation a)
            => ushort.Parse(a.ToString());
        public static explicit operator char(ArNumberScientificNotation a)
            => (char)int.Parse(a.ToString());
        public static explicit operator int(ArNumberScientificNotation a)
            => int.Parse(a.ToString());
        public static explicit operator uint(ArNumberScientificNotation a)
            => uint.Parse(a.ToString());
        public static explicit operator long(ArNumberScientificNotation a)
            => long.Parse(a.ToString());
        public static explicit operator ulong(ArNumberScientificNotation a)
            => ulong.Parse(a.ToString());
        public static explicit operator float(ArNumberScientificNotation a)
            => float.Parse(a.ToString());
        public static explicit operator double(ArNumberScientificNotation a)
            => double.Parse(a.ToString());
        public static explicit operator decimal(ArNumberScientificNotation a)
            => decimal.Parse(a.ToString());


        public static bool operator >(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.CompareTo(b) == 1;
        public static bool operator >=(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.CompareTo(b) != -1;
        public static bool operator <(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.CompareTo(b) == -1;
        public static bool operator <=(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.CompareTo(b) != 1;
        public static bool operator ==(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => a.Equals(b);
        public static bool operator !=(ArNumberScientificNotation a, ArNumberScientificNotation b)
            => !a.Equals(b);

        //public static ArNumberScientificNotation operator +(ArNumberScientificNotation a, ArNumberScientificNotation b)
        //    => AddMinus(a, b);
        //public static ArNumberScientificNotation operator -(ArNumberScientificNotation a, ArNumberScientificNotation b)
        //    => AddMinus(a, b, false);
        //public static ArNumberScientificNotation operator ++(ArNumberScientificNotation a)
        //    => AddMinus(a, 1);
        //public static ArNumberScientificNotation operator --(ArNumberScientificNotation a)
        //    => AddMinus(a, 1, false);
    }
}
