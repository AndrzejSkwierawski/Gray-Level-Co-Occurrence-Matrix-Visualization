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
		private int startMatrixPosX = 20;
		private int startMatrixPosY = 20;


		int[,] startMatrix = new int[MatrixSize, MatrixSize];
		int[,] changeMatrix;
		int[,] transponedMatrix;
		int[,] addedMatrix;

		double[,] finalMatrix;

		private int sum;
		private double sumFinal;

		private double[,] devidedMatrix = new double[MatrixSize, MatrixSize];
		private Button[,] tabBtn  = new Button[MatrixSize, MatrixSize];

		public Form1()
		{
			InitializeComponent();
			InitalizeMatrix(startMatrix);
			Start();
			countOccurance(startMatrix, changeMatrix, transponedMatrix);
			viewMatrix(changeMatrix, 120, 20);
			viewMatrix(transponedMatrix, 220, 20);
			sumMatrix(changeMatrix, transponedMatrix, addedMatrix, ref sum);
			viewMatrix(addedMatrix, 320, 20);
			//todo: display sum
			viewText(sum.ToString(), 320, 120);
			devideMatrix(addedMatrix, sum, finalMatrix, ref sumFinal);
			viewMatrix(finalMatrix, 420, 20);
			//todo: display sumFinal
			viewText(sumFinal.ToString(), 420, 120);

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
			int newMatrixSize = GetMaxValue(startMatrix);
			changeMatrix = new int[newMatrixSize, newMatrixSize];
			transponedMatrix = new int[newMatrixSize, newMatrixSize];
			addedMatrix = new int[newMatrixSize, newMatrixSize];
			finalMatrix = new double[newMatrixSize, newMatrixSize];
		}

		private void viewMatrix(int[,] matrix, int posX, int posY)
		{
			int size = GetMaxValue(startMatrix);
		Button[,] tabBtn2 = new Button[size, size];

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					tabBtn2[i, j] = new Button();
					tabBtn2[i, j].Enabled = false;
					tabBtn2[i, j].BackColor = Color.White;
					tabBtn2[i, j].Location = new Point(posX + cellSize * i, posY + cellSize * j);
					tabBtn2[i, j].Size = new Size(cellSize, cellSize);
					tabBtn2[i, j].Visible = true;
					tabBtn2[i, j].Text = matrix[i, j].ToString();
					this.Controls.Add(tabBtn2[i, j]);
				}
			}
		}

		private void viewMatrix(double[,] matrix, int posX, int posY)
		{
			int size = GetMaxValue(startMatrix);
			Button[,] tabBtn2 = new Button[size, size];

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					tabBtn2[i, j] = new Button();
					tabBtn2[i, j].Enabled = false;
					tabBtn2[i, j].BackColor = Color.White;
					tabBtn2[i, j].Location = new Point(posX + cellSize*3 * i, posY + cellSize * j);
					tabBtn2[i, j].Size = new Size(cellSize*3, cellSize);
					tabBtn2[i, j].Visible = true;
					tabBtn2[i, j].Text = matrix[i, j].ToString();
					this.Controls.Add(tabBtn2[i, j]);
				}
			}
		}

		private void viewText(string text, int posX, int posY)
		{
			int size = GetMaxValue(startMatrix);

					Button tabBtn = new Button();
					tabBtn.Enabled = false;
					tabBtn.BackColor = Color.White;
					tabBtn.Location = new Point(posX + cellSize, posY + cellSize);
					tabBtn.Size = new Size(cellSize*2, cellSize);
					tabBtn.Visible = true;
					tabBtn.Text = text;
					this.Controls.Add(tabBtn);
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
			return max+1;
		}

		private void countOccurance(int[,] inputMatrix, int[,] outputMatrix, int[,] transpozedOutputMatrix)
		{
			for (int i = 0; i < MatrixSize; i++)
			{
				for (int j = 0; j < MatrixSize; j++)
				{
					outputMatrix[i,j] =  SearchForOccurance(inputMatrix, j, i);
					transpozedOutputMatrix[i, j] = SearchForOccurance(inputMatrix, i, j);
				}
			}
		}

		private int SearchForOccurance(int[,] inputMatrix, int x, int y)
		{
			int count = 0;
			for (int i = 0; i < MatrixSize; i++)
			{
				for (int j = 0; j < MatrixSize-1; j++)
				{
					if (inputMatrix[i,j] == x)
					{
						if (inputMatrix[i, j+1] == y)
						{
							count++;
						}
					}
				}
			}
			return count;
		}

		private void sumMatrix(int[,] input1, int[,]input2, int[,]output, ref int sum)
		{
			int size = GetMaxValue(startMatrix);
			sum = 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					output[i, j] = input1[i, j] + input2[i, j];
					sum += output[i, j];
				}
			}
		}

		private void devideMatrix(int[,] input, int devider, double[,] output, ref double sum)
		{
			int size = GetMaxValue(startMatrix);
			sum = 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					output[i, j] = input[i, j] / (double)devider;
					sum += output[i, j];
				}
			}
		}


	}
}
