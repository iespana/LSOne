using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class NewHospitalityTypeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewHospitalityTypeDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbRestaurant = new DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSalesType = new DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddRestaurant = new ContextButton();
            this.btnAddSalesType = new ContextButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
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
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbRestaurant
            // 
            resources.ApplyResources(this.cmbRestaurant, "cmbRestaurant");
            this.cmbRestaurant.MaxLength = 32767;
            this.cmbRestaurant.Name = "cmbRestaurant";
            this.cmbRestaurant.SelectedData = null;
            this.cmbRestaurant.SkipIDColumn = false;
            this.cmbRestaurant.RequestData += new System.EventHandler(this.cmbRestaurant_RequestData);
            this.cmbRestaurant.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbSalesType
            // 
            resources.ApplyResources(this.cmbSalesType, "cmbSalesType");
            this.cmbSalesType.MaxLength = 32767;
            this.cmbSalesType.Name = "cmbSalesType";
            this.cmbSalesType.SelectedData = null;
            this.cmbSalesType.SkipIDColumn = true;
            this.cmbSalesType.RequestData += new System.EventHandler(this.cmbSalesType_RequestData);
            this.cmbSalesType.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnAddRestaurant
            // 
            this.btnAddRestaurant.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRestaurant.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddRestaurant, "btnAddRestaurant");
            this.btnAddRestaurant.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddRestaurant.MinimumSize = new System.Drawing.Size(24, 24);
            this.btnAddRestaurant.Name = "btnAddRestaurant";
            this.btnAddRestaurant.Click += new System.EventHandler(this.btnAddRestaurant_Click);
            // 
            // btnAddSalesType
            // 
            this.btnAddSalesType.BackColor = System.Drawing.Color.Transparent;
            this.btnAddSalesType.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddSalesType, "btnAddSalesType");
            this.btnAddSalesType.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnAddSalesType.MinimumSize = new System.Drawing.Size(24, 24);
            this.btnAddSalesType.Name = "btnAddSalesType";
            this.btnAddSalesType.Click += new System.EventHandler(this.btnAddSalesType_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // NewHospitalityTypeDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddSalesType);
            this.Controls.Add(this.btnAddRestaurant);
            this.Controls.Add(this.cmbRestaurant);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSalesType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewHospitalityTypeDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbSalesType, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbRestaurant, 0);
            this.Controls.SetChildIndex(this.btnAddRestaurant, 0);
            this.Controls.SetChildIndex(this.btnAddSalesType, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbRestaurant;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbSalesType;
        private System.Windows.Forms.Label label4;
        private ContextButton btnAddRestaurant;
        private ContextButton btnAddSalesType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}