using System;
using System.Globalization;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    //ArNumber 架構

    //1 0 => Positive
    //  1 => Negative
    //2 0 => 5  bit  Exponent
    //  1 => 13 bit  Exponent
    //  2 => 29 bit  Exponent
    //  3 => 60 bit  Exponnet
    //N Exponent
    //N Digits 二進位表示

    //Special Number
    // 0 11 11111 11111111 11111111 11111111 11111111 11111111 11111111 11111111 Positive Infinity
    // 1 11 11111 11111111 11111111 11111111 11111111 11111111 11111111 11111111 Negative Infinity
    // 0 11 11111 11111111 11111111 11111111 11111111 11111111 11111111 11111111 0 NaN 
    public struct ArNumber //: IComparable, IComparable<ArNumber>, IConvertible, IEquatable<ArNumber>, IFormattable
    {
        private byte[] _Data;
        public int Legnth { get; set; }
        public static ArNumber Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return new ArNumber();
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
        public string ToString(string format, IFormatProvider provider)
        {
            return "";
        }

        public void Test()
        {
            //BitConverter.s
        }
        
    }
}
