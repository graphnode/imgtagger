using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Graphnode.Deploy.Updating;
using Tagger.Database;
using Tagger.Properties;
using Tagger.Utils;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tagger
{
    public partial class FrmMain : Form
    {
        public FrmHelp frmHelp;
        public FrmDetails frmDetails;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            toolStripAddFolder.Enabled = false;
            toolStripAddImage.Enabled = false;
            toolStripAddTag.Enabled = false;
            toolStripTagList.Enabled = false;
            toolStripDetails.Enabled = false;

            toolStripMenuItem7.Enabled = false;

            toolStripMenuItem2.Enabled = false;
            toolStripMenuItem3.Enabled = false;

            Version version = Assembly.GetEntryAssembly().GetName().Version;

            this.Text = this.Text + " " + version.ToString();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (Settings.Default.ShowIntro)
                (new FrmIntro()).ShowDialog(this);
            else
                if (!Settings.Default.StartDatabase.Equals(""))
                {
                    this.OpenDatabase(Settings.Default.StartDatabase);
                }

            if (Settings.Default.FirstTime)
                frmHelp.Show(this.dockPanel, DockState.DockRight);

            Settings.Default.FirstTime = false;
            Settings.Default.Save();

            autoUpdater.Run(true, this);
        }

        public void OpenDatabase(string filename)
        {
            Manager.Initialize(filename);

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (System.IO.File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, new DeserializeDockContent(GetContentFromPersistString));
            else
            {
                FrmTagList tags = new FrmTagList();
                tags.Show(this.dockPanel, DockState.DockLeftAutoHide);

                FrmDisplay search = new FrmDisplay();
                search.Show(this.dockPanel, DockState.Document);
            }

            toolStripAddFolder.Enabled = true;
            toolStripAddImage.Enabled = true;
            toolStripAddTag.Enabled = true;
            toolStripTagList.Enabled = true;
            toolStripDetails.Enabled = true;

            toolStripMenuItem7.Enabled = true;

            toolStripMenuItem2.Enabled = true;
            toolStripMenuItem3.Enabled = true;
        }

        public void CloseDatabase()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel.SaveAsXml(configFile);

            for (int i = dockPanel.Contents.Count - 1; i > -1; i--)
            {
                DockContent dc = (dockPanel.Contents[i] as DockContent);
                dc.Hide(); dc.Close();
            }

            toolStripAddFolder.Enabled = false;
            toolStripAddImage.Enabled = false;
            toolStripAddTag.Enabled = false;
            toolStripTagList.Enabled = false;
            toolStripDetails.Enabled = false;

            toolStripMenuItem7.Enabled = false;

            toolStripMenuItem2.Enabled = false;
            toolStripMenuItem3.Enabled = false;

            if (Manager.Connection != null)
                Manager.Terminate();
        }
        
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FrmDisplay).ToString())
                return new FrmDisplay();
            else if (persistString == typeof(FrmTagList).ToString())
                return new FrmTagList();
            else if (persistString == typeof(FrmDetails).ToString())
            {
                frmDetails = new FrmDetails();
                return frmDetails;
            }
            else if (persistString == typeof(FrmHelp).ToString())
            {
                frmHelp = new FrmHelp();
                return frmHelp;
            }
            return null;
        }

        public void RefreshAllSearches()
        {
            foreach (FrmDisplay fs in dockPanel.Documents)
                fs.RefreshContents();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CloseDatabase();
        }

        private void createNewDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Manager.Connection != null)
            {
                DialogResult dr = MessageBox.Show("Creating a new database will close the currently open database.\nDo you wish to proceed?", "Create New Database", MessageBoxButtons.OKCancel);
                if (dr != DialogResult.OK) return;
            }

            if (databaseSaveDialog.ShowDialog() == DialogResult.OK)
            {
                if (Manager.Connection != null) this.CloseDatabase();

                if (System.IO.File.Exists(databaseSaveDialog.FileName))
                    System.IO.File.Delete(databaseSaveDialog.FileName);

                this.OpenDatabase(databaseSaveDialog.FileName);
            }
        }

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Manager.Connection != null)
            {
                DialogResult dr = MessageBox.Show("Opening a new database will close the currently open database.\nDo you wish to proceed?", "Open Database", MessageBoxButtons.OKCancel);
                if (dr != DialogResult.OK) return;
            }

            if (databaseOpenDialog.ShowDialog() == DialogResult.OK)
            {
                if (Manager.Connection != null) this.CloseDatabase();
                this.OpenDatabase(databaseOpenDialog.FileName);
            }
        }

        private void closeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CloseDatabase();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            FrmDisplay search = new FrmDisplay();
            search.Show(dockPanel, DockState.Document);
        }

        private void tagsWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DockContent content in dockPanel.Contents)
            {
                if (content is FrmTagList)
                {
                    content.Show();
                    return;
                }
            }
            FrmTagList tags = new FrmTagList();
            tags.Show(dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDisplay search = new FrmDisplay();
            search.Show(dockPanel);
        }

        private void addImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageDialog.ShowDialog(this) == DialogResult.OK)
            {
                FrmUpload frm = new FrmUpload(imageDialog.FileNames);
                frm.ShowDialog(this);
                //this.RefreshAllSearches();
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                FrmUpload frm = new FrmUpload(Directory.GetFiles(folderDialog.SelectedPath));                
                frm.ShowDialog(this);
                //this.RefreshAllSearches();
            }
        }

        private void tagPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTagOpt tagprop = new FrmTagOpt();
            tagprop.ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new FrmOptions()).ShowDialog(this);
        }

        private void imageDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmDetails == null) frmDetails = new FrmDetails();
            frmDetails.Show(dockPanel, DockState.DockLeftAutoHide);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new FrmAbout()).ShowDialog();
        }

        private void helpToolStrip_Click(object sender, EventArgs e)
        {
            if (frmHelp == null) frmHelp = new FrmHelp();
            frmHelp.Show(this.dockPanel, DockState.DockRight);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoUpdater.Run(this);
        }

        private void saveCurrentBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog1.OpenFile();
                BinaryFormatter bformatter = new BinaryFormatter();

                TagBookmark newTagBookmark = new TagBookmark(TagBookmarkTemp.TagBookmarkList);

                bformatter.Serialize(stream, newTagBookmark);
                stream.Close();
            }
        }

        private void loadBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = openFileDialog1.OpenFile();
                BinaryFormatter bformatter = new BinaryFormatter();

                TagBookmark newTagBookmark = (TagBookmark)bformatter.Deserialize(stream);
                TagBookmarkTemp.TagBookmarkList = newTagBookmark.TagBookmarkList;
            }

            foreach (FrmDisplay fs in this.dockPanel.Documents)
                fs.RefreshBookmarkList();
        }
    }
}
