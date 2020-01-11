using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLCM
{
	public partial class Form1 : Form
	{
		int[,] startMatrix = new int[4, 4];
		int[,] changeMatrix = new int[4, 4];
		int[,] transponedMatrix = new int[4, 4];
		int[,] addedMatrix = new int[4, 4];

		int sume;

		double[,] devidedMatrix = new double[4, 4];

		Button[,] tabBtn;
		public Form1()
		{
			InitializeComponent();
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					startMatrix[i, j] = 1;
				}
			}


			tabBtn = new Button[4, 4];
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					tabBtn[i, j] = new Button();
					//tabBtn[i, j].SetBounds(31+20*i,31+20*j,20,20);
					tabBtn[i, j].Enabled = false;
					tabBtn[i, j].BackColor = Color.White;
					tabBtn[i, j].Location = new Point(21 + 20 * i, 21 + 20 * j);
					tabBtn[i, j].Size = new Size(20, 20);
					tabBtn[i, j].Visible = true;
					tabBtn[i, j].Text = startMatrix[i, j].ToString();
					this.Controls.Add(tabBtn[i, j]);
				}
			}
		}
	}
}
