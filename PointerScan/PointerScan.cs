using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common.CollectionExtensions;
using BizHawk.Emulation.Common;
using PointerScan.Util;
using PointerScan.Util.Watches;
using PointerScan.Util.Watches.WatchList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Pointer = PointerScan.Util.Pointer;

namespace PointerScan
{
    [ExternalTool("PointerScan")]
    public partial class PointerScan : ToolFormBase, IExternalToolForm
    {
        protected override string WindowTitleStatic => Messages.AppName;

        private ApiContainer? _apiContainer;
        public ApiContainer? APIContainer
        {
            get => _apiContainer;
            set
            {
                _apiContainer = value;
                if (value != null)
                {
                    InitAPIContainer();
                }
            }
        }

        [RequiredService]
        private IMemoryDomains? _memoryDomains { get; set; }

        private IMemoryDomains MemoryDomains
            => _memoryDomains!;

        private bool _romLoaded;
        public bool IsRomLoaded
        {
            get => _romLoaded;
            private set
            {
                _romLoaded = value;
                RomLoadedUpdated();
            }
        }

        public FileVersionInfo PointerScanVersion { get; }

        private readonly Dictionary<uint, Pointer> Pointers = new Dictionary<uint, Pointer>();
        private PointerWatchList _watches;
        private IEnumerable<int> SelectedIndices => WatchListView.SelectedRows;
        private IEnumerable<PointerWatch> SelectedItems => SelectedIndices.Select(index => _watches[index]);
        private IEnumerable<PointerWatch> SelectedWatches => SelectedItems.Where(x => !x.IsSeparator);
        private IEnumerable<PointerWatch> SelectedSeparators => SelectedItems.Where(x => x.IsSeparator);

        private uint Target = 0;
        private AddressSize AddressSize = 0;
        private WatchSize ValueSize = 0;
        private uint MaxDepth = 0;
        private uint MaxOffset = 0;
        private uint RAMOffset = 0;
        private bool AlignOffset = false;

        private string _sortedColumn;
        private bool _sortReverse;

        public enum ToolState
        {
            ERROR,
            READY,
            FIRST_SCAN,
            ACTIVE,
            RESCANNING,
            IDLE,
        }
        private ToolState state = ToolState.ERROR;
        public ToolState State
        {
            get => state;
            private set
            {
                OldState = state;
                state = value;
            }
        }
        public ToolState OldState { get; private set; } = ToolState.ERROR;

        //[ConfigPersist]
        public PointerScanSettings Settings { get; set; }

        public class PointerScanSettings
        {
            public PointerScanSettings()
            {
                Columns = new List<RollColumn>
                {
                    new RollColumn { Name = PointerWatchList.Address, Text = "Address", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset1, Text = "Offset 1", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset2, Text = "Offset 2", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset3, Text = "Offset 3", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset4, Text = "Offset 4", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset5, Text = "Offset 5", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Offset6, Text = "Offset 6", UnscaledWidth = 60},
                    new RollColumn { Name = PointerWatchList.Value, Text = "Value", UnscaledWidth = 100},
                    new RollColumn { Name = PointerWatchList.Prev, Text = "Prev", UnscaledWidth = 60, Visible = false},
                    new RollColumn { Name = PointerWatchList.ChangesCol, Text = "Changes", UnscaledWidth = 60, Visible = false},
                };
            }

            public List<RollColumn> Columns { get; set; }
            public bool DoubleClickToPoke { get; set; } = true;
        }

        protected override void GeneralUpdate() => FrameUpdate();

        public PointerScan()
        {
            InitializeComponent();
            WatchListView.QueryItemBkColor += WatchListView_QueryItemBkColor;
            WatchListView.QueryItemText += WatchListView_QueryItemText;
            WatchListView.ColumnClick += WatchListView_ColumnClick;
            WatchListView.SelectedIndexChanged += WatchListView_SelectedIndexChanged;

            PointerScanVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            VersionLabel.Text = $"{WindowTitleStatic} v{PointerScanVersion.ProductVersion.Substring(0, 5)}";

            Settings = new PointerScanSettings();

            WatchListView.AllColumns.AddRange(Settings.Columns);
            WatchListView.Refresh();
        }

