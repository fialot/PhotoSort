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


namespace PhotoSort
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        List<PhotoFile> PhotoList;

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
            DateTime date;
            bool rewriteExif;
            PhotoList = new List<PhotoFile>();

            txtLog.AppendText("Creating file list..." + Environment.NewLine);

            foreach (ffolder item in olvFolders.Objects)
            {
                try
                {
                    string[] fileList = System.IO.Directory.GetFiles(item.Path);
                    foreach (string file in fileList)
                    {
                        rewriteExif = false;
                        date = GetImgDate(file);
                        if (item.TimeShift != TimeSpan.Zero)
                        {
                            date += item.TimeShift;
                            if (chbWriteToExif.Checked) rewriteExif = true;
                        }


                        PhotoList.Add(new PhotoFile(file, item.Name, item.TimeShift, date, rewriteExif));
                    }
                } catch (Exception Err)
                {
                    txtLog.AppendText("Error: " + Err.Message + Environment.NewLine);
                }
                
            }

            txtLog.AppendText("Sorting..." + Environment.NewLine);
            PhotoList = PhotoList.OrderBy(o => o.Date).ToList();

            // ----- Delete destination folder -----
            if (chbClearDestFolder.Checked)
            {
                string[] destFileList = System.IO.Directory.GetFiles(txtDestFolder.Text);
                foreach (string item in destFileList) System.IO.File.Delete(item);
            }

            // ----- Copy -----

            txtLog.AppendText("Copying..." + Environment.NewLine);
            for (int i = 0; i < PhotoList.Count; i++)
            {
                try
                {
                    string destPath = txtDestFolder.Text + System.IO.Path.DirectorySeparatorChar + Format(txtFileMask.Text, i + 1, System.IO.Path.GetFileName(PhotoList[i].Path), PhotoList[i].Folder, PhotoList[i].Date);
                    if (PhotoList[i].RewriteExif)
                    {
                        SetImgDate(PhotoList[i].Path, PhotoList[i].Date, destPath);
                    } else 
                        System.IO.File.Copy(PhotoList[i].Path, destPath, true);
                    if (chbSetImgDate.Checked)
                    {
                        System.IO.File.SetCreationTime(destPath, PhotoList[i].Date);
                        System.IO.File.SetLastWriteTime(destPath, PhotoList[i].Date);
                    }
                        
                }
                catch (Exception err)
                {
                    txtLog.AppendText("Error: " + err.Message);
                }
            }

            txtLog.AppendText("Done." + Environment.NewLine);
            MessageBox.Show("Done");
            
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


        private DateTime GetImgDate(string path)
        {
            Image img = Bitmap.FromFile(path);

            foreach (System.Drawing.Imaging.PropertyItem item in img.PropertyItems)
            {
                if (item.Id == 36867)
                {
                    img.Dispose();
                    return GetDate(item.Value, item.Len);
                }
            }

            foreach (System.Drawing.Imaging.PropertyItem item in img.PropertyItems)
            {
                if (item.Id == 306)
                {
                    img.Dispose();
                    return GetDate(item.Value, item.Len);
                }
            }

            img.Dispose();
            return DateTime.MinValue;
        }

        private bool SetImgDate(string path, DateTime date, string destPath)
        {
            try
            {
                Image img = Bitmap.FromFile(path);

                foreach (System.Drawing.Imaging.PropertyItem item in img.PropertyItems)
                {
                    if (item.Id == 306)
                    {
                        item.Value = SetDate(date);
                        img.SetPropertyItem(item);
                    }
                    else if (item.Id == 36867)
                    {
                        item.Value = SetDate(date);
                        img.SetPropertyItem(item);
                    }
                    else if (item.Id == 36868)
                    {
                        item.Value = SetDate(date);
                        img.SetPropertyItem(item);
                    }
                }

                img.Save(destPath);
                img.Dispose();
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

    }
}
