using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //Dictionary of characters
        Dictionary<char, double> characters = new Dictionary<char, double>();
        //Dictionary of bigrams or trigrams
        public Dictionary<string, double> ngrams = new Dictionary<string, double>();

        public void CharactrFrequency(Dictionary<char, double> chrctrs, Dictionary<string, double> ngrams, int totalCount, string anlsType)
        {
            if (anlsType == "Character")
            {
                //The number of keys in the dictionary
                int countKeysDict = chrctrs.Keys.Count;
                char[] keysDict = new char[countKeysDict];
                chrctrs.Keys.CopyTo(keysDict, 0);

                for (int iter = 0; iter < countKeysDict; iter++)
                {
                    chrctrs[keysDict[iter]] /= totalCount;
                }
            }
            else
            {
                int countKeysDict = ngrams.Keys.Count;
                string[] keysDict = new string[countKeysDict];
                ngrams.Keys.CopyTo(keysDict, 0);

                for (int iter = 0; iter < countKeysDict; iter++)
                {
                    ngrams[keysDict[iter]] /= totalCount;
                }
            }
        }

        public void Print(Dictionary<char, double> chrctrs, Dictionary<string, double> ngrams, int totalCount, string anlsType)
        {
            if (anlsType == "Character")
            {
                if (radioButton1.Checked)
                {
                    SortedDictionary<char, double> sortedDict = new SortedDictionary<char, double>(chrctrs);
                    foreach (KeyValuePair<char, double> kvp in sortedDict)
                    {
                        this.chart1.Series["Frequency"].Points.AddXY(kvp.Key.ToString(), kvp.Value);
                    }
                }
                else
                    if (radioButton2.Checked)
                    {
                        var items = from pair in chrctrs
                                    orderby pair.Value descending
                                    select pair;

                        foreach (KeyValuePair<char, double> kvp in items)
                        {
                            this.chart1.Series["Frequency"].Points.AddXY(kvp.Key.ToString(), kvp.Value);
                            listBox1.Items.Add(kvp.Key.ToString());
                        }
                    }
            }
            else
            {
                int maxKeys = 30, 
                    iter    = 1;

                var items = from pair in ngrams
                        orderby pair.Value descending
                        select pair;

                foreach (KeyValuePair<string, double> kvp in items)
                {
                    if (iter > maxKeys)
                        break;
                    else
                    {
                        this.chart1.Series["Frequency"].Points.AddXY(kvp.Key.ToString(), kvp.Value);
                        listBox1.Items.Add(kvp.Key.ToString());
                        iter++;
                    }
                }
            }
        }

        public void CharactersDictionary(string allText, Dictionary<char, double> chrctrs, int textLength)
        {
            int iterator = 0;
            //Repetition of a character in the text
            double chrctrRecur = 1;
            iterator = 0;

            //Creation dictionary of characters
            while (iterator < textLength)
            {
                if (!chrctrs.ContainsKey(allText[iterator]) && Char.IsLetter(allText[iterator]))
                {
                    chrctrs.Add(allText[iterator], chrctrRecur);
                }
                else
                    if (chrctrs.ContainsKey(allText[iterator]))
                    {
                        chrctrs[allText[iterator]]++;
                    }
                iterator++;
            }
        }

        public void BigramsDictionary(string allText, Dictionary<string, double> bigramsDict, int textLength)
        {
            string bigram;
            //Repetition of a character in the text
            double chrctrRecur = 1;

            //Creating dictionary of bigrams
            for (int i = 0; i < textLength - 1; i++)
            {
                bigram = string.Concat(allText[i], allText[i + 1]);

                if (!bigramsDict.ContainsKey(bigram) && Char.IsLetter(allText[i]) && Char.IsLetter(allText[i + 1]))
                {
                    bigramsDict.Add(bigram, chrctrRecur);
                }
                else
                    if (bigramsDict.ContainsKey(bigram))
                    {
                        bigramsDict[bigram]++;
                    }
            }
        }

        public void TrigramsDictionary(string allText, Dictionary<string, double> trigramsDict, int textLength)
        {
            string trigram;
            //Repetition of a character in the text
            double chrctrRecur = 1;

            //Creating dictionary of trigrams
            for (int i = 0; i < textLength - 2; i++)
            {
                trigram = string.Concat(allText[i], allText[i + 1], allText[i + 2]);

                if (!trigramsDict.ContainsKey(trigram) && Char.IsLetter(allText[i]) && Char.IsLetter(allText[i + 1]) && Char.IsLetter(allText[i + 2]))
                {
                    trigramsDict.Add(trigram, chrctrRecur);
                }
                else
                    if (trigramsDict.ContainsKey(trigram))
                    {
                        trigramsDict[trigram]++;
                    }
            }
        }

        public void ReadFile(string pathFile, Dictionary<char, double> chrctrs, Dictionary<string, double> ngrams, out int totalCountChrctrs, string anlsType)
        {
            string allText = File.ReadAllText(pathFile).ToLower();
            totalCountChrctrs = allText.Length;

            switch (anlsType)
            {
                case "Character":
                    CharactersDictionary(allText, chrctrs, totalCountChrctrs);
                    break;
                case "Bigram":
                    BigramsDictionary(allText, ngrams, totalCountChrctrs);
                    break;
                case "Trigram":
                    TrigramsDictionary(allText, ngrams, totalCountChrctrs);
                    break;
            }
        }
        
        public Form1()
        {
            InitializeComponent();


            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Interval = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            listBox1.Items.Clear();

            string analysisType;
            int totalCountCharacters;
            
            analysisType = comboBox1.SelectedItem.ToString();
            ReadFile(textBox1.Text, characters, ngrams, out totalCountCharacters, analysisType);
            CharactrFrequency(characters, ngrams, totalCountCharacters, analysisType);
            Print(characters, ngrams, totalCountCharacters, analysisType);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            textBox1.Text = filename;
            MessageBox.Show("Файл відкрито успішно.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Character")
            {
                radioButton1.Enabled = true;
                button5.Enabled = false;
            }
            else
                if (comboBox1.SelectedItem.ToString() == "Bigram")
                {
                    radioButton1.Enabled = false;
                    radioButton2.Checked = true;
                    button5.Enabled = true;
                }
                else
                    if (comboBox1.SelectedItem.ToString() == "Trigram")
                    {
                        radioButton1.Enabled = false;
                        radioButton2.Checked = true;
                        button5.Enabled = false;
                    }
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string analysisType;
            int totalCountCharacters;

            analysisType = comboBox1.SelectedItem.ToString();
            ReadFile(textBox1.Text, characters, ngrams, out totalCountCharacters, analysisType);
            CharactrFrequency(characters, ngrams, totalCountCharacters, analysisType);

            Form form2 = new Form2(ngrams);
            form2.Show();
            this.Hide();
        }




    }
}
