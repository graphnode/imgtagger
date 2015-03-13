using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows.Forms;
using Tagger.Database;
using WeifenLuo.WinFormsUI.Docking;

namespace Tagger
{
    public partial class FrmDetails : DockContent
    {
        private LinkedList<File> files;
        
        public FrmDetails()
        {
            InitializeComponent();

            files = new LinkedList<File>();
        }

        public void AddFile(File file)
        {
            if (!files.Contains(file))
            {
                listView1.BeginUpdate();
                foreach (Tag tag in file.GetTags())
                {
                    TagItem ti = listView1.FindItemWithText(tag.Name) as TagItem;
                    if (ti != null)
                        ti.FileCount++;
                    else
                    {
                        ti = new TagItem(tag);
                        ti.FileCount++;
                        listView1.Items.Add(ti);
                    }
                }
                listView1.EndUpdate();
                files.AddLast(file);
                this.UpdateStatus();
            }
        }

        public void RemoveFile(File file)
        {
            if (files.Contains(file))
            {
                listView1.BeginUpdate();
                foreach (Tag tag in file.GetTags())
                {
                    TagItem ti = listView1.FindItemWithText(tag.Name) as TagItem;
                    if (ti != null)
                    {
                        ti.FileCount--;
                        if (ti.FileCount == 0)
                            listView1.Items.Remove(ti);
                    }
                }
                listView1.EndUpdate();
                files.Remove(file);
                this.UpdateStatus();
            }
        }

        public void ClearFiles()
        {
            files.Clear();
            listView1.Clear();
        }

        public void UpdateStatus()
        {
            if (files.Count == 1)
            {
                TxtFilename.Text = System.IO.Path.GetFileName((files.First.Value as Tagger.Database.File).Name);
                TxtHash.Text = (files.First.Value as Tagger.Database.File).modtime;
            }
            else if (files.Count > 1)
            {
                TxtFilename.Text = "Multiple Files";
                TxtHash.Text = "Multiple Files";
            }
            else
            {
                TxtFilename.Text = "...";
                TxtHash.Text = "...";
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (files != null)
            {
                DbTransaction trans = Manager.Connection.BeginTransaction();
                listView1.BeginUpdate();
                foreach (TagItem ti in listView1.SelectedItems)
                {
                    foreach (Tagger.Database.File f in files)
                    {
                            f.RemoveTag(ti.Tag);
                    }
                    listView1.Items.Remove(ti);
                }

                listView1.EndUpdate();
                trans.Commit();

                //(this.DockPanel.Parent as FrmMain).RefreshAllSearches();
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            ArrayList array = e.Data.GetData(typeof(ArrayList)) as ArrayList;
            DbTransaction trans = Manager.Connection.BeginTransaction();
            listView1.BeginUpdate();
            foreach (Tagger.Database.File f in files)
            {
                foreach (Tag t in array)
                {
                    f.AddTag(t);
                    listView1.Items.Add(new TagItem(t));
                }
            }
            listView1.EndUpdate();
            trans.Commit();
        }
    }
}
