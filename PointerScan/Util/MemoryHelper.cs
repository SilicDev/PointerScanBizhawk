using BizHawk.Client.Common;
using System;

namespace PointerScan.Util
{
    public class MemoryHelper
    {
        public enum AddressSize : byte
        {
            None = 0,
            ONE = 1,
            TWO = 2,
            THREE = 3,
            FOUR = 4,
        }
        public static uint ReadMemory(ApiContainer api, AddressSize addr_size, uint addr, string domain = "", bool big_endian = true)
        {
            api.Memory.SetBigEndian(big_endian);
            switch (addr_size)
            {
                case AddressSize.ONE:
                    return api.Memory.ReadByte(addr, domain);
                case AddressSize.TWO:
                    return api.Memory.ReadU16(addr, domain);
                case AddressSize.THREE:
                    return api.Memory.ReadU24(addr, domain);
                case AddressSize.FOUR:
                    return api.Memory.ReadU32(addr, domain);
            }
            throw new NotSupportedException();
        }
    }
}
