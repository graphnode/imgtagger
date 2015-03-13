using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tagger.Database;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections;
using System.Data.Common;
using Tagger.Controls;

namespace Tagger
{
    public partial class FrmTagList : DockContent
    {
        public FrmTagList()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Tagger.Properties.Resources.tag_blue.GetHicon());
        }

        private void FrmTags_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            LinkedList<Tag> tags = Tagger.Database.Tag.GetAll();

            Utils.TagListSortXML.CreateListXML(tags);

            List<TagItem> tiList = new List<TagItem>();
            foreach (Tag t in tags)
            {
                tiList.Add(new TagItem(t));
            }

            List<TagItem> sortedTagItemList = Utils.TagListSortXML.ReadListXML(tiList);

            foreach (TagItem ti in sortedTagItemList)
                listView1.Items.Add(ti);

            listView1.EndUpdate();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ArrayList array = new ArrayList();
            foreach (TagItem ti in listView1.SelectedItems) array.Add(ti);
            listView1.DoDragDrop(array, DragDropEffects.Link | DragDropEffects.Copy);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                (new FrmDisplay(listView1.SelectedItems[0].Text)).Show(this.DockPanel, DockState.Document);
        }

        #region Add Tag
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmTagOpt topt = new FrmTagOpt();
            topt.ShowDialog();

            if (topt.Tag != null)
            {
                TagItem ti = new TagItem(topt.Tag);
                listView1.BeginUpdate();
                listView1.Items.Add(ti);
                listView1.SelectedItems.Clear();
                ti.Selected = true;
                listView1.EnsureVisible(ti.Index);
                listView1.EndUpdate();

                List<TagItem> tiList = new List<TagItem>();
                foreach (TagItem tis in listView1.Items)
                    tiList.Add(tis);
                Utils.TagListSortXML.UpdateListXML(tiList);
            }
        } 
        #endregion

        #region Edit Tag
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                ListViewItem lvi = listView1.SelectedItems[0];

                FrmTagOpt topt = new FrmTagOpt((lvi as TagItem).Tag);
                topt.ShowDialog();

                if (topt.Tag != null)
                {
                    Tag t = topt.Tag;
                    listView1.BeginUpdate();
                    listView1.Items.Remove(lvi);
                    TagItem ti = new TagItem(t);
                    listView1.Items.Add(ti as ListViewItem);
                    listView1.EnsureVisible(ti.Index);
                    listView1.SelectedItems.Clear();
                    ti.Selected = true;
                    listView1.EndUpdate();

                    //(this.DockPanel.Parent as FrmMain).RefreshAllSearches();
                }
            }
        } 
        #endregion

        #region Remove Tag
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete the selected tags?", "Delete Tags", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                listView1.BeginUpdate();
                foreach (TagItem ti in listView1.SelectedItems)
                {
                    ti.Tag.Delete();
                    listView1.Items.Remove(ti);
                }
                listView1.EndUpdate();

                List<TagItem> tiList = new List<TagItem>();
                foreach (TagItem tis in listView1.Items)
                    tiList.Add(tis);
                Utils.TagListSortXML.UpdateListXML(tiList);

                //(this.DockPanel.Parent as FrmMain).RefreshAllSearches();
            }
        }
        #endregion

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                toolStripButton2.Enabled =
                    toolStripButton3.Enabled = true;
            }
            else
            {
                toolStripButton2.Enabled = toolStripButton3.Enabled = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                string tags = "";
                foreach (TagItem ti in listView1.SelectedItems)
                    tags += ti.Tag.Name + " ";
                (new FrmDisplay(tags)).Show(this.DockPanel, DockState.Document);
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView1.GetItemAt(e.X, e.Y) != null)
            {
                this.contextMenuStrip1.Show(listView1, e.Location);
            }
        }

        private void addThisTagToTagBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TagItem ti in listView1.SelectedItems)
            {
                TagBookmarkItem tagBookMarkItem = new TagBookmarkItem(ti.Tag);
                TagBookmarkTemp.TagBookmarkList.Add(tagBookMarkItem);
            }

            FrmMain frmMain = (FrmMain)Application.OpenForms["FrmMain"];

            foreach (FrmDisplay fs in frmMain.dockPanel.Documents)
                fs.RefreshBookmarkList();
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
            listView1.Focus();
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            listView1.SelectedItems.Clear();

            Point p = listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem item = listView1.GetItemAt(p.X, p.Y);

            if (item != null && e.Data.GetDataPresent(typeof(ArrayList)))
            {
                e.Effect = DragDropEffects.Link;
                item.Selected = true;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Point p = listView1.PointToClient(new Point(e.X, e.Y));
            TagItem item = listView1.GetItemAt(p.X, p.Y) as TagItem;

            if (item != null && e.Data.GetDataPresent(typeof(ArrayList)))
            {
                List<TagItem> tiList = new List<TagItem>();

                ArrayList array = e.Data.GetData(typeof(ArrayList)) as ArrayList;
                //TagItem droppedTi = (TagItem)array[0];

                foreach(TagItem droppedTis in array)
                    listView1.Items.Remove(droppedTis);

                foreach (TagItem ti in listView1.Items)
                {
                    if (ti.Tag.Id == item.Tag.Id)
                    {
                        tiList.Add(ti);
                        foreach(TagItem droppedTis in array)
                            tiList.Add(droppedTis);
                    }
                    else tiList.Add(ti);
                }

                listView1.BeginUpdate();
                listView1.Items.Clear();
                foreach (TagItem ti in tiList)
                {
                    listView1.Items.Add(ti);
                }
                listView1.EndUpdate();

                Utils.TagListSortXML.UpdateListXML(tiList);
            }
        }

        private void viewlistdetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.View.Equals(View.List))
                listView1.View = View.Details;
            else listView1.View = View.List;
        }

        private void sortToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.Sorting.Equals(SortOrder.Ascending))
            {
                listView1.Sorting = SortOrder.None;
                listView1.BeginUpdate();
                listView1.Items.Clear();
                LinkedList<Tag> tags = Tagger.Database.Tag.GetAll();

                foreach (Tag t in tags)
                {
                    listView1.Items.Add(new TagItem(t));
                }
                listView1.EndUpdate();

            }
            else listView1.Sorting = SortOrder.Ascending;
        }
    }

    public class TagItem : ListViewItem
    {
        public Tag Tag;
        public int FileCount;

        public TagItem(Tag t)
            : base()
        {
            this.Tag = t;
            this.Text = Tag.Name;
            this.ForeColor = Color.FromArgb(Tag.Color);
            this.FileCount = 0;
        }
    }
}
