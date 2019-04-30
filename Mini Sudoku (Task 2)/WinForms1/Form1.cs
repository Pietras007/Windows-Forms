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

namespace WinForms1
{
    public partial class Form1 : Form
    {
        (Button, int)[] tabValue = new (Button, int)[16];
        NumericUpDown numericUpDown = null;

        public Form1()
        {
            InitializeComponent();
            int i = 0;
            foreach (Button item in tableLayoutPanel1.Controls)
            {
                item.MouseDown += MouseClick;
                tabValue[i] = (item, 0);
                i++;
            }
            Array.Reverse(tabValue);
        }

        private void MouseClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Button button = sender as Button;
            int index = 0;
            foreach((Button, int) b in tabValue)
            {
                if(b.Item1.Equals(button))
                {
                    break;
                }
                index++;
            }

            if (me.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu context = new ContextMenu();
                context.MenuItems.Add("", delegate (object send, EventArgs ee) { changeNumber(sender, e, index, 0); });
                context.MenuItems.Add("1", delegate (object send, EventArgs ee) { changeNumber(sender, e, index, 1); });
                context.MenuItems.Add("2", delegate (object send, EventArgs ee) { changeNumber(sender, e, index, 2); });
                context.MenuItems.Add("3", delegate (object send, EventArgs ee) { changeNumber(sender, e, index, 3); });
                context.MenuItems.Add("4", delegate (object send, EventArgs ee) { changeNumber(sender, e, index, 4); });
                tableLayoutPanel1.ContextMenu = context;
                
            }

            if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (tabValue[index].Item2 == 4)
                {
                    tabValue[index].Item2 = 0;
                }
                else
                {
                    tabValue[index].Item2++;
                }
                ToResetText();
                ToResetColor();
            }
        }

        public void changeNumber(object sender, EventArgs e, int index, int number)
        {
            tabValue[index].Item2 = number;
            ToResetText();
            ToResetColor();
        }

        public void ToResetFont()
        {
            foreach ((Button, int) b in tabValue)
            {
                b.Item1.Font = new Font("Microsoft Sans Serif", 14);
            }
        }

        private void ToResetText()
        {
            foreach ((Button, int) b in tabValue)
            {
                if (b.Item2 == 0)
                {
                    b.Item1.Text = "";
                }
                else
                {
                    b.Item1.Text = b.Item2.ToString();
                }
            }
        }

        private void ToResetColor()
        {
            foreach((Button, int) b in tabValue)
            {
                b.Item1.BackColor = Color.White;
            }

            //All possibilities
            int[][] table = new int[12][];
            table[0] = new int[] { 0, 1, 4, 5 };
            table[1] = new int[] { 2, 3, 6, 7 };
            table[2] = new int[] { 8, 9, 12, 13 };
            table[3] = new int[] { 10, 11, 14, 15 };
            table[4] = new int[] { 0, 1, 2, 3 };
            table[5] = new int[] { 4, 5, 6, 7 };
            table[6] = new int[] { 8, 9, 10, 11 };
            table[7] = new int[] { 12, 13, 14, 15 };
            table[8] = new int[] { 0, 4, 8, 12 };
            table[9] = new int[] { 1, 5, 9, 13 };
            table[10] = new int[] { 2, 6, 10, 14 };
            table[11] = new int[] { 3, 7, 11, 15 };

            for(int q=0;q<table.Length;q++)
            {
                for(int i=0;i<table[q].Length;i++)
                {
                    for (int j = i + 1; j < table[q].Length; j++)
                    {
                        if (tabValue[table[q][i]].Item2 != 0)
                        {
                            if (tabValue[table[q][i]].Item2 == tabValue[table[q][j]].Item2)
                            {
                                tabValue[table[q][i]].Item1.BackColor = Color.Red;
                                tabValue[table[q][j]].Item1.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void ToResetNumeric()
        {
            if (numericUpDown != null)
            {
                numericUpDown.Value = 14;
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tabValue.Length; i++)
            {
                tabValue[i].Item2 = 0;
            }

            ToResetNumeric();
            ToResetText();
            ToResetColor();
            ToResetFont();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown = sender as NumericUpDown;
            int fontSize = (int)numericUpDown.Value;
            foreach ((Button, int) b in tabValue)
            {
                b.Item1.Font = new Font("Microsoft Sans Serif", fontSize);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files|*.txt";
            openFileDialog.Title = "Select a Text File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
            ToResetNumeric();
            ToResetText();
            ToResetColor();
            ToResetFont();
        }

        private void LoadFile(string path)
        {
            List<int> read = new List<int>();
            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    string line;
                    while ((line = stream.ReadLine()) != null)
                    {
                        foreach (var word in line.Split(','))
                        {
                            int numer;
                            Int32.TryParse(word, out numer);
                            read.Add(numer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The file could not be read: " + ex.Message);
            }
            int i = 0;
            foreach(int num in read)
            {
                tabValue[i].Item2 = num;
                i++;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files|*.txt";
            saveFileDialog.Title = "Select a Text File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveFileDialog.FileName);
            }
        }

        public void SaveFile(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    int sideLength = 4;
                    for(int i=0;i< sideLength; i++)
                    {
                        StringBuilder sb = new StringBuilder(8);
                        for (int j=0;j< sideLength; j++)
                        {
                            if (tabValue[i * sideLength + j].Item2 != 0)
                                sb.Append(tabValue[i * sideLength + j].Item2.ToString());
                            if (j < sideLength - 1)
                                sb.Append(",");
                        }
                        sw.WriteLine(sb);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected problem accured: " + ex.Message);
            }
        }
    }
}
