using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class FormProfileLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProfileLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFormType = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblFormLayout = new System.Windows.Forms.Label();
            this.btnEditFormType = new LSOne.Controls.ContextButton();
            this.btnAddFormType = new LSOne.Controls.ContextButton();
            this.cmbFormLayout = new LSOne.Controls.DualDataComboBox();
            this.cmbFormType = new LSOne.Controls.DualDataComboBox();
            this.btnEditFormLayout = new LSOne.Controls.ContextButton();
            this.btnAddFormLayout = new LSOne.Controls.ContextButton();
            this.lblCopies = new System.Windows.Forms.Label();
            this.ntbCopies = new LSOne.Controls.NumericTextBox();
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
            // lblFormType
            // 
            resources.ApplyResources(this.lblFormType, "lblFormType");
            this.lblFormType.Name = "lblFormType";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblFormLayout
            // 
            resources.ApplyResources(this.lblFormLayout, "lblFormLayout");
            this.lblFormLayout.Name = "lblFormLayout";
            // 
            // btnEditFormType
            // 
            this.btnEditFormType.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFormType.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFormType, "btnEditFormType");
            this.btnEditFormType.Name = "btnEditFormType";
            this.btnEditFormType.Click += new System.EventHandler(this.btnEditFormType_Click);
            // 
            // btnAddFormType
            // 
            this.btnAddFormType.BackColor = System.Drawing.Color.Transparent;
            this.btnAddFormType.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddFormType, "btnAddFormType");
            this.btnAddFormType.Name = "btnAddFormType";
            this.btnAddFormType.Click += new System.EventHandler(this.btnAddFormType_Click);
            // 
            // cmbFormLayout
            // 
            this.cmbFormLayout.AddList = null;
            this.cmbFormLayout.AllowKeyboardSelection = false;
            this.cmbFormLayout.Cursor = System.Windows.Forms.Cursors.No;
            resources.ApplyResources(this.cmbFormLayout, "cmbFormLayout");
            this.cmbFormLayout.MaxLength = 32767;
            this.cmbFormLayout.Name = "cmbFormLayout";
            this.cmbFormLayout.NoChangeAllowed = false;
            this.cmbFormLayout.OnlyDisplayID = false;
            this.cmbFormLayout.RemoveList = null;
            this.cmbFormLayout.RowHeight = ((short)(22));
            this.cmbFormLayout.SecondaryData = null;
            this.cmbFormLayout.SelectedData = null;
            this.cmbFormLayout.SelectedDataID = null;
            this.cmbFormLayout.SelectionList = null;
            this.cmbFormLayout.SkipIDColumn = true;
            this.cmbFormLayout.RequestData += new System.EventHandler(this.cmbFormLayout_RequestData);
            this.cmbFormLayout.SelectedDataChanged += new System.EventHandler(this.cmbFormLayout_SelectedDataChanged);
            // 
            // cmbFormType
            // 
            this.cmbFormType.AddList = null;
            this.cmbFormType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFormType, "cmbFormType");
            this.cmbFormType.MaxLength = 32767;
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.NoChangeAllowed = false;
            this.cmbFormType.OnlyDisplayID = false;
            this.cmbFormType.RemoveList = null;
            this.cmbFormType.RowHeight = ((short)(22));
            this.cmbFormType.SecondaryData = null;
            this.cmbFormType.SelectedData = null;
            this.cmbFormType.SelectedDataID = null;
            this.cmbFormType.SelectionList = null;
            this.cmbFormType.SkipIDColumn = true;
            this.cmbFormType.RequestData += new System.EventHandler(this.cmbFormType_RequestData);
            this.cmbFormType.SelectedDataChanged += new System.EventHandler(this.cmbFormType_SelectedDataChanged);
            // 
            // btnEditFormLayout
            // 
            this.btnEditFormLayout.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFormLayout.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFormLayout, "btnEditFormLayout");
            this.btnEditFormLayout.Name = "btnEditFormLayout";
            this.btnEditFormLayout.Click += new System.EventHandler(this.btnEditFormLayout_Click);
            // 
            // btnAddFormLayout
            // 
            this.btnAddFormLayout.BackColor = System.Drawing.Color.Transparent;
            this.btnAddFormLayout.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddFormLayout, "btnAddFormLayout");
            this.btnAddFormLayout.Name = "btnAddFormLayout";
            this.btnAddFormLayout.Click += new System.EventHandler(this.btnAddFormLayout_Click);
            // 
            // lblCopies
            // 
            resources.ApplyResources(this.lblCopies, "lblCopies");
            this.lblCopies.Name = "lblCopies";
            // 
            // ntbCopies
            // 
            this.ntbCopies.AllowDecimal = false;
            this.ntbCopies.AllowNegative = false;
            this.ntbCopies.CultureInfo = null;
            this.ntbCopies.DecimalLetters = 6;
            this.ntbCopies.ForeColor = System.Drawing.Color.Black;
            this.ntbCopies.HasMinValue = true;
            resources.ApplyResources(this.ntbCopies, "ntbCopies");
            this.ntbCopies.MaxValue = 999D;
            this.ntbCopies.MinValue = 1D;
            this.ntbCopies.Name = "ntbCopies";
            this.ntbCopies.Value = 0D;
            this.ntbCopies.ValueChanged += new System.EventHandler(this.ntbCopies_ValueChanged);
            // 
            // FormProfileLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbCopies);
            this.Controls.Add(this.lblCopies);
            this.Controls.Add(this.btnAddFormLayout);
            this.Controls.Add(this.btnEditFormLayout);
            this.Controls.Add(this.btnAddFormType);
            this.Controls.Add(this.btnEditFormType);
            this.Controls.Add(this.cmbFormLayout);
            this.Controls.Add(this.cmbFormType);
            this.Controls.Add(this.lblFormLayout);
            this.Controls.Add(this.lblFormType);
            this.Controls.Add(this.panel2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "FormProfileLineDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblFormType, 0);
            this.Controls.SetChildIndex(this.lblFormLayout, 0);
            this.Controls.SetChildIndex(this.cmbFormType, 0);
            this.Controls.SetChildIndex(this.cmbFormLayout, 0);
            this.Controls.SetChildIndex(this.btnEditFormType, 0);
            this.Controls.SetChildIndex(this.btnAddFormType, 0);
            this.Controls.SetChildIndex(this.btnEditFormLayout, 0);
            this.Controls.SetChildIndex(this.btnAddFormLayout, 0);
            this.Controls.SetChildIndex(this.lblCopies, 0);
            this.Controls.SetChildIndex(this.ntbCopies, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFormType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbFormLayout;
        private DualDataComboBox cmbFormType;
        private System.Windows.Forms.Label lblFormLayout;
        private ContextButton btnAddFormType;
        private ContextButton btnEditFormType;
        private ContextButton btnAddFormLayout;
        private ContextButton btnEditFormLayout;
        private System.Windows.Forms.Label lblCopies;
        private NumericTextBox ntbCopies;
    }
}