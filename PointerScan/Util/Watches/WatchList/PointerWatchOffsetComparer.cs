using System.Collections.Generic;

namespace PointerScan.Util.Watches.WatchList
{
    public sealed partial class PointerWatchList
    {
        private class PointerWatchOffsetComparer : PointerWatchAddressComparer, IComparer<PointerWatch>
        {
            private readonly int _offset_idx = 0;

            public PointerWatchOffsetComparer(int offset_idx)
            {
                _offset_idx = offset_idx;
            }

            public override int Compare(PointerWatch x, PointerWatch y)
            {
                if (Equals(x, y))
                {
                    return 0;
                }

                for (int idx = _offset_idx; idx >= 0; idx--)
                {
                    if (idx >= x.Offsets.Count)
                    {
                        if (idx >= y.Offsets.Count)
                        {
                            continue;
                        }

                        return -1;
                    }

                    if (idx >= y.Offsets.Count)
                    {
                        return 1;
                    }

                    if (x.Offsets[idx].Equals(y.Offsets[idx]))
                    {
                        continue;
                    }

                    return x.Offsets[idx].CompareTo(y.Offsets[idx]);
                }

                return base.Compare(x, y);
            }
        }
    }
}
