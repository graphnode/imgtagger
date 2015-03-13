using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Tagger.Utils;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using Tagger.Database;
using Tagger.Controls;
using System.Collections;
using Tagger.Properties;
using System.Data.Common;

namespace Tagger
{
    public partial class FrmDisplay : DockContent
    {
        private string tags = "";

        private int PageSize = Settings.Default.ThumbnailCount;
        private int CurrentPage = 0;
        private int TotalPages = 0;

        public FrmDisplay()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Tagger.Properties.Resources.folder.GetHicon());
        }

        void TagBookmark_Click(object sender, EventArgs e)
        {
            DbTransaction trans = Manager.Connection.BeginTransaction();
            TagBookmarkItem tbi = (TagBookmarkItem)sender;
            Tag t = tbi.Tag;
            foreach (FileItem fi in listView1.SelectedItems)
            {
                fi.File.AddTag(t);
            }
            trans.Commit();
        }

        public FrmDisplay(string tags)
            : this()
        {
            this.tags = tags;
            this.findTextBox.Text = tags;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.tags = findTextBox.Text.ToLower();

            if (this.tags.Equals("")) this.Text = "Search: *";
            else this.Text = "Search: " + tags;

            this.ChangePageTo(0);
        }

        private LinkedList<string> stringToTags(string str)
        {
            LinkedList<String> ts = new LinkedList<String>();
            foreach (String tag_name in this.tags.Trim().Split(' '))
                ts.AddLast(tag_name);
            return ts;
        }

        private void FrmSearch_Shown(object sender, EventArgs e)
        {
            if (this.tags.Equals("")) this.Text = "Search: *";
            else this.Text = "Search: " + tags;

            this.ChangePageTo(0);
        }

        public void ChangePageTo(int page)
        {
            LinkedList<string> tags = stringToTags(this.tags);

            int img_count;

            if (this.tags.Equals("")) img_count = File.CountAll();
            else img_count = File.CountByTags(tags);

            this.TotalPages = (int)Math.Ceiling(img_count / (double)this.PageSize);

            page = Math.Max(0, Math.Min(this.TotalPages - 1, page));

            bool new_page = (this.CurrentPage != page);
            this.CurrentPage = page;

            if (this.TotalPages > 1)
            {
                if (page == 0) firstButton.Enabled = previousButton.Enabled = false;
                else firstButton.Enabled = previousButton.Enabled = true;

                if (page == this.TotalPages - 1) lastButton.Enabled = nextButton.Enabled = false;
                else lastButton.Enabled = nextButton.Enabled = true;
            }
            else
            {
                firstButton.Enabled = previousButton.Enabled = false;
                lastButton.Enabled = nextButton.Enabled = false;
            }

            LblTotalImages.Text = img_count + " Images";

            pageLabel.Text = (this.CurrentPage + ((TotalPages != 0) ? 1 : 0)).ToString().PadLeft(this.TotalPages.ToString().Length, '0') + "/" + this.TotalPages;

            LinkedList<Tagger.Database.File> files;
            if (this.tags.Equals(""))
                files = File.GetAll(this.CurrentPage * this.PageSize, this.PageSize);
            else
                files = File.GetByTags(tags, this.CurrentPage * this.PageSize, this.PageSize);

            if (new_page) listView1.Clear();
            listView1.BeginUpdate();
            if (!new_page) listView1.Clear();
            foreach (File file in files)
            {
                listView1.Items.Add(new FileItem(file));
            }
            listView1.EndUpdate();
        }

        public void RefreshContents()
        {
            this.ChangePageTo(this.CurrentPage);
        }

        private void firstButton_Click(object sender, EventArgs e)
        {
            this.ChangePageTo(0);
        }
        private void previousButton_Click(object sender, EventArgs e)
        {
            this.ChangePageTo(this.CurrentPage - 1);
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            this.ChangePageTo(this.CurrentPage + 1);
        }
        private void lastButton_Click(object sender, EventArgs e)
        {
            this.ChangePageTo(this.TotalPages - 1);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                File f = (listView1.SelectedItems[0] as FileItem).File as File;
                if (System.IO.File.Exists(f.Name))
                {
                    if (Settings.Default.ImageViewer.Equals("WindowsDefault"))
                        Process.Start("rundll32.exe", @"C:\WINDOWS\System32\shimgvw.dll,ImageView_Fullscreen " + f.Name);
                    else
                        Process.Start(f.Name);
                }
                else
                    MessageBox.Show("The file " + f.Name + " was not found!\nPlease verify if the file is in the correct location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private SolidBrush sb = new SolidBrush(Color.FromArgb(120, Color.AliceBlue));
        private Pen sel_p = new Pen(Color.SkyBlue, 3);
        private Pen bad_p = new Pen(Color.Red, 3);

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            File f = (e.Item as FileItem).File;
            int x = e.Bounds.X + (e.Bounds.Width - f.Thumbnail.Width) / 2;
            int y = e.Bounds.Y + (e.Bounds.Height - f.Thumbnail.Height) / 2;

            if (System.IO.File.Exists(f.Name))
            {
                e.Graphics.DrawImage(f.Thumbnail, x, y);
            }
            else
            {
                //e.Graphics.DrawImage(Util.MakeGrayscale(f.Thumbnail), x, y);
                e.Graphics.DrawImage(f.Thumbnail, x, y);
                Rectangle r = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 6, e.Bounds.Width - 12, e.Bounds.Height - 12);
                e.Graphics.DrawRectangle(bad_p, r);
            }

            if (e.Item.Selected)
            {
                Rectangle r = new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, e.Bounds.Width - 6, e.Bounds.Height - 6);
                e.Graphics.FillRectangle(sb, r);
                e.Graphics.DrawRectangle(sel_p, r);
            }

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
            FileItem item = listView1.GetItemAt(p.X, p.Y) as FileItem;

            if (item != null && e.Data.GetDataPresent(typeof(ArrayList)))
            {
                ArrayList array = e.Data.GetData(typeof(ArrayList)) as ArrayList;
                foreach (Tag t in array)
                    item.File.AddTag(t);

                //(this.DockPanel.Parent as FrmMain).RefreshAllSearches();
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = listView1.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    FrmTagList ft = null;
                    foreach (DockContent content in this.DockPanel.Contents)
                    {
                        if (content is FrmTagList)
                        {
                            ft = content as FrmTagList;
                        }
                    }

                    if (ft != null && ft.listView1.SelectedItems.Count != 0)
                    {
                        addToThisImageToolStripMenuItem.Visible = true;

                        string str;
                        if (ft.listView1.SelectedItems.Count == 1)
                            str = "the tag \"" + (ft.listView1.SelectedItems[0] as TagItem).Text + "\"";
                        else
                            str = "the " + ft.listView1.SelectedItems.Count + " tags";
                        string imgs_str = (listView1.SelectedItems.Count > 1) ? "these images" : "this image";

                        addToThisImageToolStripMenuItem.Text = "Link " + str + " to " + imgs_str + ".";
                    }
                    else
                    {
                        addToThisImageToolStripMenuItem.Visible = false;
                    }

                    contextMenuStrip1.Show(listView1, new Point(e.X, e.Y));
                }
            }

            if (e.Button == MouseButtons.XButton1) previousButton_Click(sender, e);
            if (e.Button == MouseButtons.XButton2) nextButton_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove the selected images?\n(The files *will not* be deleted.)", "Remove Images", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                foreach (FileItem item in listView1.SelectedItems)
                {
                    item.File.Delete();
                }
                (this.DockPanel.Parent as FrmMain).RefreshAllSearches();
            }
        }

        private void findTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) toolStripButton1_Click(sender, e);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            findTextBox.Text = "";
            toolStripButton1_Click(sender, e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //File f = (listView1.SelectedItems.Count != 0) ? (listView1.SelectedItems[0] as FileItem).File : null;
            //FrmDetails frm = (this.DockPanel.Parent as FrmMain).frmDetails;

            //if (frm != null)
            //{
            //    frm.ClearFiles();
            //    foreach (File file in listView1.SelectedItems)
            //    {
            //        frm.AddFile(file);
            //    }
            //}
        }

        private void addToThisImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTagList ft = null;
            foreach (DockContent content in this.DockPanel.Contents)
            {
                if (content is FrmTagList)
                {
                    ft = content as FrmTagList;
                }
            }

            if (ft != null)
            {
                DbTransaction trans = Manager.Connection.BeginTransaction();
                foreach (TagItem ti in ft.listView1.SelectedItems)
                {
                    Tag t = ti.Tag;
                    foreach (FileItem fi in listView1.SelectedItems)
                    {
                        fi.File.AddTag(t);
                    }

                }
                trans.Commit();

                //(this.DockPanel.Parent as FrmMain).RefreshAllSearches();
            }
        }

        private void findTextBox_Leave(object sender, EventArgs e)
        {
            findTextBox.Text = findTextBox.Text.ToLower();
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listView1.SelectedItems.Count != 0)
            {
                File f = (listView1.SelectedItems[0] as FileItem).File as File;
                if (System.IO.File.Exists(f.Name))
                {
                    if (Settings.Default.ImageViewer.Equals("WindowsDefault"))
                        Process.Start("rundll32.exe", @"C:\WINDOWS\System32\shimgvw.dll,ImageView_Fullscreen " + f.Name);
                    else
                        Process.Start(f.Name);
                }
                else
                    MessageBox.Show("The file " + f.Name + " was not found!\nPlease verify if the file is in the correct location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (e.KeyCode == Keys.Left && e.Alt)
                this.ChangePageTo(this.CurrentPage - 1);
            else if (e.KeyCode == Keys.Right && e.Alt)
                this.ChangePageTo(this.CurrentPage + 1);
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            FrmDetails frm = (this.DockPanel.Parent as FrmMain).frmDetails;

            if (frm != null)
            {
                if (e.IsSelected)
                    frm.AddFile((e.Item as FileItem).File);
                else
                    frm.RemoveFile((e.Item as FileItem).File);

                if (listView1.SelectedItems.Count == 0)
                    frm.ClearFiles();
            }
        }

        private void FrmDisplay_Load(object sender, EventArgs e)
        {
            //Builds Bookmark List
            foreach (TagBookmarkItem tbi in TagBookmarkTemp.TagBookmarkList)
            {
                contextMenuStrip1.Items.Add((TagBookmarkItem)tbi.Clone());
                //tagBookmarkToolStripMenuItem.DropDownItems.Add((TagBookmarkItem)tbi.Clone());
            }
            for (int i = 4; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Click += new EventHandler(TagBookmark_Click);
            }
        }

        #region Refresh Bookmark List
        /// <summary>
        /// Rebuilds bookmarks list
        /// </summary>
        public void RefreshBookmarkList()
        {
            for (int i = 4; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items.Remove(contextMenuStrip1.Items[i]);
            }

            //tagBookmarkToolStripMenuItem.DropDownItems.Clear();

            foreach (TagBookmarkItem tbi in TagBookmarkTemp.TagBookmarkList)
            {
                contextMenuStrip1.Items.Add((TagBookmarkItem)tbi.Clone());
            }

            for (int i = 4; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Click += new EventHandler(TagBookmark_Click);
            }
        } 
        #endregion

        #region Page selection textbox
        private void txtbxPageSelect_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                try
                {
                    int page = Int32.Parse(txtbxPageSelect.Text);
                    ChangePageTo(page - 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    txtbxPageSelect.SelectAll();
                }
            }
        } 
        #endregion
    }
}