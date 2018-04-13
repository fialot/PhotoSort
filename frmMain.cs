using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using myFunctions;
using ExifLib;
using ExifLibrary;


namespace PhotoSort
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        List<PhotoFile> PhotoList;
        AbortableBackgroundWorker process;

        public struct sortSettings
        {
            public bool WriteShiftedTime;
            public bool SetImgTime;
            public bool ClearFolder;
            public string DestFolder;
            public string FileMask;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " v" + Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2);

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            olvFolders.SetObjects(new List<ffolder>());
            colTimeShift.AspectGetter = delegate (object x) { return ((ffolder)x).TimeShift.ToString(); };
            colTimeShift.AspectPutter = delegate (object x, object newValue) { try { ((ffolder)x).TimeShift = TimeSpan.Parse((string)newValue); } catch { } };

            txtDestFolder.Text = Properties.Settings.Default.DestFolder;
            string[] SourceFolders = Properties.Settings.Default.SourceFolders.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            txtFileMask.Text = Properties.Settings.Default.Mask;

            foreach (string item in SourceFolders)
            {
                ffolder folder = new ffolder(System.IO.Path.GetFileName(item), item);
                olvFolders.AddObject(folder);
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;


            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ffolder folder = new ffolder(System.IO.Path.GetFileName(dialog.FileName), dialog.FileName);
                olvFolders.AddObject(folder);
            }
            dialog.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            olvFolders.SetObjects(new List<ffolder>());
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            olvFolders.RemoveObjects(olvFolders.CheckedObjects);
        }

        private void btnDestFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtDestFolder.Text = dialog.FileName;
            }
            dialog.Dispose();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if ((string)btnStart.Tag != "run")
            {
                if (!System.IO.Directory.Exists(txtDestFolder.Text))
                {
                    MessageBox.Show("Destination directory not exits!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (olvFolders.Items.Count == 0)
                {
                    MessageBox.Show("No input folders!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sortSettings set;
                set.ClearFolder = chbClearDestFolder.Checked;
                set.SetImgTime = chbSetImgDate.Checked;
                set.WriteShiftedTime = chbWriteToExif.Checked;
                set.DestFolder = txtDestFolder.Text;
                set.FileMask = txtFileMask.Text;

                process = new AbortableBackgroundWorker();
                process.DoWork += Sorting;
                process.RunWorkerCompleted += WorkComplete;
                process.ProgressChanged += ProgressChanged;
                process.RunWorkerAsync(set);
                process.WorkerSupportsCancellation = true;
                process.WorkerReportsProgress = true;

                btnStart.Text = "Stop";
                btnStart.Tag = "run";
                txtLog.Text = "";
                progBar.Value = 0;
            }
            else
            {
                process.CancelAsync();
                process.Abort();
                process.Dispose();
            }

        }

        private string Format(string format, int index, string fileName, string folderName, DateTime date)
        {
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

            int pos = format.IndexOf("%i");
            if (pos >= 0)
            {
                int size = Conv.ToIntDef(format.Substring(pos + 2, 1), -1);
                if (size >= 0)
                {
                    format = format.Replace("%i" + size.ToString(), index.ToString("D" + size.ToString()));
                }
                else
                {
                    format = format.Replace("%i", index.ToString());
                }
            }
            format = format.Replace("%N", fileName);
            format = format.Replace("%F", folderName);

            format = format.Replace("%yyyy", date.Year.ToString("D4"));
            format = format.Replace("%yy", (date.Year-2000).ToString("D2"));
            format = format.Replace("%MM", date.Month.ToString("D2"));
            format = format.Replace("%M", date.Month.ToString());
            format = format.Replace("%dd", date.Day.ToString("D2"));
            format = format.Replace("%d", date.Day.ToString());

            format = format.Replace("%hh", date.Hour.ToString("D2"));
            format = format.Replace("%h", date.Hour.ToString());
            format = format.Replace("%mm", date.Minute.ToString("D2"));
            format = format.Replace("%m", date.Minute.ToString());
            format = format.Replace("%ss", date.Second.ToString("D2"));
            format = format.Replace("%s", date.Second.ToString());

            return format + ext;
        }

        enum dateImgStat { None, Exif, File}


        private DateTime GetImgDate(string path)
        {
            dateImgStat status;
            return GetImgDate(path, out status);
        }

        private DateTime GetImgDate(string path, out dateImgStat status)
        {
            DateTime date;
            status = dateImgStat.None;
            try
            {
                try
                {
                    using (ExifReader reader = new ExifReader(path))
                    {
                        if (reader.GetTagValue<DateTime>(ExifTags.DateTimeOriginal, out date))
                        {
                            status = dateImgStat.Exif;
                            return date;
                        }
                        else if (reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out date))
                        {
                            status = dateImgStat.Exif;
                            return date;
                        }
                        else if (reader.GetTagValue<DateTime>(ExifTags.DateTime, out date))
                        {
                            status = dateImgStat.Exif;
                            return date;
                        }
                    }

                } catch { }

                status = dateImgStat.File;
                return System.IO.File.GetCreationTime(path);
            }
            catch
            {
                status = dateImgStat.None;
                return DateTime.MinValue;
            }
        }

        private bool SetImgDate(string path, DateTime date, string destPath)
        {
            try
            {
                ImageFile file = ImageFile.FromFile(path);
                foreach (ExifProperty item in file.Properties)
                {
                    if (item.Name == "DateTime")
                        item.Value = date;
                    if (item.Name == "DateTimeOriginal")
                        item.Value = date;
                    if (item.Name == "DateTimeDigitized")
                        item.Value = date;
                }
                file.Save(destPath);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        private DateTime GetDate(byte[] value, int len)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

            string result = System.Text.Encoding.ASCII.GetString(value, 0, len - 1);
            try
            {
                return DateTime.ParseExact(result, "yyyy:MM:dd HH:mm:ss", provider);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        private byte[] SetDate(DateTime date)
        {
            byte[] result = new byte[20];

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            string strDate = date.ToString("yyyy:MM:dd HH:mm:ss");

            Encoding.ASCII.GetBytes(strDate, 0, strDate.Length, result, 0);

            return result;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.DestFolder = txtDestFolder.Text;
            string sourceFolder = "" ;
            foreach(ffolder item in olvFolders.Objects)
            {
                if (sourceFolder != "") sourceFolder += ";";
                sourceFolder += item.Path;
            }
            Properties.Settings.Default.SourceFolders = sourceFolder;
            Properties.Settings.Default.Mask = txtFileMask.Text;
            Properties.Settings.Default.Save();
        }

        private void Log(string text, Color color)
        {
            Invoke(new Action(() =>
            {
                txtLog.Select(txtLog.Text.Length, 0);
                txtLog.SelectionColor = color;
                txtLog.AppendText(text + Environment.NewLine);
            }));
        }


        private void Sorting(object sender, DoWorkEventArgs e)
        {
            const int progSearch = 50;

            DateTime date;
            bool rewriteExif;
            PhotoList = new List<PhotoFile>();
            sortSettings set = (sortSettings)e.Argument;
            AbortableBackgroundWorker worker = (AbortableBackgroundWorker)sender;
            float progress = 0;
            float progresFInc = 0, progresInc = 0;


            // ----- Creating file list -----
            Log("Creating file list...", Color.Black);
            int folderCount = olvFolders.Items.Count;
            if (folderCount == 0)
            {
                Log("Warning: No input folders!", Color.Orange);
                return;
            }
            progresFInc = progSearch / folderCount;
            foreach (ffolder item in olvFolders.Objects)
            {
                string procFile = "";
                try
                {
                    // ----- Search folders -----
                    Log("Search in: " + item.Path, Color.Black);
                    string[] fileList = System.IO.Directory.GetFiles(item.Path);
                    progresInc = progresFInc / fileList.Length;
                    foreach (string file in fileList)
                    {
                        procFile = System.IO.Path.GetFileName(file);
                        rewriteExif = false;
                        dateImgStat status;
                        date = GetImgDate(file, out status);
                        if (status == dateImgStat.File) Log("Warning in " + procFile + ": No image time in EXIF -> Using File date", Color.Orange);

                        if (date != DateTime.MinValue)
                        {
                            if (item.TimeShift != TimeSpan.Zero)
                            {
                                date += item.TimeShift;
                                if (set.WriteShiftedTime) rewriteExif = true;
                            }
                        }
                        else
                            Log("Warning in " + procFile + ": No valid image time.", Color.Orange);

                        PhotoList.Add(new PhotoFile(file, item.Name, item.TimeShift, date, rewriteExif));
                        progress += progresInc;
                        worker.ReportProgress((int)progress);
                    }
                }
                catch (Exception Err)
                {
                    Log("Error in " + procFile + ": " + Err.Message, Color.Red);
                }

            }

            // ----- Check if found photos -----
            if (PhotoList.Count == 0)
            {
                Log("Warning: No photos found!", Color.Orange);
                return;
            }

            // ----- Sorting -----
            Log("Sorting...", Color.Black);
            PhotoList = PhotoList.OrderBy(o => o.Date).ToList();

            // ----- Delete destination folder -----
            if (set.ClearFolder)
            {
                string[] destFileList = System.IO.Directory.GetFiles(txtDestFolder.Text);
                foreach (string item in destFileList) System.IO.File.Delete(item);
            }

            // ----- Copy -----
            Log("Copying...", Color.Black);
            progresInc = (100.0f - progSearch) / PhotoList.Count;
            for (int i = 0; i < PhotoList.Count; i++)
            {
                try
                {
                    string destPath = set.DestFolder  + System.IO.Path.DirectorySeparatorChar + Format(set.FileMask, i + 1, System.IO.Path.GetFileName(PhotoList[i].Path), PhotoList[i].Folder, PhotoList[i].Date);
                    if (PhotoList[i].RewriteExif)
                    {
                        SetImgDate(PhotoList[i].Path, PhotoList[i].Date, destPath);
                    }
                    else
                        System.IO.File.Copy(PhotoList[i].Path, destPath, true);
                    if (set.SetImgTime)
                    {
                        if (PhotoList[i].Date != DateTime.MinValue)
                        {
                            System.IO.File.SetCreationTime(destPath, PhotoList[i].Date);
                            System.IO.File.SetLastWriteTime(destPath, PhotoList[i].Date);
                        }
                        else
                        {
                            Log("Warning in " + System.IO.Path.GetFileName(destPath) + ": File time from EXIF not saved (no time in EXIF).", Color.Orange);
                        }
                        
                    }
                    progress += progresInc;
                    worker.ReportProgress((int)progress);
                }
                catch (Exception Err)
                {
                    Log("Error in " + System.IO.Path.GetFileName(PhotoList[i].Path) + ": " + Err.Message, Color.Red);
                }
            }
            worker.ReportProgress(100);
        }

        private void WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                txtLog.Select(txtLog.Text.Length, 0);
                txtLog.SelectionColor = Color.Black;

                if (e.Cancelled)
                {
                    Log("----- ABORTED -----", Color.Black);
                }
                else
                {
                    Log("----- WORK DONE -----", Color.Black);
                }

                btnStart.Tag = "";
                btnStart.Text = "Start";
                progBar.Value = 0;

            }));

            if (e.Cancelled)
                MessageBox.Show("Work Aborted!");
            else
                MessageBox.Show("Work Complete");
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                progBar.Value = e.ProgressPercentage;
            }
            catch (Exception) { }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    }
}
