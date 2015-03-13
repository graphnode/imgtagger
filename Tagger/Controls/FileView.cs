using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tagger.Database;
using System.Drawing;

namespace Tagger.Controls
{
    public class FileView : ListView
    {
        public FileView() : base()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }

    public class FileItem : ListViewItem
    {
        //public bool Targeted;
        public File File;

        public FileItem(File f)
        {
            //this.Targeted = false;
            this.File = f;
        }

        public override string ToString()
        {
            return this.File.Hash;
        }
    }
}
