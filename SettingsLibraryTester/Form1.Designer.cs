namespace SettingsLibraryTester
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
			this.cboProfiles = new System.Windows.Forms.ComboBox();
			this.btnNew = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDoubleValue = new System.Windows.Forms.MaskedTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtIntegerValue = new System.Windows.Forms.MaskedTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cboBooleanValue = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtStringValue = new System.Windows.Forms.TextBox();
			this.cboTrigger = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtButtonA = new System.Windows.Forms.TextBox();
			this.txtButtonB = new System.Windows.Forms.TextBox();
			this.txtButtonX = new System.Windows.Forms.TextBox();
			this.txtButtonY = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cboProfiles
			// 
			this.cboProfiles.FormattingEnabled = true;
			this.cboProfiles.Location = new System.Drawing.Point(12, 12);
			this.cboProfiles.Name = "cboProfiles";
			this.cboProfiles.Size = new System.Drawing.Size(257, 21);
			this.cboProfiles.TabIndex = 0;
			this.cboProfiles.SelectedIndexChanged += new System.EventHandler(this.cboProfiles_SelectedIndexChanged);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(306, 3);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 37);
			this.btnNew.TabIndex = 1;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(56, 83);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Double Option";
			// 
			// txtDoubleValue
			// 
			this.txtDoubleValue.Location = new System.Drawing.Point(214, 80);
			this.txtDoubleValue.Mask = "00000.0000";
			this.txtDoubleValue.Name = "txtDoubleValue";
			this.txtDoubleValue.Size = new System.Drawing.Size(100, 20);
			this.txtDoubleValue.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(56, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Integer Option";
			// 
			// txtIntegerValue
			// 
			this.txtIntegerValue.Location = new System.Drawing.Point(214, 117);
			this.txtIntegerValue.Mask = "00000";
			this.txtIntegerValue.Name = "txtIntegerValue";
			this.txtIntegerValue.Size = new System.Drawing.Size(100, 20);
			this.txtIntegerValue.TabIndex = 5;
			this.txtIntegerValue.ValidatingType = typeof(int);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(56, 156);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Boolean Option";
			// 
			// cboBooleanValue
			// 
			this.cboBooleanValue.FormattingEnabled = true;
			this.cboBooleanValue.Items.AddRange(new object[] {
            "true",
            "false"});
			this.cboBooleanValue.Location = new System.Drawing.Point(193, 153);
			this.cboBooleanValue.Name = "cboBooleanValue";
			this.cboBooleanValue.Size = new System.Drawing.Size(121, 21);
			this.cboBooleanValue.TabIndex = 7;
			this.cboBooleanValue.Text = "true";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(56, 195);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "String Option";
			// 
			// txtStringValue
			// 
			this.txtStringValue.Location = new System.Drawing.Point(214, 192);
			this.txtStringValue.Name = "txtStringValue";
			this.txtStringValue.Size = new System.Drawing.Size(100, 20);
			this.txtStringValue.TabIndex = 9;
			// 
			// cboTrigger
			// 
			this.cboTrigger.FormattingEnabled = true;
			this.cboTrigger.Location = new System.Drawing.Point(77, 241);
			this.cboTrigger.Name = "cboTrigger";
			this.cboTrigger.Size = new System.Drawing.Size(237, 21);
			this.cboTrigger.TabIndex = 10;
			this.cboTrigger.SelectedIndexChanged += new System.EventHandler(this.cboTrigger_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(56, 305);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Button A";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(56, 345);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Button B";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(56, 380);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 13);
			this.label7.TabIndex = 13;
			this.label7.Text = "Button X";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(56, 415);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "Button Y";
			// 
			// txtButtonA
			// 
			this.txtButtonA.Location = new System.Drawing.Point(214, 302);
			this.txtButtonA.Name = "txtButtonA";
			this.txtButtonA.Size = new System.Drawing.Size(100, 20);
			this.txtButtonA.TabIndex = 15;
			this.txtButtonA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtButtonA_KeyDown);
			this.txtButtonA.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtButtonA_KeyUp);
			this.txtButtonA.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtButtonA_MouseClick);
			this.txtButtonA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtButtonA_KeyPress);
			// 
			// txtButtonB
			// 
			this.txtButtonB.Location = new System.Drawing.Point(214, 342);
			this.txtButtonB.Name = "txtButtonB";
			this.txtButtonB.Size = new System.Drawing.Size(100, 20);
			this.txtButtonB.TabIndex = 16;
			this.txtButtonB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtButtonB_KeyDown);
			this.txtButtonB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtButtonB_KeyUp);
			this.txtButtonB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtButtonB_MouseClick);
			this.txtButtonB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtButtonB_KeyPress);
			// 
			// txtButtonX
			// 
			this.txtButtonX.Location = new System.Drawing.Point(214, 377);
			this.txtButtonX.Name = "txtButtonX";
			this.txtButtonX.Size = new System.Drawing.Size(100, 20);
			this.txtButtonX.TabIndex = 17;
			this.txtButtonX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtButtonX_KeyDown);
			this.txtButtonX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtButtonX_KeyUp);
			this.txtButtonX.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtButtonX_MouseClick);
			this.txtButtonX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtButtonX_KeyPress);
			// 
			// txtButtonY
			// 
			this.txtButtonY.Location = new System.Drawing.Point(214, 412);
			this.txtButtonY.Name = "txtButtonY";
			this.txtButtonY.Size = new System.Drawing.Size(100, 20);
			this.txtButtonY.TabIndex = 18;
			this.txtButtonY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtButtonY_KeyDown);
			this.txtButtonY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtButtonY_KeyUp);
			this.txtButtonY.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtButtonY_MouseClick);
			this.txtButtonY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtButtonY_KeyPress);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 659);
			this.Controls.Add(this.txtButtonY);
			this.Controls.Add(this.txtButtonX);
			this.Controls.Add(this.txtButtonB);
			this.Controls.Add(this.txtButtonA);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cboTrigger);
			this.Controls.Add(this.txtStringValue);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cboBooleanValue);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtIntegerValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtDoubleValue);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.cboProfiles);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cboProfiles;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MaskedTextBox txtDoubleValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MaskedTextBox txtIntegerValue;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cboBooleanValue;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtStringValue;
		private System.Windows.Forms.ComboBox cboTrigger;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtButtonA;
		private System.Windows.Forms.TextBox txtButtonB;
		private System.Windows.Forms.TextBox txtButtonX;
		private System.Windows.Forms.TextBox txtButtonY;
	}
}

