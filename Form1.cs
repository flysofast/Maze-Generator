using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenMapXeTang
{
    public partial class Form1 : Form
    {
        int[,] theMainGrid;
        int mainGridSize = 15;
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Quyết định cắt theo chiều ngang hay chiều dọc
        /// </summary>
        /// <param name="width">chiều dài ô grid</param>
        /// <param name="height">chiều rộng ô grid</param>
        /// <returns>true: cắt ngang, false: cắt dọc</returns>
        bool willDoHorizontalDivide(int width, int height)
        {
            if (width < height)
            {
                return true;
            }
            else if (width > height)
            {
                return false;
            }
            else
            {
                return random.Next(2) == 0 ? true : false;
            }
        }
        /// <summary>
        /// Cắt ô
        /// </summary>
        /// <param name="x">Vị trí gốc của grid</param>
        /// <param name="y">Vị trí bắt đầu gốc của grid</param>
        /// <param name="w">chiều dài</param>
        /// <param name="h">Chiều cao</param>
        void devide(int x, int y, int w, int h)
        {
            if (progBar.Value < 90)
            {
                progBar.Value++;
            }
            bool horizontal = willDoHorizontalDivide(w, h);
            //Nếu ô ngắn hơn 2 thì ko cần cắt nữa
            if (w < 2 || h < 2) return;

            //Vị trí đường cắt
            int lineX = x + (horizontal ? 0 : random.Next(w - 2));
            int lineY = y + (horizontal ? random.Next(h - 2) : 0);
            //if (lineX == mainGridSize - 1 || lineY == mainGridSize)
            //{
            //    int a = 0;
            //}
            //Vị trí cửa trên đường cắt
            int pX = lineX + (horizontal ? random.Next(w) : 0);
            int pY = lineY + (horizontal ? 0 : random.Next(h));

            //Vector di chuyển
            int dX = horizontal ? 1 : 0;
            int dY = horizontal ? 0 : 1;

            //Số lần dịch con trỏ vẽ đường chia cắt
            int times = horizontal ? w : h;

            int lineXClone = lineX, lineYClone = lineY;

            string chieucat = horizontal ? "ngang" : "doc";
            Console.WriteLine("Cat theo chieu " + chieucat + " tai vi tri (" + lineX + "," + lineY + "), de cua o (" + pX + "," + pY + ")");
            //Kẻ đường chia cắt
            for (int i = 1; i <= times; i++)
            {

                //Nếu không phải là ô cửa
                if (lineX != pX || lineY != pY)
                {
                    //10:S (dưới), 01:E (phải)
                    theMainGrid[lineX, lineY] |= (horizontal ? 2 : 1);
                }

                lineX += dX;
                lineY += dY;
            }



            //Đệ quy
            int sx, sy, sw, sh;
            sx = x;
            sy = y;
            sw = horizontal ? w : lineXClone - x + 1;
            sh = horizontal ? lineYClone - y + 1 : h;
            devide(sx, sy, sw, sh);

            sx = horizontal ? x : lineXClone + 1;
            sy = horizontal ? lineYClone + 1 : y;
            sw = horizontal ? w : w - (lineXClone - x + 1);
            sh = horizontal ? h - (lineYClone - y + 1) : h;
            devide(sx, sy, sw, sh);

        }

        void showResult()
        {
            Console.WriteLine("Ket qua de");
            for (int i = 0; i < mainGridSize; i++)
            {
                for (int j = 0; j < mainGridSize; j++)
                {
                    Console.Write(theMainGrid[j, i] + " ");
                }
                Console.Write("\n");
            }


        }

        void showMaze()
        {
            int cellWidth = 20;
            int offSet = 50;
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            formGraphics.Clear(Color.Transparent);
            formGraphics.DrawLine(myPen, offSet, offSet, offSet + cellWidth * mainGridSize, offSet);
            formGraphics.DrawLine(myPen, offSet, offSet, offSet, offSet + cellWidth * mainGridSize);
            formGraphics.DrawLine(myPen, offSet, offSet + cellWidth * mainGridSize, offSet + cellWidth * mainGridSize, offSet + cellWidth * mainGridSize);
            formGraphics.DrawLine(myPen, offSet + cellWidth * mainGridSize, offSet, offSet + cellWidth * mainGridSize, offSet + cellWidth * mainGridSize);
            //formGraphics.DrawLine(myPen, 0, 0, 200, 200);

            Console.WriteLine("\n\n");
            for (int i = 0; i < mainGridSize; i++)
            {
                for (int j = 0; j < mainGridSize; j++)
                {
                    switch (theMainGrid[j, i])
                    {
                        case 1:
                            //Console.Write("|");
                            //formGraphics.DrawLine(myPen, 0, 0, 200, 200);
                            formGraphics.DrawLine(myPen, offSet + (1 + j) * cellWidth, offSet + (i) * cellWidth, offSet + ((1 + j) * cellWidth), offSet + (1 + i) * cellWidth);
                            break;
                        case 2:
                            //Console.Write("_");
                            formGraphics.DrawLine(myPen, offSet + (j) * cellWidth, offSet + (1 + i) * cellWidth, offSet + ((1 + j) * cellWidth), offSet + (1 + i) * cellWidth);
                            break;
                        case 3:
                            //Console.Write("_|");
                            formGraphics.DrawLine(myPen, offSet + (1 + j) * cellWidth, offSet + (i) * cellWidth, offSet + ((1 + j) * cellWidth), offSet + (1 + i) * cellWidth);
                            formGraphics.DrawLine(myPen, offSet + (j) * cellWidth, offSet + (1 + i) * cellWidth, offSet + ((1 + j) * cellWidth), offSet + (1 + i) * cellWidth);
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
                
            }
            progBar.Value=100;
            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progBar.Value = 0;
            mainGridSize = Convert.ToInt32(numSize.Value);
            theMainGrid = new int[mainGridSize, mainGridSize];
            devide(0, 0, mainGridSize, mainGridSize);
            showResult();
            showMaze();
        }
    }
}
