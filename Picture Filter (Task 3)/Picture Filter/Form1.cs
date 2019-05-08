using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picture_Filter
{
    public partial class Form1 : Form
    {
        Bitmap picture;
        Bitmap workingPicture;
        Bitmap workingPicture2;
        public Form1()
        {
            InitializeComponent();
            toolStripProgressBar1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox1.Controls.Add(pictureBox2);
            pictureBox2.Left = 10;
            pictureBox2.Top = 10;
            backgroundWorker1.WorkerReportsProgress = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG Image (.jpg)|*.jpg|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif";
            openFileDialog.Title = "Select a Image File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadImage(openFileDialog.FileName);
            }
        }

        private void LoadImage(string path)
        {
            toolStripStatusLabel1.Text = path.ToString();
            picture = new Bitmap(path);
            pictureBox1.Image = picture;
        }

        private void showColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check.Checked == true)
            {
                pictureBox2.Visible = true;
            }
            else
            {
                pictureBox2.Visible = false;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (picture != null)
            {
                Color color = picture.GetPixel(e.X, e.Y);
                pictureBox2.BackColor = color;
            }
            else
            {
                pictureBox2.BackColor = Color.White;
            }

        }

        private void apply_FilterButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                toolStripProgressBar1.Visible = true;
                workingPicture = (Bitmap)picture.Clone();
                workingPicture2 = (Bitmap)picture.Clone();

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int which_effect = 0;
            if (grayscaleButton.Checked == true)
            {
                which_effect = 0;
            }
            else if (negativeButton.Checked == true)
            {
                which_effect = 1;
            }
            else if (sharpenButton.Checked == true)
            {
                which_effect = 2;
            }

            if (picture != null)
            {
                backgroundWorker1.ReportProgress(0);
                int width = workingPicture.Width;
                int height = workingPicture.Height;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        switch (which_effect)
                        {
                            case 0:
                                Grayscale(i, j, width, height);
                                break;
                            case 1:
                                Negative(i, j, width, height);
                                break;
                            case 2:
                                Sharpen(i, j, width, height);
                                break;
                        }
                    }
                    backgroundWorker1.ReportProgress((int)((double)(i + 1) / width * 100) % 101);
                }
                backgroundWorker1.ReportProgress(100);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Image = workingPicture;
            picture = workingPicture;
            pictureBox1.Image = picture;
            toolStripProgressBar1.Visible = false;
        }

        private void Grayscale(int i, int j, int width, int height)
        {
            Color color = workingPicture.GetPixel(i, j);
            int Y = ((int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114)) % 256;
            Color endColor = Color.FromArgb(Y, Y, Y);
            workingPicture.SetPixel(i, j, endColor);
        }

        private void Negative(int i, int j, int width, int height)
        {
            Color color = workingPicture.GetPixel(i, j);
            Color endColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
            workingPicture.SetPixel(i, j, endColor);
        }

        private void Sharpen(int i, int j, int width, int height)
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            for (int x = i - 1; x <= i + 1; x++)
            {
                for (int y = j - 1; y <= j + 1; y++)
                {
                    Color c = workingPicture2.GetPixel(Clamp(x, 0, width - 1), Clamp(y, 0, height - 1));
                    if (x != i || y != j)
                    {
                        red -= c.R;
                        green -= c.G;
                        blue -= c.B;
                    }
                    else
                    {
                        red += 9 * c.R;
                        green += 9 * c.G;
                        blue += 9 * c.B;
                    }
                }
            }
            red = Clamp(red, 0, 255);
            green = Clamp(green, 0, 255);
            blue = Clamp(blue, 0, 255);

            Color endColor = Color.FromArgb(red, green, blue);
            workingPicture.SetPixel(i, j, endColor);
        }

        public int Clamp(int val, int min, int max)
        {
            if (val < min)
            {
                val = min;
            }
            else if (val > max)
            {
                val = max;
            }

            return val;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy == true)
            {
                MessageBox.Show("Working...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
    }
}
