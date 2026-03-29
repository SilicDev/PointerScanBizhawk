using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerScan
{
    public partial class PointerScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.DelayTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.DataPanel = new System.Windows.Forms.Panel();
            this.ValueSizeLabel = new System.Windows.Forms.Label();
            this.ValueSizeOptions = new System.Windows.Forms.ComboBox();
            this.AlignToggle = new System.Windows.Forms.CheckBox();
            this.PointerDataLabel = new System.Windows.Forms.Label();
            this.RAMOffsetTextBox = new System.Windows.Forms.TextBox();
            this.RAMOffsetLabel = new System.Windows.Forms.Label();
            this.AutoButton = new System.Windows.Forms.CheckBox();
            this.MaxDepthUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxDepthLabel = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ResultsLabel = new System.Windows.Forms.Label();
            this.MaxOffsetTextBox = new System.Windows.Forms.TextBox();
            this.MaxOffsetLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.AddressSizeOptions = new System.Windows.Forms.ComboBox();
            this.AddressSizeLabel = new System.Windows.Forms.Label();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.WatchListView = new BizHawk.Client.EmuHawk.InputRoll();
            this.DataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDepthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(9, 30);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(87, 13);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "No ROM loaded!";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(9, 7);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(74, 13);
            this.VersionLabel.TabIndex = 3;
            this.VersionLabel.Text = "PointerScan v";
            // 
            // DelayTooltip
            // 
            this.DelayTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // DataPanel
            // 
            this.DataPanel.Controls.Add(this.ValueSizeLabel);
            this.DataPanel.Controls.Add(this.ValueSizeOptions);
            this.DataPanel.Controls.Add(this.AlignToggle);
            this.DataPanel.Controls.Add(this.PointerDataLabel);
            this.DataPanel.Controls.Add(this.RAMOffsetTextBox);
            this.DataPanel.Controls.Add(this.RAMOffsetLabel);
            this.DataPanel.Controls.Add(this.AutoButton);
            this.DataPanel.Controls.Add(this.MaxDepthUpDown);
            this.DataPanel.Controls.Add(this.MaxDepthLabel);
            this.DataPanel.Controls.Add(this.ResetButton);
            this.DataPanel.Controls.Add(this.ResultsLabel);
            this.DataPanel.Controls.Add(this.MaxOffsetTextBox);
            this.DataPanel.Controls.Add(this.MaxOffsetLabel);
            this.DataPanel.Controls.Add(this.StartButton);
            this.DataPanel.Controls.Add(this.AddressSizeOptions);
            this.DataPanel.Controls.Add(this.AddressSizeLabel);
            this.DataPanel.Controls.Add(this.AddressTextBox);
            this.DataPanel.Controls.Add(this.AddressLabel);
            this.DataPanel.Location = new System.Drawing.Point(822, 47);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new System.Drawing.Size(150, 502);
            this.DataPanel.TabIndex = 4;
            // 
            // ValueSizeLabel
            // 
            this.ValueSizeLabel.AutoSize = true;
            this.ValueSizeLabel.Location = new System.Drawing.Point(2, 69);
            this.ValueSizeLabel.Name = "ValueSizeLabel";
            this.ValueSizeLabel.Size = new System.Drawing.Size(60, 13);
            this.ValueSizeLabel.TabIndex = 19;
            this.ValueSizeLabel.Text = "Value Size:";
            // 
            // ValueSizeOptions
            // 
            this.ValueSizeOptions.FormattingEnabled = true;
            this.ValueSizeOptions.Items.AddRange(new object[] {
            global::PointerScan.Messages.ValueSizeOptions_1Byte,
            global::PointerScan.Messages.ValueSizeOptions_2Bytes,
            global::PointerScan.Messages.ValueSizeOptions_4Bytes});
            this.ValueSizeOptions.Location = new System.Drawing.Point(75, 66);
            this.ValueSizeOptions.Name = "ValueSizeOptions";
            this.ValueSizeOptions.Size = new System.Drawing.Size(75, 21);
            this.ValueSizeOptions.TabIndex = 18;
            this.ValueSizeOptions.Text = "1 Byte";
            this.ValueSizeOptions.SelectedIndexChanged += ValueSizeOptions_SelectedIndexChanged;
            // 
            // AlignToggle
            // 
            this.AlignToggle.Checked = true;
            this.AlignToggle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlignToggle.Location = new System.Drawing.Point(3, 216);
            this.AlignToggle.Name = "AlignToggle";
            this.AlignToggle.Size = new System.Drawing.Size(148, 42);
            this.AlignToggle.TabIndex = 17;
            this.AlignToggle.Text = "Align offsets with Address size";
            this.AlignToggle.UseVisualStyleBackColor = true;
            // 
            // PointerDataLabel
            // 
            this.PointerDataLabel.Location = new System.Drawing.Point(2, 274);
            this.PointerDataLabel.Name = "PointerDataLabel";
            this.PointerDataLabel.Size = new System.Drawing.Size(148, 98);
            this.PointerDataLabel.TabIndex = 16;
            // 
            // RAMOffsetTextBox
            // 
            this.RAMOffsetTextBox.Location = new System.Drawing.Point(2, 183);
            this.RAMOffsetTextBox.Name = "RAMOffsetTextBox";
            this.RAMOffsetTextBox.Size = new System.Drawing.Size(150, 20);
            this.RAMOffsetTextBox.TabIndex = 15;
            this.RAMOffsetTextBox.Text = "0";
            // 
            // RAMOffsetLabel
            // 
            this.RAMOffsetLabel.AutoSize = true;
            this.RAMOffsetLabel.Location = new System.Drawing.Point(4, 167);
            this.RAMOffsetLabel.Name = "RAMOffsetLabel";
            this.RAMOffsetLabel.Size = new System.Drawing.Size(118, 13);
            this.RAMOffsetLabel.TabIndex = 14;
            this.RAMOffsetLabel.Text = "RAM Base offset (Hex):";
            // 
            // AutoButton
            // 
            this.AutoButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.AutoButton.Enabled = false;
            this.AutoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoButton.Location = new System.Drawing.Point(5, 449);
            this.AutoButton.Name = "AutoButton";
            this.AutoButton.Size = new System.Drawing.Size(139, 21);
            this.AutoButton.TabIndex = 13;
            this.AutoButton.Text = "Auto";
            this.AutoButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AutoButton.UseVisualStyleBackColor = true;
            this.AutoButton.Click += new System.EventHandler(this.AutoButton_Click);
            // 
            // MaxDepthUpDown
            // 
            this.MaxDepthUpDown.Location = new System.Drawing.Point(77, 138);
            this.MaxDepthUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.MaxDepthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxDepthUpDown.Name = "MaxDepthUpDown";
            this.MaxDepthUpDown.Size = new System.Drawing.Size(73, 20);
            this.MaxDepthUpDown.TabIndex = 12;
            this.MaxDepthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MaxDepthLabel
            // 
            this.MaxDepthLabel.AutoSize = true;
            this.MaxDepthLabel.Location = new System.Drawing.Point(4, 140);
            this.MaxDepthLabel.Name = "MaxDepthLabel";
            this.MaxDepthLabel.Size = new System.Drawing.Size(62, 13);
            this.MaxDepthLabel.TabIndex = 11;
            this.MaxDepthLabel.Text = "Max Depth:";
            // 
            // ResetButton
            // 
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(5, 476);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(139, 23);
            this.ResetButton.TabIndex = 10;
            this.ResetButton.Text = global::PointerScan.Messages.ResetButton_Label;
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ResultsLabel
            // 
            this.ResultsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsLabel.Location = new System.Drawing.Point(0, 384);
            this.ResultsLabel.Name = "ResultsLabel";
            this.ResultsLabel.Size = new System.Drawing.Size(150, 31);
            this.ResultsLabel.TabIndex = 9;
            this.ResultsLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // MaxOffsetTextBox
            // 
            this.MaxOffsetTextBox.Location = new System.Drawing.Point(2, 112);
            this.MaxOffsetTextBox.Name = "MaxOffsetTextBox";
            this.MaxOffsetTextBox.Size = new System.Drawing.Size(150, 20);
            this.MaxOffsetTextBox.TabIndex = 8;
            this.MaxOffsetTextBox.Text = "800";
            // 
            // MaxOffsetLabel
            // 
            this.MaxOffsetLabel.AutoSize = true;
            this.MaxOffsetLabel.Location = new System.Drawing.Point(4, 96);
            this.MaxOffsetLabel.Name = "MaxOffsetLabel";
            this.MaxOffsetLabel.Size = new System.Drawing.Size(89, 13);
            this.MaxOffsetLabel.TabIndex = 7;
            this.MaxOffsetLabel.Text = "Max Offset (Hex):";
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Location = new System.Drawing.Point(5, 418);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(139, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = global::PointerScan.Messages.StartButton_Start;
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // AddressSizeOptions
            // 
            this.AddressSizeOptions.FormattingEnabled = true;
            this.AddressSizeOptions.Items.AddRange(new object[] {
            global::PointerScan.Messages.AddressSizeOptions_1Byte,
            global::PointerScan.Messages.AddressSizeOptions_2Bytes,
            global::PointerScan.Messages.AddressSizeOptions_4Bytes});
            this.AddressSizeOptions.Location = new System.Drawing.Point(75, 42);
            this.AddressSizeOptions.Name = "AddressSizeOptions";
            this.AddressSizeOptions.Size = new System.Drawing.Size(75, 21);
            this.AddressSizeOptions.TabIndex = 5;
            this.AddressSizeOptions.Text = "1 Byte";
            // 
            // AddressSizeLabel
            // 
            this.AddressSizeLabel.AutoSize = true;
            this.AddressSizeLabel.Location = new System.Drawing.Point(2, 45);
            this.AddressSizeLabel.Name = "AddressSizeLabel";
            this.AddressSizeLabel.Size = new System.Drawing.Size(71, 13);
            this.AddressSizeLabel.TabIndex = 2;
            this.AddressSizeLabel.Text = "Address Size:";
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(0, 19);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(150, 20);
            this.AddressTextBox.TabIndex = 1;
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(2, 3);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(76, 13);
            this.AddressLabel.TabIndex = 0;
            this.AddressLabel.Text = "Address (Hex):";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 531);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(801, 18);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // WatchListView
            // 
            this.WatchListView.AllowColumnReorder = true;
            this.WatchListView.AllowColumnResize = true;
            this.WatchListView.AllowDrop = true;
            this.WatchListView.AllowMassNavigationShortcuts = true;
            this.WatchListView.AllowRightClickSelection = true;
            this.WatchListView.AlwaysScroll = false;
            this.WatchListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WatchListView.CellHeightPadding = 0;
            this.WatchListView.CellWidthPadding = 0;
            this.WatchListView.ChangeSelectionWhenPaging = true;
            this.WatchListView.FullRowSelect = true;
            this.WatchListView.HorizontalOrientation = false;
            this.WatchListView.LetKeysModifySelection = false;
            this.WatchListView.Location = new System.Drawing.Point(12, 46);
            this.WatchListView.Name = "WatchListView";
            this.WatchListView.RowCount = 0;
            this.WatchListView.ScrollSpeed = 0;
            this.WatchListView.SeekingCutoffInterval = 0;
            this.WatchListView.Size = new System.Drawing.Size(801, 503);
            this.WatchListView.TabIndex = 1;
            // 
            // PointerScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DataPanel);
            this.Controls.Add(this.WatchListView);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.StatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PointerScan";
            this.DataPanel.ResumeLayout(false);
            this.DataPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDepthUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ToolTip DelayTooltip;
        private System.Windows.Forms.Panel DataPanel;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label AddressSizeLabel;
        private System.Windows.Forms.ComboBox AddressSizeOptions;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox MaxOffsetTextBox;
        private System.Windows.Forms.Label MaxOffsetLabel;
        private System.Windows.Forms.Label ResultsLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.NumericUpDown MaxDepthUpDown;
        private System.Windows.Forms.Label MaxDepthLabel;
        private System.Windows.Forms.CheckBox AutoButton;
        private System.Windows.Forms.Label RAMOffsetLabel;
        private System.Windows.Forms.TextBox RAMOffsetTextBox;
        private System.Windows.Forms.Label PointerDataLabel;
        private System.Windows.Forms.CheckBox AlignToggle;
        private BizHawk.Client.EmuHawk.InputRoll WatchListView;
        private System.Windows.Forms.Label ValueSizeLabel;
        private System.Windows.Forms.ComboBox ValueSizeOptions;
    }
}
