using System.Collections.Generic;

namespace PointerScan.Util.Watches.WatchList
{
    public sealed partial class PointerWatchList
    {
        private class PointerWatchTargetAddressComparer : PointerWatchEqualityComparer, IComparer<PointerWatch>
        {
            public int Compare(PointerWatch x, PointerWatch y)
            {
                if (Equals(x, y))
                {
                    return 0;
                }

                if (x.TargetAddress.Equals(y.TargetAddress))
                {
                    if (x.Domain.Name.Equals(y.Domain.Name, System.StringComparison.Ordinal))
                    {
                        return x.Size.CompareTo(y.Size);
                    }

                    return string.CompareOrdinal(x.Domain.Name, y.Domain.Name);
                }

                return x.TargetAddress.CompareTo(y.TargetAddress);
            }
        }
    }
}
