using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class SubJobReplicationPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobReplicationPage));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkUpdateRepCounter = new System.Windows.Forms.CheckBox();
            this.ntbRepCounterInterval = new LSOne.Controls.NumericTextBox();
            this.cmbReplCounterField = new System.Windows.Forms.ComboBox();
            this.cmbRecordsSentField = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCounterJob = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvCounters = new LSOne.Controls.ListView();
            this.colLocation = new LSOne.Controls.Columns.Column();
            this.colCounter = new LSOne.Controls.Columns.Column();
            this.chkUpdateRepCounterOnEmptyInt = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkUpdateRepCounter
            // 
            resources.ApplyResources(this.chkUpdateRepCounter, "chkUpdateRepCounter");
            this.chkUpdateRepCounter.Name = "chkUpdateRepCounter";
            this.chkUpdateRepCounter.UseVisualStyleBackColor = true;
            // 
            // ntbRepCounterInterval
            // 
            this.ntbRepCounterInterval.AllowDecimal = false;
            this.ntbRepCounterInterval.AllowNegative = false;
            this.ntbRepCounterInterval.CultureInfo = null;
            this.ntbRepCounterInterval.DecimalLetters = 2;
            this.ntbRepCounterInterval.ForeColor = System.Drawing.Color.Black;
            this.ntbRepCounterInterval.HasMinValue = false;
            resources.ApplyResources(this.ntbRepCounterInterval, "ntbRepCounterInterval");
            this.ntbRepCounterInterval.MaxValue = 0D;
            this.ntbRepCounterInterval.MinValue = 0D;
            this.ntbRepCounterInterval.Name = "ntbRepCounterInterval";
            this.ntbRepCounterInterval.Value = 0D;
            // 
            // cmbReplCounterField
            // 
            this.cmbReplCounterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReplCounterField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbReplCounterField, "cmbReplCounterField");
            this.cmbReplCounterField.Name = "cmbReplCounterField";
            this.cmbReplCounterField.DropDown += new System.EventHandler(this.cmbReplCounterField_DropDown);
            this.cmbReplCounterField.SelectedValueChanged += new System.EventHandler(this.cmbReplCounterField_SelectedValueChanged);
            // 
            // cmbRecordsSentField
            // 
            this.cmbRecordsSentField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecordsSentField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRecordsSentField, "cmbRecordsSentField");
            this.cmbRecordsSentField.Name = "cmbRecordsSentField";
            this.cmbRecordsSentField.DropDown += new System.EventHandler(this.cmbRecordsSentField_DropDown);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbCounterJob
            // 
            this.cmbCounterJob.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCounterJob.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCounterJob, "cmbCounterJob");
            this.cmbCounterJob.Name = "cmbCounterJob";
            this.cmbCounterJob.SelectedIndexChanged += new System.EventHandler(this.cmbCounterJob_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnsEditAddRemove);
            this.panel1.Controls.Add(this.lvCounters);
            this.panel1.Controls.Add(this.cmbCounterJob);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Name = "panel1";
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddDelete_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btRemove_Click);
            // 
            // lvCounters
            // 
            resources.ApplyResources(this.lvCounters, "lvCounters");
            this.lvCounters.BuddyControl = null;
            this.lvCounters.Columns.Add(this.colLocation);
            this.lvCounters.Columns.Add(this.colCounter);
            this.lvCounters.ContentBackColor = System.Drawing.Color.White;
            this.lvCounters.DefaultRowHeight = ((short)(22));
            this.lvCounters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCounters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCounters.HeaderHeight = ((short)(25));
            this.lvCounters.Name = "lvCounters";
            this.lvCounters.OddRowColor = System.Drawing.Color.White;
            this.lvCounters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCounters.SecondarySortColumn = ((short)(-1));
            this.lvCounters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCounters.SortSetting = "0:1";
            this.lvCounters.SelectionChanged += new System.EventHandler(this.lvCounters_SelectionChanged);
            this.lvCounters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCounters_RowDoubleClick);
            // 
            // colLocation
            // 
            this.colLocation.AutoSize = true;
            this.colLocation.DefaultStyle = null;
            resources.ApplyResources(this.colLocation, "colLocation");
            this.colLocation.InternalSort = true;
            this.colLocation.MaximumWidth = ((short)(0));
            this.colLocation.MinimumWidth = ((short)(10));
            this.colLocation.SecondarySortColumn = ((short)(-1));
            this.colLocation.Tag = null;
            this.colLocation.Width = ((short)(50));
            // 
            // colCounter
            // 
            this.colCounter.AutoSize = true;
            this.colCounter.DefaultStyle = null;
            resources.ApplyResources(this.colCounter, "colCounter");
            this.colCounter.InternalSort = true;
            this.colCounter.MaximumWidth = ((short)(0));
            this.colCounter.MinimumWidth = ((short)(10));
            this.colCounter.SecondarySortColumn = ((short)(-1));
            this.colCounter.Tag = null;
            this.colCounter.Width = ((short)(50));
            // 
            // chkUpdateRepCounterOnEmptyInt
            // 
            resources.ApplyResources(this.chkUpdateRepCounterOnEmptyInt, "chkUpdateRepCounterOnEmptyInt");
            this.chkUpdateRepCounterOnEmptyInt.Name = "chkUpdateRepCounterOnEmptyInt";
            this.chkUpdateRepCounterOnEmptyInt.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // SubJobReplicationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmbRecordsSentField);
            this.Controls.Add(this.cmbReplCounterField);
            this.Controls.Add(this.ntbRepCounterInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkUpdateRepCounterOnEmptyInt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkUpdateRepCounter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "SubJobReplicationPage";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkUpdateRepCounter;
        private NumericTextBox ntbRepCounterInterval;
        private System.Windows.Forms.ComboBox cmbReplCounterField;
        private System.Windows.Forms.ComboBox cmbRecordsSentField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCounterJob;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private ListView lvCounters;
        private LSOne.Controls.Columns.Column colLocation;
        private LSOne.Controls.Columns.Column colCounter;
        private System.Windows.Forms.CheckBox chkUpdateRepCounterOnEmptyInt;
        private System.Windows.Forms.Label label5;
        private ContextButtons btnsEditAddRemove;
    }
}
