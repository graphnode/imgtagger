using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tagger.Database;
using WeifenLuo.WinFormsUI.Docking;
using Tagger.Properties;

namespace Tagger
{
    public partial class FrmIntro : Form
    {
        private bool old_check = false;
        private FrmMain frmMain;

        public FrmIntro()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Tagger.Properties.Resources.asterisk_orange.GetHicon());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                old_check = checkBox1.Checked;
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
            }
            else
            {
                checkBox1.Checked = old_check;
                checkBox1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (frmMain.databaseSaveDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetSettings(frmMain.databaseSaveDialog.FileName);
                frmMain.OpenDatabase(frmMain.databaseSaveDialog.FileName);

                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(this.Owner);
            if (frmMain.databaseOpenDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetSettings(frmMain.databaseOpenDialog.FileName);
                frmMain.OpenDatabase(frmMain.databaseOpenDialog.FileName);

                this.Close();
            }
        }

        private void SetSettings(string filename)
        {
            Settings.Default.ShowIntro = !checkBox1.Checked;

            if (checkBox2.Checked)
                Settings.Default.StartDatabase = filename;

            Settings.Default.Save();
        }

        private void FrmIntro_Shown(object sender, EventArgs e)
        {

            this.frmMain = (this.Owner) as FrmMain;
        }
    }
}
