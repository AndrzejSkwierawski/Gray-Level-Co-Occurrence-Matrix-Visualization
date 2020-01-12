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
        int delllay = 5;
        private int cellSize = 40;
        private int startMatrixPosX = 20;
        private int startMatrixPosY = 20;
        int MatrixSize;
        List<List<int>> startMatrix = new List<List<int>>();
        Button[,] tabBtn;
        Button[,] changeMatrix;
        Button[,] transponedMatrix;
        Button[,] addedMatrix;
        Button[,] finalMatrix;

        Button sum;
        Button sumFinal;
        

        public Form1()
        {
            InitializeComponent();
            InitalizeMatrix(startMatrix);
            Start();
           
       }

        /// <summary>
		/// Renders matrix on first step
		/// </summary>
		private void Start()
        {
            int sizeListY = startMatrix.Count;
            int sizeListX = 0;
            if (sizeListY > 0)
            {
                sizeListX = startMatrix[0].Count;
            }

            Console.WriteLine(sizeListX + "  " + sizeListY);
            tabBtn = new Button[sizeListX, sizeListY];
            for (int i = 0; i < sizeListX; i++)
            {
                for (int j = 0; j < sizeListY; j++)
                {
                    tabBtn[i, j] = new Button();
                    tabBtn[i, j].Enabled = false;
                    tabBtn[i, j].BackColor = Color.White;
                    tabBtn[i, j].Location = new Point(startMatrixPosX + cellSize * i, startMatrixPosY + cellSize * j);
                    tabBtn[i, j].Size = new Size(cellSize, cellSize);
                    tabBtn[i, j].Visible = true;
                    tabBtn[i, j].Text = startMatrix[j][i].ToString();
                    this.Controls.Add(tabBtn[i, j]);
                }
            }

        }

        private void viewMatrix(Button[,] tabBtn2, int posX, int posY)
        {
            int size = GetMaxValue(startMatrix) ;

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
                    tabBtn2[i, j].Text = "0";
                    this.Controls.Add(tabBtn2[i, j]);
                    this.Refresh();
                }
            }
        }

        private void viewText(ref Button tabBtn, string text, int posX, int posY)
        {
            int size = GetMaxValue(startMatrix);

            tabBtn = new Button();
            tabBtn.Enabled = false;
            tabBtn.BackColor = Color.White;
            tabBtn.Location = new Point(posX + cellSize, posY + cellSize);
            tabBtn.Size = new Size(cellSize * 2, cellSize);
            tabBtn.Visible = true;
            tabBtn.Text = text;
            this.Controls.Add(tabBtn);
        }

        private void InitalizeMatrix(List<List<int>> matrix)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("exampleMatrix.txt"))
                {

                    string[] tmp;
                    List<int> tmpList;
                    int i = 0;
                    String line;
                    // Read the stream to a string, and write the string to the console.
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        tmp = line.Split('\t');
                        tmpList = new List<int>();
                        for (int j = 0; j < tmp.Length; j++)
                        {
                            Console.WriteLine(j);
                            tmpList.Add(int.Parse(tmp[j]));
                        }
                        matrix.Add(tmpList);
                        i++;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            MatrixSize = GetMaxValue(startMatrix);
            changeMatrix = new Button[MatrixSize, MatrixSize];
            transponedMatrix = new Button[MatrixSize, MatrixSize];
            addedMatrix = new Button[MatrixSize, MatrixSize];
            finalMatrix = new Button[MatrixSize, MatrixSize];
        }

        /// <summary>
		/// Gets the maximum value of matrix
		/// </summary>
		/// <param name="matrix">The Matrix</param>
		/// <returns>Maximum Value</returns>
		private int GetMaxValue(List<List<int>> matrix)
        {
            int sizeListY = startMatrix.Count;
            int sizeListX = 0;
            if (sizeListY > 0)
            {
                sizeListX = startMatrix[0].Count;
            }
            int max = 0;
            for (int i = 0; i < sizeListY; i++)
            {
                for (int j = 0; j < sizeListX; j++)
                {
                    if (matrix[i][j] > max)
                    {
                        max = matrix[i][j];

                        Console.WriteLine(i + ":" + j);
                    }
                }
            }
            Console.WriteLine(max);
            return max + 1;
        }

        private void countOccurance(Button[,] inputMatrix, Button[,] outputMatrix)
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                { 
                    outputMatrix[i, j].BackColor = Color.Gray;
                    outputMatrix[i, j].Text = SearchForOccurance(inputMatrix, j, i).ToString();
                   this.Refresh();
                    System.Threading.Thread.Sleep(delllay);
                    outputMatrix[i, j].BackColor = Color.White;
                    
                }
            }
           
        }

        private int SearchForOccurance(Button[,] inputMatrix, int x, int y)
        {
            int count = 0;
            Console.WriteLine("X=" + inputMatrix.GetLength(0) + " Y=" + inputMatrix.GetLength(1));

            for (int i = 0; i < inputMatrix.GetLength(0)-1; i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1); j++)
                {
                    if (int.Parse(inputMatrix[i, j].Text) == x)
                    {
                        if (int.Parse(inputMatrix[i + 1, j].Text) == y)
                        {
                            count++;
                            inputMatrix[i, j].BackColor = Color.Green;
                            inputMatrix[i+1, j].BackColor = Color.Green;
                            this.Refresh();
                            System.Threading.Thread.Sleep(delllay);
                            inputMatrix[i, j].BackColor = Color.White;
                            inputMatrix[i+1, j].BackColor = Color.White;
                        }
                    }
                }
            }
            return count;
        }

        private void transpozeMatrix(Button[,] inputMatrix, Button[,] transpozedOutputMatrix)
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                   
                   
                    inputMatrix[j, i].BackColor = Color.Yellow;
                    transponedMatrix[i, j].Text = inputMatrix[j, i].Text;
                    transponedMatrix[i, j].BackColor = Color.Orange;
                    this.Refresh();
                    System.Threading.Thread.Sleep(delllay);
                    inputMatrix[j, i].BackColor = Color.White;
                    transponedMatrix[i, j].BackColor = Color.White;
                    this.Refresh();

                }
                this.Refresh();
                System.Threading.Thread.Sleep(delllay);
            }
        }

        private void sumMatrix(Button[,] input1, Button[,] input2, Button[,] output)
        {
            int size = GetMaxValue(startMatrix);
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(delllay);
                    input1[i, j].BackColor = Color.Yellow;
                    input2[i, j].BackColor = Color.Yellow;
                    output[i, j].BackColor = Color.Green;
                    output[i, j].Text = (int.Parse(input1[i, j].Text) + int.Parse(input2[i, j].Text)).ToString();
                    this.Refresh();
                    System.Threading.Thread.Sleep(delllay);
                    input1[i, j].BackColor = Color.White;
                    input2[i, j].BackColor = Color.White;
                    output[i, j].BackColor = Color.White;
                    this.Refresh();
                }
            }
        }

        private void sumCount(Button[,] input, ref Button output)
        {
            int size = GetMaxValue(startMatrix);
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(delllay);
                    input[i, j].BackColor = Color.Yellow;
                    output.BackColor = Color.Gray;
                    sum += int.Parse(input[i, j].Text);
                    output.Text =  sum.ToString();
                    this.Refresh();
                    System.Threading.Thread.Sleep(delllay);

                    input[i, j].BackColor = Color.White;
                    output.BackColor = Color.White;
                    this.Refresh();
                }
            }
        }

        private void devideMatrix(Button[,] input, Button devider, Button[,] output)
        {
            int size = GetMaxValue(startMatrix);
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    output[i, j].Text = Math.Round((int.Parse(input[i, j].Text) / double.Parse(devider.Text)), 2).ToString();
                    sum += double.Parse(output[i, j].Text);
                }
            }
        }

        private void sumFinalCount(Button[,] input, ref Button output)
        {
            int size = GetMaxValue(startMatrix);
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(delllay);
                    input[i, j].BackColor = Color.Yellow;
                    output.BackColor = Color.Gray;
                    sum += double.Parse(input[i, j].Text);
                    output.Text = sum.ToString();
                    this.Refresh();
                    System.Threading.Thread.Sleep(delllay);

                    input[i, j].BackColor = Color.White;
                    output.BackColor = Color.White;
                    this.Refresh();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            viewMatrix(changeMatrix, startMatrixPosX + cellSize * (MatrixSize + 1), startMatrixPosY);
            countOccurance(tabBtn, changeMatrix);
            viewMatrix(transponedMatrix, startMatrixPosX + cellSize * (MatrixSize * 2 + 2), startMatrixPosY);
            transpozeMatrix(changeMatrix, transponedMatrix);
            viewMatrix(addedMatrix, startMatrixPosX + cellSize * (MatrixSize + 1), startMatrixPosY + cellSize * (MatrixSize + 1));
            sumMatrix(changeMatrix, transponedMatrix, addedMatrix);
            
            //todo: display sum
            viewText(ref sum, "0", startMatrixPosX + cellSize, startMatrixPosY + cellSize * (MatrixSize + 1));
            sumCount(addedMatrix,ref sum);
            viewMatrix(finalMatrix, startMatrixPosX + cellSize * (MatrixSize * 2 + 2), startMatrixPosY + cellSize * (MatrixSize + 1));
            devideMatrix(addedMatrix, sum, finalMatrix);
            //todo: display sumFinal
            viewText(ref sumFinal, "0", startMatrixPosX + cellSize, startMatrixPosY + cellSize * (MatrixSize + 2));
            sumFinalCount(finalMatrix, ref sumFinal);
        }
    }
}



