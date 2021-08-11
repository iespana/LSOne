using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    partial class StatementDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatementDialog));
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dtpStartingTime = new System.Windows.Forms.DateTimePicker();
            this.dtpStartingDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndingDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpEndingTime = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbSince2 = new LSOne.Controls.DualDataComboBox();
            this.cmbSince1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbFromPeriods = new System.Windows.Forms.RadioButton();
            this.rbFromManual = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbSince4 = new LSOne.Controls.DualDataComboBox();
            this.cmbSince3 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rbToPeriods = new System.Windows.Forms.RadioButton();
            this.rbToManual = new System.Windows.Forms.RadioButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dtpStartingTime
            // 
            resources.ApplyResources(this.dtpStartingTime, "dtpStartingTime");
            this.dtpStartingTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartingTime.Name = "dtpStartingTime";
            this.dtpStartingTime.ShowUpDown = true;
            this.dtpStartingTime.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // dtpStartingDate
            // 
            resources.ApplyResources(this.dtpStartingDate, "dtpStartingDate");
            this.dtpStartingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartingDate.Name = "dtpStartingDate";
            this.dtpStartingDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // dtpEndingDate
            // 
            resources.ApplyResources(this.dtpEndingDate, "dtpEndingDate");
            this.dtpEndingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndingDate.Name = "dtpEndingDate";
            this.dtpEndingDate.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dtpEndingTime
            // 
            resources.ApplyResources(this.dtpEndingTime, "dtpEndingTime");
            this.dtpEndingTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndingTime.Name = "dtpEndingTime";
            this.dtpEndingTime.ShowUpDown = true;
            this.dtpEndingTime.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.FormatData);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbSince2);
            this.groupBox1.Controls.Add(this.cmbSince1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rbFromPeriods);
            this.groupBox1.Controls.Add(this.rbFromManual);
            this.groupBox1.Controls.Add(this.dtpStartingTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpStartingDate);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbSince2
            // 
            this.cmbSince2.AddList = null;
            this.cmbSince2.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSince2, "cmbSince2");
            this.cmbSince2.MaxLength = 32767;
            this.cmbSince2.Name = "cmbSince2";
            this.cmbSince2.OnlyDisplayID = false;
            this.cmbSince2.RemoveList = null;
            this.cmbSince2.RowHeight = ((short)(22));
            this.cmbSince2.SecondaryData = null;
            this.cmbSince2.SelectedData = null;
            this.cmbSince2.SelectedDataID = null;
            this.cmbSince2.SelectionList = null;
            this.cmbSince2.SkipIDColumn = false;
            this.cmbSince2.RequestData += new System.EventHandler(this.cmbSince2_RequestData);
            this.cmbSince2.PropertyChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbSince2.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbSince1
            // 
            this.cmbSince1.FormattingEnabled = true;
            this.cmbSince1.Items.AddRange(new object[] {
            resources.GetString("cmbSince1.Items"),
            resources.GetString("cmbSince1.Items1"),
            resources.GetString("cmbSince1.Items2")});
            resources.ApplyResources(this.cmbSince1, "cmbSince1");
            this.cmbSince1.Name = "cmbSince1";
            this.cmbSince1.SelectedIndexChanged += new System.EventHandler(this.cmbSince1_SelectedIndexChanged);
            this.cmbSince1.SelectedValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // rbFromPeriods
            // 
            resources.ApplyResources(this.rbFromPeriods, "rbFromPeriods");
            this.rbFromPeriods.Checked = true;
            this.rbFromPeriods.Name = "rbFromPeriods";
            this.rbFromPeriods.TabStop = true;
            this.rbFromPeriods.UseVisualStyleBackColor = true;
            this.rbFromPeriods.CheckedChanged += new System.EventHandler(this.rbFrom_CheckedChanged);
            // 
            // rbFromManual
            // 
            resources.ApplyResources(this.rbFromManual, "rbFromManual");
            this.rbFromManual.Name = "rbFromManual";
            this.rbFromManual.UseVisualStyleBackColor = true;
            this.rbFromManual.CheckedChanged += new System.EventHandler(this.rbFrom_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbSince4);
            this.groupBox2.Controls.Add(this.cmbSince3);
            this.groupBox2.Controls.Add(this.dtpEndingDate);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.rbToPeriods);
            this.groupBox2.Controls.Add(this.dtpEndingTime);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.rbToManual);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cmbSince4
            // 
            this.cmbSince4.AddList = null;
            this.cmbSince4.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSince4, "cmbSince4");
            this.cmbSince4.MaxLength = 32767;
            this.cmbSince4.Name = "cmbSince4";
            this.cmbSince4.OnlyDisplayID = false;
            this.cmbSince4.RemoveList = null;
            this.cmbSince4.RowHeight = ((short)(22));
            this.cmbSince4.SecondaryData = null;
            this.cmbSince4.SelectedData = null;
            this.cmbSince4.SelectedDataID = null;
            this.cmbSince4.SelectionList = null;
            this.cmbSince4.SkipIDColumn = false;
            this.cmbSince4.RequestData += new System.EventHandler(this.cmbSince4_RequestData);
            this.cmbSince4.PropertyChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbSince4.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbSince3
            // 
            this.cmbSince3.FormattingEnabled = true;
            this.cmbSince3.Items.AddRange(new object[] {
            resources.GetString("cmbSince3.Items"),
            resources.GetString("cmbSince3.Items1")});
            resources.ApplyResources(this.cmbSince3, "cmbSince3");
            this.cmbSince3.Name = "cmbSince3";
            this.cmbSince3.SelectedIndexChanged += new System.EventHandler(this.cmbSince3_SelectedIndexChanged);
            this.cmbSince3.SelectedValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // rbToPeriods
            // 
            resources.ApplyResources(this.rbToPeriods, "rbToPeriods");
            this.rbToPeriods.Checked = true;
            this.rbToPeriods.Name = "rbToPeriods";
            this.rbToPeriods.TabStop = true;
            this.rbToPeriods.UseVisualStyleBackColor = true;
            this.rbToPeriods.CheckedChanged += new System.EventHandler(this.rbTo_CheckedChanged);
            // 
            // rbToManual
            // 
            resources.ApplyResources(this.rbToManual, "rbToManual");
            this.rbToManual.Name = "rbToManual";
            this.rbToManual.UseVisualStyleBackColor = true;
            this.rbToManual.CheckedChanged += new System.EventHandler(this.rbTo_CheckedChanged);
            // 
            // StatementDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "StatementDialog";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DateTimePicker dtpStartingDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStartingTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndingDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpEndingTime;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFromPeriods;
        private System.Windows.Forms.RadioButton rbFromManual;
        private DualDataComboBox cmbSince2;
        private System.Windows.Forms.ComboBox cmbSince1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DualDataComboBox cmbSince4;
        private System.Windows.Forms.ComboBox cmbSince3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbToPeriods;
        private System.Windows.Forms.RadioButton rbToManual;
    }
}