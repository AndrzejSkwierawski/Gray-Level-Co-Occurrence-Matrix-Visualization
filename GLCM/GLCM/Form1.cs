using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GLCM
{
    public partial class Form1 : Form
    {
		private int maxDellay = 3000;
		private int minDellay = 3;
        private int dellay = 50;
        private int cellSize = 40;
        private int startMatrixPosX = 50;
        private int startMatrixPosY = 50;
        private int MatrixSize;
		private int numberOfLevels;
		private double resolution;
		List<List<int>> startMatrix = new List<List<int>>();
        Button[,] dataMatrix;
        Button[,] tabBtn;
		Button[,] changeMatrix;
        Button[,] transponedMatrix;
        Button[,] addedMatrix;
        Button[,] finalMatrix;

        Button sum;
        Button sumFinal;
        Thread thread2;


        private string fileName;

		private double maxPixelValue = 255.0; 

		public Form1()
        {
            InitializeComponent();
			startToolStripMenuItem.Enabled = false;
			this.numberOfLevels = Convert.ToInt16(NoL.Value);
			resolution = maxPixelValue / (this.numberOfLevels - 1);
            comboBox1.SelectedIndex = 0;
            thread2 = new Thread(initThread);

        }

        void initThread()
        {

        }

        private void demoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fileName = "exampleMatrix.txt";
			clearAll();
			InitalizeMatrix(startMatrix);
			Start();
		}

		/// <summary>
		/// Selects the file whitch will be written
		/// </summary>
		private void OpenFile()
		{
			this.openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				clearAll();
				this.Refresh();
				fileName = openFileDialog1.FileName;
				InitalizeMatrix(startMatrix);
				Start();
			}
		}

		private void ClearMatrix(Button[,] matrix)
		{
			if (matrix != null)
			{
				int sizeX = matrix.GetLength(0);
				int sizeY = matrix.GetLength(1);
				for (int i = 0; i < sizeX; i++)
				{
					for (int j = 0; j < sizeY; j++)
					{

                        this.Invoke(new Action(() => Controls.Remove(matrix[i, j])));
					}
				}
			}
		}

		private void ClearText(Button text)
		{
			if (text != null)
			{
                this.Invoke(new Action(() => Controls.Remove(text)));
			}
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
			dataMatrix = new Button[sizeListX, sizeListY];
            for (int i = 0; i < sizeListX; i++)
            {
                for (int j = 0; j < sizeListY; j++)
                {
					dataMatrix[i, j] = new Button();
					dataMatrix[i, j].Enabled = false;
					dataMatrix[i, j].BackColor = Color.White;
					dataMatrix[i, j].Location = new Point(startMatrixPosX + cellSize * i, startMatrixPosY + cellSize * j);
					dataMatrix[i, j].Size = new Size(cellSize, cellSize);
					dataMatrix[i, j].Visible = true;
					dataMatrix[i, j].Text = startMatrix[j][i].ToString();
                    this.Controls.Add(dataMatrix[i, j]);
                }
            }
			tabBtn = new Button[dataMatrix.GetLength(0), dataMatrix.GetLength(1)];
			startToolStripMenuItem.Enabled = true;
        }

		private void ConvertToLevels()
		{
            double tmpResolution = 0 ;
			int sizeX = startMatrix.Count;
			int sizeY = startMatrix[0].Count;
			for (int i = 0; i < sizeX; i++)
			{
				for(int j = 0; j < sizeY; j++)
				{
					tabBtn[i, j].BackColor = Color.Gray;
                    this.Invoke(new Action(() => tmpResolution = resolution));
                    this.Invoke(new Action(() => tabBtn[i, j].Text = Convert.ToInt16((Convert.ToInt16(dataMatrix[i, j].Text) / tmpResolution)).ToString()));
                    
                    this.Invoke(new Action(() => Refresh()));
                   // this.Refresh();
					System.Threading.Thread.Sleep(dellay);
					tabBtn[i, j].BackColor = Color.White;
				}
			}
		}

		/// <summary>
		/// ViewMatrix
		/// </summary>
		/// <param name="matrix">The Matrix</param>
		/// <param name="posX">Start position x</param>
		/// <param name="posY">Start position y</param>
		private void viewMatrix(Button[,] matrix, int posX, int posY, bool origin = false)
        {
			int sizeX;
			int sizeY;

			if (origin)
			{
				sizeX = startMatrix.Count;
				sizeY = startMatrix[0].Count;
			}
			else
			{
				sizeX = this.numberOfLevels;
				sizeY = sizeX;
			}
			for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    matrix[i, j] = new Button();
                    matrix[i, j].Enabled = false;
                    matrix[i, j].BackColor = Color.White;
                    matrix[i, j].Location = new Point(posX + cellSize * i, posY + cellSize * j);
                    matrix[i, j].Size = new Size(cellSize, cellSize);
                    matrix[i, j].Visible = true;
                    matrix[i, j].Text = "0";

                    this.Invoke(new Action(() => Controls.Add(matrix[i, j])));
                    this.Invoke(new Action(() => Refresh()));
                     
                    //this.Controls.Add(matrix[i, j]);
                    //this.Refresh();
                }
            }
        }

		/// <summary>
		/// Views text in one field
		/// </summary>
		/// <param name="field">Field</param>
		/// <param name="text">Content</param>
		/// <param name="posX">Start posiotion x</param>
		/// <param name="posY">Start posiotion y</param>
		private void viewText(ref Button field, string text, int posX, int posY)
        {
            field = new Button();
            field.Enabled = false;
            field.BackColor = Color.White;
            field.Location = new Point(posX + cellSize, posY + cellSize);
            field.Size = new Size(cellSize * 2, cellSize);
            field.Visible = true;
            field.Text = text;
            Button fieldTmp = field;
            Invoke(new Action(() => Controls.Add(fieldTmp)));
           // this.Controls.Add(field);

        }

		/// <summary>
		/// Gets the values from choosen file.
		/// </summary>
		/// <param name="matrix">The Matrix</param>
        private void InitalizeMatrix(List<List<int>> matrix)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
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

            MatrixSize = this.numberOfLevels;
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

	

		/// <summary>
		/// Transpozes given matrix
		/// </summary>
		/// <param name="inputMatrix">Input Matrix</param>
		/// <param name="transpozedOutputMatrix">Transpozed Matrix</param>
		private void transpozeMatrix(Button[,] inputMatrix, Button[,] transpozedOutputMatrix)
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                    inputMatrix[j, i].BackColor = Color.Yellow;
                    this.Invoke(new Action(()=>transponedMatrix[i, j].Text = inputMatrix[j, i].Text));
                    transponedMatrix[i, j].BackColor = Color.Orange;
                    this.Invoke(new Action(() => Refresh()));
                    System.Threading.Thread.Sleep(dellay);
                    inputMatrix[j, i].BackColor = Color.White;
                    transponedMatrix[i, j].BackColor = Color.White;
                    this.Invoke(new Action(() => Refresh()));
                }
                this.Invoke(new Action(() => Refresh()));
                System.Threading.Thread.Sleep(dellay);
            }
        }

		/// <summary>
		/// Sums all values in given matrixes.
		/// </summary>
		/// <param name="input1">Matrix1</param>
		/// <param name="input2">Matrix2</param>
		/// <param name="output">Sum of two matrixes</param>
		private void sumMatrix(Button[,] input1, Button[,] input2, Button[,] output)
        {
			//int size = GetMaxValue(startMatrix);
			int size = this.numberOfLevels;
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(dellay);
                    input1[i, j].BackColor = Color.Yellow;
                    input2[i, j].BackColor = Color.Yellow;
                    output[i, j].BackColor = Color.Green;
                    //output[i, j].Text = (int.Parse(input1[i, j].Text) + int.Parse(input2[i, j].Text)).ToString();
                    this.Invoke(new Action(() => output[i, j].Text = (int.Parse(input1[i, j].Text) + int.Parse(input2[i, j].Text)).ToString()));
                    this.Invoke(new Action(() => Refresh()));
                    System.Threading.Thread.Sleep(dellay);
                    input1[i, j].BackColor = Color.White;
                    input2[i, j].BackColor = Color.White;
                    output[i, j].BackColor = Color.White;
                    this.Invoke(new Action(() => Refresh()));
                }
            }
        }

        private void sumCount(Button[,] input, ref Button output)
        {
			//int size = GetMaxValue(startMatrix);
			int size = this.numberOfLevels;
			int sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(dellay);
                    input[i, j].BackColor = Color.Yellow;
                    output.BackColor = Color.Gray;
                    sum += int.Parse(input[i, j].Text);
                    Button outputTmp = output;
                     this.Invoke(new Action(() => outputTmp.Text =  sum.ToString()));
                    this.Invoke(new Action(() => Refresh()));
                    System.Threading.Thread.Sleep(dellay);

                    input[i, j].BackColor = Color.White;
                    output.BackColor = Color.White;
                    this.Invoke(new Action(() => Refresh()));
                }
            }
        }

        private void devideMatrix(Button[,] input, Button devider, Button[,] output)
        {
			//int size = GetMaxValue(startMatrix);
			int size = this.numberOfLevels;
			double sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.Invoke(new Action(() => output[i, j].Text = Math.Round((int.Parse(input[i, j].Text) / double.Parse(devider.Text)), 2).ToString()));
                    sum += double.Parse(output[i, j].Text);
                }
            }
        }

        private void sumFinalCount(Button[,] input, ref Button output)
        {
			//int size = GetMaxValue(startMatrix);
			int size = this.numberOfLevels;
			double sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    System.Threading.Thread.Sleep(dellay);
                    input[i, j].BackColor = Color.Yellow;
                    output.BackColor = Color.Gray;
                    sum += double.Parse(input[i, j].Text);
                    Button outputTmp = output;
                    this.Invoke(new Action(() => outputTmp.Text = sum.ToString()));
                    this.Invoke(new Action(() => Refresh()));
                    System.Threading.Thread.Sleep(dellay);

                    input[i, j].BackColor = Color.White;
                    output.BackColor = Color.White;
                    this.Invoke(new Action(() => Refresh()));
                }
            }
        }

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{
            button1.Enabled = true;
            button2.Enabled = false;
            comboBox1.Enabled = false;

            thread2 = new Thread(simulation);
            
            thread2.Start();

		}

        private void simulation()
        {
            dellay = getDelay();
            menuStrip1.Invoke(new Action(() => startToolStripMenuItem.Enabled = false));
            menuStrip1.Invoke(new Action(() => demoToolStripMenuItem.Enabled = false));
            menuStrip1.Invoke(new Action(() => openToolStripMenuItem.Enabled = false));
            //startToolStripMenuItem.Enabled = false;
            //demoToolStripMenuItem.Enabled = false;
            //openToolStripMenuItem.Enabled = false;

            clearAll(false);

			viewMatrix(tabBtn, startMatrixPosX + cellSize * (MatrixSize + 1), startMatrixPosY, true);
			ConvertToLevels();

			viewMatrix(changeMatrix, startMatrixPosX + cellSize * (MatrixSize * 2 + 2), startMatrixPosY);
			countOccurance(tabBtn, changeMatrix);
			viewMatrix(transponedMatrix, startMatrixPosX + cellSize * (MatrixSize * 3 + 3), startMatrixPosY);
			transpozeMatrix(changeMatrix, transponedMatrix);
			viewMatrix(addedMatrix, startMatrixPosX + cellSize * (MatrixSize + 1), startMatrixPosY + cellSize * (MatrixSize + 1));
			sumMatrix(changeMatrix, transponedMatrix, addedMatrix);

			viewText(ref sum, "0", startMatrixPosX + cellSize, startMatrixPosY + cellSize * (MatrixSize + 1));
			sumCount(addedMatrix, ref sum);
			viewMatrix(finalMatrix, startMatrixPosX + cellSize * (MatrixSize * 2 + 2), startMatrixPosY + cellSize * (MatrixSize + 1));
			devideMatrix(addedMatrix, sum, finalMatrix);

			viewText(ref sumFinal, "0", startMatrixPosX + cellSize, startMatrixPosY + cellSize * (MatrixSize + 2));
			sumFinalCount(finalMatrix, ref sumFinal);

            menuStrip1.Invoke(new Action(() => startToolStripMenuItem.Enabled = true));
            menuStrip1.Invoke(new Action(() => demoToolStripMenuItem.Enabled = true));
            menuStrip1.Invoke(new Action(() => openToolStripMenuItem.Enabled = true));
            this.Invoke(new Action(() => comboBox1.Enabled = true));
            thread2.Interrupt();

   //         startToolStripMenuItem.Enabled = true;
			//demoToolStripMenuItem.Enabled = true;
			//openToolStripMenuItem.Enabled = true;
        }

		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			const string message = " Andrzej Skwierawski,\n Michał Woźniak";
			const string caption = "Copyright ©. All right reserved. ";
			var result = MessageBox.Show(message, caption,
										 MessageBoxButtons.OK,
										 MessageBoxIcon.Information);

		}

		private void speedBar_Scroll(object sender, EventArgs e)
		{
			dellay = getDelay();
		}

		private int getDelay()
		{
            var speed = 0 ;
            speedBar.Invoke(new Action(() =>  speed= speedBar.Value));
            
			return maxDellay - speed * minDellay * 10;
		}

		private void NoL_ValueChanged(object sender, EventArgs e)
		{
			this.numberOfLevels = Convert.ToInt16(NoL.Value);
			resolution = maxPixelValue / (this.numberOfLevels - 1);
			clearAll(false);
			MatrixSize = this.numberOfLevels;
			changeMatrix = new Button[MatrixSize, MatrixSize];
			transponedMatrix = new Button[MatrixSize, MatrixSize];
			addedMatrix = new Button[MatrixSize, MatrixSize];
			finalMatrix = new Button[MatrixSize, MatrixSize];
		}

		private void clearAll(bool origin = true)
		{
			if (origin)
			{
				startMatrix.Clear();
				ClearMatrix(dataMatrix);
			}
			ClearMatrix(tabBtn);
			ClearMatrix(changeMatrix);
			ClearMatrix(transponedMatrix);
			ClearMatrix(addedMatrix);
			ClearMatrix(finalMatrix);
			ClearText(sum);
			ClearText(sumFinal);
            this.Invoke(new Action(() => Refresh()));
			//this.Refresh();
		}

        private void button1_Click(object sender, EventArgs e)
        {
            thread2.Suspend();
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread2.Resume();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {            
            if (thread2.IsAlive)
            {
                thread2.Abort();
            }                      
        }


        /// <summary>
        /// Counts all occurances in inputMatrix and sets those values in outputMatrix
        /// </summary>
        /// <param name="inputMatrix">Input Matrix</param>
        /// <param name="outputMatrix">Output Matrix</param>
        private void countOccurance(Button[,] inputMatrix, Button[,] outputMatrix)
        {
            int switchTmp = 0;
            this.Invoke(new Action(() => switchTmp = comboBox1.SelectedIndex));
            for (int i = 0; i < MatrixSize; i++)
            {
                for (int j = 0; j < MatrixSize; j++)
                {
                    outputMatrix[i, j].BackColor = Color.Gray;
                    switch(switchTmp)
                    {
                        case 0:
                            this.Invoke(new Action(() => outputMatrix[i, j].Text = SearchForOccurance10(inputMatrix, j, i).ToString()));
                            break;
                        case 1:
                            this.Invoke(new Action(() => outputMatrix[i, j].Text = SearchForOccurance01(inputMatrix, j, i).ToString()));
                            break;
                        case 2:
                            this.Invoke(new Action(() => outputMatrix[i, j].Text = SearchForOccurance11(inputMatrix, j, i).ToString()));
                            break;
                        case 3:
                            this.Invoke(new Action(() => outputMatrix[i, j].Text = SearchForOccurance1m1(inputMatrix, j, i).ToString()));
                            break;
                    }

                    this.Invoke(new Action(() => Refresh()));
                    System.Threading.Thread.Sleep(dellay);
                    outputMatrix[i, j].BackColor = Color.White;
                }
            }

        }

        /// <summary>
        /// Searches for occurance in given matrix
        /// </summary>
        /// <param name="inputMatrix">The Matrix</param>
        /// <param name="x">Occurance from values</param>
        /// <param name="y">Occurance to values</param>
        /// <returns>Total amound of occurances in given matrix</returns>
        private int SearchForOccurance01(Button[,] inputMatrix, int x, int y)
        {
            int count = 0;
            Console.WriteLine("X=" + inputMatrix.GetLength(0) + " Y=" + inputMatrix.GetLength(1));

            for (int i = 0; i < inputMatrix.GetLength(0) ; i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1) - 1; j++)
                {
                    if (Convert.ToInt16(inputMatrix[i, j].Text) == x)
                    {
                        if (int.Parse(inputMatrix[i , j + 1].Text) == y)
                        {
                            count++;
                            inputMatrix[i, j].BackColor = Color.Green;
                            inputMatrix[i, j+1].BackColor = Color.Green;
                            this.Invoke(new Action(() => Refresh()));
                            //this.Refresh();
                            System.Threading.Thread.Sleep(dellay);
                            inputMatrix[i, j].BackColor = Color.White;
                            inputMatrix[i , j + 1].BackColor = Color.White;
                        }
                    }
                }
            }
            return count;
        }

        private int SearchForOccurance10(Button[,] inputMatrix, int x, int y)
        {
            int count = 0;
            Console.WriteLine("X=" + inputMatrix.GetLength(0) + " Y=" + inputMatrix.GetLength(1));

            for (int i = 0; i < inputMatrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1); j++)
                {
                    if (Convert.ToInt16(inputMatrix[i, j].Text) == x)
                    {
                        if (int.Parse(inputMatrix[i + 1, j].Text) == y)
                        {
                            count++;
                            inputMatrix[i, j].BackColor = Color.Green;
                            inputMatrix[i + 1, j].BackColor = Color.Green;
                            this.Invoke(new Action(() => Refresh()));
                            //this.Refresh();
                            System.Threading.Thread.Sleep(dellay);
                            inputMatrix[i, j].BackColor = Color.White;
                            inputMatrix[i + 1, j].BackColor = Color.White;
                        }
                    }
                }
            }
            return count;
        }

        private int SearchForOccurance11(Button[,] inputMatrix, int x, int y)
        {
            int count = 0;
            Console.WriteLine("X=" + inputMatrix.GetLength(0) + " Y=" + inputMatrix.GetLength(1));

            for (int i = 0; i < inputMatrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1) - 1; j++)
                {
                    if (Convert.ToInt16(inputMatrix[i, j].Text) == x)
                    {
                        if (int.Parse(inputMatrix[i + 1, j + 1].Text) == y)
                        {
                            count++;
                            inputMatrix[i, j].BackColor = Color.Green;
                            inputMatrix[i + 1, j + 1].BackColor = Color.Green;
                            this.Invoke(new Action(() => Refresh()));
                            //this.Refresh();
                            System.Threading.Thread.Sleep(dellay);
                            inputMatrix[i, j].BackColor = Color.White;
                            inputMatrix[i + 1, j + 1].BackColor = Color.White;
                        }
                    }
                }
            }
            return count;
        }

        private int SearchForOccurance1m1(Button[,] inputMatrix, int x, int y)
        {
            int count = 0;
            Console.WriteLine("X=" + inputMatrix.GetLength(0) + " Y=" + inputMatrix.GetLength(1));

            for (int i = 1; i < inputMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1) - 1; j++)
                {
                    if (Convert.ToInt16(inputMatrix[i, j].Text) == x)
                    {
                        if (int.Parse(inputMatrix[i - 1, j + 1].Text) == y)
                        {
                            count++;
                            inputMatrix[i, j].BackColor = Color.Green;
                            inputMatrix[i - 1, j + 1].BackColor = Color.Green;
                            this.Invoke(new Action(() => Refresh()));
                            //this.Refresh();
                            System.Threading.Thread.Sleep(dellay);
                            inputMatrix[i, j].BackColor = Color.White;
                            inputMatrix[i - 1, j + 1].BackColor = Color.White;
                        }
                    }
                }
            }
            return count;
        }

    }
}



