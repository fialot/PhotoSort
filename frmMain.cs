﻿using System;
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
using System.Resources;
using MediaInfo;
using System.IO;
using System.Globalization;

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

        ResourceManager resources = new ResourceManager("PhotoSort.languages.lang", typeof(frmMain).Assembly);

        public struct sortSettings
        {
            public bool WriteShiftedTime;
            public bool SetImgTime;
            public bool ClearFolder;
            public string DestFolder;
            public string FileMask;
        }

        #region Form

        /// <summary>
        /// Get Language String
        /// </summary>
        /// <param name="ID">ID of string</param>
        /// <param name="def">Default value</param>
        /// <returns></returns>
        private string lng(string ID, string def)
        {
            string res;
            try
            {
                res = (string)resources.GetObject(ID);
                if (res == null) return def;
            }
            catch
            {
                return def;
            }
            return res;
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            // ----- Get app version -----
            this.Text = this.Text + " v" + Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2);

            // ----- Init ObjectListView -----
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            olvFolders.SetObjects(new List<ffolder>());
            colTimeShift.AspectGetter = delegate (object x) { return ((ffolder)x).TimeShift.ToString(); };
            colTimeShift.AspectPutter = delegate (object x, object newValue) { try { ((ffolder)x).TimeShift = TimeSpan.Parse((string)newValue); } catch { } };

            // ----- Load settings -----
            txtDestFolder.Text = Properties.Settings.Default.DestFolder;
            string[] SourceFolders = Properties.Settings.Default.SourceFolders.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            string[] TimeShifts = Properties.Settings.Default.TimeShift.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            cbFileMask.Text = Properties.Settings.Default.Mask;
            chbWriteToExif.Checked = Properties.Settings.Default.WriteSTExif;
            chbSetImgDate.Checked = Properties.Settings.Default.SetFileDate;
            chbClearDestFolder.Checked = Properties.Settings.Default.ClearFolder;

            // ----- Add source folders to ObjectListView -----
            for (int i = 0; i < SourceFolders.Length; i++)
            {
                TimeSpan shift = TimeSpan.Zero;
                if (SourceFolders.Length == TimeShifts.Length)
                {
                    try
                    {
                        shift = TimeSpan.Parse(TimeShifts[i]);
                    }
                    catch { }
                }
                
                ffolder folder = new ffolder(System.IO.Path.GetFileName(SourceFolders[i]), SourceFolders[i], shift);
                olvFolders.AddObject(folder);
            }
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ----- Save Settings -----
            Properties.Settings.Default.DestFolder = txtDestFolder.Text;
            string sourceFolder = "";
            string timeShift = "";
            foreach (ffolder item in olvFolders.Objects)
            {
                if (sourceFolder != "") sourceFolder += ";";
                if (timeShift != "") timeShift += ";";
                sourceFolder += item.Path;
                timeShift += item.TimeShift.ToString();
            }
            Properties.Settings.Default.SourceFolders = sourceFolder;
            Properties.Settings.Default.TimeShift = timeShift;
            Properties.Settings.Default.Mask = cbFileMask.Text;
            Properties.Settings.Default.WriteSTExif = chbWriteToExif.Checked;
            Properties.Settings.Default.SetFileDate = chbSetImgDate.Checked;
            Properties.Settings.Default.ClearFolder = chbClearDestFolder.Checked;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Components

        /// <summary>
        /// Button Add source folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            // ----- Set Init Dir -----
            if (olvFolders.GetItemCount() > 0)
            {
                ffolder item = (ffolder)olvFolders.GetItem(olvFolders.GetItemCount() - 1).RowObject;
                if (System.IO.Directory.Exists(item.Path))
                {
                    dialog.InitialDirectory = item.Path;
                }
            }
            

            // ----- Show open dialog -----
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ffolder folder = new ffolder(System.IO.Path.GetFileName(dialog.FileName), dialog.FileName);
                olvFolders.AddObject(folder);           // add folder to OLV
            }
            dialog.Dispose();
        }

        /// <summary>
        /// Button Clear source folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            olvFolders.SetObjects(new List<ffolder>());
        }

        /// <summary>
        /// Button Remove selected source folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            olvFolders.RemoveObjects(olvFolders.CheckedObjects);
        }

        /// <summary>
        /// Button Add Destination folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDestFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            // ----- Set Init Dir -----
            if (System.IO.Directory.Exists(txtDestFolder.Text))
            {
                dialog.InitialDirectory = txtDestFolder.Text;
            }

            // ----- Show open dialog -----
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtDestFolder.Text = dialog.FileName;
            }
            dialog.Dispose();
        }

        /// <summary>
        /// Button Start Sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // ----- Start -----
            if ((string)btnStart.Tag != "run")
            {
                // ----- Check if path exists -----
                if (!System.IO.Directory.Exists(txtDestFolder.Text))
                {
                    MessageBox.Show(lng("DestDirectoryNotExits", "Destination directory not exits!") , lng("Error", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    return;
                }

                if (olvFolders.Items.Count == 0)
                {
                    MessageBox.Show(lng("W_NoInputFolders", "No input folders!"), lng("Error", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                // ----- Thread Settings -----
                sortSettings set;
                set.ClearFolder = chbClearDestFolder.Checked;
                set.SetImgTime = chbSetImgDate.Checked;
                set.WriteShiftedTime = chbWriteToExif.Checked;
                set.DestFolder = txtDestFolder.Text;
                set.FileMask = cbFileMask.Text;

                // ----- Create thread -----
                process = new AbortableBackgroundWorker();
                process.DoWork += Sorting;
                process.RunWorkerCompleted += WorkComplete;
                process.ProgressChanged += ProgressChanged;
                process.RunWorkerAsync(set);                // start thread
                process.WorkerSupportsCancellation = true;
                process.WorkerReportsProgress = true;       // turn on reporting

                // ----- Set button settings -----
                btnStart.Text = lng("Stop", "Stop");
                btnStart.Tag = "run";
                txtLog.Text = "";
                progBar.Value = 0;
            }
            // ----- Stop -----
            else
            {
                // ----- Cancel Thread -----
                process.CancelAsync();
                process.Abort();
                process.Dispose();
            }

        }

        /// <summary>
        /// txtLog Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            // ----- Scroll down if changed -----
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
        
        #endregion

        #region Functions

        /// <summary>
        /// Format file name grom mask
        /// </summary>
        /// <param name="format">Mask</param>
        /// <param name="index">File index</param>
        /// <param name="fileName">File name</param>
        /// <param name="folderName">Parrent folder name</param>
        /// <param name="date">File date</param>
        /// <returns>Formated filename</returns>
        private string Format(string format, int index, string fileName, string folderName, DateTime date)
        {
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

            // ----- Format Index -----
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

            // ----- Format File names -----
            format = format.Replace("%N", fileName);
            format = format.Replace("%F", folderName);

            // ----- Format Date ------
            format = format.Replace("%yyyy", date.Year.ToString("D4"));
            format = format.Replace("%yy", (date.Year - 2000).ToString("D2"));
            format = format.Replace("%y", date.Year.ToString());
            format = format.Replace("%MM", date.Month.ToString("D2"));
            format = format.Replace("%M", date.Month.ToString());
            format = format.Replace("%dd", date.Day.ToString("D2"));
            format = format.Replace("%d", date.Day.ToString());

            // ----- Format time -----
            format = format.Replace("%hh", date.Hour.ToString("D2"));
            format = format.Replace("%h", date.Hour.ToString());
            format = format.Replace("%mm", date.Minute.ToString("D2"));
            format = format.Replace("%m", date.Minute.ToString());
            format = format.Replace("%ss", date.Second.ToString("D2"));
            format = format.Replace("%s", date.Second.ToString());

            return format + ext;
        }

        #region Exif

        enum dateImgStat { None, Exif, File, FileName }

        /// <summary>
        /// Get Image Date
        /// </summary>
        /// <param name="path">Filename</param>
        /// <returns>Date</returns>
        private DateTime GetImgDate(string path)
        {
            dateImgStat status;
            return GetImgDate(path, out status);
        }


        /// <summary>
        /// Get Movie Date
        /// </summary>
        /// <param name="path">Filename</param>
        /// <param name="status">Date status</param>
        /// <returns>Date</returns>
        private DateTime GetMovDate(string path, out dateImgStat status)
        {
            status = dateImgStat.None;
            try
            {
                try
                {
                    MediaInfoWrapper wrap = new MediaInfoWrapper(path);
                    //MediaInfo.
                    MediaInfoList list = new MediaInfoList(false);
                    list.Open(path);
                    var creation = list.Get(0, StreamKind.General, 0, "com.apple.quicktime.creationdate");
                    if (creation == "")
                        creation = list.Get(0, StreamKind.General, 0, "Recorded_Date");
                    list.Close();
                    DateTime creationDate;
                    string fileDateName = Path.GetFileNameWithoutExtension(path).Substring(4);
                    if (DateTime.TryParse(creation, out creationDate))
                    {
                        status = dateImgStat.Exif;
                        return creationDate;
                    } 
                    else if (DateTime.TryParseExact(fileDateName,
                        "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out creationDate))
                    {
                        status = dateImgStat.FileName;
                        return creationDate;
                    }
                    else if (wrap.Tags != null && wrap.Tags.RecordedDate != null)
                    {
                        status = dateImgStat.Exif;
                        return wrap.Tags.RecordedDate ?? DateTime.Now;
                    }
                    else if (wrap.Tags != null && wrap.Tags.EncodedDate != null)
                    {
                        status = dateImgStat.Exif;
                        return wrap.Tags.EncodedDate ?? DateTime.Now;
                    }
                    else
                    {
                        status = dateImgStat.File;
                        return System.IO.File.GetCreationTime(path);
                    }
                }
                catch { }

                status = dateImgStat.File;
                return System.IO.File.GetCreationTime(path);
            }
            catch
            {
                status = dateImgStat.None;
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Get Image Date
        /// </summary>
        /// <param name="path">Filename</param>
        /// <param name="status">Date status</param>
        /// <returns>Date</returns>
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

                }
                catch { }

                // ----- Get date from file name (e.g. IMG_20240811_090138.jpg) ---
                string fileDateName = Path.GetFileNameWithoutExtension(path).Substring(4);
                if (DateTime.TryParseExact(fileDateName,
                        "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    status = dateImgStat.FileName;
                    return date;
                }

                status = dateImgStat.File;
                return System.IO.File.GetCreationTime(path);
            }
            catch
            {
                status = dateImgStat.None;
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Set Image date
        /// </summary>
        /// <param name="path">Source file path</param>
        /// <param name="date">Date</param>
        /// <param name="destPath">Destination file path</param>
        /// <returns>Returns true if write ok</returns>
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
            catch (Exception Err)
            {

                return false;
            }

            return true;
        }

        #endregion

        #region Worker

        /// <summary>
        /// Log to txtLog
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        private void Log(string text, Color color)
        {
            Invoke(new Action(() =>
            {
                txtLog.Select(txtLog.Text.Length, 0);           // Log to end
                txtLog.SelectionColor = color;                  // Set color
                txtLog.AppendText(text + Environment.NewLine);  // Log text
            }));
        }

        /// <summary>
        /// Sorting function
        /// </summary>
        /// <param name="sender">Worker</param>
        /// <param name="e"></param>
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


            // ----- Check input folders -----
            int folderCount = olvFolders.Items.Count;
            if (folderCount == 0)
            {
                Log(lng("Warning", "Warning") + ": " + lng("W_NoInputFolders", "No input folders!") , Color.Orange); 
                return;
            }
            progresFInc = progSearch / folderCount;         // Compute progressBar Increment

            // ----- Creating file list -----
            Log(lng("CreatingFileList", "Creating file list") + "...", Color.Black);
            foreach (ffolder item in olvFolders.Objects)
            {
                string procFile = "";
                try
                {
                    // ----- Search files in folders -----
                    string[] fileList = System.IO.Directory.GetFiles(item.Path);
                    Log(lng("SearchIn", "Search in:") + " " + item.Path + " (" + fileList.Length.ToString() + ")", Color.Black);
                    progresInc = progresFInc / fileList.Length;         // Compute progressBar Increment
                    foreach (string file in fileList)
                    {
                        procFile = System.IO.Path.GetFileName(file);    // get file
                        string ext = System.IO.Path.GetExtension(file).ToLower();   // check Extension
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".tiff")
                        {
                            rewriteExif = false;
                            dateImgStat status;
                            date = GetImgDate(file, out status);            // read file date
                            if (status == dateImgStat.File) Log(lng("W_In", "Warning in") + " " + procFile + ": " + lng("W_NoImageTime", "No image time in EXIF -> Using File date."), Color.Orange);

                            // ----- Process date -----
                            if (date != DateTime.MinValue)
                            {
                                if (item.TimeShift != TimeSpan.Zero)
                                {
                                    date += item.TimeShift;
                                    if (set.WriteShiftedTime) rewriteExif = true;
                                }
                            }
                            else
                                Log(lng("W_In", "Warning in") + " " + procFile + ": " + lng("W_NoValidImageTime", "No valid image time."), Color.Orange);

                            // ----- Add File to list -----
                            PhotoList.Add(new PhotoFile(file, item.Name, item.TimeShift, date, rewriteExif));
                        }
                        else if (ext == ".mov" || ext == ".mp4" || ext == ".mkv" || ext == ".avi" || ext == ".flv" || ext == ".webm" || ext == ".wmv" || ext == ".asf" || ext == ".mpg" || ext == ".mpeg" || ext == ".m4v" || ext == ".3gp")
                        {
                            rewriteExif = false;
                            dateImgStat status;
                            date = GetMovDate(file, out status);            // read file date
                            if (status == dateImgStat.File) Log(lng("W_In", "Warning in") + " " + procFile + ": " + lng("W_NoMovTime", "No movie time in MediaInfo -> Using File date."), Color.Orange);

                            // ----- Process date -----
                            if (date != DateTime.MinValue)
                            {
                                if (item.TimeShift != TimeSpan.Zero)
                                {
                                    date += item.TimeShift;
                                    //if (set.WriteShiftedTime) rewriteExif = true;
                                }
                            }
                            else
                                Log(lng("W_In", "Warning in") + " " + procFile + ": " + lng("W_NoValidImageTime", "No valid image time."), Color.Orange);

                            // ----- Add File to list -----
                            PhotoList.Add(new PhotoFile(file, item.Name, item.TimeShift, date, rewriteExif));
                        }

                        // ----- Show progress -----
                        progress += progresInc;
                        worker.ReportProgress((int)progress);           // Show progress
                    }
                }
                catch (Exception Err)
                {
                    Log(lng("E_In", "Error in") + " " + procFile + ": " + Err.Message, Color.Red);
                }

            }

            // ----- Check if found photos -----
            if (PhotoList.Count == 0)
            {
                Log(lng("W_NoPhotos", "Warning: No photos found!"), Color.Orange);
                return;
            }

            // ----- Sorting -----
            
            Log(lng("Sorting", "Sorting") + "... (" + PhotoList.Count.ToString() + ")", Color.Black);
            PhotoList = PhotoList.OrderBy(o => o.Date).ToList();

            progress = progSearch;
            worker.ReportProgress((int)progress);           // Show progress

            // ----- Delete destination folder -----
            if (set.ClearFolder)
            {
                string[] destFileList = System.IO.Directory.GetFiles(txtDestFolder.Text);
                foreach (string item in destFileList) System.IO.File.Delete(item);
            }

            // ----- Copy -----
            Log(lng("Copying", "Copying") + "...", Color.Black);
            progresInc = (100.0f - progSearch) / PhotoList.Count;
            for (int i = 0; i < PhotoList.Count; i++)
            {
                try
                {
                    // ----- Get Destination path -----
                    string destPath = set.DestFolder + System.IO.Path.DirectorySeparatorChar + Format(set.FileMask, i + 1, System.IO.Path.GetFileName(PhotoList[i].Path), PhotoList[i].Folder, PhotoList[i].Date);

                    // ----- Create path if not exist ----
                    string pathDir = System.IO.Path.GetDirectoryName(destPath);
                    if (!System.IO.Directory.Exists(pathDir))
                    {
                        System.IO.Directory.CreateDirectory(pathDir);
                    }
                    
                    // ----- If write exif -> set new date -----
                    if (PhotoList[i].RewriteExif)
                    {
                        SetImgDate(PhotoList[i].Path, PhotoList[i].Date, destPath);
                    }
                    else
                        System.IO.File.Copy(PhotoList[i].Path, destPath, true);

                    // ----- If update file date -> update date from EXIF -----
                    if (set.SetImgTime)
                    {
                        if (PhotoList[i].Date != DateTime.MinValue)
                        {
                            System.IO.File.SetCreationTime(destPath, PhotoList[i].Date);
                            System.IO.File.SetLastWriteTime(destPath, PhotoList[i].Date);
                        }
                        else
                        {
                            Log(lng("W_In", "Warning in") + " " + System.IO.Path.GetFileName(destPath) + ": " + lng("W_TimeNotSaved", "File time from EXIF not saved (no time in EXIF).") + "", Color.Orange);
                        }

                    }

                    // ----- Show progress -----
                    progress += progresInc;
                    worker.ReportProgress((int)progress);           // Show progress
                }
                catch (Exception Err)
                {
                    Log(lng("E_In", "Error in") + " " + System.IO.Path.GetFileName(PhotoList[i].Path) + ": " + Err.Message, Color.Red);
                }
            }
            Log(lng("CopyDone", "Copying done") + " (" + PhotoList.Count.ToString() + " " + lng("Photos", "photos") + ")", Color.Black);
            worker.ReportProgress(100);
        }

        /// <summary>
        /// Sorting Complete
        /// </summary>
        /// <param name="sender">Worker</param>
        /// <param name="e"></param>
        private void WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Log("----- " + lng("Aborted", "ABORTED") + " -----", Color.Black);
            }
            else
            {
                Log("----- " + lng("WorkDone", "WORK DONE") + " -----", Color.Black);
            }

            Invoke(new Action(() =>
            {
                btnStart.Tag = "";
                btnStart.Text = lng("Start", "Start");
                progBar.Value = 0;
            }));

            if (e.Cancelled)
                MessageBox.Show(lng("WorkAborted", "Work Aborted!") , lng("Work", "Work"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(lng("WorkComplete", "Work Complete."), lng("Work", "Work"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show Sorting Progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                progBar.Value = e.ProgressPercentage;   // Show progres in progressbar
            }
            catch (Exception) { }
        }








        #endregion

        #endregion

        private void imgMask_Click(object sender, EventArgs e)
        {
            string text = @"%i - file index
%i4 - file index with fixes number length(for example: % i4-> 0001)
%N - old file name
%F - parrent folder name
%yyyy - photo year(4 digits)
%yy - photo year(last 2 digits)
%y - photo year
%MM - photo month(2 digits)
%M - photo month
%dd - photo day(2 digits)
%d - photo day
%hh - photo hour(2 digits)
%h - photo hour
%mm - photo minutes(2 digits)
%m - photo minutes
%ss - photo seconds(2 digits)
%s - photo seconds";
            MessageBox.Show(lng("MaskInfo", text), lng("UsedMaskSymbols", "Used Mask Symbols"), MessageBoxButtons.OK, MessageBoxIcon.None);
        }

    }
}
