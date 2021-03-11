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
    //e = 0 = 1 % 0 = 1

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

    //Data =>
    //4     EndDigitsCount
    //4-60  Exponent

    //Number=>
    //NumberBytes
    //Last 1 Sign
    public class ArNumber
    {
        private byte[] _Data;
        private byte[] _Numbers;

        //public bool Negative
        //{
        //    get;
        //    set;
        //    get => _Data[_Data.Length - 1] >> 7 == 1;
        //    set
        //    {
        //        if (_Numbers.Length == 4 && _Numbers[0] == 0 &&
        //            _Numbers[1] == 0 && _Numbers[2] == 0 && _Numbers[3] == 0)
        //            return;
        //        _Data[_Data.Length - 1] = value ? (byte)(_Data[_Data.Length - 1] | 128) : (byte)(_Data[_Data.Length - 1] & 127);
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

        public void SetNumberBlock(int index, int value)
        {
            //參考E
            //_Numbers = new byte[30];
            int valueLength = Math.Abs(value).ToString().Length;
            //_Numbers
            //int firstUsed = ((_Data[_Data.Length - 1] & 15) * 2 + 5) / 5;
            int firstBitUsed = ((_Data[_Data.Length - 1] & 15) * 10 + 2) / 3;
            int j = index == 0 ? 0 : ((index - 1) * 30 + firstBitUsed) / 4;
            byte[] result;
            if(index == 0 || j + 4 > _Numbers.Length)                
            {
                if(index == 0)
                    _Data[_Data.Length - 1] = (byte)(_Data[_Data.Length - 1] & 15 | (16 * valueLength));
                
                switch (valueLength)
                {   
                    case 1: //4
                        _Numbers[j] = (byte)(_Numbers[j] & 240 | (value & 15));
                        break;
                    case 2: //7
                        _Numbers[j] = (byte)(_Numbers[j] & 128 | (value & 127));
                        break;
                    case 3: //10
                        result = BitConverter.GetBytes((short)value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        _Numbers[j] = result[0];
                        _Numbers[j + 1] = (byte)(_Numbers[j + 1] & 252 | (result[1] & 3));
                        break;
                    case 4: //14
                        result = BitConverter.GetBytes((short)value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        _Numbers[j] = result[0];
                        _Numbers[j + 1] = (byte)(_Numbers[j + 1] & 192 | (result[1] & 63));
                        break;
                    case 5: //17
                        result = BitConverter.GetBytes(value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        for (int i = j; i < j + 2; i++)
                            _Numbers[i] = result[i];
                        _Numbers[j + 2] = (byte)(_Numbers[j + 2] & 254 | (result[2] & 1));
                        break;
                    case 6: //20
                        result = BitConverter.GetBytes(value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        for (int i = j; i < j + 2; i++)
                            _Numbers[i] = result[i];
                        _Numbers[j + 2] = (byte)(_Numbers[j + 2] & 240 | (result[2] & 15));
                        break;
                    case 7: //24
                        result = BitConverter.GetBytes(value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        for (int i = j; i < j + 3; i++)
                            _Numbers[i] = result[i];
                        break;
                    case 8: //27
                        result = BitConverter.GetBytes(value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        for (int i = 0; i < j + 3; i++)
                            _Numbers[i] = result[i];
                        _Numbers[j + 3] = (byte)(_Numbers[j + 3] & 248 | (result[3] & 7));
                        break;
                    case 9: //30
                        result = BitConverter.GetBytes(value);
                        if (!BitConverter.IsLittleEndian)
                            result = Reverse(result);
                        for (int i = 0; i < j + 3; i++)
                            _Numbers[i] = result[i];
                        _Numbers[j + 3] = (byte)(_Numbers[j + 3] & 192 | (result[3] & 63));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else // 4
            {   
                result = BitConverter.GetBytes(value);
                if (!BitConverter.IsLittleEndian)
                    result = Reverse(result);
                for (int i = j; i < j + 4; i++)
                    _Numbers[i] = result[i];
            }
        }

        public int GetNumberBlock(int index)
        {
            int firstUsed = ((_Data[_Data.Length - 1] & 15) * 2 + 5) / 5;
            int j = index * 4;
            //int valueLegth =             
            if (index == 0)
            {
                //if (firstUsed == 1)
                //    return _Numbers[j];
                //else if (firstUsed == 2)
                //    if (BitConverter.IsLittleEndian)
                //        return (int)BitConverter.ToInt16(new byte[] { _Numbers[j], _Numbers[j + 1] }, 0);
                //    else
                //        return (int)BitConverter.ToInt16(new byte[] { _Numbers[j + 1], _Numbers[j] }, 0);
                //else if(firstUsed == 3)
                //    if (BitConverter.IsLittleEndian)
                //        return (int)BitConverter.ToInt16(new byte[] { _Numbers[j], _Numbers[j + 1] }, 0);
                //    else
                //        return (int)BitConverter.ToInt16(new byte[] { _Numbers[j + 1], _Numbers[j] }, 0);
                //else

            }
            else
            {

            }
            //參考E
            return 1;
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