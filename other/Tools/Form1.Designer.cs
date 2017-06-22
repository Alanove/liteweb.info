namespace Tools
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
			this.Tabs = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.decrypt = new System.Windows.Forms.Button();
			this.encrypt = new System.Windows.Forms.Button();
			this.Result = new System.Windows.Forms.TextBox();
			this.Text = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.Calc = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.ResultText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Key = new System.Windows.Forms.TextBox();
			this.Password = new System.Windows.Forms.TextBox();
			this.Dec = new System.Windows.Forms.CheckBox();
			this.Tabs.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// Tabs
			// 
			this.Tabs.Controls.Add(this.tabPage1);
			this.Tabs.Controls.Add(this.tabPage2);
			this.Tabs.Controls.Add(this.tabPage3);
			this.Tabs.Location = new System.Drawing.Point(3, 12);
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(612, 571);
			this.Tabs.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.decrypt);
			this.tabPage1.Controls.Add(this.encrypt);
			this.tabPage1.Controls.Add(this.Result);
			this.tabPage1.Controls.Add(this.Text);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(604, 545);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Connection String";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// decrypt
			// 
			this.decrypt.Location = new System.Drawing.Point(304, 250);
			this.decrypt.Name = "decrypt";
			this.decrypt.Size = new System.Drawing.Size(75, 23);
			this.decrypt.TabIndex = 3;
			this.decrypt.Text = "Decrypt";
			this.decrypt.UseVisualStyleBackColor = true;
			this.decrypt.Click += new System.EventHandler(this.decrypt_Click);
			// 
			// encrypt
			// 
			this.encrypt.Location = new System.Drawing.Point(202, 250);
			this.encrypt.Name = "encrypt";
			this.encrypt.Size = new System.Drawing.Size(75, 23);
			this.encrypt.TabIndex = 2;
			this.encrypt.Text = "Encrypt";
			this.encrypt.UseVisualStyleBackColor = true;
			this.encrypt.Click += new System.EventHandler(this.encrypt_Click);
			// 
			// Result
			// 
			this.Result.Location = new System.Drawing.Point(5, 292);
			this.Result.Multiline = true;
			this.Result.Name = "Result";
			this.Result.Size = new System.Drawing.Size(593, 247);
			this.Result.TabIndex = 1;
			// 
			// Text
			// 
			this.Text.Location = new System.Drawing.Point(5, 6);
			this.Text.Multiline = true;
			this.Text.Name = "Text";
			this.Text.Size = new System.Drawing.Size(593, 218);
			this.Text.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(604, 545);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Domains";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.Dec);
			this.tabPage3.Controls.Add(this.Calc);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.ResultText);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.Key);
			this.tabPage3.Controls.Add(this.Password);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(604, 545);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Password";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// Calc
			// 
			this.Calc.Location = new System.Drawing.Point(504, 188);
			this.Calc.Name = "Calc";
			this.Calc.Size = new System.Drawing.Size(75, 23);
			this.Calc.TabIndex = 6;
			this.Calc.Text = "Calc";
			this.Calc.UseVisualStyleBackColor = true;
			this.Calc.Click += new System.EventHandler(this.Calc_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 124);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(37, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Result";
			// 
			// ResultText
			// 
			this.ResultText.Location = new System.Drawing.Point(18, 143);
			this.ResultText.Name = "ResultText";
			this.ResultText.Size = new System.Drawing.Size(562, 20);
			this.ResultText.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Key";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Password";
			// 
			// Key
			// 
			this.Key.Location = new System.Drawing.Point(18, 95);
			this.Key.Name = "Key";
			this.Key.Size = new System.Drawing.Size(562, 20);
			this.Key.TabIndex = 1;
			// 
			// Password
			// 
			this.Password.Location = new System.Drawing.Point(18, 47);
			this.Password.Name = "Password";
			this.Password.Size = new System.Drawing.Size(563, 20);
			this.Password.TabIndex = 0;
			// 
			// Dec
			// 
			this.Dec.AutoSize = true;
			this.Dec.Location = new System.Drawing.Point(18, 170);
			this.Dec.Name = "Dec";
			this.Dec.Size = new System.Drawing.Size(63, 17);
			this.Dec.TabIndex = 7;
			this.Dec.Text = "Decrypt";
			this.Dec.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 595);
			this.Controls.Add(this.Tabs);
			this.Name = "Form1";
			this.Tabs.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl Tabs;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox Text;
		private System.Windows.Forms.TextBox Result;
		private System.Windows.Forms.Button encrypt;
		private System.Windows.Forms.Button decrypt;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox Password;
		private System.Windows.Forms.TextBox Key;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox ResultText;
		private System.Windows.Forms.Button Calc;
		private System.Windows.Forms.CheckBox Dec;

	}
}

