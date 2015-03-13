namespace Tagger
{
    partial class FrmDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.findLabel = new System.Windows.Forms.ToolStripLabel();
            this.findTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.findButton = new System.Windows.Forms.ToolStripButton();
            this.lastButton = new System.Windows.Forms.ToolStripButton();
            this.nextButton = new System.Windows.Forms.ToolStripButton();
            this.txtbxPageSelect = new System.Windows.Forms.ToolStripTextBox();
            this.pageLabel = new System.Windows.Forms.ToolStripLabel();
            this.previousButton = new System.Windows.Forms.ToolStripButton();
            this.firstButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LblTotalImages = new System.Windows.Forms.ToolStripLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToThisImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fecharToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new Tagger.Controls.FileView();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findLabel,
            this.findTextBox,
            this.findButton,
            this.lastButton,
            this.nextButton,
            this.txtbxPageSelect,
            this.pageLabel,
            this.previousButton,
            this.firstButton,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.LblTotalImages});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(579, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // findLabel
            // 
            this.findLabel.Name = "findLabel";
            this.findLabel.Size = new System.Drawing.Size(44, 24);
            this.findLabel.Text = "Search:";
            // 
            // findTextBox
            // 
            this.findTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.findTextBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findTextBox.Margin = new System.Windows.Forms.Padding(1, 2, 1, 4);
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.findTextBox.Size = new System.Drawing.Size(120, 21);
            this.findTextBox.Leave += new System.EventHandler(this.findTextBox_Leave);
            this.findTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.findTextBox_KeyUp);
            // 
            // findButton
            // 
            this.findButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findButton.Image = global::Tagger.Properties.Resources.find;
            this.findButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(23, 24);
            this.findButton.Text = "Search";
            this.findButton.ToolTipText = "Search Tags";
            this.findButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // lastButton
            // 
            this.lastButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lastButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lastButton.Image = global::Tagger.Properties.Resources.resultset_last;
            this.lastButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lastButton.Name = "lastButton";
            this.lastButton.Size = new System.Drawing.Size(23, 24);
            this.lastButton.ToolTipText = "Last Page";
            this.lastButton.Click += new System.EventHandler(this.lastButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.nextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nextButton.Image = global::Tagger.Properties.Resources.resultset_next;
            this.nextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(23, 24);
            this.nextButton.ToolTipText = "Next Page";
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // txtbxPageSelect
            // 
            this.txtbxPageSelect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtbxPageSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtbxPageSelect.Name = "txtbxPageSelect";
            this.txtbxPageSelect.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtbxPageSelect.Size = new System.Drawing.Size(40, 27);
            this.txtbxPageSelect.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtbxPageSelect_KeyUp);
            // 
            // pageLabel
            // 
            this.pageLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pageLabel.Name = "pageLabel";
            this.pageLabel.Size = new System.Drawing.Size(37, 24);
            this.pageLabel.Text = "?? / ??";
            // 
            // previousButton
            // 
            this.previousButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.previousButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.previousButton.Image = global::Tagger.Properties.Resources.resultset_previous;
            this.previousButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(23, 24);
            this.previousButton.ToolTipText = "Previous Page";
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // firstButton
            // 
            this.firstButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.firstButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.firstButton.Image = global::Tagger.Properties.Resources.resultset_first;
            this.firstButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new System.Drawing.Size(23, 24);
            this.firstButton.ToolTipText = "First Page";
            this.firstButton.Click += new System.EventHandler(this.firstButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Tagger.Properties.Resources.cross;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 24);
            this.toolStripButton1.Text = "Clear Search";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // LblTotalImages
            // 
            this.LblTotalImages.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.LblTotalImages.Name = "LblTotalImages";
            this.LblTotalImages.Size = new System.Drawing.Size(0, 24);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToThisImageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(191, 76);
            // 
            // addToThisImageToolStripMenuItem
            // 
            this.addToThisImageToolStripMenuItem.Name = "addToThisImageToolStripMenuItem";
            this.addToThisImageToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.addToThisImageToolStripMenuItem.Text = "Add <...> to this Image";
            this.addToThisImageToolStripMenuItem.Visible = false;
            this.addToThisImageToolStripMenuItem.Click += new System.EventHandler(this.addToThisImageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.toolStripMenuItem1.Text = "View Properties";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(190, 22);
            this.toolStripMenuItem2.Text = "Remove";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fecharToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(108, 26);
            // 
            // fecharToolStripMenuItem
            // 
            this.fecharToolStripMenuItem.Image = global::Tagger.Properties.Resources.cross;
            this.fecharToolStripMenuItem.Name = "fecharToolStripMenuItem";
            this.fecharToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.fecharToolStripMenuItem.Text = "Fechar";
            this.fecharToolStripMenuItem.Click += new System.EventHandler(this.fecharToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 27);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(579, 339);
            this.listView1.TabIndex = 1;
            this.listView1.TileSize = new System.Drawing.Size(150, 150);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listView1_DrawItem);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.listView1_DragOver);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // FrmDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 366);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmDisplay";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.TabPageContextMenuStrip = this.contextMenuStrip2;
            this.Text = "All Images";
            this.Load += new System.EventHandler(this.FrmDisplay_Load);
            this.Shown += new System.EventHandler(this.FrmSearch_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel findLabel;
        private System.Windows.Forms.ToolStripTextBox findTextBox;
        private System.Windows.Forms.ToolStripButton findButton;
        private System.Windows.Forms.ToolStripButton previousButton;
        private System.Windows.Forms.ToolStripButton nextButton;
        private System.Windows.Forms.ToolStripLabel pageLabel;
        private System.Windows.Forms.ToolStripButton lastButton;
        private System.Windows.Forms.ToolStripButton firstButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem addToThisImageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fecharToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel LblTotalImages;
        public Tagger.Controls.FileView listView1;
        private System.Windows.Forms.ToolStripTextBox txtbxPageSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}