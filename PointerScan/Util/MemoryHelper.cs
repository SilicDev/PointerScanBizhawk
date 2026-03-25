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

        [Obsolete]
        public static uint ReadMemory(ApiContainer api, uint addr, AddressSize addr_size, MemoryDomain domain)
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

        [Obsolete]
        public static uint ReadMemory(ApiContainer api, uint addr, AddressSize addr_size, string domain, MemoryDomain.Endian endian = MemoryDomain.Endian.Big)
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

        public static uint ReadMemory(MemoryDomain domain, uint addr, AddressSize addr_size, MemoryDomain.Endian? endian = null)
        {
            if (endian == null)
            {
                endian = domain.EndianType;
            }
            switch (addr_size)
            {
                case AddressSize.ONE:
                    return domain.PeekByte(addr);
                case AddressSize.TWO:
                    return domain.PeekUshort(addr, endian == MemoryDomain.Endian.Big);
                case AddressSize.FOUR:
                    return domain.PeekUint(addr, endian == MemoryDomain.Endian.Big);
            }
            throw new NotSupportedException();
        }
    }
}
