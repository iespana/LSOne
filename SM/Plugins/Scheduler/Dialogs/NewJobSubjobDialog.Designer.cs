using System;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class NewJobSubjobDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewJobSubjobDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbSource = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.contextButtonAdd = new LSOne.Controls.ContextButton();
            this.cmbSubjobs = new LSOne.Controls.DualDataComboBox();
            this.lblEditPreview = new System.Windows.Forms.Label();
            this.lvlEditPreview = new LSOne.Controls.ListView();
            this.colSubJob = new LSOne.Controls.Columns.Column();
            this.colChange = new LSOne.Controls.Columns.Column();
            this.colIcon = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cmbSource
            // 
            this.cmbSource.AddList = null;
            resources.ApplyResources(this.cmbSource, "cmbSource");
            this.cmbSource.MaxLength = 32767;
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.RemoveList = null;
            this.cmbSource.RowHeight = ((short)(22));
            this.cmbSource.SecondaryData = null;
            this.cmbSource.SelectedData = null;
            this.cmbSource.SelectionList = null;
            this.cmbSource.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbSource_DropDown);
            this.cmbSource.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbSource_FormatData);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // contextButtonAdd
            // 
            resources.ApplyResources(this.contextButtonAdd, "contextButtonAdd");
            this.contextButtonAdd.Context = LSOne.Controls.ButtonType.Add;
            this.contextButtonAdd.Name = "contextButtonAdd";
            this.contextButtonAdd.Click += new System.EventHandler(this.contextButtonAdd_Click);
            // 
            // cmbSubjobs
            // 
            this.cmbSubjobs.AddList = null;
            this.cmbSubjobs.AllowKeyboardSelection = false;
            this.cmbSubjobs.EnableTextBox = true;
            resources.ApplyResources(this.cmbSubjobs, "cmbSubjobs");
            this.cmbSubjobs.MaxLength = 32767;
            this.cmbSubjobs.Name = "cmbSubjobs";
            this.cmbSubjobs.OnlyDisplayID = false;
            this.cmbSubjobs.RemoveList = null;
            this.cmbSubjobs.RowHeight = ((short)(22));
            this.cmbSubjobs.SecondaryData = null;
            this.cmbSubjobs.SelectedData = null;
            this.cmbSubjobs.SelectedDataID = null;
            this.cmbSubjobs.SelectionList = null;
            this.cmbSubjobs.ShowDropDownOnTyping = true;
            this.cmbSubjobs.SkipIDColumn = false;
            this.cmbSubjobs.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbSubjobs_DropDown);
            this.cmbSubjobs.SelectedDataChanged += new System.EventHandler(this.cmbSubjobs_SelectedDataChanged);
            // 
            // lblEditPreview
            // 
            resources.ApplyResources(this.lblEditPreview, "lblEditPreview");
            this.lblEditPreview.BackColor = System.Drawing.Color.Transparent;
            this.lblEditPreview.Name = "lblEditPreview";
            // 
            // lvlEditPreview
            // 
            resources.ApplyResources(this.lvlEditPreview, "lvlEditPreview");
            this.lvlEditPreview.BuddyControl = null;
            this.lvlEditPreview.Columns.Add(this.colSubJob);
            this.lvlEditPreview.Columns.Add(this.colChange);
            this.lvlEditPreview.Columns.Add(this.colIcon);
            this.lvlEditPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.DefaultRowHeight = ((short)(22));
            this.lvlEditPreview.DimSelectionWhenDisabled = true;
            this.lvlEditPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvlEditPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlEditPreview.HeaderHeight = ((short)(25));
            this.lvlEditPreview.Name = "lvlEditPreview";
            this.lvlEditPreview.OddRowColor = System.Drawing.Color.White;
            this.lvlEditPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SecondarySortColumn = ((short)(-1));
            this.lvlEditPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvlEditPreview.SortSetting = "0:1";
            this.lvlEditPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvlEditPreview_CellAction);
            // 
            // colSubJob
            // 
            this.colSubJob.AutoSize = true;
            this.colSubJob.Clickable = false;
            this.colSubJob.DefaultStyle = null;
            resources.ApplyResources(this.colSubJob, "colSubJob");
            this.colSubJob.MaximumWidth = ((short)(0));
            this.colSubJob.MinimumWidth = ((short)(200));
            this.colSubJob.RelativeSize = 0;
            this.colSubJob.Tag = null;
            this.colSubJob.Width = ((short)(200));
            // 
            // colChange
            // 
            this.colChange.AutoSize = true;
            this.colChange.Clickable = false;
            this.colChange.DefaultStyle = null;
            resources.ApplyResources(this.colChange, "colChange");
            this.colChange.MaximumWidth = ((short)(0));
            this.colChange.MinimumWidth = ((short)(10));
            this.colChange.Tag = null;
            this.colChange.Width = ((short)(80));
            // 
            // colIcon
            // 
            this.colIcon.Clickable = false;
            this.colIcon.DefaultStyle = null;
            resources.ApplyResources(this.colIcon, "colIcon");
            this.colIcon.MaximumWidth = ((short)(10));
            this.colIcon.MinimumWidth = ((short)(10));
            this.colIcon.RelativeSize = 10;
            this.colIcon.Tag = null;
            this.colIcon.Width = ((short)(50));
            // 
            // NewJobSubjobDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblEditPreview);
            this.Controls.Add(this.lvlEditPreview);
            this.Controls.Add(this.cmbSubjobs);
            this.Controls.Add(this.contextButtonAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.HasHelp = true;
            this.Name = "NewJobSubjobDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbSource, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.contextButtonAdd, 0);
            this.Controls.SetChildIndex(this.cmbSubjobs, 0);
            this.Controls.SetChildIndex(this.lvlEditPreview, 0);
            this.Controls.SetChildIndex(this.lblEditPreview, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label3;
        private DropDownFormComboBox cmbSource;
        private System.Windows.Forms.Label label1;
        private ContextButton contextButtonAdd;
        private DualDataComboBox cmbSubjobs;
        private System.Windows.Forms.Label lblEditPreview;
        private LSOne.Controls.ListView lvlEditPreview;
        private LSOne.Controls.Columns.Column colSubJob;
        private LSOne.Controls.Columns.Column colChange;
        private LSOne.Controls.Columns.Column colIcon;
    }
}