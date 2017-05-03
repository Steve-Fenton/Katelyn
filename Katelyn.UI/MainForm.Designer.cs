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
            this.label1 = new System.Windows.Forms.Label();
            this.DepthLabel = new System.Windows.Forms.Label();
            this.CrawlDepth = new System.Windows.Forms.TextBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.CrawlStart = new System.Windows.Forms.Button();
            this.CrawlAddress = new System.Windows.Forms.TextBox();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.OutputTab = new System.Windows.Forms.TabPage();
            this.OutputListBox = new System.Windows.Forms.ListBox();
            this.ErrorTab = new System.Windows.Forms.TabPage();
            this.ErrorListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StoreResultCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddressGroupBox.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.OutputTab.SuspendLayout();
            this.ErrorTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CrawlProgress
            // 
            this.CrawlProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CrawlProgress.Enabled = false;
            this.CrawlProgress.Location = new System.Drawing.Point(0, 834);
            this.CrawlProgress.Name = "CrawlProgress";
            this.CrawlProgress.Size = new System.Drawing.Size(1718, 23);
            this.CrawlProgress.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(1718, 834);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 9;
            // 
            // AddressGroupBox
            // 
            this.AddressGroupBox.Controls.Add(this.StoreResultCheckBox);
            this.AddressGroupBox.Controls.Add(this.label2);
            this.AddressGroupBox.Controls.Add(this.label1);
            this.AddressGroupBox.Controls.Add(this.DepthLabel);
            this.AddressGroupBox.Controls.Add(this.CrawlDepth);
            this.AddressGroupBox.Controls.Add(this.ClearButton);
            this.AddressGroupBox.Controls.Add(this.CrawlStart);
            this.AddressGroupBox.Controls.Add(this.CrawlAddress);
            this.AddressGroupBox.Location = new System.Drawing.Point(8, 12);
            this.AddressGroupBox.Name = "AddressGroupBox";
            this.AddressGroupBox.Size = new System.Drawing.Size(844, 211);
            this.AddressGroupBox.TabIndex = 9;
            this.AddressGroupBox.TabStop = false;
            this.AddressGroupBox.Text = "Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Address";
            // 
            // DepthLabel
            // 
            this.DepthLabel.AutoSize = true;
            this.DepthLabel.Location = new System.Drawing.Point(14, 96);
            this.DepthLabel.Name = "DepthLabel";
            this.DepthLabel.Size = new System.Drawing.Size(69, 25);
            this.DepthLabel.TabIndex = 4;
            this.DepthLabel.Text = "Depth";
            // 
            // CrawlDepth
            // 
            this.CrawlDepth.Location = new System.Drawing.Point(138, 96);
            this.CrawlDepth.Name = "CrawlDepth";
            this.CrawlDepth.Size = new System.Drawing.Size(133, 31);
            this.CrawlDepth.TabIndex = 2;
            this.CrawlDepth.Text = "5";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(683, 49);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(125, 51);
            this.ClearButton.TabIndex = 102;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // CrawlStart
            // 
            this.CrawlStart.Location = new System.Drawing.Point(552, 49);
            this.CrawlStart.Name = "CrawlStart";
            this.CrawlStart.Size = new System.Drawing.Size(125, 51);
            this.CrawlStart.TabIndex = 101;
            this.CrawlStart.Text = "Start";
            this.CrawlStart.UseVisualStyleBackColor = true;
            this.CrawlStart.Click += new System.EventHandler(this.CrawlStart_Click);
            // 
            // CrawlAddress
            // 
            this.CrawlAddress.Location = new System.Drawing.Point(138, 49);
            this.CrawlAddress.Name = "CrawlAddress";
            this.CrawlAddress.Size = new System.Drawing.Size(372, 31);
            this.CrawlAddress.TabIndex = 0;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.OutputTab);
            this.MainTabControl.Controls.Add(this.ErrorTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1718, 710);
            this.MainTabControl.TabIndex = 1000;
            // 
            // OutputTab
            // 
            this.OutputTab.Controls.Add(this.OutputListBox);
            this.OutputTab.Location = new System.Drawing.Point(8, 39);
            this.OutputTab.Name = "OutputTab";
            this.OutputTab.Padding = new System.Windows.Forms.Padding(3);
            this.OutputTab.Size = new System.Drawing.Size(1702, 663);
            this.OutputTab.TabIndex = 0;
            this.OutputTab.Text = "Output";
            this.OutputTab.UseVisualStyleBackColor = true;
            // 
            // OutputListBox
            // 
            this.OutputListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputListBox.FormattingEnabled = true;
            this.OutputListBox.ItemHeight = 25;
            this.OutputListBox.Location = new System.Drawing.Point(3, 3);
            this.OutputListBox.Name = "OutputListBox";
            this.OutputListBox.Size = new System.Drawing.Size(1696, 657);
            this.OutputListBox.TabIndex = 0;
            // 
            // ErrorTab
            // 
            this.ErrorTab.Controls.Add(this.ErrorListBox);
            this.ErrorTab.Location = new System.Drawing.Point(8, 39);
            this.ErrorTab.Name = "ErrorTab";
            this.ErrorTab.Padding = new System.Windows.Forms.Padding(3);
            this.ErrorTab.Size = new System.Drawing.Size(1702, 683);
            this.ErrorTab.TabIndex = 1;
            this.ErrorTab.Text = "Errors";
            this.ErrorTab.UseVisualStyleBackColor = true;
            // 
            // ErrorListBox
            // 
            this.ErrorListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorListBox.FormattingEnabled = true;
            this.ErrorListBox.ItemHeight = 25;
            this.ErrorListBox.Location = new System.Drawing.Point(3, 3);
            this.ErrorListBox.Name = "ErrorListBox";
            this.ErrorListBox.Size = new System.Drawing.Size(1696, 677);
            this.ErrorListBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Store";
            // 
            // StoreResultCheckBox
            // 
            this.StoreResultCheckBox.AutoSize = true;
            this.StoreResultCheckBox.Location = new System.Drawing.Point(138, 143);
            this.StoreResultCheckBox.Name = "StoreResultCheckBox";
            this.StoreResultCheckBox.Size = new System.Drawing.Size(335, 29);
            this.StoreResultCheckBox.TabIndex = 3;
            this.StoreResultCheckBox.Text = "(stores results for comparison)";
            this.StoreResultCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1718, 857);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.CrawlProgress);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.ErrorTab.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox OutputListBox;
        private System.Windows.Forms.TabPage ErrorTab;
        private System.Windows.Forms.ListBox ErrorListBox;
        private System.Windows.Forms.CheckBox StoreResultCheckBox;
        private System.Windows.Forms.Label label2;
    }
}