        private void InitAPIContainer()
        {
        }

        private void RomLoadedUpdated()
        {
            StartButton.Enabled = IsRomLoaded;
            StartButton.Text = Messages.StartButton_Start;
            AutoButton.Enabled = false;
            ResetButton.Enabled = false;
            Pointers.Clear();
        }

        public override void Restart()
        {
            IsRomLoaded = false;
            try
            {
                if (APIContainer != null)
                {
                    if (_watches != null && _watches.All(w => w.Domain == null || MemoryDomains.Select(m => m.Name).Contains(w.Domain.Name)))
                    {
                        _watches.RefreshDomains(MemoryDomains, Config.RamWatchDefinePrevious);
                        //_watches.Reload();

                    }
                    else
                    {
                        _watches = new PointerWatchList(MemoryDomains, APIContainer.Emulation.GetSystemId());
                        //NewWatchList(true);
                    }
                    var gameInfo = APIContainer.Emulation.GetGameInfo();
                    if (gameInfo != null)
                    {
                        var romName = gameInfo.Name;
                        var isEmpty = string.IsNullOrEmpty(romName) || romName == "Null";
                        if (isEmpty)
                        {
                            State = ToolState.ERROR;
                            SetStatus(Messages.StatusLabel_NoROM, Color.Red);
                        }
                        else
                        {
                            SetStatus(romName);
                            IsRomLoaded = true;
                            State = ToolState.READY;
                        }
                    }
                }
            }
            catch (Exception)
            {
                IsRomLoaded = false;
                State = ToolState.ERROR;
                SetStatus(Messages.StatusLabel_NoROM, Color.Red);
            }
        }

        public override void UpdateValues(ToolFormUpdateType type)
        {
            switch (type)
            {
                case ToolFormUpdateType.PostFrame:
                case ToolFormUpdateType.General:
                    FrameUpdate();
                    break;
                case ToolFormUpdateType.FastPostFrame:
                    MinimalUpdate();
                    break;
            }
        }

        private void FrameUpdate()
        {
            if (_watches.Count is not 0)
            {
                if (State == ToolState.ACTIVE)
                {
                    _watches.UpdateValues(Config.RamWatchDefinePrevious);
                    Clean();
                }
                WatchListView.RowCount = _watches.Count;
            }
        }

        private void MinimalUpdate()
        {
            if (_watches.Count is not 0)
            {
                if (State == ToolState.ACTIVE)
                {
                    _watches.UpdateValues(Config.RamWatchDefinePrevious);
                    Clean();
                }
            }
        }

        private void SetStatus(string message, Color? color = null)
        {
            StatusLabel.Text = message;
            StatusLabel.ForeColor = color ?? Color.Black;
        }

