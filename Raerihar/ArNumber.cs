using System;
using System.Collections.Generic;
using System.Text;


//V4 Design

//_Data
//Exponent 4 bit 12bit 28bit 60bit
//Last 4 bit
//0~8      BigNumberHead 型別1
//9        byte          型別2
//10       short
//11       int
//12       long
//13       int.int       型別3
//14       long.long     型別4
//15(reserve)

//_Numbers
// 9 1 byte
//10 2 bytes
//11 4 bytes
//12 8 bytes
//13 8 bytes
//14 16 bytes
//0~8 first 1 sign 倒序排列
//300存為 => 300 E+2
//0.4存為 => 4   E-1

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ArNumber
    {
    //    private byte[] _Data;
    //    private byte[] _Numbers;

    //    public ArNumber()
    //    {
    //        _Data = new byte[1];            
    //        _Numbers = new byte[1];
    //    }
    //    public ArNumber(ArNumber a)
    //    {
    //        _Data = new byte[a._Data.Length];
    //        for (int i = 0; i < a._Data.Length; i++)
    //            _Data[i] = a._Data[i];
    //        _Numbers = new byte[a._Numbers.Length];
    //        for (int i = 0; i < a._Numbers.Length; i++)
    //            _Numbers[i] = a._Numbers[i];
    //    }
    }
}
