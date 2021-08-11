namespace LSOne.ViewPlugins.ReportViewer.Dialogs
{
    partial class AddReportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddReportDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbReportDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelectReportDescription = new System.Windows.Forms.Button();
            this.btnSelectReport = new System.Windows.Forms.Button();
            this.tbReportFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            // tbReportDescription
            // 
            resources.ApplyResources(this.tbReportDescription, "tbReportDescription");
            this.tbReportDescription.Name = "tbReportDescription";
            this.tbReportDescription.ReadOnly = true;
            this.tbReportDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnSelectReportDescription
            // 
            resources.ApplyResources(this.btnSelectReportDescription, "btnSelectReportDescription");
            this.btnSelectReportDescription.Name = "btnSelectReportDescription";
            this.btnSelectReportDescription.UseVisualStyleBackColor = true;
            this.btnSelectReportDescription.Click += new System.EventHandler(this.btnSelectReportDescription_Click);
            // 
            // btnSelectReport
            // 
            resources.ApplyResources(this.btnSelectReport, "btnSelectReport");
            this.btnSelectReport.Name = "btnSelectReport";
            this.btnSelectReport.UseVisualStyleBackColor = true;
            this.btnSelectReport.Click += new System.EventHandler(this.btnSelectReport_Click);
            // 
            // tbReportFile
            // 
            resources.ApplyResources(this.tbReportFile, "tbReportFile");
            this.tbReportFile.Name = "tbReportFile";
            this.tbReportFile.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            resources.GetString("cmbCategory.Items"),
            resources.GetString("cmbCategory.Items1"),
            resources.GetString("cmbCategory.Items2"),
            resources.GetString("cmbCategory.Items3"),
            resources.GetString("cmbCategory.Items4"),
            resources.GetString("cmbCategory.Items5"),
            resources.GetString("cmbCategory.Items6"),
            resources.GetString("cmbCategory.Items7"),
            resources.GetString("cmbCategory.Items8")});
            resources.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.Name = "cmbCategory";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // AddReportDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.btnSelectReport);
            this.Controls.Add(this.tbReportFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectReportDescription);
            this.Controls.Add(this.tbReportDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "AddReportDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbReportDescription, 0);
            this.Controls.SetChildIndex(this.btnSelectReportDescription, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbReportFile, 0);
            this.Controls.SetChildIndex(this.btnSelectReport, 0);
            this.Controls.SetChildIndex(this.cmbCategory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbReportDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectReport;
        private System.Windows.Forms.TextBox tbReportFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectReportDescription;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label2;
    }
}