using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerScan.Util.Watches.WatchList
{
    public sealed partial class PointerWatchList
    {
        private class PointerWatchEqualityComparer : IEqualityComparer<PointerWatch>
        {
            public bool Equals(PointerWatch x, PointerWatch y)
            {
                if (x is null)
                {
                    return y is null;
                }

                if (y is null)
                {
                    return false;
                }

                return ReferenceEquals(x, y);
            }

            public int GetHashCode(PointerWatch obj) => obj.GetHashCode();
        }
    }
}
