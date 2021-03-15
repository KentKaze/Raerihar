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


    //300
    // 3
    // e = 2

    //      300
    //      ---



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
    public sealed class ArNumber : IEquatable<ArNumber>, IComparable, IComparable<ArNumber>, IFormattable, ICloneable, IConvertible
    {
        public const long ExponentMaxValue = 576460752303423487;
        public const long ExponentMinValue = -576460752303423488;
        public const byte MaximumDisplayedDigitsCount = 17;
        private const int MaximumBytesCount = 4166668;

        private byte[] _Data;
        private byte[] _Numbers;

        public long DigitsCount
        {
            get
            {
                int lastDigits = _Data[_Data.Length - 1] & 15;
                int bitUsed = (lastDigits * 10 + 2) / 3 + 1;
                if (_Numbers.Length == bitUsed / 8 + 1 && (_Numbers[_Numbers.Length - 1] >> (bitUsed % 8) == 0))
                    return lastDigits;
                else
                    return lastDigits + PostiveRemainder(Exponent + 1, 9) + (_Numbers.Length * 8 - bitUsed - (PostiveRemainder(Exponent + 1, 9) * 10 + 2) / 3) / 10 * 3;
            }
        }
        public bool Negative
        {
            get => (_Numbers[0] & 1) == 1;
            set => _Numbers[0] = (byte)(_Numbers[0] & 254 | (value ? 1 : 0));
        }
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
            _Data[0] = 1;
            _Numbers = new byte[1];
        }

        private ArNumber(long bytesCount, long e, bool isNegative)
        {
            SetExponent(e);
            _Numbers = new byte[bytesCount];
            Negative = isNegative;
        }

        public ArNumber(ArNumber a)
        {
            _Data = a._Data;
            _Numbers = a._Numbers;
        }

        public ArNumber(sbyte value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(byte value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(short value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(ushort value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(uint value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(int value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(ulong value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(long value)
            : this()
            => LoadInteger(value.ToString(), this);
        public ArNumber(decimal value)
            : this()
            => Parse(value.ToString("F4"), this);
        public ArNumber(float value)
            : this()
            => Parse(value.ToString("G9"), this);
        public ArNumber(double value)
            : this()
            => Parse(value.ToString("G17"), this);

        //private int GetIndexCount()
        //{
        //    int tail = PostiveRemainder(Exponent + 1, 9);
        //    return (int)(1 + (DigitsCount - tail) / 9 + ((DigitsCount - tail) % 9 <= 0 ? 0 : 1));
        //}

        private long GetBits(long digitsLength)
        {
            int tail = PostiveRemainder(Exponent + 1, 9);
            if (tail <= digitsLength)
                return (tail * 10 + 2) / 3 +
                    (digitsLength - tail) / 9 * 30 +
                    ((digitsLength - tail) % 9 * 10 + 2) / 3 + 1;
            else
                return (tail * 10 + 2) / 3 + 1;
        }

        private static int PostiveRemainder(long a, int b)
        {
            int result = (int)(a % b);
            return result >= 0 ? result : result + b;
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
        private void SetExponent(long e)
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
            if (_Data != null && _Data.Length != 0)
                result[result.Length - 1] |= (byte)(_Data[_Data.Length - 1] & 15);
            _Data = result;
        }

        public void SetNumberBlock(int index, uint value, int digitsCount)
        {
            //已使用多少Bit
            long bitUsed = index == 0 ? 1 : (index - 1) * 30 +
                ((_Data[_Data.Length - 1] & 15) * 10 + 2) / 3 + 1;
            //從哪個Byte開始
            int j = (int)(bitUsed / 8);
            //該Byte已使用多少Bit
            int move = (int)(bitUsed % 8);
            //寫幾位
            int writeBits = (digitsCount * 10 + 2) / 3;
            //記一下首位
            if (index == 0)
                _Data[_Data.Length - 1] = (byte)(_Data[_Data.Length - 1] & 240 | digitsCount);
            while (writeBits > 0)
            {
                if (writeBits <= 8 - move)
                {
                    _Numbers[j] = (byte)(_Numbers[j] & ((((1 << 8 - writeBits - move) - 1)
                        << writeBits + move) | (1 << move) - 1) | (byte)(value << move));
                    break;
                }
                else if (move == 0)
                    _Numbers[j] = (byte)(value << move);
                else if (writeBits >= 8)
                {
                    _Numbers[j] = (byte)(_Numbers[j] & ((1 << move) - 1) | (byte)(value << move));
                    _Numbers[j + 1] = (byte)(_Numbers[j + 1] & ((1 << (8 - move)) - 1 << move)
                        | (byte)value >> (8 - move));
                }
                else
                {
                    _Numbers[j] = (byte)(_Numbers[j] & ((1 << move) - 1) | (byte)(value << move));
                    _Numbers[j + 1] = (byte)((_Numbers[j + 1] & ((1 << (8 - move)) - 1 << move)
                        & (byte)((1 << 16 - writeBits - move) - 1) << (writeBits - 8 + move))
                        | (byte)value >> (8 - move));
                }
                value = value >> 8;
                writeBits -= 8;
                j++;
            }
        }
        public uint GetNumberBlock(int index, int digitsCount)
        {
            //已使用多少Bit            
            long bitUsed = index == 0 ? 1 : (index - 1) * 30 +
                ((_Data[_Data.Length - 1] & 15) * 10 + 2) / 3 + 1;
            //從哪個Byte開始
            int j = (int)(bitUsed / 8);
            //該Byte已使用多少Bit
            int move = (int)(bitUsed % 8);
            //讀幾位
            int readBits = (digitsCount * 10 + 2) / 3;
            byte[] result = readBits >= 17 ? new byte[4] : new byte[readBits / 8 + 1];
            for (int i = 0; readBits > 0; i++)
            {
                if (readBits <= 8 - move)
                    result[i] = (byte)((byte)(_Numbers[j + i] << (8 - move - readBits)) >> (8 - readBits));
                else if (move == 0)
                    result[i] = _Numbers[j + i];
                else if (readBits >= 8)
                    result[i] = (byte)((_Numbers[j + i] >> move) | (byte)(_Numbers[j + i + 1] << (8 - move)));
                else
                    result[i] = (byte)((_Numbers[j + i] >> move) | (byte)((_Numbers[j + i + 1] & (1 << (readBits - 8 + move)) - 1) << (8 - move)));
                readBits -= 8;
            }

            if (result.Length == 1)
                return result[0];
            else if (result.Length == 2)
                if (BitConverter.IsLittleEndian)
                    return BitConverter.ToUInt16(result, 0);
                else
                    return BitConverter.ToUInt16(Reverse(result), 0);
            else
                if (BitConverter.IsLittleEndian)
                return BitConverter.ToUInt32(result, 0);
            else
                return BitConverter.ToUInt32(Reverse(result), 0);
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
                    a = new ArNumber(); // Set To 0
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

            LoadStandardData(numberString, a, isNegative, e);
        }
        private static void LoadInteger(string numberStringWithSign, ArNumber a)
        {
            bool isNegative = false;            
            if (numberStringWithSign[0] == '-')
            {
                isNegative = true;
                numberStringWithSign = numberStringWithSign.Remove(0, 1);
            }
            long e = numberStringWithSign.Length - 1;
            while (numberStringWithSign.Length > 9 && numberStringWithSign.Substring(numberStringWithSign.Length - 9, 9) == "000000000")
                numberStringWithSign = numberStringWithSign.Remove(numberStringWithSign.Length - 9, 9);
            if (numberStringWithSign == "0")
            {
                a = new ArNumber();
                return;
            }
            LoadStandardData(numberStringWithSign, a, isNegative, e);
        }
        private static void LoadStandardData(string numberString, ArNumber a, bool isNegative = false, long e = 0)
        {
            int pr = PostiveRemainder(e + 1, 9);
            if (pr % 9 != 0)
                if (pr > numberString.Length)
                    numberString = $"{numberString}{new string('0', pr - numberString.Length)}";

            a._Numbers = new byte[(a.GetBits(numberString.Length) + 7) / 8];
            a.Negative = isNegative;
            a.SetExponent(e);

            uint v;
            int digitIndex = numberString.Length;
            int substractedDigits;
            for (int i = 0; digitIndex > 0; i++)
            {
                if (i == 0)
                {
                    if ((numberString.Length - pr) % 9 != 0)
                        substractedDigits = (numberString.Length - pr) % 9;
                    else if (numberString.Length < 9)
                        substractedDigits = numberString.Length;
                    else
                        substractedDigits = 9;
                }
                else if (digitIndex < 9)
                    substractedDigits = PostiveRemainder(e + 1, 9);
                else
                    substractedDigits = 9;
                digitIndex -= substractedDigits;
                v = uint.Parse(numberString.Substring(digitIndex, substractedDigits));
                a.SetNumberBlock(i, v, substractedDigits);
            }
        }

        private static bool AdaptExponentForm(ArNumber a)
        {
            long e = a.Exponent;
            long l = a.DigitsCount; // Todo
            if ((e > 0 && e + 1 <= MaximumDisplayedDigitsCount) ||
                (e < 0 && e - l + 3 >= MaximumDisplayedDigitsCount * -1))
                return false;
            return true;
        }
        public static bool IsInteger(ArNumber a)
            => a.Exponent - a.DigitsCount + 1 >= 0;

        private static long RetouchAndCountBytes(List<uint> sumList)
        {
            while (sumList[0] == 0)
                sumList.RemoveAt(0);

            if (sumList.Count == 1)
                return ((sumList[0].ToString().Length * 10 + 2) / 3 + 8) / 8;
            else
                return ((sumList[sumList.Count - 1].ToString().Length * 10 + 2) / 3 +
                    30 * (sumList.Count - 1) + 8) / 8;
        }

        public static ArNumber Negate(ArNumber a)
        {
            ArNumber result = new ArNumber(a);
            result.Negative = !result.Negative;
            return result;
        }

        static int loop = 0;

        private static ArNumber AddMinus(ArNumber a, ArNumber b, bool isAdd = true)
        {   
            if (loop > 100)
            {
                Console.WriteLine("Loop");
                return -1;
            }
                
            if (isAdd)
            {
                loop++;
                if (a.Negative && !b.Negative)
                    return AddMinus(b, Negate(a), false);
                else if (!a.Negative && b.Negative)
                    return AddMinus(a, Negate(b), false);
            }
            else
            {
                loop++;
                if ((a.Negative && !b.Negative) || (!a.Negative && b.Negative))
                    return AddMinus(a, Negate(b), true);
                else if (b > a && !a.Negative && !b.Negative)
                    return AddMinus(Negate(b), Negate(a), false);
                else if (a > b && a.Negative && b.Negative)
                    return AddMinus(Negate(b), Negate(a), false);
            }
            //a will > b when Minus
            loop = 0;
            long a_e = a.Exponent;
            int a_tail = PostiveRemainder(a_e + 1, 9);
            long a_digitsCount = a.DigitsCount;
            if (a_tail > a_digitsCount)
                a_tail = (int)a_digitsCount;
            else if (a_tail == 0)
                a_tail = 9;
            int a_mid = (int)((a_digitsCount - a_tail) / 9);
            int a_head = (int)((a_digitsCount - a_tail) % 9);
            int a_indexCount = 1 + a_mid + (a_head > 0 ? 1 : 0);

            long b_e = b.Exponent;
            int b_tail = PostiveRemainder(b_e + 1, 9);
            long b_digitsCount = b.DigitsCount;
            if (b_tail > b_digitsCount)
                b_tail = (int)b_digitsCount;
            else if (b_tail == 0)
                b_tail = 9;
            int b_mid = (int)((b_digitsCount - b_tail) / 9);
            int b_head = (int)((a_digitsCount - a_tail) % 9);
            int b_indexCount = 1 + b_mid + (b_head > 0 ? 1 : 0);

            //1.12
            //- -- e = 2 digits = 3 e+ 1 - digits > 0
            //3 + 5 = 
            long AlastE = a_e - a_digitsCount + 1 - a_head == 0 ? 0 : 9 - a_head;
            long BlastE = b_e - b_digitsCount + 1 - b_head == 0 ? 0 : 9 - b_head;
            long lastE = AlastE > BlastE ? BlastE : AlastE;
            long e = lastE;
            long sum = 0;
            int i = 0, j = 0;
            int plusA, plusB;
            int carry = 0;
            int digitsA;
            int digitsB;
            List<uint> sumList = new List<uint>();
            while (e < a_e + 1 || e < b_e + 1)
            {
                if (e > a_e - a_digitsCount && e < a_e + 1)
                {
                    if (i == 0)
                    {
                        digitsA = a._Data[a._Data.Length - 1] & 15;
                        plusA = (int)a.GetNumberBlock(i, digitsA);
                        if (a_e + 1 - a_digitsCount < 0)
                            plusA *= (int)Math.Pow(10, 9 - digitsA);
                    }
                    else if (i == a_indexCount - 1)
                    {
                        digitsA = a_tail;
                        plusA = (int)a.GetNumberBlock(i, digitsA);
                    }
                    else
                    {
                        if (i > b_indexCount - 1)
                            Console.WriteLine("OOOOA");
                        digitsA = 9;
                        plusA = (int)a.GetNumberBlock(i, digitsA);
                    }
                    i++;
                }
                else
                    plusA = 0;
                if (e > b_e - b_digitsCount && e < b_e + 1)
                {
                    if (j == 0)
                    {
                        digitsB = b._Data[b._Data.Length - 1] & 15;
                        plusB = (int)b.GetNumberBlock(j, digitsB);
                        if (b_e + 1 - b_digitsCount < 0)
                            plusB *= (int)Math.Pow(10, 9 - digitsB);
                    }
                    else if (j == b_indexCount - 1)
                    {
                        digitsB = b_tail;
                        plusB = (int)b.GetNumberBlock(j, digitsB);
                    }
                    else
                    {
                        if (j > b_indexCount - 1)
                            Console.WriteLine("OOOOB");
                        digitsB = 9;
                        plusB = (int)b.GetNumberBlock(j, digitsB);
                    }
                    j++;
                }
                else
                    plusB = 0;

                if (isAdd)
                    sum = plusA + plusB + carry;
                else
                    sum = plusA - plusB + carry;
                if (sum > 999999999)
                {
                    sum -= 1000000000;
                    carry = 1;
                }
                else if (sum < 0)
                {
                    sum += 1000000000;
                    carry = -1;
                }
                else
                    carry = 0;

                sumList.Add((uint)sum);
                e += 9;
            }

            if (carry == 1)
                sumList.Add(1);
            else
                e -= 10 - sum.ToString().Length;

            ArNumber result = new ArNumber(RetouchAndCountBytes(sumList), e, a.Negative);
            if (sumList.Count == 1)
            {
                result.SetNumberBlock(0, sumList[0], sumList[0].ToString().Length);
                return result;
            }
            result.SetNumberBlock(0, sumList[0], 9);
            for (i = 1; i < sumList.Count - 1; i++)
                result.SetNumberBlock(i, sumList[i], 9);
            result.SetNumberBlock(sumList.Count - 1, sumList[sumList.Count - 1], sumList[sumList.Count - 1].ToString().Length);
            return result;
        }
        public string GetNumbersToString()
        {
            StringBuilder numbers = new StringBuilder();
            int tail = PostiveRemainder(Exponent + 1, 9);
            long digitsCount = DigitsCount;

            if (tail > digitsCount)
                tail = (int)digitsCount;
            else if (tail == 0)
                tail = 9;
            int mid = (int)((digitsCount - tail) / 9);
            int head = (int)((digitsCount - tail) % 9);
            int indexCount = 1 + mid + (head > 0 ? 1 : 0);

            numbers.Append(GetNumberBlock(indexCount - 1, tail));
            for (int i = indexCount - 2; i >= 1; i--)
                numbers.Append(GetNumberBlock(i, 9).ToString().PadLeft(9, '0'));
            if (head > 0)
                numbers.Append(GetNumberBlock(0, head).ToString().PadLeft(head, '0'));
            else if (indexCount - 1 != 0)
                numbers.Append(GetNumberBlock(0, 9).ToString().PadLeft(9, '0'));
            while (numbers.Length > 1 && numbers[numbers.Length - 1] == '0')
                numbers.Remove(numbers.Length - 1, 1);
            return numbers.ToString();
        }
        private string ToString(int digitsDisplay, char format, IFormatProvider provider)
        {
            string numbers = GetNumbersToString();
            if (digitsDisplay < 0)
                throw new ArgumentOutOfRangeException(nameof(digitsDisplay));
            else if (digitsDisplay == 0 || digitsDisplay > numbers.Length)
                digitsDisplay = numbers.Length;
            // TO DO if e > int overflow   
            long e = Exponent;
            if (format == 'G')
                if (AdaptExponentForm(this))
                    format = 'E';
                else if (IsInteger(this))
                    format = 'D';
                else
                    format = 'F';

            //if(format == 'C' && format == 'N' && format == 'P')
            //CDFNPXEG
            StringBuilder result = new StringBuilder();
            result.Append(numbers);
            if (result.Length != 1 && format == 'E')
                result.Insert(1, '.');
            if (e != 0)
            {
                if (format == 'E')
                    result.AppendFormat("E{0}{1}", e > 0 ? "+" : "", e);
                else if (format == 'D')
                {
                    if (e > digitsDisplay - 1)
                        result.Append(new string('0', (int)e - digitsDisplay + 1));
                    else
                    {
                        if (e > 0)
                            result.Remove((int)e + 1, result.Length - (int)e - 1);
                        else
                            return "0";
                    }
                }
                else if (format == 'F')
                {
                    if (e > digitsDisplay - 1)
                        result.Append(new string('0', (int)e - digitsDisplay + 1));
                    else if (e > 0)
                        result.Insert((int)e + 1, '.');
                    else
                        result.Insert(0, $"0.{new string('0', Math.Abs((int)e + 1))}");
                }
            }
            if (Negative)
                result.Insert(0, '-');
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
                case 'D':
                case 'F':
                case 'N':
                case 'P':
                case 'X':
                case 'E':
                case 'G':
                    return ToString(length, format[0], provider);
                default:
                    throw new FormatException(string.Format("The '{0}' format string is not supported.", format));
            }
        }
        public bool Equals(ArNumber other)
        {
            if (_Numbers.Length != other._Numbers.Length)
                return false;
            if (_Data.Length != other._Data.Length)
                return false;
            for (int i = 0; i < _Data.Length; i++)
                if (_Data[i] != other._Data[i])
                    return false;
            for (int i = 0; i < _Numbers.Length; i++)
                if (_Numbers[i] != other._Numbers[i])
                    return false;
            return true;
        }

        public int CompareTo(ArNumber other)
        {
            if (Equals(other))
                return 0;
            else if (!Negative && other.Negative)
                return 1;
            else if (Negative && !other.Negative)
                return -1;
            int result = Negative && other.Negative ? -1 : 1;
            if (Exponent > other.Exponent)
                return result;
            else if (Exponent < other.Exponent)
                return result * -1;
            //(To Do - Temp)
            return GetNumbersToString().CompareTo(other.GetNumbersToString());
            //int indexCount = GetIndexCount();
            //int otherIndexCount = other.GetIndexCount();
            //for (int i = 0; i < indexCount && i < otherIndexCount; i++)
            //    if (GetNumberBlock(indexCount - i, i == 0) > other.GetNumberBlock(otherIndexCount - i, i == 0))
            //        return result;
            //if (indexCount > otherIndexCount)
            //    return result;
            //else if (indexCount < otherIndexCount)
            //    return result * -1;
            //throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int result = _Data[0].GetHashCode();
            for (int i = 1; i < _Data.Length; i++)
                result ^= _Data[i].GetHashCode();
            for (int i = 0; i < _Numbers.Length; i++)
                result ^= _Numbers[i].GetHashCode();
            return result;
        }
        public override bool Equals(object obj)
        {
            ArNumber ar = obj as ArNumber;
            if (ar == null)
                return false;
            return Equals(ar);
        }
        public int CompareTo(object obj)
           => CompareTo((ArNumber)obj);
        public object Clone()
            => new ArNumber(this);
        public TypeCode GetTypeCode()
            => TypeCode.Object;
        public bool ToBoolean(IFormatProvider provider)
            => throw new InvalidCastException();
        public byte ToByte(IFormatProvider provider)
            => (byte)this;
        public char ToChar(IFormatProvider provider)
            => (char)this;
        public DateTime ToDateTime(IFormatProvider provider)
            => throw new InvalidCastException();
        public decimal ToDecimal(IFormatProvider provider)
            => (decimal)this;
        public double ToDouble(IFormatProvider provider)
            => (double)this;
        public short ToInt16(IFormatProvider provider)
            => (short)this;
        public int ToInt32(IFormatProvider provider)
            => (int)this;
        public long ToInt64(IFormatProvider provider)
            => (long)this;
        public sbyte ToSByte(IFormatProvider provider)
            => (sbyte)this;
        public float ToSingle(IFormatProvider provider)
            => (float)this;
        public object ToType(Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(this, conversionType, provider);
        public ushort ToUInt16(IFormatProvider provider)
            => (ushort)this;
        public uint ToUInt32(IFormatProvider provider)
            => (uint)this;
        public ulong ToUInt64(IFormatProvider provider)
            => (ulong)this;

        public static implicit operator ArNumber(sbyte a)
            => new ArNumber(a);
        public static implicit operator ArNumber(byte a)
            => new ArNumber(a);
        public static implicit operator ArNumber(short a)
            => new ArNumber(a);
        public static implicit operator ArNumber(ushort a)
            => new ArNumber(a);
        public static implicit operator ArNumber(char a)
            => new ArNumber(a);
        public static implicit operator ArNumber(int a)
            => new ArNumber(a);
        public static implicit operator ArNumber(uint a)
            => new ArNumber(a);
        public static implicit operator ArNumber(long a)
            => new ArNumber(a);
        public static implicit operator ArNumber(ulong a)
            => new ArNumber(a);
        public static implicit operator ArNumber(decimal a)
            => new ArNumber(a);
        public static implicit operator ArNumber(float a)
            => new ArNumber(a);
        public static implicit operator ArNumber(double a)
            => new ArNumber(a);
        public static explicit operator sbyte(ArNumber a)
            => sbyte.Parse(a.ToString());
        public static explicit operator byte(ArNumber a)
            => byte.Parse(a.ToString());
        public static explicit operator short(ArNumber a)
            => short.Parse(a.ToString());
        public static explicit operator ushort(ArNumber a)
            => ushort.Parse(a.ToString());
        public static explicit operator char(ArNumber a)
            => (char)int.Parse(a.ToString());
        public static explicit operator int(ArNumber a)
            => int.Parse(a.ToString());
        public static explicit operator uint(ArNumber a)
            => uint.Parse(a.ToString());
        public static explicit operator long(ArNumber a)
            => long.Parse(a.ToString());
        public static explicit operator ulong(ArNumber a)
            => ulong.Parse(a.ToString());
        public static explicit operator float(ArNumber a)
            => float.Parse(a.ToString());
        public static explicit operator double(ArNumber a)
            => double.Parse(a.ToString());
        public static explicit operator decimal(ArNumber a)
            => decimal.Parse(a.ToString());

        public static bool operator >(ArNumber a, ArNumber b)
            => a.CompareTo(b) == 1;
        public static bool operator >=(ArNumber a, ArNumber b)
            => a.CompareTo(b) != -1;
        public static bool operator <(ArNumber a, ArNumber b)
            => a.CompareTo(b) == -1;
        public static bool operator <=(ArNumber a, ArNumber b)
            => a.CompareTo(b) != 1;
        public static bool operator ==(ArNumber a, ArNumber b)
            => a.Equals(b);
        public static bool operator !=(ArNumber a, ArNumber b)
            => !a.Equals(b);

        public static ArNumber operator +(ArNumber a, ArNumber b)
            => AddMinus(a, b);
        public static ArNumber operator -(ArNumber a, ArNumber b)
            => AddMinus(a, b, false);
    }
}


//private struct ArNumberInfo
//{
//    public int HeadDigits { get; set; }
//    public int MidCount { get; set; }
//    public int TailDigits { get; set; }
//    public long TotalDigits => HeadDigits + TailDigits + MidCount * 9;

//    public ArNumberInfo(int tailDigits)
//        : this(tailDigits, 0, 0)
//    { }
//    public ArNumberInfo(int tailDigits, int headDigits)
//        : this(tailDigits, headDigits, 0)
//    { }

//    public ArNumberInfo(int tailDigits, int headDigits, int midCount)
//    {
//        TailDigits = tailDigits;
//        HeadDigits = headDigits;
//        MidCount = midCount;
//    }
//}