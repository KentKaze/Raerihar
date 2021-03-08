using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //ArNumber Struct
    //_Data
    //Last 1    Sign
    //N         Exponent
    //          7 bit Exponent
    //          15 bit Exponent
    //          31 bit Exponent
    //          63 bit Exponent

    //_Numbers
    //N         連續的二進位來表示十進位
    //N1-30     0 -> 999999999
    //N31-60    0 -> 999999999
    //N61-90    0 -> 999999999
    //N91-120   0 -> 999999999

    //New ArNumber :D

    //Index Digits   Bits   Bytes
    //0     1~9        30       4
    //1     10~18      60       8
    //2     19~27      90      12
    //3     28~36     120      15 
    //4     37~45     150      19
    //5     46~54     180      23
    //6     55~63     210      27
    //7     64~72     240      30
    //8     73~81     270      34
    //9     82~90     300      38
    //10    91~99     330      42
    //11  100~102     360      45
    //12  103~105     390      49
    //13  106~108     420      53
    //14  109~111     450      57
    //15  112~114     480      60

    //3 => 15, 7 => 30, 11 => 45, 14 => 60

    //index = (digits - 1) / 9
    //bytes = index * 15 + 18 / 4
    //bytes = ((digits - 1) / 9) * 15 + 18 / 4
    //index = bytes * 4 - 6 / 15
    public sealed class ArNumber : IEquatable<ArNumber>, IComparable, IComparable<ArNumber>, IFormattable, ICloneable, IConvertible
    {
        public const long ExponentMaxValue = 1152921504606846976;
        public const long ExponentMinValue = -1152921504606846977;
        public const byte MaximumDisplayedDigitsCount = 20;
        private const int MaximumBytesCount = 4166668;

        private byte[] _Data;
        private byte[] _Numbers;
        private int DigitSetLength => (_Numbers.Length * 4 - 6) / 15 + 1;
        public bool Negative
        {
            get => _Data[_Data.Length - 1] >> 7 == 1;
            set
            {   
                //if (Digits == "0")
                //    return;
                _Data[_Data.Length - 1] = value ? (byte)(_Data[_Data.Length - 1] | 128) : (byte)(_Data[_Data.Length - 1] & 127);
            }
        }
        public long Exponent
        {
            get
            {
                long result;
                byte lastByte = _Data[_Data.Length - 1];
                _Data[_Data.Length - 1] &= 127;
                _Data[_Data.Length - 1] |= (byte)(_Data[_Data.Length - 1] << 1 & 128);

                switch (_Data.Length)
                {
                    case 1:
                        result = (sbyte)_Data[0];
                        break;
                    case 2:
                        if (BitConverter.IsLittleEndian)
                            result = BitConverter.ToInt16(_Data, 0);
                        else
                            result = BitConverter.ToInt16(new byte[] { _Data[1], _Data[0] }, 0);
                        break;
                    case 4:
                        if (BitConverter.IsLittleEndian)
                            result = BitConverter.ToInt32(_Data, 0);
                        else
                            result = BitConverter.ToInt32(new byte[] { _Data[3], _Data[2], _Data[1], _Data[0] }, 0);
                        break;
                    case 8:
                        if (BitConverter.IsLittleEndian)
                            result = BitConverter.ToInt64(_Data, 0);
                        else
                            result = BitConverter.ToInt64(new byte[] { _Data[7], _Data[6], _Data[5], _Data[4], _Data[3], _Data[2], _Data[1], _Data[0] }, 0);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                _Data[_Data.Length - 1] = lastByte;
                return result;
            }
            set
            {
                byte[] result;
                if (value > -65 && value < 64)
                    result = new byte[] { (byte)value };
                else if (value > -16385 && value < 16384)
                    result = BitConverter.GetBytes((short)value);
                else if (value > -1073741825 && value < 1073741824)
                    result = BitConverter.GetBytes((int)value);
                else if (value > -4611686018427387905 && value < 4611686018427387904)
                    result = BitConverter.GetBytes(value);
                else
                    throw new ArgumentOutOfRangeException(nameof(Exponent));
                if (!BitConverter.IsLittleEndian && result.Length != 1)
                    result = Reverse(result);
                result[result.Length - 1] &= 127;
                result[result.Length - 1] |= (byte)(_Data[_Data.Length - 1] & 128);
                _Data = result;
            }
        }

        //public byte[] Digits
        //{
        //    get { return null; }
        //    set 
        //    {
        //        if (value == null || value.Length == 0)
        //            throw new ArgumentNullException(nameof(Digits));
        //        _Numbers = new byte[((value.Length - 1) / 9 * 15 + 18) / 4];
        //        int v;
        //        for(int i = 0; i < (value.Length - 1) / 9 + 1; i++)
        //        {
        //            v= 
        //            v = $"{value[i * 9 + 0]} +

        //            if (i * 9 + 8 < value.Length)
        //                v =  int.Parse(value.Substring(i * 9, 9));
        //            else
        //                v = int.Parse(value.Substring(i * 9).PadRight(9, '0'));
        //            SetDigits(i, v);
        //        }
        //    }
        //}

        //public string Digits
        //{
        //    get
        //    {
        //        StringBuilder result = new StringBuilder();
        //        for (int i = 0; i < DigitSetLength; i++)
        //            result.AppendFormat("{0:D9}", GetDigits(i));
        //        while (result.Length > 1 && result[0] == '0')
        //            result.Remove(0, 1);
        //        while (result.Length > 1 && result[result.Length - 1] == '0')
        //            result.Remove(result.Length - 1, 1);
        //        return result.ToString();
        //    }
        //    private set
        //    {
        //        //No Check
        //        _Digits = new byte[(value.Length - 1) / 3 * 5 / 4 + 2];
        //        ushort v;
        //        for (int i = 0; i < (value.Length - 1) / 3 + 1; i++)
        //        {
        //            if (i * 3 + 2 < value.Length)
        //                v = ushort.Parse(value.Substring(i * 3, 3));
        //            else
        //                v = ushort.Parse(value.Substring(i * 3).PadRight(3, '0'));
        //            SetDigits(i, v);
        //        }
        //    }
        //}

        public ArNumber(ArNumber a)
        {
            _Data = new byte[a._Data.Length];
            _Numbers = new byte[a._Numbers.Length];
            for (int i = 0; i < _Data.Length; i++)
                _Data[i] = a._Data[i];
            for (int i = 0; i < _Data.Length; i++)
                _Numbers[i] = a._Numbers[i];
        }

        public ArNumber()
        {
            _Data = new byte[1];
            _Numbers = new byte[4];
        }
        public ArNumber(sbyte value)
            : this()
        {
            if (value < 0)
                _Data[0] |= 128;
            if (Math.Abs((int)value) >= 100)
                _Data[0] |= 2;
            else if (Math.Abs((int)value) >= 10)
                _Data[0] |= 1;
            SetDigits(0, (ushort)Math.Abs((int)value));
        }
        public ArNumber(byte value)
            : this()
        {
            if (value >= 100)
                _Data[0] |= 2;
            else if (value >= 10)
                _Data[0] |= 1;
            SetDigits(0, value);
        }
        public ArNumber(ushort value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(short value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(uint value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(int value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(ulong value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(long value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(decimal value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(float value)
            : this()
            => Parse(value.ToString(), this);
        public ArNumber(double value)
            : this()
            => Parse(value.ToString(), this);

        private string GetDigitsToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < DigitSetLength; i++)
                result.AppendFormat("{0:D9}", GetDigits(i));
            while (result.Length > 1 && result[0] == '0')
                result.Remove(0, 1);
            while (result.Length > 1 && result[result.Length - 1] == '0')
                result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        private void SetDigitsByString(string s)
        {   
            _Numbers = new byte[((s.Length - 1) / 9 * 15 + 18) / 4];
            int v;
            for (int i = 0; i < (s.Length - 1) / 9 + 1; i++)
            {
                if (i * 9 + 8 < s.Length)
                    v = int.Parse(s.Substring(i * 9, 9));
                else
                    v = int.Parse(s.Substring(i * 9).PadRight(9, '0'));
                SetDigits(i, v);
            }
        }

        private void SetDigits(int index, int value)
        {
            byte[] buffer;
            if (BitConverter.IsLittleEndian)
                buffer = BitConverter.GetBytes(value);
            else
                buffer = Reverse(BitConverter.GetBytes(value));
            int j = index * 15 / 4;
            switch (index % 4)
            {
                case 0:
                    _Numbers[j] = buffer[0]; // 8
                    _Numbers[j + 1] = buffer[1]; //8
                    _Numbers[j + 2] = buffer[2]; //8
                    _Numbers[j + 3] = (byte)(buffer[3] << 2); //6
                    break;
                case 1:
                    _Numbers[j] |= (byte)(buffer[0] >> 6 & 3); //2
                    _Numbers[j + 1] = (byte)(buffer[0] << 2); //6
                    _Numbers[j + 1] |= (byte)(buffer[1] >> 6 & 3); //2
                    _Numbers[j + 2] = (byte)(buffer[1] << 2); //6
                    _Numbers[j + 2] |= (byte)(buffer[2] >> 6 & 3); //2
                    _Numbers[j + 3] = (byte)(buffer[2] << 2); //6
                    _Numbers[j + 3] |= (byte)(buffer[3] >> 4 & 3); //2
                    _Numbers[j + 4] = (byte)(buffer[3] << 4); //4
                    break;
                case 2:
                    _Numbers[j] |= (byte)(buffer[0] >> 4 & 15); //4
                    _Numbers[j + 1] = (byte)(buffer[0] << 4); //4
                    _Numbers[j + 1] |= (byte)(buffer[1] >> 4 & 15); //4
                    _Numbers[j + 2] = (byte)(buffer[1] << 4); //4
                    _Numbers[j + 2] |= (byte)(buffer[2] >> 4 & 15); //4
                    _Numbers[j + 3] = (byte)(buffer[2] << 4); //4
                    _Numbers[j + 3] |= (byte)(buffer[3] >> 2 & 15); //4
                    _Numbers[j + 4] = (byte)(buffer[3] << 6); //2
                    break;
                case 3:
                    _Numbers[j] |= (byte)(buffer[0] >> 2 & 63); //6
                    _Numbers[j + 1] = (byte)(buffer[0] << 6); //2
                    _Numbers[j + 1] |= (byte)(buffer[1] >> 2 & 63); //6
                    _Numbers[j + 2] = (byte)(buffer[1] << 6); //2
                    _Numbers[j + 2] |= (byte)(buffer[2] >> 2 & 63); //6
                    _Numbers[j + 3] = (byte)(buffer[2] << 6); //2
                    _Numbers[j + 3] |= (byte)(buffer[3]); //6
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        private int GetDigits(int index)
        {
            int j = index * 15 / 4;
            switch (index % 4)
            {
                case 0:
                    return BitConverter.ToInt32(new byte[] {
                    _Numbers[j], _Numbers[j + 1], _Numbers[j + 2],
                    (byte)(_Numbers[j + 3] >> 2 & 63) }, 0);
                case 1:
                    return BitConverter.ToInt32(new byte[] {
                    (byte)((byte)(_Numbers[j] << 6) | (_Numbers[j + 1] >> 2 & 63)),
                    (byte)((byte)(_Numbers[j + 1] << 6) | (_Numbers[j + 2] >> 2 & 63)),
                    (byte)((byte)(_Numbers[j + 2] << 6) | (_Numbers[j + 3] >> 2 & 63)),
                    (byte)((byte)(_Numbers[j + 3] << 4 & 48) | (_Numbers[j + 4] >> 4 & 15)) }, 0);
                case 2:
                    return BitConverter.ToInt32(new byte[] {
                    (byte)(_Numbers[j] << 4 | _Numbers[j + 1] >> 4 & 15),
                    (byte)(_Numbers[j + 1] << 4 | _Numbers[j + 2] >> 4 & 15),
                    (byte)(_Numbers[j + 2] << 4 | _Numbers[j + 3] >> 4 & 15),
                    (byte)(_Numbers[j + 3] << 2 & 48 | _Numbers[j + 4] >> 6 & 3) }, 0);
                case 3:
                    return BitConverter.ToInt32(new byte[] {
                    (byte)(_Numbers[j] << 2 | _Numbers[j + 1] >> 6 & 3),
                    (byte)(_Numbers[j + 1] << 2 | _Numbers[j + 2] >> 6 & 3),
                    (byte)(_Numbers[j + 2] << 2 | _Numbers[j + 3] >> 6 & 3),
                    (byte)(_Numbers[j + 3] & 48) }, 0);
                default:
                    throw new NotImplementedException();
            }
        }
        private static void Parse(string s, ArNumber a, NumberStyles style = NumberStyles.None, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            bool isNegative = false;
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
                isNegative = true;
                numberString = numberString.Remove(0, 1);
            }
            while (numberString.Length > 1 && numberString[0] == '0')
                numberString = numberString.Remove(0, 1);

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
                while (numberString[numberString.Length - 1] == '0')
                    numberString = numberString.Remove(numberString.Length - 1, 1);
                numberString = numberString.Remove(pointIndex, 1);
                if (numberString.Length == 0)
                {
                    a = new ArNumber(); // Set To 0 To DO
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

            while (numberString.Length > 1 && numberString[numberString.Length - 1] == '0')
                numberString = numberString.Remove(numberString.Length - 1, 1);
            a.Exponent = e;
            a.SetDigitsByString(numberString);
            a.Negative = isNegative;
        }

        private static bool AdaptExponentForm(ArNumber a)
        {
            long e = a.Exponent;
            int l = a.GetDigitsToString().Length; // Todo
            if ((e > 0 && e + 1 <= MaximumDisplayedDigitsCount) ||
                (e < 0 && e - l + 1 >= MaximumDisplayedDigitsCount * -1))
                return false;
            return true;
        }
        public static bool IsInteger(ArNumber a)
            => a.Exponent - a.GetDigitsToString().Length + 1 >= 0; // Todo
        private string ToString(int digits, char format, IFormatProvider provider)
        {
            string numbers = GetDigitsToString();
            if (digits < 0)
                throw new ArgumentOutOfRangeException(nameof(digits));
            else if (digits == 0 || digits > numbers.Length)
                digits = numbers.Length;
            // TO DO
            long e = Exponent; // if e > int overflow            
            if (format == 'G')
                if (AdaptExponentForm(this))
                    format = 'E';
                else if (IsInteger(this))
                    format = 'D';
                else
                    format = 'C';

            StringBuilder result = new StringBuilder();
            result.Append(numbers);
            if (result.Length != 1 && format == 'E')
                result.Insert(1, '.');
            if (e != 0)
            {
                if (format == 'E')
                    result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
                else if(format == 'D')
                {
                    if (e > digits - 1)
                        result.Append(new string('0', (int)e - digits + 1));
                }   
                else if (format == 'C')
                {
                    if (e > digits - 1)
                        result.Append(new string('0', (int)e - digits + 1));
                    else if (e > 0)
                        result.Insert((int)e + 1, '.');
                    else
                        result.Insert(0, $"0.{new string('0', Math.Abs((int)e + 1))}");
                }   
            }
            //-744999665.2453299: -7449996652453299
            //34594088007308696: 34594088007308696000000000000000
            //2170.6907744747728: 2170690774474772800
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
                    return ToString(length, format[0], provider);
                case 'D':
                    return ToString(length, format[0], provider);
                //case 'F':
                //case 'N':
                //case 'P':
                //case 'R':
                //case 'X':
                case 'E':
                case 'G':
                    return ToString(length, format[0], provider);
                default:
                    throw new FormatException(string.Format("The '{0}' format string is not supported.", format));
            }
        }

        private byte[] Reverse(byte[] array)
        {
            for (int i = 0; i < array.Length / 2; i++)
            {
                byte buffer = array[i];
                array[i] = array[array.Length - i - 1];
                array[array.Length - i - 1] = buffer;
            }
            return array;
        }

        public static ArNumber Parse(string s)
            => Parse(s, NumberStyles.None, null);

        public static ArNumber Parse(string s, NumberStyles style)
            => Parse(s, style, null);

        public static ArNumber Parse(string s, IFormatProvider provider)
            => Parse(s, NumberStyles.None, provider);

        public static ArNumber Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArNumber result = new ArNumber();
            Parse(s, result, style, provider);
            return result;
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ArNumber result)
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

        public bool Equals(ArNumber other)
        {
            for (int i = 0; i < _Data.Length; i++)
                if (_Data[i] != other._Data[i])
                    return false;
            for (int i = 0; i < _Numbers.Length; i++)
                if (_Numbers[i] != other._Numbers[i])
                    return false;
            return true;
        }

        public int CompareTo(ArNumber other)
        {
            if (Equals(other))
                return 0;
            else if (!Negative && other.Negative)
                return 1;
            else if (Negative && !other.Negative)
                return -1;
            int result = Negative && Negative ? -1 : 1;
            if (Exponent > other.Exponent)
                return result;
            else if (Exponent < other.Exponent)
                return result * -1;
            for (int i = 0; i < DigitSetLength; i++)
                if (GetDigits(i) > other.GetDigits(i))
                    return result;
            return other.DigitSetLength > DigitSetLength ? result * -1 : 0;
        }
        public int CompareTo(object obj)
            => CompareTo((ArNumber)obj);
        public object Clone()
            => new ArNumber(this);
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

        public static implicit operator ArNumber(sbyte a)
            => new ArNumber(a);
        public static implicit operator ArNumber(byte a)
            => new ArNumber(a);
        public static implicit operator ArNumber(short a)
            => new ArNumber(a);
        public static implicit operator ArNumber(ushort a)
            => new ArNumber(a);
        public static implicit operator ArNumber(char a)
            => new ArNumber((uint)a);
        public static implicit operator ArNumber(int a)
            => new ArNumber(a);
        public static implicit operator ArNumber(uint a)
            => new ArNumber(a);
        public static implicit operator ArNumber(long a)
            => new ArNumber(a);
        public static implicit operator ArNumber(ulong a)
            => new ArNumber(a);
        public static implicit operator ArNumber(decimal a)
            => new ArNumber(a);
        public static implicit operator ArNumber(float a)
            => new ArNumber(a);
        public static implicit operator ArNumber(double a)
            => new ArNumber(a);
        public static explicit operator sbyte(ArNumber a)
            => sbyte.Parse(a.ToString("D"));
        public static explicit operator byte(ArNumber a)
            => byte.Parse(a.ToString("D"));
        public static explicit operator short(ArNumber a)
            => short.Parse(a.ToString("D"));
        public static explicit operator ushort(ArNumber a)
            => ushort.Parse(a.ToString("D"));
        public static explicit operator char(ArNumber a)
            => char.Parse(a.ToString("D"));
        public static explicit operator int(ArNumber a)
            => int.Parse(a.ToString("D"));
        public static explicit operator uint(ArNumber a)
            => uint.Parse(a.ToString("D"));
        public static explicit operator long(ArNumber a)
            => long.Parse(a.ToString("D"));
        public static explicit operator ulong(ArNumber a)
            => ulong.Parse(a.ToString("D"));
        public static explicit operator float(ArNumber a)
            => float.Parse(a.ToString());
        public static explicit operator double(ArNumber a)
            => double.Parse(a.ToString());
        public static explicit operator decimal(ArNumber a)
            => decimal.Parse(a.ToString());
        public override int GetHashCode()
        {
            int result = _Data[0].GetHashCode();
            for (int i = 1; i < _Data.Length; i++)
                result ^= _Data[i].GetHashCode();
            for (int i = 0; i < _Numbers.Length; i++)
                result ^= _Numbers[i].GetHashCode();
            return result;
        }
        public override bool Equals(object obj)
        {
            ArNumber ar = obj as ArNumber;
            if (ar == null)
                return false;
            return Equals(ar);
        }
        public static bool operator >(ArNumber a, ArNumber b)
            => a.CompareTo(b) == 1;
        public static bool operator >=(ArNumber a, ArNumber b)
            => a.CompareTo(b) != -1;
        public static bool operator <(ArNumber a, ArNumber b)
            => a.CompareTo(b) == -1;
        public static bool operator <=(ArNumber a, ArNumber b)
            => a.CompareTo(b) != 1;
        public static bool operator ==(ArNumber a, ArNumber b)
            => a.Equals(b);
        public static bool operator !=(ArNumber a, ArNumber b)
            => !a.Equals(b);
    }
}
