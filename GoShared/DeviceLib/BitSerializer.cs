using CommunityToolkit.Diagnostics;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.DeviceLib
{
    public class BitSerializer
    {
        const int BITS_PER_BYTE = 8;
        readonly byte[] data;
        int pointer = 0;
        byte value = 0;
        readonly static byte[] bitOperationValues = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x8, 0x4, 0x2, 0x1 };

        public BitSerializer(int max)
        {
            Guard.IsEqualTo(BITS_PER_BYTE, bitOperationValues.Length);

            data = new byte[max];
        }

        public void Enqueue(bool bitValue)
        {
            //Console.WriteLine("pointer[" + pointer + "] => " + bitValue);
            // 기본 0(false) 상태여서 true 경우만 처리하면 되갓으..

            if (bitValue)
            {
                int index = pointer / BITS_PER_BYTE;    // 0~max
                int bit = pointer % BITS_PER_BYTE;      // 0~7

                byte v = bitOperationValues[bit];
                data[index] |= v;
            }

            pointer++;
        }
        public void NextByte()
        {
            int bit = pointer % BITS_PER_BYTE;      // 0~7
            if (bit > 0)
                pointer += ((BITS_PER_BYTE-1) - bit);
        }

        public byte[] Retrieve()
        {
            return data;
        }
    }
}
