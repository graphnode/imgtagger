namespace Tagger.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Tagger.Utils;
    using System.Collections.Generic;
    using Tagger.Database;

    public class ThumbnailViewer : Control
    {
        #region Properties
        private int virtualItemCount;
        public int VirtualItemCount
        { 
            get { return virtualItemCount; } 
            set { 
                virtualItemCount = value;
                UpdateScrollBars();
            } 
        }        
        
        private Size itemSize;
        public Size ItemSize
        {
            get { return itemSize; }
            set { itemSize = value; }
        }

        private Range visibleItems;
        public Range VisibleItems
        {
            get { return visibleItems; }
        }
        #endregion

        public event RequestEventHandler RequestItems;
        
        private VScrollBar vScrollBar;
        private int rowSize;

        private Range itemsRange;
        private List<Thumbnail> itemsThumbnails;

        public ThumbnailViewer()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            //this.vScrollBar.Enabled = false;
            this.vScrollBar.LargeChange = 100;
            this.vScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(16, -1);
            this.vScrollBar.ValueChanged += new EventHandler(vScrollBar_ValueChanged);
            // 
            // ThumbnailViewer
            // 
            this.BackColor = Color.FromKnownColor(KnownColor.Window);
            this.itemSize = new Size(150, 150);
            this.itemsRange = new Range();
            this.itemsThumbnails = new List<Thumbnail>();
            this.Size = new Size(300, 300);
            this.ResizeRedraw = true;
            this.Controls.Add(this.vScrollBar);
            this.ResumeLayout(false);
        }

        private void UpdateScrollBars()
        {
            int current_line = vScrollBar.Value / itemSize.Height;
            int col_count = (this.Width - (vScrollBar.Visible ? 16 : 0)) / itemSize.Width;
            int line_count = (this.Height / itemSize.Height) + 2;

            int total_lines = (int)Math.Ceiling(virtualItemCount / (double)col_count);

            double ratio = (vScrollBar.Maximum == 0)? 0 : (double)vScrollBar.Value / vScrollBar.Maximum;

            vScrollBar.Minimum = 0;
            vScrollBar.Maximum = total_lines * itemSize.Height;
            vScrollBar.Value = Math.Max(0, Math.Min(vScrollBar.Maximum-this.Height, (int)(vScrollBar.Maximum * ratio)));

            vScrollBar.LargeChange = this.Height;
            vScrollBar.SmallChange = 32;

            vScrollBar.Visible = (vScrollBar.Maximum >= this.Height);

            int screen_count = col_count * line_count;
            int current_screen = current_line / screen_count;

            int min = Math.Max(0, current_screen * screen_count - screen_count);
            int max = Math.Min(virtualItemCount, current_screen * screen_count + screen_count*2);

            if (itemsRange.Minimum != min || itemsRange.Maximum != max)
            {
                itemsRange = new Range(min, max);

                itemsThumbnails.Clear();

                if (RequestItems != null)
                {
                    RequestEventArgs rargs = new RequestEventArgs(itemsRange);
                    RequestItems(this, rargs);

                    if (rargs.Items != null)
                        itemsThumbnails.AddRange(rargs.Items);
                }
            }

            this.Invalidate();
        }

        protected void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            int current_line = vScrollBar.Value / itemSize.Height;
            int col_count = (this.Width - (vScrollBar.Visible ? 16 : 0)) / itemSize.Width;
            int line_count = (this.Height / itemSize.Height) + 2;

            int min = Math.Max(0, current_line * this.rowSize);
            int max = Math.Min(virtualItemCount, this.visibleItems.Minimum + line_count * this.rowSize);
            
            if (visibleItems.Minimum != min || visibleItems.Maximum != max) {
                visibleItems = new Range(min, max);
            }

            int screen_count = col_count * line_count;
            int current_screen = current_line / screen_count;

            min = Math.Max(0, current_screen * screen_count - screen_count);
            max = Math.Min(virtualItemCount, current_screen * screen_count + screen_count * 2);

            if (itemsRange.Minimum != min || itemsRange.Maximum != max)
            {
                itemsRange = new Range(min, max);

                itemsThumbnails.Clear();

                if (RequestItems != null)
                {
                    RequestEventArgs rargs = new RequestEventArgs(itemsRange);
                    RequestItems(this, rargs);

                    if (rargs.Items != null)
                        itemsThumbnails.AddRange(rargs.Items);
                }
            }

            this.Invalidate();
        }

        public virtual int GetItemIndexAt(int x, int y)
        {
            int padding = ((this.Width - (vScrollBar.Visible ? 16 : 0)) - rowSize * itemSize.Width) / (rowSize + 1);

            int line = (y + vScrollBar.Value) / itemSize.Height;
            int col = x / (itemSize.Width + padding);
            int index = line * rowSize + col;

            if (x - col * (itemSize.Width + padding) < padding || index > virtualItemCount)
                index = -1;

            return index;
        }

        protected virtual void OnRequestItems(RequestEventArgs e)
        {
            if (RequestItems != null)
                RequestItems(this, e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            MouseEventArgs me = e as MouseEventArgs;
            int index = GetItemIndexAt(me.X, me.Y);
            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            vScrollBar.SetBounds(this.Width - vScrollBar.Width - 2, 0 + 2, vScrollBar.Width, this.Height - 4);
            UpdateScrollBars();
            this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            /*
            if (vScrollBar.Maximum >= this.Height)
            {
                int value = e.Delta / 40 * vScrollBar.SmallChange;
                vScrollBar.Value = Math.Min(vScrollBar.Maximum - this.Height, Math.Max(vScrollBar.Minimum, vScrollBar.Value - value));
            }
            */
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.rowSize = (this.Width - (vScrollBar.Visible ? 16 : 0)) / itemSize.Width;

            int padding;
            if (virtualItemCount < rowSize) padding = 16;
            else padding = ((this.Width - (vScrollBar.Visible ? 16 : 0)) - rowSize * itemSize.Width) / (rowSize + 1);

            int i = itemsRange.Minimum;
            foreach (Thumbnail thumb in itemsThumbnails)
            {
                int x = (i % rowSize) * itemSize.Width + padding * (i % rowSize + 1);
                int y = (i / rowSize) * itemSize.Height - vScrollBar.Value;

                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, itemSize.Width, itemSize.Height));

                /*
                int x = e.ItemRect.X + (e.ItemRect.Width - thumb.Image.Width) / 2;
                int y = e.ItemRect.Y + (e.ItemRect.Height - thumb.Image.Height) / 2;
                e.Graphics.DrawImage(thumb.Image, new Point(x, y));

                if (thumbnailViewer1.selected_index == e.ItemIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Azure)), new Rectangle(x, y, thumb.Image.Width, thumb.Image.Height));
                }
                */

                i++;
            }


            ControlPaint.DrawBorder3D(e.Graphics, this.ClientRectangle, Border3DStyle.Sunken);
        }
       
    }

    public class Thumbnail
    {
        public File File;
        public string Hash;
        public Bitmap Image;

        public Thumbnail() {}

        public Thumbnail(File file)
        {
            this.File = file;
            this.Hash = Util.Md5FromFile(file.Name);
            this.Image = file.Thumbnail;
        }
    }


    public class RequestEventArgs : EventArgs
    {
        public Range Range;
        public List<Thumbnail> Items;

        public RequestEventArgs(Range range)
        {
            this.Range = range;
        }
    }
    public delegate void RequestEventHandler(object sender, RequestEventArgs args);

}
