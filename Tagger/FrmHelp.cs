using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections;
using System.IO;

namespace Tagger
{
    public partial class FrmHelp : DockContent
    {
        public FrmHelp()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Tagger.Properties.Resources.help.GetHicon());
        }

        private void FrmHelp_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate(Path.GetFullPath(".") + "/Manual/index.htm");
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            LblTitle.Text = webBrowser.Document.Title;

            BtnPrevious.Enabled = webBrowser.CanGoBack;
            BtnNext.Enabled = webBrowser.CanGoForward;
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack)
                webBrowser.GoBack();
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            webBrowser.Navigate(Path.GetFullPath(".") + "/Manual/index.htm");
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoForward)
                webBrowser.GoForward();
        }
    }
}
