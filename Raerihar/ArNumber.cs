using System;
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
    public class ArNumber
    {
        private byte[] _Data;
        private byte[] _Numbers;
        

        

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


        public void SetNumber(int index, int value, int move)
        {
            _Numbers = new byte[30];

            //_Numbers = new byte[((s.Length - 1) / 9 * 15 + 18) / 4];
        }

        public int GetNumber(int index, int move)
        {

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