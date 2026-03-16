using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
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
        public static uint ReadMemory(ApiContainer api, AddressSize addr_size, uint addr, MemoryDomain domain)
        {
            api.Memory.SetBigEndian(domain.EndianType == MemoryDomain.Endian.Big);
            switch (addr_size)
            {
                case AddressSize.ONE:
                    return api.Memory.ReadByte(addr, domain.Name);
                case AddressSize.TWO:
                    return api.Memory.ReadU16(addr, domain.Name);
                case AddressSize.THREE:
                    return api.Memory.ReadU24(addr, domain.Name);
                case AddressSize.FOUR:
                    return api.Memory.ReadU32(addr, domain.Name);
            }
            throw new NotSupportedException();
        }

        public static uint ReadMemory(ApiContainer api, AddressSize addr_size, uint addr, string domain, MemoryDomain.Endian endian = MemoryDomain.Endian.Big)
        {
            api.Memory.SetBigEndian(endian == MemoryDomain.Endian.Big);
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
