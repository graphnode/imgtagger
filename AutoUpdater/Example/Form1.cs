using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Example
{
    public partial class Form1 : Form
    {
        private string url = "http://localhost/updater/info.xml";

        public Form1()
        {
            InitializeComponent();
            this.label1.BackColor = Color.FromArgb(160, Color.Gray);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Version: " + AutoUpdater.CurrentVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool res = AutoUpdater.Check(url);

            if (AutoUpdater.Check(url))
            {
                DialogResult dr = MessageBox.Show("New version found! Do you wish to update?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    AutoUpdater.Update(url);
                }
            }
            else
            {
                MessageBox.Show("No new version found.");
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rc, Color.SkyBlue, Color.SlateBlue, 45F))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}
