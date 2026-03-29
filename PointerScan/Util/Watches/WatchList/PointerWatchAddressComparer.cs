using System;
using System.Collections.Generic;

namespace PointerScan.Util.Watches.WatchList
{
    public sealed partial class PointerWatchList
    {
        private class PointerWatchAddressComparer : PointerWatchEqualityComparer, IComparer<PointerWatch>
        {
            public virtual int Compare(PointerWatch x, PointerWatch y)
            {
                if (Equals(x, y))
                {
                    return 0;
                }

                if (x.Address.Equals(y.Address))
                {
                    if (x.Domain.Name.Equals(y.Domain.Name, System.StringComparison.Ordinal))
                    {
                        return x.Size.CompareTo(y.Size);
                    }

                    return string.CompareOrdinal(x.Domain.Name, y.Domain.Name);
                }

                return x.Address.CompareTo(y.Address);
            }
        }
    }
}
