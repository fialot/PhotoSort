namespace PhotoSort
{
    partial class frmMain
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbSources = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.olvFolders = new BrightIdeasSoftware.ObjectListView();
            this.colName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFullName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colTimeShift = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbDestination = new System.Windows.Forms.GroupBox();
            this.btnDestFolder = new System.Windows.Forms.Button();
            this.txtDestFolder = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chbClearDestFolder = new System.Windows.Forms.CheckBox();
            this.txtFileMask = new System.Windows.Forms.TextBox();
            this.lblFileMask = new System.Windows.Forms.Label();
            this.chbSetImgDate = new System.Windows.Forms.CheckBox();
            this.chbWriteToExif = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.gbSources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvFolders)).BeginInit();
            this.gbDestination.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSources
            // 
            resources.ApplyResources(this.gbSources, "gbSources");
            this.gbSources.Controls.Add(this.btnClear);
            this.gbSources.Controls.Add(this.olvFolders);
            this.gbSources.Controls.Add(this.btnDel);
            this.gbSources.Controls.Add(this.btnAdd);
            this.gbSources.Name = "gbSources";
            this.gbSources.TabStop = false;
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // olvFolders
            // 
            resources.ApplyResources(this.olvFolders, "olvFolders");
            this.olvFolders.AllColumns.Add(this.colName);
            this.olvFolders.AllColumns.Add(this.colFullName);
            this.olvFolders.AllColumns.Add(this.colTimeShift);
            this.olvFolders.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvFolders.CellEditUseWholeCell = false;
            this.olvFolders.CheckBoxes = true;
            this.olvFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colFullName,
            this.colTimeShift});
            this.olvFolders.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFolders.GridLines = true;
            this.olvFolders.Name = "olvFolders";
            this.olvFolders.OverlayText.Text = resources.GetString("resource.Text");
            this.olvFolders.ShowGroups = false;
            this.olvFolders.UseCompatibleStateImageBehavior = false;
            this.olvFolders.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.AspectName = "Name";
            resources.ApplyResources(this.colName, "colName");
            this.colName.IsEditable = false;
            // 
            // colFullName
            // 
            this.colFullName.AspectName = "Path";
            resources.ApplyResources(this.colFullName, "colFullName");
            // 
            // colTimeShift
            // 
            this.colTimeShift.AspectName = "TimeShift";
            resources.ApplyResources(this.colTimeShift, "colTimeShift");
            // 
            // btnDel
            // 
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Name = "btnDel";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbDestination
            // 
            resources.ApplyResources(this.gbDestination, "gbDestination");
            this.gbDestination.Controls.Add(this.btnDestFolder);
            this.gbDestination.Controls.Add(this.txtDestFolder);
            this.gbDestination.Name = "gbDestination";
            this.gbDestination.TabStop = false;
            // 
            // btnDestFolder
            // 
            resources.ApplyResources(this.btnDestFolder, "btnDestFolder");
            this.btnDestFolder.Name = "btnDestFolder";
            this.btnDestFolder.UseVisualStyleBackColor = true;
            this.btnDestFolder.Click += new System.EventHandler(this.btnDestFolder_Click);
            // 
            // txtDestFolder
            // 
            resources.ApplyResources(this.txtDestFolder, "txtDestFolder");
            this.txtDestFolder.Name = "txtDestFolder";
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // gbSettings
            // 
            resources.ApplyResources(this.gbSettings, "gbSettings");
            this.gbSettings.Controls.Add(this.chbClearDestFolder);
            this.gbSettings.Controls.Add(this.txtFileMask);
            this.gbSettings.Controls.Add(this.lblFileMask);
            this.gbSettings.Controls.Add(this.chbSetImgDate);
            this.gbSettings.Controls.Add(this.btnStart);
            this.gbSettings.Controls.Add(this.chbWriteToExif);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.TabStop = false;
            // 
            // chbClearDestFolder
            // 
            resources.ApplyResources(this.chbClearDestFolder, "chbClearDestFolder");
            this.chbClearDestFolder.Checked = true;
            this.chbClearDestFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbClearDestFolder.Name = "chbClearDestFolder";
            this.chbClearDestFolder.UseVisualStyleBackColor = true;
            // 
            // txtFileMask
            // 
            resources.ApplyResources(this.txtFileMask, "txtFileMask");
            this.txtFileMask.Name = "txtFileMask";
            // 
            // lblFileMask
            // 
            resources.ApplyResources(this.lblFileMask, "lblFileMask");
            this.lblFileMask.Name = "lblFileMask";
            // 
            // chbSetImgDate
            // 
            resources.ApplyResources(this.chbSetImgDate, "chbSetImgDate");
            this.chbSetImgDate.Checked = true;
            this.chbSetImgDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSetImgDate.Name = "chbSetImgDate";
            this.chbSetImgDate.UseVisualStyleBackColor = true;
            // 
            // chbWriteToExif
            // 
            resources.ApplyResources(this.chbWriteToExif, "chbWriteToExif");
            this.chbWriteToExif.Checked = true;
            this.chbWriteToExif.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbWriteToExif.Name = "chbWriteToExif";
            this.chbWriteToExif.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            resources.ApplyResources(this.txtLog, "txtLog");
            this.txtLog.Name = "txtLog";
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // progBar
            // 
            resources.ApplyResources(this.progBar, "progBar");
            this.progBar.Name = "progBar";
            this.progBar.Step = 1;
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbDestination);
            this.Controls.Add(this.gbSources);
            this.Name = "frmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbSources.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvFolders)).EndInit();
            this.gbDestination.ResumeLayout(false);
            this.gbDestination.PerformLayout();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSources;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnStart;
        private BrightIdeasSoftware.ObjectListView olvFolders;
        private System.Windows.Forms.GroupBox gbDestination;
        private BrightIdeasSoftware.OLVColumn colName;
        private BrightIdeasSoftware.OLVColumn colFullName;
        private BrightIdeasSoftware.OLVColumn colTimeShift;
        private System.Windows.Forms.Button btnDestFolder;
        private System.Windows.Forms.TextBox txtDestFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox chbWriteToExif;
        private System.Windows.Forms.CheckBox chbSetImgDate;
        private System.Windows.Forms.TextBox txtFileMask;
        private System.Windows.Forms.Label lblFileMask;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.CheckBox chbClearDestFolder;
        private System.Windows.Forms.ProgressBar progBar;
    }
}

