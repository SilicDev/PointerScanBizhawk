using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PointerScan.Util.Watches.WatchList
{
    public sealed partial class PointerWatchList : IList<PointerWatch>
    {
        public const string Address = "AddressColumn";
        public const string Offset1 = "Offset1Column";
        public const string Offset2 = "Offset2Column";
        public const string Offset3 = "Offset3Column";
        public const string Offset4 = "Offset4Column";
        public const string Offset5 = "Offset5Column";
        public const string Offset6 = "Offset6Column";
        public const string Value = "ValueColumn";
        public const string Prev = "PrevColumn";
        public const string ChangesCol = "ChangesColumn";
        public const string Diff = "DiffColumn";
        public const string Type = "TypeColumn";
        public const string Domain = "DomainColumn";
        public const string Notes = "NotesColumn";

        private static readonly Dictionary<string, IComparer<PointerWatch>> WatchComparers;

        private readonly List<PointerWatch> _watchList = new List<PointerWatch>(0);
        private readonly string _systemId;
        private IMemoryDomains _memoryDomains;

        static PointerWatchList()
        {
            WatchComparers = new Dictionary<string, IComparer<PointerWatch>>
            {
                [Address] = new PointerWatchAddressComparer(),
                [Offset1] = new PointerWatchOffsetComparer(0),
                [Offset2] = new PointerWatchOffsetComparer(1),
                [Offset3] = new PointerWatchOffsetComparer(2),
                [Offset4] = new PointerWatchOffsetComparer(3),
                [Offset5] = new PointerWatchOffsetComparer(4),
                [Offset6] = new PointerWatchOffsetComparer(5),
                [Value] = new PointerWatchTargetAddressComparer()
            };
        }

        public int Count => _watchList.Count;
        public bool IsReadOnly => false;
        public PointerWatch this[int index]
        {
            get => _watchList[index];
            set => _watchList[index] = value;
        }
        public bool Changes { get; set; }
        public string CurrentFileName { get; set; } = string.Empty;
        public int WatchCount => _watchList.Count(watch => !watch.IsSeparator);

        public PointerWatchList(IMemoryDomains core, string systemId)
        {
            _memoryDomains = core;
            _systemId = systemId;
        }

        public void Add(PointerWatch watch)
        {
            _watchList.Add(watch);
            Changes = true;
        }
        public void Clear()
        {
            _watchList.Clear();
            Changes = false;
            CurrentFileName = "";
        }

        public bool Contains(PointerWatch watch)
        {
            return _watchList.Contains(watch);
        }

        public void CopyTo(PointerWatch[] array, int arrayIndex)
        {
            _watchList.CopyTo(array, arrayIndex);
        }

        public bool Remove(PointerWatch watch)
        {
            bool result = _watchList.Remove(watch);
            if (result)
            {
                Changes = true;
            }

            return result;
        }

        public int IndexOf(PointerWatch watch)
        {
            return _watchList.IndexOf(watch);
        }

        public void Insert(int index, PointerWatch watch)
        {
            _watchList.Insert(index, watch);
            Changes = true;
        }

        public void InsertRange(int index, IEnumerable<PointerWatch> collection)
        {
#if NET6_0_OR_GREATER
			if (collection.TryGetNonEnumeratedCount(out var n) && n is 0) return;
#else
            if (collection is ICollection<PointerWatch> hasCount && hasCount.Count is 0) return;
#endif
            _watchList.InsertRange(index, collection);
            Changes = true;
        }

        public void RemoveAt(int index)
        {
            _watchList.RemoveAt(index);
            Changes = true;
        }

        public IEnumerator<PointerWatch> GetEnumerator()
        {
            return _watchList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        { 
            return GetEnumerator();
        }

        public void AddRange(IEnumerable<PointerWatch> watches)
        {
            foreach (var watch in watches)
            {
                _watchList.Add(watch);
            }
            Changes = true;
        }

        public void ClearChangeCounts()
        {
            foreach (var watch in _watchList) watch.ClearChangeCount();
        }


        public void RefreshDomains(IMemoryDomains core, PreviousType previousType)
        {
            _memoryDomains = core;
            foreach (var watch in _watchList)
            {
                if (watch.IsSeparator)
                {
                    return;
                }

                watch.Domain = core[watch.Domain.Name];
                watch.ResetPrevious();
                watch.Update(previousType);
                watch.ClearChangeCount();
            }
        }

        public void UpdateValues(PreviousType previousType)
        {
            foreach (var watch in _watchList)
            {
                watch.Update(previousType);
            }
        }


        public void OrderWatches(string column, bool reverse)
        {
            if (WatchComparers.ContainsKey(column))
            {
                var separatorIndices = new List<int>();
                for (var i = 0; i < _watchList.Count; i++)
                {
                    if (_watchList[i].IsSeparator)
                    {
                        separatorIndices.Add(i);
                    }
                }
                separatorIndices.Add(_watchList.Count);

                // Sort "blocks" of addresses between separators.
                int startIndex = 0;
                foreach (int index in separatorIndices)
                {
                    _watchList.Sort(startIndex, index - startIndex, WatchComparers[column]);
                    if (reverse)
                    {
                        _watchList.Reverse(startIndex, index - startIndex);
                    }
                    startIndex = index + 1;
                }
                Changes = true;
            }

        }

    }
}
