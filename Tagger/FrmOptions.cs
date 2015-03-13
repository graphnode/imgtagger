using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tagger.Properties;
using System.IO;

namespace Tagger
{
    public partial class FrmOptions : Form
    {
        public FrmOptions()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = !Settings.Default.ShowIntro;
            checkBox2.Checked = !Settings.Default.StartDatabase.Equals("");
            numericUpDown1.Value = Settings.Default.ThumbnailCount;
            if (Settings.Default.ImageViewer.Equals("FileDefault"))
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.SelectedIndex = 1;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = button3.Enabled = checkBox2.Checked;
            textBox1.Text = Settings.Default.StartDatabase;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            numericUpDown1.Value = 50;
        }

        private void FrmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox2.Checked) {
                if (!File.Exists(textBox1.Text)) {
                    MessageBox.Show("File does not exist!", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }
                Settings.Default.StartDatabase = textBox1.Text;
            }
            else Settings.Default.StartDatabase = "";

            Settings.Default.ShowIntro = !checkBox1.Checked;
            Settings.Default.ThumbnailCount = (int)numericUpDown1.Value;

            if (comboBox1.SelectedIndex == 0)
                Settings.Default.ImageViewer = "FileDefault";
            else
                Settings.Default.ImageViewer = "WindowsDefault";

            Settings.Default.Save();
        }
    }
}
