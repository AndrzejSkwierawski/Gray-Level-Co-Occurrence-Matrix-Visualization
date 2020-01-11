using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
		}

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
		}

		private void InitalizeMatrix(int[,] matrix)
		{
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("exampleMatrix.txt"))
                {
                    bool isInitializedMatrix = false;
                    string[] tmp;
                    String line;
                    // Read the stream to a string, and write the string to the console.
                    for (int i = 0; i < MatrixSize; i++)
                    {
                        line = sr.ReadLine();
                        tmp = line.Split('\t');
                        if (isInitializedMatrix)
                        {
                            startMatrix = new int[tmp.Length, tmp.Length];
                            isInitializedMatrix = true;
                        }
                        for (int j = 0; j < MatrixSize; j++)
                        {
                            matrix[i, j] = int.Parse(tmp[j]);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
          
		}


	}
}
