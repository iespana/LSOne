namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class SubJobFilterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobFilterDialog));
            this.cmbFieldDesign = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbValue1 = new System.Windows.Forms.TextBox();
            this.tbValue2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbApplyFilter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbFieldDesign
            // 
            this.cmbFieldDesign.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbFieldDesign.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFieldDesign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFieldDesign.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFieldDesign, "cmbFieldDesign");
            this.cmbFieldDesign.Name = "cmbFieldDesign";
            this.cmbFieldDesign.SelectedIndexChanged += new System.EventHandler(this.cmbFieldDesign_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbFilterType
            // 
            this.cmbFilterType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbFilterType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbFilterType, "cmbFilterType");
            this.cmbFilterType.Name = "cmbFilterType";
            this.cmbFilterType.SelectedIndexChanged += new System.EventHandler(this.cmbFilterType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbValue1
            // 
            resources.ApplyResources(this.tbValue1, "tbValue1");
            this.tbValue1.Name = "tbValue1";
            // 
            // tbValue2
            // 
            resources.ApplyResources(this.tbValue2, "tbValue2");
            this.tbValue2.Name = "tbValue2";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbApplyFilter
            // 
            this.cmbApplyFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbApplyFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbApplyFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApplyFilter.FormattingEnabled = true;
            resources.ApplyResources(this.cmbApplyFilter, "cmbApplyFilter");
            this.cmbApplyFilter.Name = "cmbApplyFilter";
            this.cmbApplyFilter.SelectedIndexChanged += new System.EventHandler(this.cmbApplyFilter_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // SubJobFilterDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cmbApplyFilter);
            this.Controls.Add(this.cmbFilterType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbValue2);
            this.Controls.Add(this.tbValue1);
            this.Controls.Add(this.cmbFieldDesign);
            this.Controls.Add(this.label1);
            this.HasHelp = true;
            this.Name = "SubJobFilterDialog";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbFieldDesign, 0);
            this.Controls.SetChildIndex(this.tbValue1, 0);
            this.Controls.SetChildIndex(this.tbValue2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbFilterType, 0);
            this.Controls.SetChildIndex(this.cmbApplyFilter, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFieldDesign;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbValue1;
        private System.Windows.Forms.TextBox tbValue2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbApplyFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}