using System;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //ArNumber 架構

    //1         0 => Positive
    //          1 => Negative
    //2         0 => 5  bit  Exponent(Log 10) 1
    //          1 => 13 bit  Exponent(Log 10) 2
    //          2 => 29 bit  Exponent(Log 10) 4
    //          3 => 61 bit  Exponent(Log 10) 8
    //N         Exponent (First 1 is sign)
    //N         Digits 連續的二進位來表示十進位
    //N1-10     000 -> 999
    //N11-20    000 -> 999
    //N21-30    000 -> 999
    //N31-40    000 -> 999

    //Index Digits      Bytes
    //0     1~3         2Bytes
    //1     4~6         3Bytes
    //2     7~9         4Bytes
    //3     10~12       5Bytes 
    //4     13~15       7Bytes
    //5     16~18       8Bytes
    //6     19~21       9Bytes
    //7     22~24       10Bytes
    //8     25~27       12Bytes
    //9     28~30       13Bytes
    //10    31~33       14Bytes
    //11    34~36       15Bytes
    //12    37~39       17Bytes
    //13    40~42       18Bytes
    //14    43~45       19Bytes
    //15    46~48       20Bytes
    //16    49~51       22Bytes

    //bytes = index * 5 / 4 + 2
    //index = (bytes - 1) * 4 / 5
    //index = (digits - 1) / 3
    //bytes = ((digits - 1) / 3) * 5 / 4 + 1

    //Special Number
    // 1000 NaN
    // 1001 Positive Infinity
    // 1002 Negative Infinity 

    //128, 64, 32, 16, 8, 4, 2, 1
    //[CLSCompliant(false)]
    public class ArNumber : IEquatable<ArNumber>, IComparable, IComparable<ArNumber>, IFormattable, ICloneable, IConvertible
    {
        private byte[] _Data;
        private byte[] _Digits;

        public const long ExponentMaxValue = 1152921504606846976;
        public const long ExponentMinValue = -1152921504606846977;
        public static readonly ArNumber Empty = 0;
        public const byte MaximumDisplayedDigitsCount = 20;

        public bool Negative
        {            
            get =>_Data[0] >> 7 == 1;
            set
            {
                if (Digits == "0")
                    return;
                _Data[0] = value ? (byte)(_Data[0] | 128) : (byte)(_Data[0] & 127);
            }
        }
        public long Exponent
        {
            get
            {   
                switch ((byte)(_Data[0] & 96) >> 5)
                {
                    case 0:
                        return (sbyte)((sbyte)(_Data[0] << 3) >> 3);
                    case 1:
                        if (BitConverter.IsLittleEndian)
                            return BitConverter.ToInt16(new byte[] { _Data[1], (byte)((sbyte)(_Data[0] << 3) >> 3) }, 0);
                        else
                            return BitConverter.ToInt16(new byte[] { (byte)((sbyte)(_Data[0] << 3) >> 3), _Data[1] }, 0);
                    case 2:
                        if (BitConverter.IsLittleEndian)
                            return BitConverter.ToInt32(new byte[] { _Data[3], _Data[2], _Data[1], (byte)((sbyte)(_Data[0] << 3) >> 3) }, 0);
                        else
                            return BitConverter.ToInt32(new byte[] { (byte)((sbyte)(_Data[0] << 3) >> 3), _Data[1], _Data[2], _Data[3] }, 0);
                    case 3:
                        if (BitConverter.IsLittleEndian)
                            return BitConverter.ToInt64(new byte[] { _Data[7], _Data[6], _Data[5], _Data[4], _Data[3], _Data[2], _Data[1], (byte)((sbyte)(_Data[0] << 3) >> 3) }, 0);
                        else
                            return BitConverter.ToInt64(new byte[] { (byte)((sbyte)(_Data[0] << 3) >> 3), _Data[1], _Data[2], _Data[3], _Data[4], _Data[5], _Data[6], _Data[7] }, 0);
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {   
                byte[] result;
                if (value < 16 && value > -17)
                {
                    result = new byte[] { (byte)value };
                    result[0] &= 31;
                    result[0] |= (byte)(_Data[0] & 128);
                }
                else if (value < 4096 && value > -4097)
                {
                    if (BitConverter.IsLittleEndian)
                        result = Reverse(BitConverter.GetBytes((short)value));
                    else
                        result = BitConverter.GetBytes((short)value);
                    result[0] &= 31;
                    result[0] |= (byte)(_Data[0] & 128 | 32);
                }
                else if (value < 268435456 && value > -268435457)
                {
                    if (BitConverter.IsLittleEndian)
                        result = Reverse(BitConverter.GetBytes((int)value));
                    else
                        result = BitConverter.GetBytes((int)value);
                    result[0] &= 31;
                    result[0] |= (byte)(_Data[0] & 128 | 64);
                }
                else if (value < 1152921504606846976 && value > -1152921504606846977)
                {
                    if (BitConverter.IsLittleEndian)
                        result = Reverse(BitConverter.GetBytes(value));
                    else
                        result = BitConverter.GetBytes(value);
                    result[0] &= 31;
                    result[0] |= (byte)(_Data[0] & 128 | 96);
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(Exponent));
                _Data = result;
            }
        }

        public string Digits
        {
            get
            {   
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < (_Digits.Length - 1) * 4 / 5 + 1; i++)
                    result.AppendFormat("{0:D3}", GetDigits(i));
                while (result.Length > 1 && result[0] == '0')
                    result.Remove(0, 1);
                while (result.Length > 1 && result[result.Length - 1] == '0')
                    result.Remove(result.Length - 1, 1);
                return result.ToString();
            }
            private set
            {
                //No Check
                _Digits = new byte[(value.Length - 1) / 3 * 5 / 4 + 2];
                ushort v;
                for (int i = 0; i < (value.Length - 1) / 3 + 1; i++)
                {
                    if (i * 3 + 2 < value.Length)
                        v = ushort.Parse(value.Substring(i * 3, 3));
                    else
                        v = ushort.Parse(value.Substring(i * 3).PadRight(3, '0'));
                    SetDigits(i, v);
                }
            }
        }

        private void SetDigits(int index, ushort value)
        {
            byte[] buffer;
            if (BitConverter.IsLittleEndian)
                buffer = BitConverter.GetBytes(value);
            else
                buffer = Reverse(BitConverter.GetBytes(value));
            int j = index * 5 / 4;
            switch (index % 4)
            {
                case 0:
                    _Digits[j] = buffer[0]; // 8
                    _Digits[j + 1] = (byte)(buffer[1] << 6); //2
                    break;
                case 1:
                    _Digits[j] |= (byte)(buffer[0] >> 2 & 63); //6
                    _Digits[j + 1] = (byte)(buffer[0] << 6); //2
                    _Digits[j + 1] |= (byte)(buffer[1] << 4); //2
                    break;
                case 2:
                    _Digits[j] |= (byte)(buffer[0] >> 4 & 15); //4
                    _Digits[j + 1] = (byte)(buffer[0] << 4); //4
                    _Digits[j + 1] |= (byte)(buffer[1] << 2); //2
                    break;
                case 3:
                    _Digits[j] |= (byte)(buffer[0] >> 6 & 3); //2
                    _Digits[j + 1] = (byte)(buffer[0] << 2); //6
                    _Digits[j + 1] |= (byte)(buffer[1]); //2
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private ushort GetDigits(int index)
        {
            int j = index * 5 / 4;
            switch (index % 4)
            {
                case 0:
                    return BitConverter.ToUInt16(new byte[] {
                        _Digits[j],
                        (byte)(_Digits[j + 1] >> 6 & 3) }, 0);
                case 1:
                    return BitConverter.ToUInt16(new byte[] {
                        (byte)(_Digits[j] << 2 | _Digits[j + 1] >> 6 & 3),
                        (byte)(_Digits[j + 1] >> 4 & 3) }, 0);
                case 2:
                    return BitConverter.ToUInt16(new byte[] {
                        (byte)(_Digits[j] << 4 | _Digits[j + 1] >> 4 & 15),
                        (byte)(_Digits[j + 1] >> 2 & 3) }, 0);
                case 3:
                    return BitConverter.ToUInt16(new byte[] {
                        (byte)(_Digits[j] << 6 | _Digits[j + 1] >> 2 & 63),
                        (byte)(_Digits[j + 1] & 3) }, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        public ArNumber()
        {
            _Data = new byte[1];
            _Digits = new byte[2];
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
        public ArNumber(ArNumber value)
        {
            _Data = new byte[value._Data.Length];
            _Digits = new byte[value._Digits.Length];
            for (int i = 0; i < _Data.Length; i++)
                _Data[i] = value._Data[i];
            for (int i = 0; i < _Digits.Length; i++)
                _Digits[i] = value._Digits[i];
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
                    a.Negative = false;
                    a.Digits = "0";
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
            a.Digits = numberString;
            a.Negative = isNegative;
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
                result = Empty;
                return false;
            }
        }

        private string ToString(int digits, char format, IFormatProvider provider)
        {
            // TO DO
            long e = Exponent;
            if (format == 'G')
                if (AdaptExponentForm(this))
                    format = 'E';
                else if (IsInteger(this))
                    format = 'D';
                else
                    format = 'C';

            StringBuilder result = new StringBuilder();
            result.Append(Digits);
            if (result.Length != 1 && format == 'E')
                result.Insert(1, '.');
            if (e != 0 && format == 'E')
                result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
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
                throw new FormatException();
            switch (format[0])
            {
                // TO DO
                case 'C':
                    return ToString(length, format[0], provider);
                //case 'D':
                //    break;
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

        public bool Equals(ArNumber other)
        {
            for (int i = 0; i < _Data.Length; i++)
                if (_Data[i] != other._Data[i])
                    return false;
            for (int i = 0; i < _Digits.Length; i++)
                if (_Digits[i] != other._Digits[i])
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
            return Digits.CompareTo(other.Digits) * result;
        }

        private static bool AdaptExponentForm(ArNumber a)
        {
            long e = a.Exponent;
            int l = a.Digits.Length;
            if ((e > 0 && e - l + 1 <= MaximumDisplayedDigitsCount) ||
                (e < 0 && e - l + 1 >= MaximumDisplayedDigitsCount * -1))
                return false;
            return true;
        }
        public static bool IsInteger(ArNumber a)
            => a.Exponent - a.Digits.Length + 1 >= 0;
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
            for (int i = 0; i < _Digits.Length; i++)
                result ^= _Digits[i].GetHashCode();
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
