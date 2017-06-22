using System;
using System.Windows.Forms;

using lw.CTE;
using lw.Utils;

namespace Tools
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void encrypt_Click(object sender, EventArgs e)
		{
			Result.Text = Cryptography.Encrypt(Text.Text, AppConfig.__);
		}

		private void decrypt_Click(object sender, EventArgs e)
		{
			Result.Text = Cryptography.Decrypt(Text.Text, AppConfig.__);
		}

		private void Calc_Click(object sender, EventArgs e)
		{
			if(Dec.Checked)
				ResultText.Text = Cryptography.Decrypt(Password.Text, Key.Text);
			else
				ResultText.Text = Cryptography.Encrypt(Password.Text, Key.Text);
		}
	}
}
