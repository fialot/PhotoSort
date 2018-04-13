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
            this.label1 = new System.Windows.Forms.Label();
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
            this.gbSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSources.Controls.Add(this.btnClear);
            this.gbSources.Controls.Add(this.olvFolders);
            this.gbSources.Controls.Add(this.btnDel);
            this.gbSources.Controls.Add(this.btnAdd);
            this.gbSources.Location = new System.Drawing.Point(12, 12);
            this.gbSources.Name = "gbSources";
            this.gbSources.Size = new System.Drawing.Size(621, 169);
            this.gbSources.TabIndex = 0;
            this.gbSources.TabStop = false;
            this.gbSources.Text = "Source folders";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(177, 19);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // olvFolders
            // 
            this.olvFolders.AllColumns.Add(this.colName);
            this.olvFolders.AllColumns.Add(this.colFullName);
            this.olvFolders.AllColumns.Add(this.colTimeShift);
            this.olvFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvFolders.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvFolders.CellEditUseWholeCell = false;
            this.olvFolders.CheckBoxes = true;
            this.olvFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colFullName,
            this.colTimeShift});
            this.olvFolders.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFolders.GridLines = true;
            this.olvFolders.Location = new System.Drawing.Point(15, 48);
            this.olvFolders.Name = "olvFolders";
            this.olvFolders.ShowGroups = false;
            this.olvFolders.Size = new System.Drawing.Size(600, 115);
            this.olvFolders.TabIndex = 4;
            this.olvFolders.UseCompatibleStateImageBehavior = false;
            this.olvFolders.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.AspectName = "Name";
            this.colName.IsEditable = false;
            this.colName.Text = "Folder";
            this.colName.Width = 104;
            // 
            // colFullName
            // 
            this.colFullName.AspectName = "Path";
            this.colFullName.Text = "Path";
            this.colFullName.Width = 407;
            // 
            // colTimeShift
            // 
            this.colTimeShift.AspectName = "TimeShift";
            this.colTimeShift.Text = "TimeShift";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(96, 19);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "Remove";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(540, 87);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbDestination
            // 
            this.gbDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDestination.Controls.Add(this.btnDestFolder);
            this.gbDestination.Controls.Add(this.txtDestFolder);
            this.gbDestination.Location = new System.Drawing.Point(12, 187);
            this.gbDestination.Name = "gbDestination";
            this.gbDestination.Size = new System.Drawing.Size(621, 55);
            this.gbDestination.TabIndex = 5;
            this.gbDestination.TabStop = false;
            this.gbDestination.Text = "Destination Folder";
            // 
            // btnDestFolder
            // 
            this.btnDestFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDestFolder.Location = new System.Drawing.Point(583, 17);
            this.btnDestFolder.Name = "btnDestFolder";
            this.btnDestFolder.Size = new System.Drawing.Size(32, 23);
            this.btnDestFolder.TabIndex = 7;
            this.btnDestFolder.Text = "...";
            this.btnDestFolder.UseVisualStyleBackColor = true;
            this.btnDestFolder.Click += new System.EventHandler(this.btnDestFolder_Click);
            // 
            // txtDestFolder
            // 
            this.txtDestFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDestFolder.Location = new System.Drawing.Point(15, 19);
            this.txtDestFolder.Name = "txtDestFolder";
            this.txtDestFolder.Size = new System.Drawing.Size(562, 20);
            this.txtDestFolder.TabIndex = 6;
            // 
            // gbSettings
            // 
            this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSettings.Controls.Add(this.chbClearDestFolder);
            this.gbSettings.Controls.Add(this.txtFileMask);
            this.gbSettings.Controls.Add(this.label1);
            this.gbSettings.Controls.Add(this.chbSetImgDate);
            this.gbSettings.Controls.Add(this.btnStart);
            this.gbSettings.Controls.Add(this.chbWriteToExif);
            this.gbSettings.Location = new System.Drawing.Point(12, 248);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(621, 116);
            this.gbSettings.TabIndex = 8;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // chbClearDestFolder
            // 
            this.chbClearDestFolder.AutoSize = true;
            this.chbClearDestFolder.Checked = true;
            this.chbClearDestFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbClearDestFolder.Location = new System.Drawing.Point(188, 19);
            this.chbClearDestFolder.Name = "chbClearDestFolder";
            this.chbClearDestFolder.Size = new System.Drawing.Size(192, 17);
            this.chbClearDestFolder.TabIndex = 10;
            this.chbClearDestFolder.Text = "Clear destination folder before copy";
            this.chbClearDestFolder.UseVisualStyleBackColor = true;
            // 
            // txtFileMask
            // 
            this.txtFileMask.Location = new System.Drawing.Point(71, 68);
            this.txtFileMask.Name = "txtFileMask";
            this.txtFileMask.Size = new System.Drawing.Size(172, 20);
            this.txtFileMask.TabIndex = 13;
            this.txtFileMask.Text = "%i4 - %N (%F)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Files mask:";
            // 
            // chbSetImgDate
            // 
            this.chbSetImgDate.AutoSize = true;
            this.chbSetImgDate.Checked = true;
            this.chbSetImgDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSetImgDate.Location = new System.Drawing.Point(15, 42);
            this.chbSetImgDate.Name = "chbSetImgDate";
            this.chbSetImgDate.Size = new System.Drawing.Size(131, 17);
            this.chbSetImgDate.TabIndex = 11;
            this.chbSetImgDate.Text = "Set file date from EXIF";
            this.chbSetImgDate.UseVisualStyleBackColor = true;
            // 
            // chbWriteToExif
            // 
            this.chbWriteToExif.AutoSize = true;
            this.chbWriteToExif.Checked = true;
            this.chbWriteToExif.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbWriteToExif.Location = new System.Drawing.Point(15, 19);
            this.chbWriteToExif.Name = "chbWriteToExif";
            this.chbWriteToExif.Size = new System.Drawing.Size(145, 17);
            this.chbWriteToExif.TabIndex = 9;
            this.chbWriteToExif.Text = "Write shifted time to EXIF";
            this.chbWriteToExif.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 370);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(621, 94);
            this.txtLog.TabIndex = 15;
            this.txtLog.Text = "";
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // progBar
            // 
            this.progBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBar.Location = new System.Drawing.Point(12, 470);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(621, 15);
            this.progBar.Step = 1;
            this.progBar.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 489);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbDestination);
            this.Controls.Add(this.gbSources);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "PhotoSort";
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.CheckBox chbClearDestFolder;
        private System.Windows.Forms.ProgressBar progBar;
    }
}

