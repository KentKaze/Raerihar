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
                switch ((byte)(_Data[0] << 1) >> 7)
                {
                    case 0:
                        return (byte)(_Data[0] << 3) >> 3;
                    case 1:
                        return BitConverter.ToInt64(new byte[] { (byte)((byte)(_Data[0] << 3) >> 3), _Data[1] }, 0);
                    case 2:                        
                        return BitConverter.ToInt64(new byte[] { (byte)((byte)(_Data[0] << 3) >> 3), _Data[1], _Data[2], _Data[3] }, 0);
                    case 3:
                        return BitConverter.ToInt64(new byte[] { (byte)((byte)(_Data[0] << 3) >> 3), _Data[1], _Data[2], _Data[3], _Data[4], _Data[5], _Data[6], _Data[7] }, 0);
                    default:
                        throw new NotImplementedException();
                }
            }
            set {
                byte[] result = BitConverter.GetBytes(value);
                result[0] &= 224;
                if (value < 16 && value > -17)
                    result[0] |= (byte)(_Data[0] & 128);
                else if (value < 4096 && value > -4097)
                    result[0] |= (byte)(_Data[0] & 128 | 32);
                else if (value < 536870912 && value > -536870913)
                    result[0] |= (byte)(_Data[0] & 128 | 64);
                else
                    result[0] |= (byte)(_Data[0] & 128 | 96);
                _Data = result;
            }
        }

        public string Digits
        {
            get => "";
            set { }
        }


        public int Legnth { get => _Data.Length; }

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
            //if (string.IsNullOrEmpty(s))
            //    throw new ArgumentNullException(nameof(s));
            //bool negative = false;
            //long e;

            //string numberString = s;
            //int eIndex = -2, pointIndex = -1;
            //if (numberString[0] == '+')
            //    numberString = numberString.Remove(0, 1);
            //else if (numberString[0] == '-')
            //{
            //    negative = true;
            //    numberString = numberString.Remove(0, 1);
            //}
            //while (numberString.Length > 1 && numberString[0] == '0')
            //    numberString = numberString.Remove(0, 1);

            //for (int i = 0; i < numberString.Length; i++)
            //{
            //    if (i == eIndex + 1)
            //    {
            //        if (numberString[i] != '+' && numberString[i] != '-')
            //            throw new ArgumentException($"{nameof(s)}:{s}");
            //    }
            //    else if (numberString[i] == '.')
            //        if (pointIndex == -1)
            //            pointIndex = i;
            //        else
            //            throw new ArgumentException($"{nameof(s)}:{s}");
            //    else if (numberString[i] == 'E' || numberString[i] == 'e')
            //        if (i == 0)
            //            throw new ArgumentException($"{nameof(s)}:{s}");
            //        else if (eIndex == -2)
            //            eIndex = i;
            //        else
            //            throw new ArgumentException($"{nameof(s)}:{s}");
            //    else if (!char.IsDigit(numberString[i]))
            //        throw new ArgumentException($"{nameof(s)}:{s}");
            //}

            //if (eIndex != -2)
            //{
            //    Exponent = int.Parse(numberString.Substring(eIndex + 1));
            //    numberString = numberString.Remove(eIndex);
            //}
            //else
            //    Exponent = 0;

            //if (pointIndex != -1)
            //{
            //    while (numberString[numberString.Length - 1] == '0')
            //        numberString = numberString.Remove(numberString.Length - 1, 1);
            //    numberString = numberString.Remove(pointIndex, 1);
            //    if (numberString.Length == 0)
            //    {
            //        Digits = "0";
            //        return;
            //    }
            //    Exponent += pointIndex - 1;
            //    while (numberString[0] == '0')
            //    {
            //        numberString = numberString.Remove(0, 1);
            //        Exponent--;
            //    }
            //}
            //else
            //    Exponent += numberString.Length - 1;

            //while (numberString.Length > 1 && numberString[numberString.Length - 1] == '0')
            //    numberString = numberString.Remove(numberString.Length - 1, 1);
            //Digits = numberString;
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

        //private void SetDigits()
        //{

        //}

        //private string GetDigits(int startIndex)
        //{
        //    if(_Data.Length - startIndex == 1)
        //    {
        //        return _Data[startIndex].ToString();
        //    }
        //    return "";
        //}
        //private void SetExponent()
        //{

        //}

        //private long GetExponent(out int exponentLength)
        //{
        //    switch ((byte)(_Data[0] << 1) >> 7)
        //    {
        //        case 0:
        //            exponentLength = 1;
        //            return (byte)(_Data[0] << 3) >> 3;
        //        case 1:
        //            exponentLength = 2;
        //            return BitConverter.ToInt64(new byte[] { (byte)(_Data[0] << 3), _Data[1] }, 0);
        //        case 2:
        //            exponentLength = 4;
        //            return BitConverter.ToInt64(new byte[] { (byte)(_Data[0] << 3), _Data[1], _Data[2], _Data[3] }, 0);
        //        case 3:
        //            exponentLength = 8;
        //            return BitConverter.ToInt64(new byte[] { (byte)(_Data[0] << 3), _Data[1], _Data[2], _Data[3], _Data[4], _Data[5], _Data[6], _Data[7] }, 0);
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        private string ToString(int digits, char format, IFormatProvider provider)
        {
            bool negative = _Data[0] >> 7 == 1;
            long e = Exponent;
            if (format == 'G')
                format = 'E';
            StringBuilder result = new StringBuilder();
            //result.Append(GetDigits(exponentLength));
            
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
