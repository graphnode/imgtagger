namespace Tagger
{
    partial class FrmHelp
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.LblTitle = new System.Windows.Forms.ToolStripLabel();
            this.BtnNext = new System.Windows.Forms.ToolStripButton();
            this.BtnHome = new System.Windows.Forms.ToolStripButton();
            this.BtnPrevious = new System.Windows.Forms.ToolStripButton();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblTitle,
            this.BtnNext,
            this.BtnHome,
            this.BtnPrevious});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(220, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // LblTitle
            // 
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(52, 22);
            this.LblTitle.Text = "page title";
            // 
            // BtnNext
            // 
            this.BtnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnNext.Image = global::Tagger.Properties.Resources.resultset_next;
            this.BtnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(23, 22);
            this.BtnNext.Text = "toolStripButton3";
            this.BtnNext.ToolTipText = "Next Page";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnHome
            // 
            this.BtnHome.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnHome.Image = global::Tagger.Properties.Resources.house;
            this.BtnHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(23, 22);
            this.BtnHome.Text = "toolStripButton2";
            this.BtnHome.ToolTipText = "Return to Intro";
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnPrevious.Image = global::Tagger.Properties.Resources.resultset_previous;
            this.BtnPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(23, 22);
            this.BtnPrevious.ToolTipText = "Previous Page";
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(0, 25);
            this.webBrowser.MinimumSize = new System.Drawing.Size(120, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(220, 387);
            this.webBrowser.TabIndex = 2;
            this.webBrowser.TabStop = false;
            this.webBrowser.Url = new System.Uri("res://tagger.exe/Help_Intro.htm", System.UriKind.Absolute);
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            // 
            // FrmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 412);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "FrmHelp";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.FrmHelp_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel LblTitle;
        private System.Windows.Forms.ToolStripButton BtnPrevious;
        private System.Windows.Forms.ToolStripButton BtnHome;
        private System.Windows.Forms.ToolStripButton BtnNext;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}