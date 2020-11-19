using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        Dictionary<string, double> ngram;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Dictionary<string, double> ngram)
        {
            InitializeComponent();
            this.ngram = ngram;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms[0];
            form1.StartPosition = FormStartPosition.Manual;
            form1.Show();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ArrayList x = new ArrayList();
            ArrayList y = new ArrayList();
            
            foreach (KeyValuePair<string, double> keyValue in ngram)
            {
                if (!x.Contains(keyValue.Key[0]))
                {
                    x.Add(keyValue.Key[0]);
                }
                if (!y.Contains(keyValue.Key[1]))
                {
                    y.Add(keyValue.Key[1]);
                }

            }


            dataGridView1.Size = new Size(1200, 900);
            dataGridView1.RowCount = x.Count;
            dataGridView1.ColumnCount = y.Count;
            dataGridView1.RowHeadersWidth = 50;
            int i, j;
            for (i = 0; i < x.Count; ++i)
            {
                for (j = 0; j < y.Count; ++j)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                    foreach (KeyValuePair<string, double> keyValue in ngram)
                    {
                        if(keyValue.Key == String.Concat(x[i], y[j]))
                        {
                            if (keyValue.Value > 0.0069)
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            }
                            else
                                if (keyValue.Value <= 0.0069 && keyValue.Value > 0.0059)
                                {
                                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Orange;
                                }
                                else 
                                    if (keyValue.Value <= 0.0059 && keyValue.Value > 0.0045)
                                    {
                                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                                    }
                                    else
                                        if (keyValue.Value <= 0.0059 && keyValue.Value > 0.0045)
                                        {
                                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.GreenYellow;
                                        }
                                        else 
                                            if (keyValue.Value <= 0.0045 && keyValue.Value > 0.0035)
                                            {
                                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Lime;
                                            }
                                            else 
                                                if (keyValue.Value <= 0.0035 && keyValue.Value > 0.0025)
                                                {
                                                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Aqua;
                                                }
                                                else 
                                                    if (keyValue.Value <= 0.0025 && keyValue.Value > 0.0015)
                                                    {
                                                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightSkyBlue;
                                                    }
                                                    else
                                                    {
                                                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.RoyalBlue;
                                                    }
                        }
                    }                  
                }
            }

            for (int k = 0; k < y.Count; k++)
            {
                dataGridView1.Columns[k].Name = y[k].ToString();
                dataGridView1.Columns[k].Width = 30;
            }

            for (int k = 0; k < x.Count; k++)
            {
                dataGridView1.Rows[k].HeaderCell.Value= x[k].ToString();
            }
           
           
        
        }
    }
}
