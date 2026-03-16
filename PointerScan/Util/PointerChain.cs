using BizHawk.Client.Common;
using System;
using System.Collections.Generic;

namespace PointerScan.Util
{
    public class PointerChain
    {
        public uint StartAddress { get; private set; }
        public List<uint> Offsets { get; private set; }

        public bool Invalid = false;

        public PointerChain(uint startAddress, List<uint> offsets)
        {
            StartAddress = startAddress;
            Offsets = offsets;
        }

        public uint GetResultAddress(ApiContainer api, MemoryHelper.AddressSize addr_size, string domain, uint domain_offset)
        {
            uint size = api.Memory.GetMemoryDomainSize(domain);
            uint resultAddress = StartAddress;
            var address_format = "X" + ((uint)addr_size * 2);
            for (int i = 0; i < Offsets.Count; i++)
            {
                resultAddress = MemoryHelper.ReadMemory(api, addr_size, resultAddress, domain) + Offsets[i] - domain_offset;
                if (resultAddress > size - (uint)addr_size)
                {
                    throw new OutOfMemoryException(string.Format("{0:" + address_format + "} is out of bounds of memory domain {1} 0x{2:X}", resultAddress, domain, size));
                }
            }
            return resultAddress;
        }
    }
}
