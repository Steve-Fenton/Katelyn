namespace Katelyn.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.CrawlProgress = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.AddressGroupBox = new System.Windows.Forms.GroupBox();
            this.StringForRegex = new System.Windows.Forms.TextBox();
            this.SearchForLabel = new System.Windows.Forms.Label();
            this.StoreResultCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DepthLabel = new System.Windows.Forms.Label();
            this.CrawlDepth = new System.Windows.Forms.TextBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.CrawlStart = new System.Windows.Forms.Button();
            this.CrawlAddress = new System.Windows.Forms.TextBox();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.OutputTab = new System.Windows.Forms.TabPage();
            this.CrawlOutput = new System.Windows.Forms.DataGridView();
            this.ErrorTab = new System.Windows.Forms.TabPage();
            this.ErrorGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddressGroupBox.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.OutputTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CrawlOutput)).BeginInit();
            this.ErrorTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CrawlProgress
            // 
            this.CrawlProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CrawlProgress.Enabled = false;
            this.CrawlProgress.Location = new System.Drawing.Point(0, 434);
            this.CrawlProgress.Margin = new System.Windows.Forms.Padding(2);
            this.CrawlProgress.Name = "CrawlProgress";
            this.CrawlProgress.Size = new System.Drawing.Size(859, 12);
            this.CrawlProgress.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.AddressGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MainTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(859, 434);
            this.splitContainer1.SplitterDistance = 140;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 9;
            // 
            // AddressGroupBox
            // 
            this.AddressGroupBox.Controls.Add(this.StringForRegex);
            this.AddressGroupBox.Controls.Add(this.SearchForLabel);
            this.AddressGroupBox.Controls.Add(this.StoreResultCheckBox);
            this.AddressGroupBox.Controls.Add(this.label2);
            this.AddressGroupBox.Controls.Add(this.label1);
            this.AddressGroupBox.Controls.Add(this.DepthLabel);
            this.AddressGroupBox.Controls.Add(this.CrawlDepth);
            this.AddressGroupBox.Controls.Add(this.ClearButton);
            this.AddressGroupBox.Controls.Add(this.CrawlStart);
            this.AddressGroupBox.Controls.Add(this.CrawlAddress);
            this.AddressGroupBox.Location = new System.Drawing.Point(4, 6);
            this.AddressGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.AddressGroupBox.Name = "AddressGroupBox";
            this.AddressGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.AddressGroupBox.Size = new System.Drawing.Size(422, 135);
            this.AddressGroupBox.TabIndex = 9;
            this.AddressGroupBox.TabStop = false;
            this.AddressGroupBox.Text = "Settings";
            // 
            // StringForRegex
            // 
            this.StringForRegex.Location = new System.Drawing.Point(69, 97);
            this.StringForRegex.Margin = new System.Windows.Forms.Padding(2);
            this.StringForRegex.Name = "StringForRegex";
            this.StringForRegex.Size = new System.Drawing.Size(188, 20);
            this.StringForRegex.TabIndex = 4;
            // 
            // SearchForLabel
            // 
            this.SearchForLabel.AutoSize = true;
            this.SearchForLabel.Location = new System.Drawing.Point(7, 98);
            this.SearchForLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SearchForLabel.Name = "SearchForLabel";
            this.SearchForLabel.Size = new System.Drawing.Size(62, 13);
            this.SearchForLabel.TabIndex = 103;
            this.SearchForLabel.Text = "Search Exp";
            // 
            // StoreResultCheckBox
            // 
            this.StoreResultCheckBox.AutoSize = true;
            this.StoreResultCheckBox.Location = new System.Drawing.Point(69, 74);
            this.StoreResultCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.StoreResultCheckBox.Name = "StoreResultCheckBox";
            this.StoreResultCheckBox.Size = new System.Drawing.Size(165, 17);
            this.StoreResultCheckBox.TabIndex = 3;
            this.StoreResultCheckBox.Text = "(stores results for comparison)";
            this.StoreResultCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Store";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Address";
            // 
            // DepthLabel
            // 
            this.DepthLabel.AutoSize = true;
            this.DepthLabel.Location = new System.Drawing.Point(7, 50);
            this.DepthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DepthLabel.Name = "DepthLabel";
            this.DepthLabel.Size = new System.Drawing.Size(36, 13);
            this.DepthLabel.TabIndex = 4;
            this.DepthLabel.Text = "Depth";
            // 
            // CrawlDepth
            // 
            this.CrawlDepth.Location = new System.Drawing.Point(69, 50);
            this.CrawlDepth.Margin = new System.Windows.Forms.Padding(2);
            this.CrawlDepth.Name = "CrawlDepth";
            this.CrawlDepth.Size = new System.Drawing.Size(68, 20);
            this.CrawlDepth.TabIndex = 2;
            this.CrawlDepth.Text = "5";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(342, 25);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(2);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(62, 27);
            this.ClearButton.TabIndex = 102;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // CrawlStart
            // 
            this.CrawlStart.Location = new System.Drawing.Point(276, 25);
            this.CrawlStart.Margin = new System.Windows.Forms.Padding(2);
            this.CrawlStart.Name = "CrawlStart";
            this.CrawlStart.Size = new System.Drawing.Size(62, 27);
            this.CrawlStart.TabIndex = 101;
            this.CrawlStart.Text = "Start";
            this.CrawlStart.UseVisualStyleBackColor = true;
            this.CrawlStart.Click += new System.EventHandler(this.CrawlStart_Click);
            // 
            // CrawlAddress
            // 
            this.CrawlAddress.Location = new System.Drawing.Point(69, 25);
            this.CrawlAddress.Margin = new System.Windows.Forms.Padding(2);
            this.CrawlAddress.Name = "CrawlAddress";
            this.CrawlAddress.Size = new System.Drawing.Size(188, 20);
            this.CrawlAddress.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.OutputTab);
            this.MainTabControl.Controls.Add(this.ErrorTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(859, 292);
            this.MainTabControl.TabIndex = 1000;
            // 
            // OutputTab
            // 
            this.OutputTab.Controls.Add(this.CrawlOutput);
            this.OutputTab.Location = new System.Drawing.Point(4, 22);
            this.OutputTab.Margin = new System.Windows.Forms.Padding(2);
            this.OutputTab.Name = "OutputTab";
            this.OutputTab.Padding = new System.Windows.Forms.Padding(2);
            this.OutputTab.Size = new System.Drawing.Size(851, 266);
            this.OutputTab.TabIndex = 0;
            this.OutputTab.Text = "Output";
            this.OutputTab.UseVisualStyleBackColor = true;
            // 
            // CrawlOutput
            // 
            this.CrawlOutput.AllowUserToAddRows = false;
            this.CrawlOutput.AllowUserToDeleteRows = false;
            this.CrawlOutput.AllowUserToResizeRows = false;
            this.CrawlOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CrawlOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrawlOutput.Location = new System.Drawing.Point(2, 2);
            this.CrawlOutput.Margin = new System.Windows.Forms.Padding(2);
            this.CrawlOutput.Name = "CrawlOutput";
            this.CrawlOutput.ReadOnly = true;
            this.CrawlOutput.RowTemplate.Height = 33;
            this.CrawlOutput.Size = new System.Drawing.Size(847, 262);
            this.CrawlOutput.TabIndex = 0;
            this.CrawlOutput.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ColumnHeaderClick);
            // 
            // ErrorTab
            // 
            this.ErrorTab.Controls.Add(this.ErrorGridView);
            this.ErrorTab.Location = new System.Drawing.Point(4, 22);
            this.ErrorTab.Margin = new System.Windows.Forms.Padding(2);
            this.ErrorTab.Name = "ErrorTab";
            this.ErrorTab.Padding = new System.Windows.Forms.Padding(2);
            this.ErrorTab.Size = new System.Drawing.Size(851, 266);
            this.ErrorTab.TabIndex = 1;
            this.ErrorTab.Text = "Errors";
            this.ErrorTab.UseVisualStyleBackColor = true;
            // 
            // ErrorGridView
            // 
            this.ErrorGridView.AllowUserToAddRows = false;
            this.ErrorGridView.AllowUserToDeleteRows = false;
            this.ErrorGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ErrorGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorGridView.Location = new System.Drawing.Point(2, 2);
            this.ErrorGridView.Name = "ErrorGridView";
            this.ErrorGridView.ReadOnly = true;
            this.ErrorGridView.Size = new System.Drawing.Size(847, 262);
            this.ErrorGridView.TabIndex = 0;
            this.ErrorGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ErrorColumnHeaderClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 446);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.CrawlProgress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Katelyn";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.AddressGroupBox.ResumeLayout(false);
            this.AddressGroupBox.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.OutputTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CrawlOutput)).EndInit();
            this.ErrorTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar CrawlProgress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox AddressGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DepthLabel;
        private System.Windows.Forms.TextBox CrawlDepth;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button CrawlStart;
        private System.Windows.Forms.TextBox CrawlAddress;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage OutputTab;
        private System.Windows.Forms.TabPage ErrorTab;
        private System.Windows.Forms.CheckBox StoreResultCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView CrawlOutput;
        private System.Windows.Forms.TextBox StringForRegex;
        private System.Windows.Forms.Label SearchForLabel;
        private System.Windows.Forms.DataGridView ErrorGridView;
    }
}

