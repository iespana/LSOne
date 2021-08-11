namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class CreateReplicationCounterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateReplicationCounterDialog));
            this.tbTable = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbSubJob = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReadCounter = new System.Windows.Forms.Button();
            this.ntbCounter = new LSOne.Controls.NumericTextBox();
            this.cmbLocation = new LSOne.Controls.DualDataComboBox();
            this.tbJob = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tbTable
            // 
            resources.ApplyResources(this.tbTable, "tbTable");
            this.tbTable.Name = "tbTable";
            this.tbTable.ReadOnly = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // tbSubJob
            // 
            resources.ApplyResources(this.tbSubJob, "tbSubJob");
            this.tbSubJob.Name = "tbSubJob";
            this.tbSubJob.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnReadCounter
            // 
            resources.ApplyResources(this.btnReadCounter, "btnReadCounter");
            this.btnReadCounter.Name = "btnReadCounter";
            this.btnReadCounter.UseVisualStyleBackColor = true;
            this.btnReadCounter.Click += new System.EventHandler(this.btnReadCounter_Click);
            // 
            // ntbCounter
            // 
            this.ntbCounter.AllowDecimal = false;
            this.ntbCounter.AllowNegative = false;
            this.ntbCounter.CultureInfo = null;
            this.ntbCounter.DecimalLetters = 2;
            this.ntbCounter.ForeColor = System.Drawing.Color.Black;
            this.ntbCounter.HasMinValue = false;
            resources.ApplyResources(this.ntbCounter, "ntbCounter");
            this.ntbCounter.MaxValue = 0D;
            this.ntbCounter.MinValue = 0D;
            this.ntbCounter.Name = "ntbCounter";
            this.ntbCounter.Value = 0D;
            this.ntbCounter.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbLocation
            // 
            this.cmbLocation.AddList = null;
            this.cmbLocation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.MaxLength = 32767;
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.NoChangeAllowed = false;
            this.cmbLocation.OnlyDisplayID = false;
            this.cmbLocation.RemoveList = null;
            this.cmbLocation.RowHeight = ((short)(22));
            this.cmbLocation.SecondaryData = null;
            this.cmbLocation.SelectedData = null;
            this.cmbLocation.SelectedDataID = null;
            this.cmbLocation.SelectionList = null;
            this.cmbLocation.SkipIDColumn = true;
            this.cmbLocation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbLocation_DropDown);
            this.cmbLocation.SelectedDataChanged += new System.EventHandler(this.cmbLocation_SelectedDataChanged);
            // 
            // tbJob
            // 
            resources.ApplyResources(this.tbJob, "tbJob");
            this.tbJob.Name = "tbJob";
            this.tbJob.ReadOnly = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CreateReplicationCounterDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbJob);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ntbCounter);
            this.Controls.Add(this.btnReadCounter);
            this.Controls.Add(this.cmbLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbSubJob);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTable);
            this.Controls.Add(this.label6);
            this.HasHelp = true;
            this.Name = "CreateReplicationCounterDialog";
            this.Load += new System.EventHandler(this.CreateReplicationCounterDialog_Load);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.tbTable, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.tbSubJob, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.cmbLocation, 0);
            this.Controls.SetChildIndex(this.btnReadCounter, 0);
            this.Controls.SetChildIndex(this.ntbCounter, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbJob, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox tbSubJob;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReadCounter;
        private LSOne.Controls.DualDataComboBox cmbLocation;
        private System.Windows.Forms.Label label3;
        private LSOne.Controls.NumericTextBox ntbCounter;
        private System.Windows.Forms.TextBox tbJob;
        private System.Windows.Forms.Label label4;
    }
}