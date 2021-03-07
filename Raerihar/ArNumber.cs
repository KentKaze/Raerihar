using System;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //ArNumber 架構

    //1 Negative
    //2 0 => 5  bit  Exponent
    //2 1 => 13 bit  Exponent
    //2 2 => 29 bit  Exponent
    //2 3 => 61 bit  Exponnet
    //N Exponent
    //N Digits 二進位表示

    //byte[]
    public struct ArNumber //: IComparable, IComparable<ArNumber>, IConvertible, IEquatable<ArNumber>, IFormattable
    {
        private byte[] _Data;
        public int Legnth { get; set; }
        public void Test()
        {
            //BitConverter.s
        }
        
    }
}
