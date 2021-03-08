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
    public sealed class ArNumber
    {
        private byte[] _Data;
        private byte[] _Numbers;

        public bool Negative
        {
            get => _Data[_Data.Length - 1] >> 7 == 1;
            set
            {
                if (Digits == "0")
                    return;
                _Data[_Data.Length - 1] = value ? (byte)(_Data[_Data.Length - 1] | 128) : (byte)(_Data[_Data.Length - 1] & 127);
            }
        }
        public long Exponent
        {
            get
            {
                long result;
                byte lastByte = _Data[_Data.Length - 1];
                _Data[_Data.Length - 1] |= (byte)(_Data[_Data.Length - 1] << 1 & 128);

                switch (_Data.Length)
                {
                    case 1:
                        result = _Data[0];
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
                if (value < 63 && value > -64)
                    result = new byte[] { (byte)value };
                else if (value < 16383 && value > -16384)
                    result = BitConverter.GetBytes((short)value);
                else if (value < 1073741823 && value > -1073741824)
                    result = BitConverter.GetBytes((int)value);
                else if (value < 4611686018427387903 && value > -4611686018427387904)
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

        public string Digits
        {
            get
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

        //public ArNumber(sbyte value)
        //    : this()
        //{
        //    if (value < 0)
        //        _Data[0] |= 128;
        //    if (Math.Abs((int)value) >= 100)
        //        _Data[0] |= 2;
        //    else if (Math.Abs((int)value) >= 10)
        //        _Data[0] |= 1;
        //    SetDigits(0, (ushort)Math.Abs((int)value));
        //}
        //public ArNumber(byte value)
        //    : this()
        //{
        //    if (value >= 100)
        //        _Data[0] |= 2;
        //    else if (value >= 10)
        //        _Data[0] |= 1;
        //    SetDigits(0, value);
        //}
        //public ArNumber(ushort value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(short value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(uint value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(int value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(ulong value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(long value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(decimal value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(float value)
        //    : this()
        //    => Parse(value.ToString(), this);
        //public ArNumber(double value)
        //    : this()
        //    => Parse(value.ToString(), this);

        //private void SetDigits(int index, ushort value)
        //{
        //byte[] buffer;
        //if (BitConverter.IsLittleEndian)
        //    buffer = BitConverter.GetBytes(value);
        //else
        //    buffer = Reverse(BitConverter.GetBytes(value));
        //int j = index * 5 / 4;
        //switch (index % 4)
        //{
        //    case 0:
        //        _Digits[j] = buffer[0]; // 8
        //        _Digits[j + 1] = (byte)(buffer[1] << 6); //2
        //        break;
        //    case 1:
        //        _Digits[j] |= (byte)(buffer[0] >> 2 & 63); //6
        //        _Digits[j + 1] = (byte)(buffer[0] << 6); //2
        //        _Digits[j + 1] |= (byte)(buffer[1] << 4); //2
        //        break;
        //    case 2:
        //        _Digits[j] |= (byte)(buffer[0] >> 4 & 15); //4
        //        _Digits[j + 1] = (byte)(buffer[0] << 4); //4
        //        _Digits[j + 1] |= (byte)(buffer[1] << 2); //2
        //        break;
        //    case 3:
        //        _Digits[j] |= (byte)(buffer[0] >> 6 & 3); //2
        //        _Digits[j + 1] = (byte)(buffer[0] << 2); //6
        //        _Digits[j + 1] |= buffer[1]; //2
        //        break;
        //    default:
        //        throw new NotImplementedException();
        //    //}
        //}
        //private ushort GetDigits(int index)
        //{
        //int j = index * 5 / 4;
        //switch (index % 4)
        //{
        //    case 0:
        //        return BitConverter.ToUInt16(new byte[] {
        //            _Digits[j],
        //            (byte)(_Digits[j + 1] >> 6 & 3) }, 0);
        //    case 1:
        //        return BitConverter.ToUInt16(new byte[] {
        //            (byte)(_Digits[j] << 2 | _Digits[j + 1] >> 6 & 3),
        //            (byte)(_Digits[j + 1] >> 4 & 3) }, 0);
        //    case 2:
        //        return BitConverter.ToUInt16(new byte[] {
        //            (byte)(_Digits[j] << 4 | _Digits[j + 1] >> 4 & 15),
        //            (byte)(_Digits[j + 1] >> 2 & 3) }, 0);
        //    case 3:
        //        return BitConverter.ToUInt16(new byte[] {
        //            (byte)(_Digits[j] << 6 | _Digits[j + 1] >> 2 & 63),
        //            (byte)(_Digits[j + 1] & 3) }, 0);
        //    default:
        //        throw new NotImplementedException();
        //}
        //}
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
    }
}
