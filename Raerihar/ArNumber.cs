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
    //N1-10     999 -> 000
    //N11-20    999 -> 000
    //N21-30    999 -> 000
    //N31-40    999 -> 000

    //Special Number
    // 1000 NaN
    // 1001 Positive Infinity
    // 1002 Negative Infinity 

    //128, 64, 32, 16, 8, 4, 2, 1
    public struct ArNumber //: IComparable, IComparable<ArNumber>, IConvertible, IEquatable<ArNumber>, IFormattable
    {   
        private byte[] _Data;
        private byte[] _Digits;
        public bool IsNegative
        {
            get => _Data[0] >> 7 == 1;
            set => _Data[0] = value ? (byte)(_Data[0] | 128) : (byte)(_Data[0] & 127);
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
                        if(BitConverter.IsLittleEndian)
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
                return Convert.ToUInt16(_Digits[0]).ToString();
            }
            private set
            {
                //No Check

                if(value.Length % 12 != 0)
                    _Digits = new byte[value.Length / 3 + 1];
                else
                    _Digits = new byte[value.Length / 3];

                ushort v;
                byte[] buffer;
                int j = 0;
                for(int i = 0; i < value.Length; i += 3)
                {
                    if (i + 3 < value.Length)
                        v = ushort.Parse(value.Substring(i, 3));
                    else
                        v = ushort.Parse(value.Substring(i));
                    if (BitConverter.IsLittleEndian)
                        buffer = BitConverter.GetBytes(v);
                    else
                        buffer = Reverse(BitConverter.GetBytes(v));

                    if(i % 4 == 0)
                    {   
                        _Digits[j] = buffer[1]; // 8
                        _Digits[j + 1] = (byte)(buffer[0] << 6); //2
                        j++;
                    }
                    else if(i % 4 == 3)
                    {
                        _Digits[j] |= (byte)(buffer[1] >> 2 & 63); //6
                        _Digits[j + 1] = (byte)(buffer[1] << 6); //2
                        _Digits[j + 1] |= (byte)(buffer[0] << 4); //2
                        j++;
                    }
                    else if(i % 4 == 1)
                    {
                        _Digits[j] |= (byte)(buffer[1] >> 4 & 15); //4
                        _Digits[j + 1] = (byte)(buffer[1] << 4); //4
                        _Digits[j + 1] |= (byte)(buffer[0] << 2); //2
                        j++;
                    }
                    else if(i % 4 == 4)
                    {
                        _Digits[j] |= (byte)(buffer[1] >> 6 & 3); //2
                        _Digits[j + 1] = (byte)(buffer[1] << 2); //6
                        _Digits[j + 1] |= (byte)(buffer[0]); //2
                        j+=2;
                    }
                }
            }
        }


        public int Length { get => _Digits.Length; }

        //[CLSCompliant(false)]
        //public ArNumber(ulong value);
        //[CLSCompliant(false)]
        //public ArNumber(uint value);
        //public ArNumber(float value);
        //public ArNumber(long value);
        //public ArNumber(int[] bits);
        //public ArNumber(int value)
        //{
        //    _Data = new byte[1];// To Do
        //}
        //public ArNumber(double value);
        //public ArNumber(int lo, int mid, int hi, bool isNegative, byte scale);
        private byte[] Reverse(byte[] array)
        {          
            for(int i = 0; i < array.Length / 2; i++)
            {
                byte buffer = array[i];
                array[i] = array[array.Length - i - 1];
                array[array.Length - i - 1] = buffer;
            }
            return array;
        }

        public ArNumber(sbyte value)
        {
            _Data = new byte[1];
            _Digits = new byte[1];
            if (value < 0)
                _Data[0] |= 128;
            if (Math.Abs(value) >= 100)
                _Data[0] |= 2;
            else if (Math.Abs(value) >= 10)
                _Data[0] |= 1;
            _Digits[0] = (byte)Math.Abs(value);
        }

        private void ParseSelf(string s, NumberStyles style, IFormatProvider provider)
        {   
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            IsNegative = false;
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
                IsNegative = true;
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
                    Digits = "0";
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
            Digits = numberString;
            Exponent = e;
        }

        public static ArNumber Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            ArNumber result = new ArNumber();
            result.ParseSelf(s, style, provider);
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
                result = new ArNumber();
                return false;
            }
        }

        private string ToString(int digits, char format, IFormatProvider provider)
        {
            bool negative = _Data[0] >> 7 == 1;
            long e = Exponent;
            if (format == 'G')
                format = 'E';
            StringBuilder result = new StringBuilder();
            result.Append(Digits);
            if (result.Length != 1 && format == 'E')
                result.Insert(1, '.');
            if (e != 0 && format == 'E')
                result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
            if (negative)
                result.Insert(0, '-');
            return result.ToString();
        }

        public override string ToString()
            => ToString(null, null);
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

        public static implicit operator ArNumber(sbyte a)
            => new ArNumber(a);

        //public static implicit operator ArNumber(int a)
        //    => new ArNumber(a);        
    }
}
