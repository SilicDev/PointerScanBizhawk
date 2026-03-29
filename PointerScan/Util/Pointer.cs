using System.Collections.Generic;

namespace PointerScan.Util
{
    public class Pointer
    {
        public uint Address;
        public AddressSize Size;

        public Dictionary<uint, Pointer?> OffsetMap = new();

        public Pointer(uint address, AddressSize size)
        {
            Address = address;
            Size = size;
        }
    }
}
