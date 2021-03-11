﻿using System;
using System.Collections.Generic;
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
//index	5		4			3		2			1	 0
//bit	4		30		    30		30			30	10
//bits = 134
//bytes = 17
//e = +18


//		1.321974894 648943195 489415948 943195489   156
//		- --------- --------- --------- ---------   ---
//index	5		4			3		2			1	  0
//bit	4		30		    30		30			30	 10
//bits = 134
//bytes = 17
//e = 0

//Data =>
//3     EndDigitsCount
//5-61  Exponent

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
                _Data[_Data.Length - 1] = (byte)((byte)(_Data[_Data.Length - 1] << 3) >> 3);
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
            if (e > -17 && e < 16)
                result = new byte[] { (byte)e };
            else if (e > -4097 && e < 4096)
                result = BitConverter.GetBytes((short)e);
            else if (e > -268435457 && e < 268435456)
                result = BitConverter.GetBytes((int)e);
            else if (e > -2305843009213693952 && e < 2305843009213693952)
                result = BitConverter.GetBytes(e);
            else
                throw new ArgumentOutOfRangeException(nameof(e));
            if (!BitConverter.IsLittleEndian && result.Length != 1)
                result = Reverse(result);
            result[result.Length - 1] &= 31;
            result[result.Length - 1] |= (byte)(_Data[_Data.Length - 1] & 224);
            _Data = result;
        }     

        public void SetNumberBlock(int index, int value)
        {
            //參考E
            _Numbers = new byte[30];

            //_Numbers = new byte[((s.Length - 1) / 9 * 15 + 18) / 4];
        }

        public int GetNumberBlock(int index)
        {
            //參考E
            return 1;
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