using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;

namespace Graphnode
{
    public partial class FrmUpdate : Form
    {
        private string RemoteServer;
        private XmlDocument Doc;
        private WebClient Wc;

        long current_size;

        public FrmUpdate(string remoteServer, XmlDocument doc)
        {
            InitializeComponent();
            this.RemoteServer = remoteServer;
            this.Doc = doc;
        }

        private void FrmUpdate_Shown(object sender, EventArgs e)
        {
            XmlElement root = Doc.DocumentElement;

            XmlElement filesystem = root.SelectSingleNode("/update/filesystem") as XmlElement;
            LinkedList<string> files = new LinkedList<string>();
            int total_size = 0;

            foreach (XmlElement file in filesystem.ChildNodes)
            {
                files.AddLast(file.Attributes["name"].Value);
                total_size += Int32.Parse(file.Attributes["size"].Value);
            }

            progressBar.Minimum = 0;
            progressBar.Maximum = total_size;


            Wc = new WebClient();
            Wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            Wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);

            string f = files.First.Value;
            current_size = 0;
            Wc.DownloadFileAsync(new Uri(GetParentUriString(RemoteServer) + f), Path.GetTempPath() + f, files.First);
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = Math.Min(progressBar.Maximum, (int)(current_size + e.BytesReceived));

            if (e.BytesReceived == e.TotalBytesToReceive)
                current_size += e.TotalBytesToReceive;
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            LinkedListNode<string> node = e.UserState as LinkedListNode<string>;
            if (node.Next != null)
            {
                node = node.Next;
                string file = node.Value;
                Wc.DownloadFileAsync(new Uri(GetParentUriString(RemoteServer) + file), Path.GetTempPath() + file, node);
            }
            else
            {
                BtnCancel.Enabled = false;

                foreach (string file in node.List)
                {
                    try
                    {
                        File.Delete("./" + file);
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        File.Delete("./" + file + ".uptmp");
                        File.Move("./" + file, "./" + file + ".uptmp");
                    }

                    File.Move(Path.GetTempPath() + file, "./" + file);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private static string GetParentUriString(string url)
        {
            Uri uri = new Uri(url);
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments[uri.Segments.Length - 1].Length - uri.Query.Length);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
