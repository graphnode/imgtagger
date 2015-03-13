using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Generic;

namespace Graphnode.Deploy.Updating
{
    public class AutoUpdater : Component
    {
        public Version CurrentVersion;
        public WebClient wc;

        private Form owner;

        [DefaultValue("")]
        public string RemoteServer
        {
            get { return remoteServer; }
            set { this.remoteServer = value; }
        }
        private string remoteServer = "";

        public FrmUpdate frm;
        private long current_size;

        public AutoUpdater()
        {
            // Old updates clean up.
            string[] files = Directory.GetFiles(".", "*.uptmp");
            foreach (string file in files) File.Delete(file);
        }
        public AutoUpdater(IContainer container) : this()
        {
            container.Add(this);
        }

        public void Run(Form owner)
        {
            this.Run(false, owner);
        }
        public void Run(bool background, Form owner)
        {
            this.owner = owner;

            Version currentVersion = (this.CurrentVersion == null) ? Assembly.GetEntryAssembly().GetName().Version : this.CurrentVersion;
            Version nextVersion = null;

            XmlDocument doc = new XmlDocument();
            try { doc.Load(remoteServer); }
            catch (Exception e)
            {
                if (!background) MessageBox.Show("Error while contacting update server.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (doc.DocumentElement.HasAttribute("version"))
                nextVersion = new Version(doc.DocumentElement.Attributes["version"].Value);

            if (nextVersion == null || nextVersion <= currentVersion)
            {
                if (!background) MessageBox.Show("No update found.");
                return;
            }

            DialogResult dr = MessageBox.Show("A new version has been found (" + nextVersion.ToString(3) + ")." + "\n" + "Do you wish to update?", "Update Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr != DialogResult.Yes) return;

            frm = new FrmUpdate(remoteServer, doc);
            if (frm.ShowDialog(owner) == DialogResult.Cancel)
            {
                MessageBox.Show("Update was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Update complete!\nThe application will now restart.", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ApplicationRestart();                
            }
        }

        public void ApplicationRestart()
        {
            owner.Close();
            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