        public void FirstScan()
        {
            if (APIContainer != null && IsRomLoaded)
            {
                Thread.Sleep(100);
                uint addr_size_uint = (uint)AddressSize;
                var domain = MemoryDomains.MainMemory;
                var size = (uint)domain.Size;
                Pointers.Clear();
                progressBar1.Visible = true;
                StartButton.Enabled = false;
                List<uint> addresses_to_check = new List<uint>() { Target };
                ulong progress = 0;
                Dictionary<uint, uint> memoryMap = new Dictionary<uint, uint>();
                for (uint i = 0; i <= size - addr_size_uint; i += addr_size_uint)
                {
                    memoryMap[i] = MemoryHelper.ReadMemory(domain, i, AddressSize);
                }
                ulong max_progress = (ulong)(MaxDepth * memoryMap.LongCount());
                for (uint d = 0; d < MaxDepth; d++)
                {
                    List<uint> next_addresses_to_check = new List<uint>() { };
                    foreach (uint i in memoryMap.Keys)
                    {
                        progress++;
                        progressBar1.Value = (int)((progress / (double)max_progress) * progressBar1.Maximum);
                        progressBar1.Update();
                        var value = memoryMap[i] - RAMOffset;
                        foreach (uint addr in addresses_to_check)
                        {
                            if (addr >= value && addr - value < MaxOffset && (!AlignOffset || (addr - value) % addr_size_uint == 0))
                            {
                                Pointer p;
                                if (!Pointers.ContainsKey(i))
                                {
                                    p = new Pointer(i, AddressSize);
                                    Pointers.Add(i, p);
                                    next_addresses_to_check.Add(i);
                                }
                                else
                                {
                                    p = Pointers[i];
                                }
                                Pointer? t = null;
                                if (Pointers.ContainsKey(addr))
                                {
                                    t = Pointers[addr];
                                }
                                var o = addr - value;
                                if (!p.OffsetMap.ContainsKey(o))
                                    p.OffsetMap.Add(addr - value, t);
                                break;
                            }
                        }
                    }
                    addresses_to_check = next_addresses_to_check;
                }

                foreach (uint addr in Pointers.Keys)
                {
                    ResolvePointer(Pointers[addr]);
                }

                ResultsLabel.Text = string.Format(Messages.ResultsLabel_Template, _watches.Count);
                State = ToolState.IDLE;
                progressBar1.Visible = false;
                AddressSizeOptions.Enabled = false;
                ResetButton.Enabled = true;
                AutoButton.Enabled = true;
                StartButton.Enabled = true;
                StartButton.Text = Messages.StartButton_Rescan;
                _watches.UpdateValues(Config.RamWatchDefinePrevious);
                WatchListView.Refresh();
            }
        }

        private void ResolvePointer(Pointer pointer)
        {
            var offsets = new List<uint>();
            var addresses = new List<uint>();
            void Step(Pointer pointer, uint start_addr, List<uint> offsets, List<uint> addresses, uint depth)
            {
                var p = offsets.Count > 0 ? pointer.OffsetMap[offsets[offsets.Count - 1]] : pointer;
                if (p == null)
                {
                    _watches.Add(PointerWatch.GenerateWatch(MemoryDomains.MainMemory, RAMOffset, start_addr, offsets, AddressSize, ValueSize, WatchDisplayType.Hex, MemoryDomains.MainMemory.EndianType == MemoryDomain.Endian.Big));
                    return;
                }
                else if (addresses.Contains(p.Address))
                {
                    return;
                }
                else if (depth < MaxDepth)
                {
                    foreach (uint o in p.OffsetMap.Keys)
                    {
                        List<uint> list = new List<uint>();
                        list.AddRange(offsets);
                        list.Add(o);
                        List<uint> list2 = new List<uint>();
                        list2.AddRange(addresses);
                        list2.Add(p.Address);
                        Step(p, start_addr, list, list2, depth + 1);
                    }
                }
            }
            Step(pointer, pointer.Address, offsets, addresses, 0);
        }

        public void Rescan()
        {
            if (APIContainer != null && IsRomLoaded)
            {
                progressBar1.Visible = true;
                var domain = MemoryDomains.MainMemory;
                uint addr_size_uint = (uint)AddressSize;
                var address_format = "X" + (addr_size_uint * 2);
                _watches.UpdateValues(Config.RamWatchDefinePrevious);
                Clean();
                WatchListView.Refresh();
                State = AutoButton.Checked? ToolState.ACTIVE : ToolState.IDLE;
                progressBar1.Visible = false;
            }
        }

        public void Clean()
        {
            if (APIContainer != null)
            {
                progressBar1.Visible = true;
                StartButton.Enabled = false;
                ResetButton.Enabled = false;
                _watches.RemoveAll(w => !w.IsValid || w.TargetAddress != Target);
                ResultsLabel.Text = string.Format(Messages.ResultsLabel_Template, _watches.Count);
                State = ToolState.ACTIVE;
                progressBar1.Visible = false;
                StartButton.Enabled = true;
                ResetButton.Enabled = true;
                WatchListView.Refresh();
            }
        }

        private void OrderColumn(RollColumn column)
        {
            if (column.Name != _sortedColumn)
            {
                _sortReverse = false;
            }

            _watches.OrderWatches(column.Name, _sortReverse);

            _sortedColumn = column.Name;
            _sortReverse = !_sortReverse;
            WatchListView.Refresh();
        }

