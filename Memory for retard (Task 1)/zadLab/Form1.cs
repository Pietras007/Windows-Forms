using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zadLab
{
    public partial class Form1 : Form
    {
        int[] numbersTable;
        Button prevClick;
        bool isPlaying = false;
        int time = 0;
        int deleted = 0;
        public Form1()
        {
            InitializeComponent();
            CreateRandomNumbers();
            PutNumbers();
            prevClick = null;
            label1.Select();
            foreach (Button item in tableLayoutPanel2.Controls)
            {
                item.BackColor = Color.AliceBlue;
                item.MouseEnter += MouseEnter;
                item.MouseLeave += MouseLeave;
                item.Click += Click;
            } 
        }

        private void Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                Button currentClick = sender as Button;
                currentClick.BackColor = Color.IndianRed;
                if (prevClick != null && prevClick != currentClick)
                {
                    if (prevClick.Text == currentClick.Text)
                    {
                        prevClick.Visible = false;
                        currentClick.Visible = false;
                        deleted += 2;
                        if(deleted == 16)
                        {
                            EndOfGame();
                        }
                    }
                }
                if (prevClick != null)
                {
                    prevClick.BackColor = Color.AliceBlue;
                }
                prevClick = currentClick;
            }
        }

        private void EndOfGame()
        {
            timer1.Enabled = false;
            DialogResult dialogResult = MessageBox.Show("You WON! Your time: " + time.ToString(), "The End \nDo you want to play again?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                resetButton_Click(null, null);
            }
            else if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }

        private void PutNumbers()
        {
            int i = 0;
            foreach (Button item in tableLayoutPanel2.Controls)
            {
                item.Text = numbersTable[i].ToString();
                i++;
            }
        }

        private void CreateRandomNumbers()
        {
            numbersTable = new int[16];
            int[] justRandom = new int[8];
            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                bool isOk = false;
                while (!isOk)
                {
                    int number = rand.Next(8);
                    if(justRandom[number] < 2)
                    {
                        justRandom[number]++;
                        isOk = true;
                        numbersTable[i] = number + 1;
                    }
                }
            }
        }

        private void MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.Blue;
        }

        private void MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.BackColor != Color.IndianRed)
            {
                button.BackColor = Color.AliceBlue;
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            isPlaying = true;
            timer1.Enabled = true;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            CreateRandomNumbers();
            PutNumbers();
            prevClick = button1;
            foreach (Button item in tableLayoutPanel2.Controls)
            {
                item.Visible = true;
                item.BackColor = Color.AliceBlue;
            }
            isPlaying = false;
            time = 0;
            label1.Text = time.ToString();
            timer1.Enabled = false;
            deleted = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            label1.Text = time.ToString();
        }
    }
}
