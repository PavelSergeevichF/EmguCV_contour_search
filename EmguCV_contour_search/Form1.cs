using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EmguCV_contour_search
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> inputImage1 = null;
        private Bitmap bitmapImage = null;
        public Form1() 
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    //inputImage = new Image<Bgr, byte>(openFileDialog1.FileName);
                    bitmapImage = new Bitmap(openFileDialog1.FileName);
                    pictureBox1.Image = bitmapImage;
                    //pictureBox1.Image = inputImage.Bitmap;
                }
                else 
                {
                    MessageBox.Show("Файл не выбран", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void найтиКонтурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Rectangle rectangle = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);//System.Drawing
                BitmapData bmpData = bitmapImage.LockBits(rectangle, ImageLockMode.ReadWrite, bitmapImage.PixelFormat);//System.Drawing.Imaging
                Image<Bgr, byte> temp = new Image<Bgr, byte>(bitmapImage.Width, bitmapImage.Height, bmpData.Stride, bmpData.Scan0);//(IntPtr)

                inputImage1 = temp;
                Image<Gray, byte> outputImage = inputImage1.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                Mat hierarchy = new Mat();
                CvInvoke.FindContours
                    (
                    outputImage, 
                    contours, 
                    hierarchy, 
                    Emgu.CV.CvEnum.RetrType.Tree, 
                    Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple
                    );
                if(checkBox1.Checked)
                {
                    Image<Gray, byte> blackBackground = new Image<Gray, byte>(inputImage1.Width, inputImage1.Height, new Gray(0));
                    CvInvoke.DrawContours(blackBackground, contours, -1, new MCvScalar(255, 0, 0),3);
                }
                else 
                {
                    CvInvoke.DrawContours(inputImage1, contours, -1, new MCvScalar(255,0,0),3);
                    pictureBox2.Image =bitmapImage;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
