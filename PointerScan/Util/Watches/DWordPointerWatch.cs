using BizHawk.Client.Common;
using BizHawk.Common.NumberExtensions;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerScan.Util.Watches
{
    public sealed class DWordPointerWatch : PointerWatch
    {
        private uint _previous;
        private uint _value;

        public DWordPointerWatch(MemoryDomain domain, uint domain_offset, uint address, IReadOnlyList<uint> offsets, AddressSize address_size, WatchDisplayType type, bool bigEndian, string note, uint value, uint previous, int changeCount) 
            : base(domain, domain_offset, address, offsets, address_size, WatchSize.DWord, type, bigEndian, note)
        {
            _value = value == 0 ? GetDWord() : value;
            _previous = previous;
            ChangeCount = changeCount;
        }

        public static readonly IReadOnlyList<WatchDisplayType> ValidTypes = new List<WatchDisplayType>() { 
            WatchDisplayType.Unsigned,
            WatchDisplayType.Signed,
            WatchDisplayType.Hex,
            WatchDisplayType.Binary,
            WatchDisplayType.FixedPoint_20_12,
            WatchDisplayType.FixedPoint_16_16,
            WatchDisplayType.Float,
        };

        public override string Diff => $"{_value - _previous:+#;-#;0}";

        public override uint MaxValue => byte.MaxValue;

        public override uint Value => GetDWord();

        public override string ValueString => FormatValue(GetDWord());

        public override uint Previous => _previous;

        public override string PreviousString => FormatValue(_previous);

        public override bool IsValid => (Domain.Size == 0 || Address < Domain.Size - 3) && (_addresses.Count == 0 || _addresses.All((a) => a < Domain.Size - 3));

        public override IReadOnlyList<WatchDisplayType> AvailableTypes() => ValidTypes;

        public override bool Poke(string value)
        {
            try
            {
                uint val = Type switch
                {
                    WatchDisplayType.Unsigned => uint.Parse(value),
                    WatchDisplayType.Signed => (uint)int.Parse(value),
                    WatchDisplayType.Hex => uint.Parse(value, NumberStyles.HexNumber),
                    WatchDisplayType.FixedPoint_20_12 => (uint)(double.Parse(value, NumberFormatInfo.InvariantInfo) * 4096.0),
                    WatchDisplayType.FixedPoint_16_16 => (uint)(double.Parse(value, NumberFormatInfo.InvariantInfo) * 65536.0),
                    WatchDisplayType.Float => BitConverter.ToUInt32(BitConverter.GetBytes(float.Parse(value, NumberFormatInfo.InvariantInfo)), 0),
                    WatchDisplayType.Binary => Convert.ToUInt32(value, 2),
                    _ => 0,
                };

                PokeDWord(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void ResetPrevious()
        {
            _previous = GetDWord();
        }

        public override void UpdateValue(PreviousType previousType)
        {
            switch (previousType)
            {
                case PreviousType.Original:
                    return;
                case PreviousType.LastChange:
                    var temp = _value;
                    _value = GetDWord();
                    if (_value != temp)
                    {
                        _previous = _value;
                        ChangeCount++;
                    }

                    break;
                case PreviousType.LastFrame:
                    _previous = _value;
                    _value = GetDWord();
                    if (_value != Previous)
                    {
                        ChangeCount++;
                    }

                    break;
            }
        }

        public string FormatValue(uint val)
        {
            string FormatFloat()
            {
                var _float = BitConverter.ToSingle(BitConverter.GetBytes(val), 0);
                return _float.ToString(NumberFormatInfo.InvariantInfo);
            }

            string FormatBinary()
            {
                var str = Convert.ToString(val, 2).PadLeft(32, '0');
                for (var i = 28; i > 0; i -= 4)
                {
                    str = str.Insert(i, " ");
                }
                return str;
            }

            return Type switch
            {
                _ when !IsValid => "-",
                WatchDisplayType.Unsigned => val.ToString(),
                WatchDisplayType.Signed => ((int)val).ToString(),
                WatchDisplayType.Hex => $"{val:X8}",
                WatchDisplayType.FixedPoint_20_12 => ((int)val / 4096.0).ToString("0.######", NumberFormatInfo.InvariantInfo),
                WatchDisplayType.FixedPoint_16_16 => ((int)val / 65536.0).ToString("0.######", NumberFormatInfo.InvariantInfo),
                WatchDisplayType.Float => FormatFloat(),
                WatchDisplayType.Binary => FormatBinary(),
                _ => val.ToString(),
            };
        }
    }
}
