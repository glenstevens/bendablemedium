namespace GamePad360
{
    partial class MainWindow
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProfile = new System.Windows.Forms.Label();
            this.cboProfileList = new System.Windows.Forms.ComboBox();
            this.btnNewProfile = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.tabKeybinding = new System.Windows.Forms.TabPage();
            this.statusStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 493);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(478, 30);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(181, 25);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Location = new System.Drawing.Point(12, 9);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(53, 20);
            this.lblProfile.TabIndex = 1;
            this.lblProfile.Text = "Profile";
            // 
            // cboProfileList
            // 
            this.cboProfileList.FormattingEnabled = true;
            this.cboProfileList.Location = new System.Drawing.Point(16, 32);
            this.cboProfileList.Name = "cboProfileList";
            this.cboProfileList.Size = new System.Drawing.Size(328, 28);
            this.cboProfileList.TabIndex = 2;
            // 
            // btnNewProfile
            // 
            this.btnNewProfile.Location = new System.Drawing.Point(350, 32);
            this.btnNewProfile.Name = "btnNewProfile";
            this.btnNewProfile.Size = new System.Drawing.Size(116, 26);
            this.btnNewProfile.TabIndex = 3;
            this.btnNewProfile.Text = "New";
            this.btnNewProfile.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabOptions);
            this.tabControl.Controls.Add(this.tabKeybinding);
            this.tabControl.Location = new System.Drawing.Point(12, 66);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(454, 424);
            this.tabControl.TabIndex = 4;
            // 
            // tabOptions
            // 
            this.tabOptions.Location = new System.Drawing.Point(4, 29);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(446, 391);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // tabKeybinding
            // 
            this.tabKeybinding.Location = new System.Drawing.Point(4, 29);
            this.tabKeybinding.Name = "tabKeybinding";
            this.tabKeybinding.Padding = new System.Windows.Forms.Padding(3);
            this.tabKeybinding.Size = new System.Drawing.Size(446, 391);
            this.tabKeybinding.TabIndex = 1;
            this.tabKeybinding.Text = "Key Binding";
            this.tabKeybinding.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 523);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnNewProfile);
            this.Controls.Add(this.cboProfileList);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainWindow";
            this.Text = "GC360";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.ComboBox cboProfileList;
        private System.Windows.Forms.Button btnNewProfile;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.TabPage tabKeybinding;
    }
}

