using BizHawk.Client.Common;
using BizHawk.Common.NumberExtensions;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointerScan.Util.Watches
{
    public abstract class PointerWatch : IEquatable<PointerWatch>, IComparable<PointerWatch>
    {
        private MemoryDomain _domain;
        private WatchDisplayType _type;
        protected readonly List<uint> _addresses = new List<uint>();

        public PointerWatch(MemoryDomain domain, uint domain_offset, uint address, IReadOnlyList<uint> offsets, AddressSize address_size, WatchSize size, WatchDisplayType type, bool bigEndian, string note)
        {
            if (IsDisplayTypeAvailable(type))
            {
                _domain = domain;
                DomainOffset = domain_offset;
                Address = address;
                Offsets = offsets;
                AddressSize = address_size;
                Size = size;
                _type = type;
                BigEndian = bigEndian;
                Notes = note;
                _addresses = new List<uint>(Enumerable.Repeat((uint)0, Offsets.Count));
            }
            else
            {
                throw new ArgumentException($"{nameof(WatchDisplayType)} {type} is invalid for this type of {nameof(PointerWatch)}", nameof(type));
            }
        }

        public static PointerWatch GenerateWatch(MemoryDomain domain, uint domain_offset, uint address, IReadOnlyList<uint> offsets, AddressSize addressSize, WatchSize size, WatchDisplayType type, bool bigEndian, string note = "", long value = 0, long prev = 0, int changeCount = 0)
        {
            return size switch
            {
                WatchSize.Separator => SeparatorPointerWatch.NewSeparatorWatch(note),
                WatchSize.Byte => new BytePointerWatch(domain, domain_offset, address, offsets, addressSize, type, bigEndian, note, (byte)value, (byte)prev, changeCount),
                WatchSize.Word => new WordPointerWatch(domain, domain_offset, address, offsets, addressSize, type, bigEndian, note, (ushort)value, (ushort)prev, changeCount),
                WatchSize.DWord => new DWordPointerWatch(domain, domain_offset, address, offsets, addressSize, type, bigEndian, note, (uint)value, (uint)prev, changeCount),
                _ => SeparatorPointerWatch.NewSeparatorWatch(note),
            };
        }

        public uint DomainOffset { get; }

        public uint Address { get; }

        private string AddressFormatStr => _domain != null
            ? $"X{(_domain.Size - 1).NumHexDigits()}"
            : "";

        public string AddressString => Address.ToString(AddressFormatStr);

        public IReadOnlyList<uint> Offsets { get; }

        public AddressSize AddressSize { get; }

        public WatchSize Size { get; }

        public bool BigEndian { get; set; }

        public string Notes { get; set; }

        public int ChangeCount { get; protected set; }

        public bool IsSeparator => Size is WatchSize.Separator;

        public WatchDisplayType Type
        {
            get => _type;
            set
            {
                if (IsDisplayTypeAvailable(value))
                {
                    _type = value;
                }
                else
                {
                    throw new ArgumentException(message: $"WatchDisplayType {value} is invalid for this type of Watch", paramName: nameof(value));
                }
            }
        }

        public MemoryDomain Domain
        {
            get => _domain;
            internal set
            {
                if (value != null && _domain.Name == value.Name)
                {
                    _domain = value;
                }
                else
                {
                    throw new InvalidOperationException("You cannot set a different domain to a watch on the fly");
                }
            }
        }

        public virtual string ToDisplayString() => $"{Notes}: {ValueString}";

        public abstract string Diff { get; }

        public abstract uint MaxValue { get; }

        public abstract uint Value { get; }

        public abstract string ValueString { get; }

        public abstract uint Previous { get; }

        public abstract string PreviousString { get; }

        public abstract bool IsValid { get; }

        public abstract bool Poke(string value);

        public abstract IReadOnlyList<WatchDisplayType> AvailableTypes();

        public abstract void ResetPrevious();

        public abstract void UpdateValue(PreviousType previousType);

        public uint TargetAddress => IsValid ? _addresses.Count != 0 ? _addresses[_addresses.Count - 1] : Address : 0;

        public string TargetAddressString => TargetAddress.ToString(AddressFormatStr);

        public static string DisplayTypeToString(WatchDisplayType type)
        {
            return type switch
            {
                WatchDisplayType.FixedPoint_12_4 => "Fixed Point 12.4",
                WatchDisplayType.FixedPoint_20_12 => "Fixed Point 20.12",
                WatchDisplayType.FixedPoint_16_16 => "Fixed Point 16.16",
                _ => type.ToString(),
            };
        }

        public static WatchDisplayType StringToDisplayType(string name)
        {
            return name switch
            {
                "Fixed Point 12.4" => WatchDisplayType.FixedPoint_12_4,
                "Fixed Point 20.12" => WatchDisplayType.FixedPoint_20_12,
                "Fixed Point 16.16" => WatchDisplayType.FixedPoint_16_16,
                _ => (WatchDisplayType)Enum.Parse(typeof(WatchDisplayType), name),
            };
        }

        public char SizeAsChar
        {
            get
            {
                return Size switch
                {
                    WatchSize.Separator => 'S',
                    WatchSize.Byte => 'b',
                    WatchSize.Word => 'w',
                    WatchSize.DWord => 'd',
                    _ => 'S',
                };
            }
        }

        public static WatchSize SizeFromChar(char c)
        {
            return c switch
            {
                'S' => WatchSize.Separator,
                'b' => WatchSize.Byte,
                'w' => WatchSize.Word,
                'd' => WatchSize.DWord,
                _ => WatchSize.Separator,
            };
        }

        public char TypeAsChar
        {
            get
            {
                return Type switch
                {
                    WatchDisplayType.Separator => '_',
                    WatchDisplayType.Unsigned => 'u',
                    WatchDisplayType.Signed => 's',
                    WatchDisplayType.Hex => 'h',
                    WatchDisplayType.Binary => 'b',
                    WatchDisplayType.FixedPoint_12_4 => '1',
                    WatchDisplayType.FixedPoint_20_12 => '2',
                    WatchDisplayType.FixedPoint_16_16 => '3',
                    WatchDisplayType.Float => 'f',
                    _ => '_',
                };
            }
        }

        public static WatchDisplayType DisplayTypeFromChar(char c)
        {
            return c switch
            {
                '_' => WatchDisplayType.Separator,
                'u' => WatchDisplayType.Unsigned,
                's' => WatchDisplayType.Signed,
                'h' => WatchDisplayType.Hex,
                'b' => WatchDisplayType.Binary,
                '1' => WatchDisplayType.FixedPoint_12_4,
                '2' => WatchDisplayType.FixedPoint_20_12,
                '3' => WatchDisplayType.FixedPoint_16_16,
                'f' => WatchDisplayType.Float,
                _ => WatchDisplayType.Separator,
            };
        }

        public bool IsSplittable => Size is WatchSize.Word or WatchSize.DWord
            && Type is WatchDisplayType.Hex or WatchDisplayType.Binary;

        public void Update(PreviousType previous)
        {
            if (IsValid)
            {
                uint cur_address = Address;
                for (var i = 0; i < Offsets.Count; i++)
                {
                    try
                    {
                        cur_address = MemoryHelper.ReadMemory(Domain, cur_address, AddressSize) + Offsets[i] - DomainOffset;
                        _addresses[i] = cur_address;
                    }
                    catch { }
                }
                UpdateValue(previous);
            }
        }

        protected byte GetByte()
        {
            return IsValid
                ? _domain.PeekByte(TargetAddress)
                : (byte)0;
        }

        protected ushort GetWord()
        {
            return IsValid
                ? _domain.PeekUshort(TargetAddress, BigEndian)
                : (ushort)0;
        }

        protected uint GetDWord()
        {
            return IsValid
                ? _domain.PeekUint(TargetAddress, BigEndian)
                : 0;
        }

        protected void PokeByte(byte val)
        {
            if (IsValid)
            {
                _domain.PokeByte(TargetAddress, val);
            }
        }

        protected void PokeWord(ushort val)
        {
            if (IsValid)
            {
                _domain.PokeUshort(TargetAddress, val, BigEndian);
            }
        }

        protected void PokeDWord(uint val)
        {
            if (IsValid)
            {
                _domain.PokeUint(TargetAddress, val, BigEndian);
            }
        }

        public void ClearChangeCount()
        {
            ChangeCount = 0;
        }

        public bool IsDisplayTypeAvailable(WatchDisplayType type)
        {
            return AvailableTypes().Any(d => d == type);
        }

        public string ToPointerDisplayString()
        {
            if (Offsets.Count == 0)
            {
                return $"[{AddressString}]=>{ValueString}";
            }
            var o = $"[{AddressString}]=>";
            for (int i = 0; i < _addresses.Count; i++)
            {
                var offset = Offsets[i];
                var address = (_addresses[i] - offset).ToString(AddressFormatStr);
                o += $"{address}\n[{address}+{offset.ToString("X")}]=>";
            }
            return o + ValueString;
        }

        public override string ToString()
        {
            var o = $"{(Domain == null && Address == 0 ? "0" : Address.ToHexString((Domain?.Size ?? 0xFF - 1).NumHexDigits()))}\t";
            for (int i = 0; i < Offsets.Count; i++)
            {
                o += Offsets[i].ToHexString(((long)Offsets[i]).NumHexDigits()) + "\t";
            }
            o += $"{SizeAsChar}\t{TypeAsChar}\t{Convert.ToInt32(BigEndian)}\t{Domain?.Name}\t{Notes.Trim('\r', '\n')}";
            return o;
        }

        public bool Equals(PointerWatch other) => other != null && other._domain == _domain && other.Address == Address && Size == other.Size;
        
        public int CompareTo(PointerWatch other)
        {
            if (_domain != other._domain)
            {
                throw new InvalidOperationException("Watch cannot be compared through different domain");
            }

            if (Equals(other))
            {
                return 0;
            }

            if (Address.Equals(other.Address))
            {
                return ((int)Size).CompareTo((int)other.Size);
            }

            return Address.CompareTo(other.Address);
        }

        public override bool Equals(object obj)
        {
            if (obj is PointerWatch)
            {
                return Equals((PointerWatch)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Domain.GetHashCode() + (int)Address;
        }
    }
}
