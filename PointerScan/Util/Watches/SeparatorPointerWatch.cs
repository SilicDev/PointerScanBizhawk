using BizHawk.Client.Common;
using System.Collections.Generic;

namespace PointerScan.Util.Watches
{
    public sealed class SeparatorPointerWatch : PointerWatch
    {
        public static readonly IReadOnlyList<WatchDisplayType> ValidTypes = new List<WatchDisplayType>() { WatchDisplayType.Separator };

        internal SeparatorPointerWatch() : base(null, 0, 0, null, AddressSize.None, WatchSize.Separator, WatchDisplayType.Separator, true, "")
        {

        }

        public static SeparatorPointerWatch Instance => new SeparatorPointerWatch();

        public static SeparatorPointerWatch NewSeparatorWatch(string note)
        {
            return new SeparatorPointerWatch()
            {
                Notes = note
            };
        }

        public override string Diff => "";

        public override uint MaxValue => 0;

        public override uint Value => 0;

        public override string ValueString => "";

        public override uint Previous => 0;

        public override string PreviousString => "";

        public override bool IsValid => true;

        public override IReadOnlyList<WatchDisplayType> AvailableTypes() => ValidTypes;

        public override bool Poke(string value) => false;

        public override void ResetPrevious()
        {
        }

        public override void UpdateValue(PreviousType previousType)
        {
        }
    }
}
