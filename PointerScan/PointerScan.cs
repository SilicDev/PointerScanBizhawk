using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common.CollectionExtensions;
using BizHawk.Emulation.Common;
using PointerScan.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly List<PointerChain> PointerChains = new List<PointerChain> { };
        private readonly Dictionary<PointerChain, DataGridViewRow> ChainRowMap = new Dictionary<PointerChain, DataGridViewRow>();

        private uint Target = 0;
        private MemoryHelper.AddressSize AddressSize = 0;
        private uint MaxDepth = 0;
        private uint MaxOffset = 0;
        private uint RAMOffset = 0;
        private bool AlignOffset = false;

        public enum ToolState
        {
            ERROR,
            READY,
            FIRST_SCAN,
            ACTIVE,
            RESCANNING,
            CLEANING,
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

        public PointerScan()
        {
            InitializeComponent();
            PointerTable.SortCompare += PointerTable_SortCompare;
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, PointerTable, new object[] { DoubleBuffered });

            PointerScanVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            VersionLabel.Text = $"{WindowTitleStatic} v{PointerScanVersion.ProductVersion.Substring(0, 5)}";
            PointerTable.Hide();
        }
        private void InitAPIContainer()
        {
        }

        private void RomLoadedUpdated()
        {
            PointerTable.Visible = IsRomLoaded;
            StartButton.Enabled = IsRomLoaded;
            StartButton.Text = Messages.StartButton_Start;
            CleanButton.Enabled = false;
            ResetButton.Enabled = false;
            Pointers.Clear();
            PointerChains.Clear();
            ChainRowMap.Clear();
            PointerTable.Rows.Clear();
        }

        public override void Restart()
        {
            IsRomLoaded = false;
            try
            {
                if (APIContainer != null)
                {
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

        protected override void UpdateAfter()
        {
            if (APIContainer != null && IsRomLoaded)
            {
                switch (State)
                {
                    case ToolState.ERROR:
                    case ToolState.READY:
                        break;
                    case ToolState.FIRST_SCAN:
                        {
                            if (State != OldState)
                            {
                                APIContainer.EmuClient.Pause();
                                var task = Task.Run(FirstScan);
                            }
                            break;
                        }
                    case ToolState.ACTIVE:
                        {
                            // Future? Automatic updating of pointers if low enough amount
                            // Is there a faster way to access the memory? -> RAM Watch?
                            break;
                        }
                    case ToolState.RESCANNING:
                        {
                            if (State != OldState)
                            {
                                APIContainer.EmuClient.Pause();
                                var task = Task.Run(Rescan);
                            }
                            break;
                        }
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
                PointerChains.Clear();
                progressBar1.Visible = true;
                StartButton.Enabled = false;
                List<uint> addresses_to_check = new List<uint>() { Target };
                ulong max_progress = MaxDepth * size / addr_size_uint;
                ulong progress = 0;
                Dictionary<uint, uint> memoryMap = new Dictionary<uint, uint>();
                for (uint i = 0; i < size - addr_size_uint; i += addr_size_uint)
                {
                    memoryMap[i] = MemoryHelper.ReadMemory(APIContainer, AddressSize, i, domain);
                }
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
                    BuildPointerChainsStep(Pointers[addr], addr, new List<uint>(), 0);
                }

                var address_format = "X" + (addr_size_uint * 2);
                foreach (var ptr in PointerChains)
                {
                    List<object> row = new List<object>
                    {
                        string.Format("{0:" + address_format + "}", ptr.StartAddress)
                    };
                    for (int i = 0; i < 6; i++)
                    {
                        if (i < ptr.Offsets.Count)
                        {
                            row.Add(string.Format("{0:X}", ptr.Offsets[i]));
                        }
                        else
                        {
                            row.Add("");
                        }
                    }
                    progressBar1.Value = progressBar1.Maximum;
                    progressBar1.Update();
                    uint final_address = ptr.GetResultAddress(APIContainer, AddressSize, domain, RAMOffset);
                    row.Add(string.Format("{0:" + address_format + "}={1:" + address_format + "}", final_address, MemoryHelper.ReadMemory(APIContainer, AddressSize, final_address, domain.Name, MemoryDomain.Endian.Little)));
                    var idx = PointerTable.Rows.Add(row.ToArray());
                    ChainRowMap.Add(ptr, PointerTable.Rows[idx]);
                }

                ResultsLabel.Text = string.Format(Messages.ResultsLabel_Template, PointerTable.Rows.Count);
                State = ToolState.ACTIVE;
                progressBar1.Visible = false;
                ResetButton.Enabled = true;
                StartButton.Enabled = true;
                StartButton.Text = Messages.StartButton_Rescan;
            }
        }

        private void BuildPointerChainsStep(Pointer pointer, uint start_addr, List<uint> offsets, uint depth)
        {
            var p = offsets.Count > 0 ? pointer.OffsetMap[offsets[offsets.Count - 1]] : pointer;
            if (p == null)
            {
                PointerChains.Add(new PointerChain(start_addr, offsets));
                return;
            }
            else if (depth < MaxDepth)
            {
                foreach (uint o in p.OffsetMap.Keys)
                {
                    List<uint> list = new List<uint>();
                    list.AddRange(offsets);
                    list.Add(o);
                    BuildPointerChainsStep(p, start_addr, list, depth + 1);
                }
            }
        }

        public void Rescan()
        {
            if (APIContainer != null && IsRomLoaded)
            {
                var progress = 0;
                var max_progress = PointerChains.Count;
                progressBar1.Visible = true;
                StartButton.Enabled = false;
                var columns = PointerTable.ColumnCount;
                var domain = MemoryDomains.MainMemory;
                uint addr_size_uint = (uint)AddressSize;
                var address_format = "X" + (addr_size_uint * 2);
                foreach (var chain in PointerChains)
                {
                    progress++;
                    progressBar1.Value = (int)((progress / (double)max_progress) * progressBar1.Maximum);
                    progressBar1.Update();
                    var row = ChainRowMap[chain];
                    var address_text = Messages.PointerTable_Values_Unknown;
                    try
                    {
                        var final_address = chain.GetResultAddress(APIContainer, AddressSize, domain, RAMOffset);
                        if (final_address != Target)
                        {
                            chain.Invalid = true;
                            CleanButton.Enabled = true;
                        }
                        address_text = string.Format("{0:" + address_format + "}={1:" + address_format + "}", final_address, MemoryHelper.ReadMemory(APIContainer, AddressSize, final_address, domain.Name, MemoryDomain.Endian.Little));
                    }
                    catch (OutOfMemoryException)
                    {
                        address_text = Messages.PointerTable_Values_Unknown;
                        chain.Invalid = true;
                        CleanButton.Enabled = true;
                    }
                    row.Cells[columns - 1].Value = address_text;

                }
                PointerTable.Update();
                State = ToolState.ACTIVE;
                StartButton.Enabled = true;
                progressBar1.Visible = false;
            }
        }

        public void Clean()
        {
            if (APIContainer != null)
            {
                progressBar1.Visible = true;
                CleanButton.Enabled = false;
                StartButton.Enabled = false;
                ResetButton.Enabled = false;
                List<PointerChain> chains_to_remove = new List<PointerChain>();
                foreach (var chain in PointerChains)
                {
                    if (chain.Invalid)
                    {
                        PointerTable.Rows.Remove(ChainRowMap[chain]);
                        chains_to_remove.Add(chain);
                    }
                }
                var progress = 0;
                var max_progress = chains_to_remove.Count;
                chains_to_remove.ForEach(ch =>
                {
                    progress++;
                    progressBar1.Value = (int)((progress / (double)max_progress) * progressBar1.Maximum);
                    progressBar1.Update();
                    ChainRowMap.Remove(ch);
                    PointerChains.Remove(ch);
                });
                ResultsLabel.Text = string.Format(Messages.ResultsLabel_Template, PointerTable.Rows.Count);
                State = ToolState.ACTIVE;
                progressBar1.Visible = false;
                StartButton.Enabled = true;
                ResetButton.Enabled = true;
            }
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
                    AddressSize = MemoryHelper.AddressSize.None;
                    AlignOffset = AlignToggle.Checked;
                    switch (AddressSizeOptions.SelectedIndex)
                    {
                        case 0:
                            AddressSize = MemoryHelper.AddressSize.ONE;
                            break;
                        case 1:
                            AddressSize = MemoryHelper.AddressSize.TWO;
                            break;
                        case 2:
                            AddressSize = MemoryHelper.AddressSize.FOUR;
                            break;
                    }
                    if (AddressSize == MemoryHelper.AddressSize.None)
                    {
                        ResultsLabel.Text = Messages.ResultsLabel_ErrorAddressSize;
                        return;
                    }
                    State = State switch
                    {
                        ToolState.READY => ToolState.FIRST_SCAN,
                        ToolState.ACTIVE => ToolState.RESCANNING,
                        _ => throw new InvalidEnumArgumentException("Invalid State for start button action!"),
                    };
                    StartButton.Enabled = false;
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
                    PointerTable.Rows.Clear();
                    ResultsLabel.Text = "";
                    ResetButton.Enabled = false;
                    StartButton.Enabled = true;
                    StartButton.Text = Messages.StartButton_Start;
                    CleanButton.Enabled = false;
                }
            }
        }

        private void CleanButton_Click(object sender, EventArgs e)
        {
            if (APIContainer != null)
            {
                State = ToolState.CLEANING;
                APIContainer.EmuClient.Pause();
                Clean();
            }
        }

        private void PointerTable_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
            {
                e.SortResult = Math.Sign((long)uint.Parse(e.CellValue1.ToString(), NumberStyles.HexNumber) - uint.Parse(e.CellValue2.ToString(), NumberStyles.HexNumber));
                e.Handled = true;
            }
            else if (e.Column.Index == PointerTable.ColumnCount - 1)
            {
                e.SortResult = Math.Sign((long)uint.Parse(e.CellValue1.ToString().Split('=')[0], NumberStyles.HexNumber) - uint.Parse(e.CellValue2.ToString().Split('=')[0], NumberStyles.HexNumber));
                e.Handled = true;
            }
            else
            {
                var index = e.Column.Index;
                e.SortResult = 0;
                while (index >= 0 && e.SortResult == 0)
                {
                    var callValue1 = PointerTable.Rows[e.RowIndex1].Cells[index].Value.ToString();
                    var callValue2 = PointerTable.Rows[e.RowIndex2].Cells[index].Value.ToString();
                    if (string.IsNullOrEmpty(callValue1))
                    {
                        if (string.IsNullOrEmpty(callValue2))
                        {
                            e.SortResult = 0;
                        }
                        else
                        {
                            e.SortResult = -1;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(callValue2))
                        {
                            e.SortResult = 1;
                        }
                        else
                        {
                            e.SortResult = Math.Sign((long)uint.Parse(callValue1, NumberStyles.HexNumber) - uint.Parse(callValue2, NumberStyles.HexNumber));
                        }
                    }
                    index--;
                }
                e.Handled = true;
            }
        }

        private void PointerTable_CurrentCellChanged(object sender, EventArgs e)
        {
            if (APIContainer != null)
            {

                if (PointerTable.SelectedRows.Count == 1)
                {
                    var kvp = ChainRowMap.FirstOrNull(kvp => kvp.Value == PointerTable.SelectedRows[0]);
                    if (kvp != null)
                    {
                        var chain = kvp.Value.Key;
                        uint addr_size_uint = (uint)AddressSize;
                        var address_format = "X" + (addr_size_uint * 2);
                        var addr = MemoryHelper.ReadMemory(APIContainer, AddressSize, chain.StartAddress, MemoryDomains.MainMemory);
                        var msg = string.Format("[{0:" + address_format + "}]=>{1:" + address_format + "}", chain.StartAddress + RAMOffset, addr);
                        for (int i = 0; i < chain.Offsets.Count; i++)
                        {
                            uint offset = chain.Offsets[i];
                            if (i == chain.Offsets.Count - 1)
                            {
                                msg += string.Format("\n{0:" + address_format + "}+{1:X}={2:" + address_format + "}", addr, offset, addr + offset);
                            }
                            else
                            {
                                var new_addr = MemoryHelper.ReadMemory(APIContainer, AddressSize, addr + offset - RAMOffset, MemoryDomains.MainMemory);
                                msg += string.Format("\n[{0:" + address_format + "}+{1:X}]=>{2:" + address_format + "}", addr, offset, new_addr);
                                addr = new_addr;
                            }
                        }
                        PointerDataLabel.Text = msg;
                    }
                    return;
                }
            }
            PointerDataLabel.Text = "";
        }
    }
}
