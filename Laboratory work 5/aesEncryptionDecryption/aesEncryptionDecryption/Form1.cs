using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

namespace aesEncryptionDecryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            string allText = File.ReadAllText(filename);
            richTextBox1.Text = allText;
            MessageBox.Show("Файл відкрито успішно.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
	
                crypt.CryptAlgorithm = "aes";
                crypt.CipherMode = "ctr";
                crypt.KeyLength = 256;
                crypt.EncodingMode = "base64";

                string keyHex = textBox1.Text;
                crypt.SetEncodedKey(keyHex, "ascii");
                string ivHex = textBox2.Text;
                crypt.SetEncodedIV(ivHex, "ascii");

                string encStr = crypt.EncryptStringENC(richTextBox1.Text);
                richTextBox2.Text = encStr;
            }
            else
                if (radioButton2.Checked)
                {
                    string ivHex = textBox2.Text;
                    string keyHex = textBox1.Text;

                    Chilkat.Crypt2 decrypt = new Chilkat.Crypt2();
                    decrypt.CryptAlgorithm = "aes";
                    decrypt.CipherMode = "ctr";
                    decrypt.KeyLength = 256;
                    decrypt.EncodingMode = "base64";
                    decrypt.SetEncodedIV(ivHex, "ascii");
                    decrypt.SetEncodedKey(keyHex, "ascii");

                    string decStr = decrypt.DecryptStringENC(richTextBox1.Text);
                    richTextBox2.Text = decStr;
                }
        }
    }
}