        private string ComputeDisplayType(PointerWatch w)
        {
            string s = w.Size == WatchSize.Byte ? "1" : (w.Size == WatchSize.Word ? "2" : "4");
            switch (w.Type)
            {
                case WatchDisplayType.Binary:
                    s += "b";
                    break;
                case WatchDisplayType.FixedPoint_12_4:
                    s += "F";
                    break;
                case WatchDisplayType.FixedPoint_16_16:
                    s += "F6";
                    break;
                case WatchDisplayType.FixedPoint_20_12:
                    s += "F2";
                    break;
                case WatchDisplayType.Float:
                    s += "f";
                    break;
                case WatchDisplayType.Hex:
                    s += "h";
                    break;
                case WatchDisplayType.Signed:
                    s += "s";
                    break;
                case WatchDisplayType.Unsigned:
                    s += "u";
                    break;
            }

            return s + (w.BigEndian ? "B" : "L");
        }

        private void HexTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (APIContainer != null)
            {
                if (IsRomLoaded)
                {
                    if (!uint.TryParse(AddressTextBox.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out Target))
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorAddress;
                        return;
                    }
                    if (!uint.TryParse(MaxOffsetTextBox.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out MaxOffset))
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorMaxOffset;
                        return;
                    }
                    if (MaxOffset > APIContainer.Memory.GetMemoryDomainSize(APIContainer.Memory.MainMemoryName))
                    {
                        MaxOffset = APIContainer.Memory.GetMemoryDomainSize(APIContainer.Memory.MainMemoryName);
                    }
                    if (!uint.TryParse(RAMOffsetTextBox.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out RAMOffset))
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorRAMOffset;
                        return;
                    }
                    MaxDepth = (uint)MaxDepthUpDown.Value;
                    AddressSize = AddressSize.None;
                    switch (AddressSizeOptions.SelectedIndex)
                    {
                        case 0:
                            AddressSize = AddressSize.ONE;
                            break;
                        case 1:
                            AddressSize = AddressSize.TWO;
                            break;
                        case 2:
                            AddressSize = AddressSize.FOUR;
                            break;
                    }
                    if (AddressSize == AddressSize.None)
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorAddressSize;
                        return;
                    }
                    ValueSize = WatchSize.Separator;
                    switch (ValueSizeOptions.SelectedIndex)
                    {
                        case 0:
                            ValueSize = WatchSize.Byte;
                            break;
                        case 1:
                            ValueSize = WatchSize.Word;
                            break;
                        case 2:
                            ValueSize = WatchSize.DWord;
                            break;
                    }
                    if (ValueSize == WatchSize.Separator)
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorAddressSize;
                        return;
                    }
                    AlignOffset = AlignToggle.Checked;
                    State = State switch
                    {
                        ToolState.READY => ToolState.FIRST_SCAN,
                        ToolState.ACTIVE => ToolState.RESCANNING,
                        ToolState.IDLE => ToolState.RESCANNING,
                        _ => throw new InvalidEnumArgumentException("Invalid State for start button action!"),
                    };
                    if (State == ToolState.FIRST_SCAN)
                    {
                        FirstScan();
                    }
                    if (State == ToolState.RESCANNING)
                    {
                        Rescan();
                    }
                    if (APIContainer.EmuClient.IsPaused())
                    {
                        APIContainer.EmuClient.DoFrameAdvance();
                    }
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (APIContainer != null)
            {
                if (IsRomLoaded)
                {
                    State = ToolState.READY;
                    ResultsLabel.Text = "";
                    PointerDataLabel.Text = "";
                    _watches.Clear();
                    WatchListView.Refresh();
                    ResetButton.Enabled = false;
                    StartButton.Enabled = true;
                    StartButton.Text = Messages.StartButton_Start;
                    AutoButton.Enabled = false;
                    AutoButton.Checked = false;
                    AddressSizeOptions.Enabled = true;
                }
            }
        }

        private void AutoButton_Click(object sender, EventArgs e)
        {
            if (APIContainer != null)
            {
                State = AutoButton.Checked ? ToolState.ACTIVE : ToolState.IDLE;
            }
        }

        private void ValueSizeOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var watches = _watches.ToList();
            _watches.Clear();
            ValueSize = WatchSize.Separator;
            switch (ValueSizeOptions.SelectedIndex)
            {
                case 0:
                    ValueSize = WatchSize.Byte;
                    break;
                case 1:
                    ValueSize = WatchSize.Word;
                    break;
                case 2:
                    ValueSize = WatchSize.DWord;
                    break;
            }
            if (ValueSize == WatchSize.Separator)
            {
                ResultsLabel.Text = Messages.ResultsLabel_ErrorAddressSize;
                return;
            }
            foreach (PointerWatch w in watches)
            {
                _watches.Add(PointerWatch.GenerateWatch(
                    w.Domain, RAMOffset, w.Address, w.Offsets, AddressSize, ValueSize, WatchDisplayType.Hex, w.BigEndian, w.Notes, w.Value, w.Previous, w.ChangeCount));
            }
            _watches.UpdateValues(Config.RamWatchDefinePrevious);

        }

        private void WatchListView_QueryItemBkColor(int index, RollColumn column, ref Color color)
        {
            if (index >= _watches.Count)
            {
                return;
            }

            if (_watches[index].IsSeparator)
            {
                color = BackColor;
            }
            else if (!_watches[index].IsValid)
            {
                color = Color.PeachPuff;
            }
            else if (_watches[index].TargetAddress != Target)
            {
                color = Color.Pink;
            }
            else if (MainForm.CheatList.IsActive(_watches[index].Domain, _watches[index].Address))
            {
                color = Color.LightCyan;
            }
        }

        private void WatchListView_QueryItemText(int index, RollColumn column, out string text, ref int offsetX, ref int offsetY)
        {
            text = "";
            if (index >= _watches.Count)
            {
                return;
            }
            var watch = _watches[index];

            if (watch.IsSeparator)
            {
                if (column.Name == PointerWatchList.Address)
                {
                    text = watch.Notes;
                }

                return;
            }

            switch (column.Name)
            {
                case PointerWatchList.Address:
                    text = watch.AddressString;
                    break;
                case PointerWatchList.Offset1:
                    if (watch.Offsets.Count > 0)
                        text = watch.Offsets[0].ToString("X");
                    break;
                case PointerWatchList.Offset2:
                    if (watch.Offsets.Count > 1)
                        text = watch.Offsets[1].ToString("X");
                    break;
                case PointerWatchList.Offset3:
                    if (watch.Offsets.Count > 2)
                        text = watch.Offsets[2].ToString("X");
                    break;
                case PointerWatchList.Offset4:
                    if (watch.Offsets.Count > 3)
                        text = watch.Offsets[3].ToString("X");
                    break;
                case PointerWatchList.Offset5:
                    if (watch.Offsets.Count > 4)
                        text = watch.Offsets[4].ToString("X");
                    break;
                case PointerWatchList.Offset6:
                    if (watch.Offsets.Count > 5)
                        text = watch.Offsets[5].ToString("X");
                    break;
                case PointerWatchList.Value:
                    text = watch.TargetAddressString + "=>" + watch.ValueString;
                    break;
                case PointerWatchList.Prev:
                    text = watch.PreviousString;
                    break;
                case PointerWatchList.ChangesCol:
                    if (!watch.IsSeparator)
                    {
                        text = watch.ChangeCount.ToString();
                    }

                    break;
                case PointerWatchList.Diff:
                    text = watch.Diff;
                    break;
                case PointerWatchList.Type:
                    text = ComputeDisplayType(watch);
                    break;
                case PointerWatchList.Domain:
                    text = watch.Domain.Name;
                    break;
                case PointerWatchList.Notes:
                    text = watch.Notes;
                    break;
            }
        }

        private void WatchListView_ColumnClick(object sender, InputRoll.ColumnClickEventArgs e)
            => OrderColumn(e.Column!);

        private void WatchListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WatchListView.SelectedRows.Count() == 1)
            {
                PointerDataLabel.Text = _watches[WatchListView.SelectedRows.First()].ToPointerDisplayString();
            }
            else
            {
                PointerDataLabel.Text = "";
            }
        }
    }
}
