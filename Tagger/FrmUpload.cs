using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tagger.Utils;
using Tagger.Database;
using System.Data.Common;
using System.IO;

namespace Tagger
{
    public partial class FrmUpload : Form
    {
        private string[] Files;
        private bool Done = false;

        public FrmUpload(string[] files)
        {
            InitializeComponent();
            this.Files = files;
        }

        private void FrmUpload_Shown(object sender, EventArgs e)
        {
            label1.Text = "0 of " + Files.Length + " loaded.";
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.Done)
            {
                backgroundWorker1.CancelAsync();
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DbTransaction trans = Manager.Connection.BeginTransaction();
            try
            {
                int counter = 0;

                foreach (string filename in Files)
                {
                    counter++;
                    try
                    {
                        byte[] rawData = System.IO.File.ReadAllBytes(filename);
                        using (Image img = Image.FromStream(new MemoryStream(rawData)))
                        {
                            string md5 = Util.Md5FromBytes(rawData);
                            Tagger.Database.File hash_file = Tagger.Database.File.GetByHash(md5);
                            if (hash_file != null)
                            {
                                string fname = filename;
                                this.BeginInvoke(new MethodInvoker(delegate()
                                {
                                    textBox1.Text += Path.GetFileName(fname) + " not added (hash collision with " + Path.GetFileName(hash_file.Name) + ")." + Environment.NewLine;
                                }));
                                continue;
                            }

                            Tagger.Database.File f = new Tagger.Database.File();
                            f.Name = filename;
                            f.Hash = md5;
                            Bitmap thumbnail = Util.CreateThumbnail(img, 128, 128);
                            f.Thumbnail = thumbnail;

                            DateTime modifyTime = System.IO.File.GetLastWriteTime(filename);

                            string modifyMonth = string.Empty;
                            string modifyDay = string.Empty;
                            string modifyHour = string.Empty;
                            string modifyMinute = string.Empty;
                            string modifySecond = string.Empty;

                            if (modifyTime.Month.ToString().Length < 2)
                                modifyMonth = "0" + modifyTime.Month.ToString();
                            else modifyMonth = modifyTime.Month.ToString();

                            if (modifyTime.Day.ToString().Length < 2)
                                modifyDay = "0" + modifyTime.Day.ToString();
                            else modifyDay = modifyTime.Day.ToString();

                            if (modifyTime.Hour.ToString().Length < 2)
                                modifyHour = "0" + modifyTime.Hour.ToString();
                            else modifyHour = modifyTime.Hour.ToString();

                            if (modifyTime.Minute.ToString().Length < 2)
                                modifyMinute = "0" + modifyTime.Minute.ToString();
                            else modifyMinute = modifyTime.Minute.ToString();

                            if (modifyTime.Second.ToString().Length < 2)
                                modifySecond = "0" + modifyTime.Second.ToString();
                            else modifySecond = modifyTime.Second.ToString();

                            string modifyTimeStr = modifyTime.Year.ToString() + "-" + modifyMonth + "-" + modifyDay + " "
                                + modifyHour + ":" + modifyMinute + ":" + modifySecond;

                            f.modtime = modifyTimeStr;

                            f.Insert();

                            backgroundWorker1.ReportProgress((int)((counter / (double)Files.Length) * 100), new Stuff(counter, thumbnail));
                        }

                        if (backgroundWorker1.CancellationPending)
                        {
                            trans.Rollback();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is OutOfMemoryException || ex is ArgumentException)
                        {
                            string fname = filename;
                                this.BeginInvoke(new MethodInvoker(delegate()
                                {
                                    textBox1.Text += Path.GetFileName(fname) + " not added (not image)." + Environment.NewLine;
                                }));
                            continue;
                        }
                        else
                            throw;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
            }
            finally
            {
                if (!backgroundWorker1.CancellationPending)
                    trans.Commit();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Stuff stuff = e.UserState as Stuff;
            label1.Text = stuff.Count + " of " + Files.Length + " loaded.";
            progressBar1.Value = e.ProgressPercentage;

            if (stuff.Thumbnail != null && stuff.Count % 3 == 0)
            {
                if (pictureBox1.Image != null) pictureBox1.Image.Dispose();
                pictureBox1.Image = stuff.Thumbnail;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Done = true;
            button1.Text = "Close";
        }
    }

    public class Stuff
    {
        public int Count;
        public Bitmap Thumbnail;

        public Stuff(int count, Bitmap thumbnail)
        {
            this.Count = count;
            this.Thumbnail = thumbnail;
        }
    }
}
