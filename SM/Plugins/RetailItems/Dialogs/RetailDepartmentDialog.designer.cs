using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class RetailDepartmentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailDepartmentDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.btnAddRetailDivision = new LSOne.Controls.ContextButton();
            this.cmbRetailDivision = new LSOne.Controls.DualDataComboBox();
            this.lblRetailDivision = new System.Windows.Forms.Label();
            this.tbSearchAlias = new System.Windows.Forms.TextBox();
            this.lblSearchName = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddRetailDivision
            // 
            this.btnAddRetailDivision.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRetailDivision.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddRetailDivision, "btnAddRetailDivision");
            this.btnAddRetailDivision.Name = "btnAddRetailDivision";
            this.btnAddRetailDivision.Click += new System.EventHandler(this.btnAddRetailDivision_Click);
            // 
            // cmbRetailDivision
            // 
            this.cmbRetailDivision.AddList = null;
            this.cmbRetailDivision.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRetailDivision, "cmbRetailDivision");
            this.cmbRetailDivision.MaxLength = 32767;
            this.cmbRetailDivision.Name = "cmbRetailDivision";
            this.cmbRetailDivision.NoChangeAllowed = false;
            this.cmbRetailDivision.OnlyDisplayID = false;
            this.cmbRetailDivision.RemoveList = null;
            this.cmbRetailDivision.RowHeight = ((short)(22));
            this.cmbRetailDivision.SecondaryData = null;
            this.cmbRetailDivision.SelectedData = null;
            this.cmbRetailDivision.SelectedDataID = null;
            this.cmbRetailDivision.SelectionList = null;
            this.cmbRetailDivision.SkipIDColumn = true;
            this.cmbRetailDivision.RequestData += new System.EventHandler(this.cmbRetailDivision_RequestData);
            this.cmbRetailDivision.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            this.cmbRetailDivision.RequestClear += new System.EventHandler(this.cmbRetailDivision_RequestClear);
            // 
            // lblRetailDivision
            // 
            this.lblRetailDivision.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRetailDivision, "lblRetailDivision");
            this.lblRetailDivision.Name = "lblRetailDivision";
            // 
            // tbSearchAlias
            // 
            resources.ApplyResources(this.tbSearchAlias, "tbSearchAlias");
            this.tbSearchAlias.Name = "tbSearchAlias";
            // 
            // lblSearchName
            // 
            this.lblSearchName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSearchName, "lblSearchName");
            this.lblSearchName.Name = "lblSearchName";
            // 
            // RetailDepartmentDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblSearchName);
            this.Controls.Add(this.btnAddRetailDivision);
            this.Controls.Add(this.lblRetailDivision);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbSearchAlias);
            this.Controls.Add(this.cmbRetailDivision);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "RetailDepartmentDialog";
            this.Controls.SetChildIndex(this.cmbRetailDivision, 0);
            this.Controls.SetChildIndex(this.tbSearchAlias, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblRetailDivision, 0);
            this.Controls.SetChildIndex(this.btnAddRetailDivision, 0);
            this.Controls.SetChildIndex(this.lblSearchName, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private ContextButton btnAddRetailDivision;
        private DualDataComboBox cmbRetailDivision;
        private System.Windows.Forms.Label lblRetailDivision;
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.TextBox tbSearchAlias;
    }
}