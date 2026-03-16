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
            this.PointerTable = new System.Windows.Forms.DataGridView();
            this.LocationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset3Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset4Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset5Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset6Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataPanel = new System.Windows.Forms.Panel();
            this.PointerDataLabel = new System.Windows.Forms.Label();
            this.RAMOffsetTextBox = new System.Windows.Forms.TextBox();
            this.RAMOffsetLabel = new System.Windows.Forms.Label();
            this.CleanButton = new System.Windows.Forms.Button();
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
            this.AlignToggle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PointerTable)).BeginInit();
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
            // PointerTable
            // 
            this.PointerTable.AllowUserToAddRows = false;
            this.PointerTable.AllowUserToDeleteRows = false;
            this.PointerTable.AllowUserToResizeRows = false;
            this.PointerTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PointerTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocationColumn,
            this.Offset1Column,
            this.Offset2Column,
            this.Offset3Column,
            this.Offset4Column,
            this.Offset5Column,
            this.Offset6Column,
            this.ValueColumn});
            this.PointerTable.Location = new System.Drawing.Point(12, 46);
            this.PointerTable.Name = "PointerTable";
            this.PointerTable.ReadOnly = true;
            this.PointerTable.RowHeadersVisible = false;
            this.PointerTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PointerTable.Size = new System.Drawing.Size(801, 503);
            this.PointerTable.TabIndex = 1;
            this.PointerTable.CurrentCellChanged += new System.EventHandler(this.PointerTable_CurrentCellChanged);
            // 
            // LocationColumn
            // 
            this.LocationColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LocationColumn.HeaderText = global::PointerScan.Messages.PointerTable_Headers_BaseLocation;
            this.LocationColumn.Name = "LocationColumn";
            this.LocationColumn.ReadOnly = true;
            // 
            // Offset1Column
            // 
            this.Offset1Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset1Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset1;
            this.Offset1Column.Name = "Offset1Column";
            this.Offset1Column.ReadOnly = true;
            this.Offset1Column.Width = 69;
            // 
            // Offset2Column
            // 
            this.Offset2Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset2Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset2;
            this.Offset2Column.Name = "Offset2Column";
            this.Offset2Column.ReadOnly = true;
            this.Offset2Column.Width = 69;
            // 
            // Offset3Column
            // 
            this.Offset3Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset3Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset3;
            this.Offset3Column.Name = "Offset3Column";
            this.Offset3Column.ReadOnly = true;
            this.Offset3Column.Width = 69;
            // 
            // Offset4Column
            // 
            this.Offset4Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset4Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset4;
            this.Offset4Column.Name = "Offset4Column";
            this.Offset4Column.ReadOnly = true;
            this.Offset4Column.Width = 69;
            // 
            // Offset5Column
            // 
            this.Offset5Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset5Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset5;
            this.Offset5Column.Name = "Offset5Column";
            this.Offset5Column.ReadOnly = true;
            this.Offset5Column.Width = 69;
            // 
            // Offset6Column
            // 
            this.Offset6Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Offset6Column.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Offset6;
            this.Offset6Column.Name = "Offset6Column";
            this.Offset6Column.ReadOnly = true;
            this.Offset6Column.Width = 69;
            // 
            // ValueColumn
            // 
            this.ValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ValueColumn.HeaderText = global::PointerScan.Messages.PointerTable_Headers_Result;
            this.ValueColumn.Name = "ValueColumn";
            this.ValueColumn.ReadOnly = true;
            this.ValueColumn.Width = 160;
            // 
            // DataPanel
            // 
            this.DataPanel.Controls.Add(this.AlignToggle);
            this.DataPanel.Controls.Add(this.PointerDataLabel);
            this.DataPanel.Controls.Add(this.RAMOffsetTextBox);
            this.DataPanel.Controls.Add(this.RAMOffsetLabel);
            this.DataPanel.Controls.Add(this.CleanButton);
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
            // PointerDataLabel
            // 
            this.PointerDataLabel.Location = new System.Drawing.Point(2, 231);
            this.PointerDataLabel.Name = "PointerDataLabel";
            this.PointerDataLabel.Size = new System.Drawing.Size(148, 141);
            this.PointerDataLabel.TabIndex = 16;
            // 
            // RAMOffsetTextBox
            // 
            this.RAMOffsetTextBox.Location = new System.Drawing.Point(0, 153);
            this.RAMOffsetTextBox.Name = "RAMOffsetTextBox";
            this.RAMOffsetTextBox.Size = new System.Drawing.Size(150, 20);
            this.RAMOffsetTextBox.TabIndex = 15;
            this.RAMOffsetTextBox.Text = "0";
            // 
            // RAMOffsetLabel
            // 
            this.RAMOffsetLabel.AutoSize = true;
            this.RAMOffsetLabel.Location = new System.Drawing.Point(2, 137);
            this.RAMOffsetLabel.Name = "RAMOffsetLabel";
            this.RAMOffsetLabel.Size = new System.Drawing.Size(118, 13);
            this.RAMOffsetLabel.TabIndex = 14;
            this.RAMOffsetLabel.Text = "RAM Base offset (Hex):";
            // 
            // CleanButton
            // 
            this.CleanButton.Enabled = false;
            this.CleanButton.Location = new System.Drawing.Point(5, 449);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(139, 21);
            this.CleanButton.TabIndex = 13;
            this.CleanButton.Text = global::PointerScan.Messages.CleanButton_Label;
            this.CleanButton.UseVisualStyleBackColor = true;
            this.CleanButton.Click += new System.EventHandler(this.CleanButton_Click);
            // 
            // MaxDepthUpDown
            // 
            this.MaxDepthUpDown.Location = new System.Drawing.Point(75, 108);
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
            this.MaxDepthLabel.Location = new System.Drawing.Point(2, 110);
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
            this.MaxOffsetTextBox.Location = new System.Drawing.Point(0, 82);
            this.MaxOffsetTextBox.Name = "MaxOffsetTextBox";
            this.MaxOffsetTextBox.Size = new System.Drawing.Size(150, 20);
            this.MaxOffsetTextBox.TabIndex = 8;
            this.MaxOffsetTextBox.Text = "800";
            // 
            // MaxOffsetLabel
            // 
            this.MaxOffsetLabel.AutoSize = true;
            this.MaxOffsetLabel.Location = new System.Drawing.Point(2, 66);
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
            // AlignToggle
            // 
            this.AlignToggle.Checked = true;
            this.AlignToggle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlignToggle.Location = new System.Drawing.Point(1, 186);
            this.AlignToggle.Name = "AlignToggle";
            this.AlignToggle.Size = new System.Drawing.Size(148, 42);
            this.AlignToggle.TabIndex = 17;
            this.AlignToggle.Text = "Align offsets with Address size";
            this.AlignToggle.UseVisualStyleBackColor = true;
            // 
            // PointerScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DataPanel);
            this.Controls.Add(this.PointerTable);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.StatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PointerScan";
            ((System.ComponentModel.ISupportInitialize)(this.PointerTable)).EndInit();
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
        private System.Windows.Forms.DataGridView PointerTable;
        private System.Windows.Forms.Panel DataPanel;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label AddressSizeLabel;
        private System.Windows.Forms.ComboBox AddressSizeOptions;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox MaxOffsetTextBox;
        private System.Windows.Forms.Label MaxOffsetLabel;
        private System.Windows.Forms.Label ResultsLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset1Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset2Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset3Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset4Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset5Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset6Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.NumericUpDown MaxDepthUpDown;
        private System.Windows.Forms.Label MaxDepthLabel;
        private System.Windows.Forms.Button CleanButton;
        private System.Windows.Forms.Label RAMOffsetLabel;
        private System.Windows.Forms.TextBox RAMOffsetTextBox;
        private System.Windows.Forms.Label PointerDataLabel;
        private System.Windows.Forms.CheckBox AlignToggle;
    }
}
