using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //ArNumber 架構

    //1         0 => Positive
    //          1 => Negative    
    //N         Exponent (First 1 is sign)
    //          7 bit Exponent
    //          15 bit Exponent
    //          31 bit Exponent
    //          63 bit Exponent

    //N         Digits 連續的二進位來表示十進位
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


        private void SetDigits(int index, ushort value)
        {
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
            //}
        }
        private ushort GetDigits(int index)
        {
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
        }
    }
}
