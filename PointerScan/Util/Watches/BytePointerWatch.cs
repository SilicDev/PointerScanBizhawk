using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerScan.Util.Watches
{
    public sealed class BytePointerWatch : PointerWatch
    {
        private byte _previous;
        private byte _value;

        public BytePointerWatch(MemoryDomain domain, uint domain_offset, uint address, IReadOnlyList<uint> offsets, AddressSize address_size, WatchDisplayType type, bool bigEndian, string note, byte value, byte previous, int changeCount) 
            : base(domain, domain_offset, address, offsets, address_size, WatchSize.Byte, type, bigEndian, note)
        {
            _value = value == 0 ? GetByte() : value;
            _previous = previous;
            ChangeCount = changeCount;
        }

        public static readonly IReadOnlyList<WatchDisplayType> ValidTypes = new List<WatchDisplayType>() { 
            WatchDisplayType.Unsigned,
            WatchDisplayType.Signed,
            WatchDisplayType.Hex,
            WatchDisplayType.Binary,
        };

        public override string Diff => $"{_value - _previous:+#;-#;0}";

        public override uint MaxValue => byte.MaxValue;

        public override uint Value => GetByte();

        public override string ValueString => FormatValue(GetByte());

        public override uint Previous => _previous;

        public override string PreviousString => FormatValue(_previous);

        public override bool IsValid => (Domain.Size == 0 || Address < Domain.Size) && (_addresses.Count == 0 || _addresses.All((a) => a < Domain.Size));

        public override IReadOnlyList<WatchDisplayType> AvailableTypes() => ValidTypes;

        public override bool Poke(string value)
        {
            try
            {
                byte val = Type switch
                {
                    WatchDisplayType.Unsigned => byte.Parse(value),
                    WatchDisplayType.Signed => (byte)sbyte.Parse(value),
                    WatchDisplayType.Hex => byte.Parse(value, NumberStyles.HexNumber),
                    WatchDisplayType.Binary => Convert.ToByte(value, 2),
                    _ => 0,
                };

                PokeByte(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void ResetPrevious()
        {
            _previous = GetByte();
        }

        public override void UpdateValue(PreviousType previousType)
        {
            switch (previousType)
            {
                case PreviousType.Original:
                    return;
                case PreviousType.LastChange:
                    var temp = _value;
                    _value = GetByte();
                    if (_value != temp)
                    {
                        _previous = _value;
                        ChangeCount++;
                    }

                    break;
                case PreviousType.LastFrame:
                    _previous = _value;
                    _value = GetByte();
                    if (_value != Previous)
                    {
                        ChangeCount++;
                    }

                    break;
            }
        }

        public string FormatValue(byte val)
        {
            return Type switch
            {
                _ when !IsValid => "-",
                WatchDisplayType.Unsigned => val.ToString(),
                WatchDisplayType.Signed => ((sbyte)val).ToString(),
                WatchDisplayType.Hex => $"{val:X2}",
                WatchDisplayType.Binary => Convert.ToString(val, 2).PadLeft(8, '0').Insert(4, " "),
                _ => val.ToString(),
            };
        }
    }
}
