namespace KernelUtilitiesTester
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblActiveWindow = new System.Windows.Forms.Label();
            this.btnFindWindow = new System.Windows.Forms.Button();
            this.txtWindowTitle = new System.Windows.Forms.TextBox();
            this.btnSendText = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTargetWindow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeybind = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Active Window";
            // 
            // lblActiveWindow
            // 
            this.lblActiveWindow.AutoSize = true;
            this.lblActiveWindow.Location = new System.Drawing.Point(194, 9);
            this.lblActiveWindow.Name = "lblActiveWindow";
            this.lblActiveWindow.Size = new System.Drawing.Size(0, 20);
            this.lblActiveWindow.TabIndex = 1;
            // 
            // btnFindWindow
            // 
            this.btnFindWindow.Location = new System.Drawing.Point(390, 69);
            this.btnFindWindow.Name = "btnFindWindow";
            this.btnFindWindow.Size = new System.Drawing.Size(172, 33);
            this.btnFindWindow.TabIndex = 2;
            this.btnFindWindow.Text = "Find Window";
            this.btnFindWindow.UseVisualStyleBackColor = true;
            this.btnFindWindow.Click += new System.EventHandler(this.btnFindWindow_Click);
            // 
            // txtWindowTitle
            // 
            this.txtWindowTitle.Location = new System.Drawing.Point(12, 69);
            this.txtWindowTitle.Name = "txtWindowTitle";
            this.txtWindowTitle.Size = new System.Drawing.Size(357, 26);
            this.txtWindowTitle.TabIndex = 3;
            // 
            // btnSendText
            // 
            this.btnSendText.Location = new System.Drawing.Point(12, 157);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(357, 33);
            this.btnSendText.TabIndex = 4;
            this.btnSendText.Text = "Start intermittent test";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Target Window";
            // 
            // lblTargetWindow
            // 
            this.lblTargetWindow.AutoSize = true;
            this.lblTargetWindow.Location = new System.Drawing.Point(194, 117);
            this.lblTargetWindow.Name = "lblTargetWindow";
            this.lblTargetWindow.Size = new System.Drawing.Size(0, 20);
            this.lblTargetWindow.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Keybind";
            // 
            // txtKeybind
            // 
            this.txtKeybind.Location = new System.Drawing.Point(79, 208);
            this.txtKeybind.Name = "txtKeybind";
            this.txtKeybind.Size = new System.Drawing.Size(290, 26);
            this.txtKeybind.TabIndex = 9;
            this.txtKeybind.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtKeybind_MouseClick);
            this.txtKeybind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeybind_KeyDown);
            this.txtKeybind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeybind_KeyPress);
            this.txtKeybind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtKeybind_KeyUp);
            // 
            // txtHistory
            // 
            this.txtHistory.AcceptsReturn = true;
            this.txtHistory.Location = new System.Drawing.Point(12, 240);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.Size = new System.Drawing.Size(550, 423);
            this.txtHistory.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 675);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.txtKeybind);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTargetWindow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.txtWindowTitle);
            this.Controls.Add(this.btnFindWindow);
            this.Controls.Add(this.lblActiveWindow);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblActiveWindow;
        private System.Windows.Forms.Button btnFindWindow;
        private System.Windows.Forms.TextBox txtWindowTitle;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTargetWindow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeybind;
        private System.Windows.Forms.TextBox txtHistory;
    }
}

