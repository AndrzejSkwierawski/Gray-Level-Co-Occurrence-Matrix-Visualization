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
		public const int MatrixSize = 4;

		private int cellSize = 20;
		private int startMatrixPosX = 21;
		private int startMatrixPosY = 21;


		int[,] startMatrix = new int[MatrixSize, MatrixSize];
		int[,] changeMatrix = new int[MatrixSize, MatrixSize];
		int[,] transponedMatrix = new int[MatrixSize, MatrixSize];
		int[,] addedMatrix = new int[MatrixSize, MatrixSize];

		private int sum;

		private double[,] devidedMatrix = new double[MatrixSize, MatrixSize];
		private Button[,] tabBtn  = new Button[MatrixSize, MatrixSize];

		public Form1()
		{
			InitializeComponent();
			InitalizeMatrix(startMatrix);
			Start();
			//tabBtn[0, 0].Text = GetMaxValue(startMatrix).ToString();
		}

		/// <summary>
		/// Renders matrix on first step
		/// </summary>
		private void Start()
		{
			for (int i = 0; i < MatrixSize; i++)
			{
				for (int j = 0; j < MatrixSize; j++)
				{
					tabBtn[i, j] = new Button();
					tabBtn[i, j].Enabled = false;
					tabBtn[i, j].BackColor = Color.White;
					tabBtn[i, j].Location = new Point(startMatrixPosX + cellSize * i, startMatrixPosY + cellSize * j);
					tabBtn[i, j].Size = new Size(cellSize, cellSize);
					tabBtn[i, j].Visible = true;
					tabBtn[i, j].Text = startMatrix[i, j].ToString();
					this.Controls.Add(tabBtn[i, j]);
				}
			}

			//after we have the needed 
			//int newMatrixSize
		}

		private void InitalizeMatrix(int[,] matrix)
		{
			for (int i = 0; i < MatrixSize; i++)
			{
				for (int j = 0; j < MatrixSize; j++)
				{
					matrix[i, j] = i*j;
				}
			}
		}
		/// <summary>
		/// Gets the maximum value of matrix
		/// </summary>
		/// <param name="matrix">The Matrix</param>
		/// <returns>Maximum Value</returns>
		private int GetMaxValue(int[,] matrix)
		{
			int max = 0;
			for (int i = 0; i < MatrixSize; i++)
			{
				for (int j = 0; j < MatrixSize; j++)
				{
					if (matrix[i, j] > max)
					{
						max = matrix[i, j];
					}
				}
			}
			return max;
		}

		


	}
}
