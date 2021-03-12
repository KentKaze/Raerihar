using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{

    //Align Practice

    //位數		bits	 Byte
    //1		    4			1
    //2	    	7			1
    //3	    	10			2
    //4	    	14			2
    //5	    	17			3
    //6	    	20			3
    //7	    	24			3
    //8	    	27			4
    //9	    	30			4


    //		1 321974894 648943195.489415948 943195489  156
    //		- --------- --------- --------- ---------  ---
    //index	5		4	 	3		  2        1        0
    //bit	4		30	    30	     30	 	  30       10
    //bits = 134
    //bytes = 17
    //e = +18  (18 + 1) % 9 = 1

    //		1 321974894 648943195.489415948 943195489  156
    //		- --------- --------- --------- ---------  ---
    //index	5		4	 	3		  2        1        0
    //bit	4		30	    30	     30	 	  30       10
    //bits = 134
    //bytes = 17
    //e = +108 = (108 + 1) % 9 = 1


    //		1.321974894 648943195 489415948 943195489   156
    //		- --------- --------- --------- ---------   ---
    //index	5		4			3		2			1	  0
    //bit	4		30		    30		30			30	 10
    //bits = 134
    //bytes = 17
    //e = 0 = (0 + 1) % 9 = 1 0v0?

    //		132 197489464 894319548 941594894 319548915 6
    //		--- --------- --------- --------- --------- -
    //index	5       4		3			2       1	    0
    //bit	4		30		    30		30			30	 10
    //bits = 134
    //bytes = 17
    //e = -7 = (-7 + 1) % 9 = 3

    //		13219748 946489431 954894159 489431954 89156
    //		-------- --------- --------- --------- -----
    //index	4		    3           2	       1      0
    //bit	    27		30	    30		    30		 17
    //bits = 134
    //bytes = 17
    //e = -100 = (-100 + 1) % 9 = 0
    //(e + 1) % 9 = 0

    //Data =>
    //4     EndDigitsCount
    //4-60  Exponent

    //Number=>
    //NumberBytes
    //First 1 Sign

    //Head and Tail
    //30.52 =>30 52
    //        -- --
    //        e = 1 tail = 2

    //3    => 3
    //        -
    //        e = 0 tail = 1 (repeat)
    //0.5  => 0.5
    //        e = -1 tail = 1
    //0.0576 => 0.0576
    //        e = -2 tail = 7 (repeat)
    // e > 0
    public class ArNumber
    {
        private byte[] _Data;
        private byte[] _Numbers;

        //public long DigitsCount
        //{
        //    //get => _Data[0] & 1;
        //    //set => _Data[0] = _Data[0] & 254 | value
        //    {

        //        ((_Data[_Data.Length - 1] & 240) >> 4)
        //        if (_Data.Length <= 4)
        //            return;
        //        //(Exponent + 1) % 9 + ((_Data[_Data.Length - 1] & 240) >> 4)
        //    } 
        //}
        public bool Negative
        {
            get => (_Numbers[0] & 1) == 1;
            set => _Numbers[0] = (byte)(_Numbers[0] & 254 | (value ? 1 : 0));
        }

            //public long DigitsCount
            //{
            //    get
            //    {
            //        +_Data[_Data.Length - 1] & 240) >> 4;
            //    }
            //}
        public long Exponent
        { 
            get
            {
                long result;
                byte lastByte = _Data[_Data.Length - 1];
                if ((_Data[_Data.Length - 1] & 128) == 128)
                    _Data[_Data.Length - 1] = (byte)((byte)(_Data[_Data.Length - 1] >> 4) | 240);
                else
                    _Data[_Data.Length - 1] = (byte)((byte)(_Data[_Data.Length - 1] >> 4) & 15);
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
        }

        public ArNumber()
        {
            _Data = new byte[1];
            _Numbers = new byte[100];
        }

        public ArNumber(ArNumber a)
        {
            _Data = a._Data;
            _Numbers = a._Numbers;
        }

        private int GetIndexCount(long digitsLength)
        {
            int tail = (int)((Exponent + 1) % 9);
            return (int)(1 + (digitsLength - tail) / 9 + (digitsLength - tail) % 9);
        }

        private long GetBits(long digitsLength)
        {   
            int tail = (int)((Exponent + 1) % 9);
            return (tail * 10 + 2) / 3 + 
                (digitsLength - tail) / 9 * 30 + 
                ((digitsLength - tail) % 9 * 10 + 2) / 3 + 1;
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

        public void SetExponent(long e)
        {
            byte[] result;
            if (e > -9 && e < 8)
                result = new byte[] { (byte)e };
            else if (e > -2049 && e < 2048)
                result = BitConverter.GetBytes((short)e);
            else if (e > -134217729 && e < 134217728)
                result = BitConverter.GetBytes((int)e);
            else if (e > -576460752303423489 && e < 576460752303423488)
                result = BitConverter.GetBytes(e);
            else
                throw new ArgumentOutOfRangeException(nameof(e));
            if (!BitConverter.IsLittleEndian && result.Length != 1)
                result = Reverse(result);
            result[result.Length - 1] = (byte)(result[result.Length - 1] << 4);
            result[result.Length - 1] |= (byte)(_Data[_Data.Length - 1] & 15);
            _Data = result;
        }

        //value 不接受負值
        public void SetNumberBlock(int index, int value)
        {   
            //已使用多少Bit
            long bitUsed = index == 0 ? 1 : (index - 1) * 30 +
                (((_Data[_Data.Length - 1] & 240) >> 4) * 10 + 2) / 3 + 1;
            //從哪個Byte開始
            int j = (int)(bitUsed / 8);
            //該Byte已使用多少Bit
            int move = (int)(bitUsed % 8);

            //寫幾位
            int writeBits;
            if (index == 0)
            {
                //記一下首位
                _Data[_Data.Length - 1] = (byte)(_Data[_Data.Length - 1] & 15 | (16 * value.ToString().Length));
                writeBits = (((_Data[_Data.Length - 1] & 240) >> 4) * 10 + 2) / 3;
            }
            else if ((bitUsed + 1) / 8 + 4 > _Data.Length) //最後一位
                writeBits = (value.ToString().Length * 10 + 2) / 3;
            else
                writeBits = 30;

            //2情況 + 1情況
            while(writeBits > 0)
            {
                if (writeBits <= 8 - move)
                {
                    _Numbers[j] = (byte)(_Numbers[j] & ((((1 << 8 - writeBits - move) - 1) 
                        << writeBits + move) | (1 << move) - 1) | (value << move));
                    break;
                }
                else if (move == 0)
                    _Numbers[j] = (byte)(value << move);
                else
                {
                    //test
                    _Numbers[j] = (byte)(_Numbers[j] & ((1 << move) - 1) | (byte)(value << move));
                    _Numbers[j + 1] = (byte)(_Numbers[j + 1] & (((1 << (8 - writeBits - move)) - 1)
                        << (writeBits + move)) | (byte)value >> (8 - move));
                }
                value = value >> 8;
                writeBits -= 8;
                j++;
            }
        }

        //private string GetNumbersToString()
        //{
        //    StringBuilder result = new StringBuilder();
        //    for (int i = 0; i < DigitSetLength; i++)
        //        result.AppendFormat("{0:D9}", GetDigits(i));
        //    while (result.Length > 1 && result[0] == '0')
        //        result.Remove(0, 1);
        //    while (result.Length > 1 && result[result.Length - 1] == '0')
        //        result.Remove(result.Length - 1, 1);
        //    return result.ToString();
        //}

        //需先設定Negative和Exponenet
        //private void SetNumbersByString(string s)
        //{
        //    long e = Exponent;

        //    _Numbers = new byte[((s.Length - 1) / 9 * 15 + 18) / 4];
        //    int v;
        //    for (int i = 0; i < (s.Length - 1) / 9 + 1; i++)
        //    {
        //        if (i * 9 + 8 < s.Length)
        //            v = int.Parse(s.Substring(i * 9, 9));
        //        else
        //            v = int.Parse(s.Substring(i * 9).PadRight(9, '0'));
        //        SetDigits(i, v);
        //    }
        //}

        public int GetNumberBlock(int index)
        {
            //已使用多少Bit            
            long bitUsed = index == 0 ? 1 : (index - 1) * 30 +
                (((_Data[_Data.Length - 1] & 240) >> 4) * 10 + 2) / 3 + 1;
            //從哪個Byte開始
            int j = (int)(bitUsed / 8);
            //該Byte已使用多少Bit
            int move = (int)(bitUsed % 8);

            //讀幾位
            int readBits;
            if (index == 0)
                readBits = (((_Data[_Data.Length - 1] & 240) >> 4) * 10 + 2) / 3;
            else if ((bitUsed + 1) / 8 + 4 > _Data.Length) //最後一位
                readBits = (int)((Exponent + 1) % 9 * 10 + 2) / 3;
            else
                readBits = 30;
          
            byte[] result = readBits >= 17 ? new byte[4] : new byte[readBits / 8 + 1];
            for(int i = 0; readBits > 0; i++)
            {
                if (readBits <= 8 - move)
                    result[i] = (byte)((byte)(_Numbers[j + i] << (8 - move - readBits)) >> (8 - readBits));
                else if (move == 0)
                    result[i] = _Numbers[j + i];
                else
                    result[i] = (byte)((_Numbers[j + i] >> move) | (byte)(_Numbers[j + i + 1] << (8 - move)));
                readBits -= 8;
            }

            if (result.Length == 1)
                return (sbyte)result[0];
            else if (result.Length == 2)
                if(BitConverter.IsLittleEndian)
                    return BitConverter.ToInt16(result, 0);
                else
                    return BitConverter.ToInt16(Reverse(result), 0);
            else
                if (BitConverter.IsLittleEndian)
                    return BitConverter.ToInt32(result, 0);
                else
                    return BitConverter.ToInt32(Reverse(result), 0);         
        }

        public static bool TryParse(string s, out ArNumber result)
            => TryParse(s, NumberStyles.None, null, out result);
        public static bool TryParse(string s, NumberStyles style, out ArNumber result)
            => TryParse(s, style, null, out result);
        public static bool TryParse(string s, IFormatProvider provider, out ArNumber result)
            => TryParse(s, NumberStyles.None, provider, out result);
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

            a.Negative = isNegative;
            a.SetExponent(e);
            a._Numbers = new byte[a.GetBits(numberString.Length)];
            //int indexCount = a.GetIndexCount(numberString.Length);
            int v;
            int digitIndex = 0;
            for (int i = 0; digitIndex < numberString.Length; i++)
            {
                if(digitIndex == 0)
                {
                    v = int.Parse(numberString.Substring(digitIndex, (int)((e + 1) % 9)));
                    digitIndex += (int)((e + 1) % 9);
                }
                else if(numberString.Length - digitIndex < 9)
                {
                    v = int.Parse(numberString.Substring(digitIndex));
                    digitIndex = numberString.Length;
                }
                else
                {
                    v = int.Parse(numberString.Substring(digitIndex, 9));
                    digitIndex += 9;
                }
                a.SetNumberBlock(i, v);
            }            
        }

        //private static bool AdaptExponentForm(ArNumber a)
        //{
        //    long e = a.Exponent;
        //    int l = a.GetDigitsToString().Length; // Todo
        //    if ((e > 0 && e + 1 <= MaximumDisplayedDigitsCount) ||
        //        (e < 0 && e - l + 3 >= MaximumDisplayedDigitsCount * -1))
        //        return false;
        //    return true;
        //}

        //public static bool IsInteger(ArNumber a)
        //    => a.Exponent - a.GetDigitsToString().Length + 1 >= 0; // Todo
        private string ToString(int digits, char format, IFormatProvider provider)
        {
            //string numbers = GetDigitsToString();
            //if (digits < 0)
            //    throw new ArgumentOutOfRangeException(nameof(digits));
            //else if (digits == 0 || digits > numbers.Length)
            //    digits = numbers.Length;
            //// TO DO
            //long e = Exponent; // if e > int overflow            
            //if (format == 'G')
            //    format = 'E';
            //    //if (AdaptExponentForm(this))
            //    //    format = 'E';
            //    //else if (IsInteger(this))
            //    //    format = 'D';
            //    //else
            //    //    format = 'C';

            StringBuilder result = new StringBuilder();
            //result.Append(numbers);
            //if (result.Length != 1 && format == 'E')
            //    result.Insert(1, '.');
            //if (e != 0)
            //{
            //    if (format == 'E')
            //        result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
            //    else if (format == 'D')
            //    {
            //        if (e > digits - 1)
            //            result.Append(new string('0', (int)e - digits + 1));
            //        else
            //        {
            //            if (e > 0)
            //                result.Remove((int)e + 1, result.Length - (int)e - 1);
            //            else
            //                return "0";
            //        }
            //    }
            //    else if (format == 'C')
            //    {
            //        if (e > digits - 1)
            //            result.Append(new string('0', (int)e - digits + 1));
            //        else if (e > 0)
            //            result.Insert((int)e + 1, '.');
            //        else
            //            result.Insert(0, $"0.{new string('0', Math.Abs((int)e + 1))}");
            //    }
            //}
            //if (Negative)
            //    result.Insert(0, '-');
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
    }
}

// byte[] buffer;
//if (BitConverter.IsLittleEndian)
//    buffer = BitConverter.GetBytes(value);
//else
//    buffer = Reverse(BitConverter.GetBytes(value));
//int j = index * 15 / 4;
//switch (index % 4)
//{
//    case 0:
//        _Numbers[j] = buffer[0]; // 8
//        _Numbers[j + 1] = buffer[1]; //8
//        _Numbers[j + 2] = buffer[2]; //8
//        _Numbers[j + 3] = (byte)(buffer[3] << 2); //6
//        break;
//    case 1:
//        _Numbers[j] |= (byte)(buffer[0] >> 6 & 3); //2
//        _Numbers[j + 1] = (byte)(buffer[0] << 2); //6
//        _Numbers[j + 1] |= (byte)(buffer[1] >> 6 & 3); //2
//        _Numbers[j + 2] = (byte)(buffer[1] << 2); //6
//        _Numbers[j + 2] |= (byte)(buffer[2] >> 6 & 3); //2
//        _Numbers[j + 3] = (byte)(buffer[2] << 2); //6
//        _Numbers[j + 3] |= (byte)(buffer[3] >> 4 & 3); //2
//        _Numbers[j + 4] = (byte)(buffer[3] << 4); //4
//        break;
//    case 2:
//        _Numbers[j] |= (byte)(buffer[0] >> 4 & 15); //4
//        _Numbers[j + 1] = (byte)(buffer[0] << 4); //4
//        _Numbers[j + 1] |= (byte)(buffer[1] >> 4 & 15); //4
//        _Numbers[j + 2] = (byte)(buffer[1] << 4); //4
//        _Numbers[j + 2] |= (byte)(buffer[2] >> 4 & 15); //4
//        _Numbers[j + 3] = (byte)(buffer[2] << 4); //4
//        _Numbers[j + 3] |= (byte)(buffer[3] >> 2 & 15); //4
//        _Numbers[j + 4] = (byte)(buffer[3] << 6); //2
//        break;
//    case 3:
//        _Numbers[j] |= (byte)(buffer[0] >> 2 & 63); //6
//        _Numbers[j + 1] = (byte)(buffer[0] << 6); //2
//        _Numbers[j + 1] |= (byte)(buffer[1] >> 2 & 63); //6
//        _Numbers[j + 2] = (byte)(buffer[1] << 6); //2
//        _Numbers[j + 2] |= (byte)(buffer[2] >> 2 & 63); //6
//        _Numbers[j + 3] = (byte)(buffer[2] << 6); //2
//        _Numbers[j + 3] |= buffer[3]; //6
//        break;
//    default:
//        throw new NotImplementedException();
//}