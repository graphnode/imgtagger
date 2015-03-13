using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tagger.Database;
using System.Globalization;

namespace Tagger
{
    public partial class FrmTagOpt : Form
    {
        public Tag Tag;

        public FrmTagOpt()
        {
            InitializeComponent();

            if (this.Tag == null)
                this.Text = "New Tag";
        }

        public FrmTagOpt(Tag tag) : this()
        {
            this.Tag = tag;

            this.TxtName.Text = tag.Name;
            this.TxtColor.Text = (tag.Color & 0xFFFFFF).ToString("X6");
            this.TxtDescription.Text = tag.Description;

            this.Text = "Editing " + tag.Name;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = label4.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color c = colorDialog1.Color;
                TxtColor.Text = (c.ToArgb() & 0xFFFFFF).ToString("X6");
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            TxtColor.Text = (Tag == null) ? "000000" : (Tag.Color & 0xFFFFFF).ToString("X6");
        }

        private void TxtColor_TextChanged(object sender, EventArgs e)
        {
            int hex = (int)(0xFF000000 + Convert.ToInt32(TxtColor.Text, 16));
            Color c = Color.FromArgb(hex);
            label4.BackColor = c;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSaveClose_Click(object sender, EventArgs e)
        {
            bool new_tag = (Tag == null);

            if (new_tag) Tag = new Tag();

            Tag.Name = TxtName.Text.Replace(' ', '_').ToLower(); ;
            Tag.Color =  int.Parse(TxtColor.Text, NumberStyles.AllowHexSpecifier);
            Tag.Description = TxtDescription.Text;

            if (new_tag) Tag.Insert();
            else Tag.Update();

            this.Close();
        }

        private void TxtName_Leave(object sender, EventArgs e)
        {
            TxtName.Text = TxtName.Text.Replace(' ', '_').ToLower();
        }
    }
}
