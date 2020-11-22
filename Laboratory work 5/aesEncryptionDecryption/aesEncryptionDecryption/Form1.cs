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

                // AES is also known as Rijndael.		
                crypt.CryptAlgorithm = "aes";

                // CipherMode may be "ctr", "cfb", "ecb" or "cbc"
                crypt.CipherMode = "ctr";

                // KeyLength may be 128, 192, 256
                crypt.KeyLength = 256;

                // Counter mode emits the exact number of bytes input, and therefore 
                // padding is not used.  The PaddingScheme property does not apply with CTR mode.

                // EncodingMode specifies the encoding of the output for
                // encryption, and the input for decryption.
                // It may be "hex", "url", "base64", "quoted-printable", or many other choices.
                crypt.EncodingMode = "base64";

                // The secret key must equal the size of the key.  For
                // 256-bit encryption, the binary secret key is 32 bytes.
                // For 128-bit encryption, the binary secret key is 16 bytes.
                string keyHex = textBox1.Text;
                crypt.SetEncodedKey(keyHex, "ascii");

                // An initialization vector (nonce) is required if using CTR mode.
                // The length of the IV is equal to the algorithm's block size.
                // It is NOT equal to the length of the key.
                string ivHex = textBox2.Text;
                crypt.SetEncodedIV(ivHex, "ascii");

                // Encrypt a string...
                // The input string is 44 ANSI characters (i.e. 44 bytes), so
                // the output should be 48 bytes (a multiple of 16).
                // Because the output is a hex string, it should
                // be 96 characters long (2 chars per byte).
                string encStr = crypt.EncryptStringENC(richTextBox1.Text);

                richTextBox2.Text = encStr;
            }
            else
                if (radioButton2.Checked)
                {
                    string ivHex = textBox2.Text;

                    // The secret key must equal the size of the key.  For
                    // 256-bit encryption, the binary secret key is 32 bytes.
                    // For 128-bit encryption, the binary secret key is 16 bytes.
                    string keyHex = textBox1.Text;

                    // Encrypt a string...
                    // The input string is 44 ANSI characters (i.e. 44 bytes), so
                    // the output should be 48 bytes (a multiple of 16).
                    // Because the output is a hex string, it should
                    // be 96 characters long (2 chars per byte).

                    Chilkat.Crypt2 decrypt = new Chilkat.Crypt2();

                    decrypt.CryptAlgorithm = "aes";
                    decrypt.CipherMode = "ctr";
                    decrypt.KeyLength = 256;
                    decrypt.EncodingMode = "base64";
                    decrypt.SetEncodedIV(ivHex, "ascii");
                    decrypt.SetEncodedKey(keyHex, "ascii");

                    // Now decrypt:
                    string decStr = decrypt.DecryptStringENC(richTextBox1.Text);

                    richTextBox2.Text = decStr;
                }
        }
    }
}
