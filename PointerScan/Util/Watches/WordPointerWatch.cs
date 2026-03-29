using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PointerScan.Util.Watches
{
    public sealed class WordPointerWatch : PointerWatch
    {
        private ushort _previous;
        private ushort _value;

        public WordPointerWatch(MemoryDomain domain, uint domain_offset, uint address, IReadOnlyList<uint> offsets, AddressSize address_size, WatchDisplayType type, bool bigEndian, string note, ushort value, ushort previous, int changeCount) 
            : base(domain, domain_offset, address, offsets, address_size, WatchSize.Word, type, bigEndian, note)
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
            WatchDisplayType.FixedPoint_12_4,
        };

        public override string Diff => $"{_value - _previous:+#;-#;0}";

        public override uint MaxValue => ushort.MaxValue;

        public override uint Value => GetWord();

        public override string ValueString => FormatValue(GetWord());

        public override uint Previous => _previous;

        public override string PreviousString => FormatValue(_previous);

        public override bool IsValid => (Domain.Size == 0 || Address < Domain.Size - 1) && (_addresses.Count == 0 || _addresses.All((a) => a < Domain.Size - 1));

        public override IReadOnlyList<WatchDisplayType> AvailableTypes() => ValidTypes;

        public override bool Poke(string value)
        {
            try
            {
                ushort val = Type switch
                {
                    WatchDisplayType.Unsigned => ushort.Parse(value),
                    WatchDisplayType.Signed => (ushort)short.Parse(value),
                    WatchDisplayType.Hex => ushort.Parse(value, NumberStyles.HexNumber),
                    WatchDisplayType.Binary => Convert.ToUInt16(value, 2),
                    WatchDisplayType.FixedPoint_12_4 => (ushort)(double.Parse(value, NumberFormatInfo.InvariantInfo) * 16.0),
                    _ => 0,
                };

                PokeWord(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void ResetPrevious()
        {
            _previous = GetWord();
        }

        public override void UpdateValue(PreviousType previousType)
        {
            switch (previousType)
            {
                case PreviousType.Original:
                    return;
                case PreviousType.LastChange:
                    var temp = _value;
                    _value = GetWord();
                    if (_value != temp)
                    {
                        _previous = _value;
                        ChangeCount++;
                    }

                    break;
                case PreviousType.LastFrame:
                    _previous = _value;
                    _value = GetWord();
                    if (_value != Previous)
                    {
                        ChangeCount++;
                    }

                    break;
            }
        }

        public string FormatValue(ushort val)
        {
            return Type switch
            {
                _ when !IsValid => "-",
                WatchDisplayType.Unsigned => val.ToString(),
                WatchDisplayType.Signed => ((short)val).ToString(),
                WatchDisplayType.Hex => $"{val:X4}",
                WatchDisplayType.FixedPoint_12_4 => ((short)val / 16.0).ToString("F4", NumberFormatInfo.InvariantInfo),
                WatchDisplayType.Binary => Convert
                    .ToString(val, 2)
                    .PadLeft(16, '0')
                    .Insert(8, " ")
                    .Insert(4, " ")
                    .Insert(14, " "),
                _ => val.ToString(),
            };
        }
    }
}
