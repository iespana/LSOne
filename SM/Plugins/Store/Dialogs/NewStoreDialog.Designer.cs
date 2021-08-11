using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    partial class NewStoreDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStoreDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddCurrency = new LSOne.Controls.ContextButton();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.cmbTouchButtonLayout = new LSOne.Controls.DualDataComboBox();
            this.cmbFunctionalityProfile = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddTouchButtonLayout = new LSOne.Controls.ContextButton();
            this.btnAddFunctionalityProfile = new LSOne.Controls.ContextButton();
            this.btnAddRegion = new LSOne.Controls.ContextButton();
            this.lblRegion = new System.Windows.Forms.Label();
            this.cmbRegion = new LSOne.Controls.DualDataComboBox();
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnAddCurrency
            // 
            this.btnAddCurrency.BackColor = System.Drawing.Color.Transparent;
            this.btnAddCurrency.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCurrency, "btnAddCurrency");
            this.btnAddCurrency.Name = "btnAddCurrency";
            this.btnAddCurrency.Click += new System.EventHandler(this.btnAddCurrency_Click);
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.AddList = null;
            this.cmbCurrency.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.NoChangeAllowed = false;
            this.cmbCurrency.OnlyDisplayID = false;
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SecondaryData = null;
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectedDataID = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = false;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // lblID
            // 
            resources.ApplyResources(this.lblID, "lblID");
            this.lblID.Name = "lblID";
            // 
            // cmbTouchButtonLayout
            // 
            this.cmbTouchButtonLayout.AddList = null;
            this.cmbTouchButtonLayout.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTouchButtonLayout, "cmbTouchButtonLayout");
            this.cmbTouchButtonLayout.MaxLength = 32767;
            this.cmbTouchButtonLayout.Name = "cmbTouchButtonLayout";
            this.cmbTouchButtonLayout.NoChangeAllowed = false;
            this.cmbTouchButtonLayout.OnlyDisplayID = false;
            this.cmbTouchButtonLayout.RemoveList = null;
            this.cmbTouchButtonLayout.RowHeight = ((short)(22));
            this.cmbTouchButtonLayout.SecondaryData = null;
            this.cmbTouchButtonLayout.SelectedData = null;
            this.cmbTouchButtonLayout.SelectedDataID = null;
            this.cmbTouchButtonLayout.SelectionList = null;
            this.cmbTouchButtonLayout.SkipIDColumn = true;
            this.cmbTouchButtonLayout.RequestData += new System.EventHandler(this.cmbTouchButtonLayout_RequestData);
            this.cmbTouchButtonLayout.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbFunctionalityProfile
            // 
            this.cmbFunctionalityProfile.AddList = null;
            this.cmbFunctionalityProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFunctionalityProfile, "cmbFunctionalityProfile");
            this.cmbFunctionalityProfile.MaxLength = 32767;
            this.cmbFunctionalityProfile.Name = "cmbFunctionalityProfile";
            this.cmbFunctionalityProfile.NoChangeAllowed = false;
            this.cmbFunctionalityProfile.OnlyDisplayID = false;
            this.cmbFunctionalityProfile.RemoveList = null;
            this.cmbFunctionalityProfile.RowHeight = ((short)(22));
            this.cmbFunctionalityProfile.SecondaryData = null;
            this.cmbFunctionalityProfile.SelectedData = null;
            this.cmbFunctionalityProfile.SelectedDataID = null;
            this.cmbFunctionalityProfile.SelectionList = null;
            this.cmbFunctionalityProfile.SkipIDColumn = true;
            this.cmbFunctionalityProfile.RequestData += new System.EventHandler(this.cmbFunctionalityProfile_RequestData);
            this.cmbFunctionalityProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // btnAddTouchButtonLayout
            // 
            this.btnAddTouchButtonLayout.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTouchButtonLayout.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddTouchButtonLayout, "btnAddTouchButtonLayout");
            this.btnAddTouchButtonLayout.Name = "btnAddTouchButtonLayout";
            this.btnAddTouchButtonLayout.Click += new System.EventHandler(this.btnAddTouchButtonLayout_Click);
            // 
            // btnAddFunctionalityProfile
            // 
            this.btnAddFunctionalityProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnAddFunctionalityProfile.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddFunctionalityProfile, "btnAddFunctionalityProfile");
            this.btnAddFunctionalityProfile.Name = "btnAddFunctionalityProfile";
            this.btnAddFunctionalityProfile.Click += new System.EventHandler(this.btnAddFunctionalityProfile_Click);
            // 
            // btnAddRegion
            // 
            this.btnAddRegion.BackColor = System.Drawing.Color.Transparent;
            this.btnAddRegion.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddRegion, "btnAddRegion");
            this.btnAddRegion.Name = "btnAddRegion";
            this.btnAddRegion.Click += new System.EventHandler(this.btnAddRegion_Click);
            // 
            // lblRegion
            // 
            resources.ApplyResources(this.lblRegion, "lblRegion");
            this.lblRegion.Name = "lblRegion";
            // 
            // cmbRegion
            // 
            this.cmbRegion.AddList = null;
            this.cmbRegion.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRegion, "cmbRegion");
            this.cmbRegion.MaxLength = 32767;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.NoChangeAllowed = false;
            this.cmbRegion.OnlyDisplayID = false;
            this.cmbRegion.RemoveList = null;
            this.cmbRegion.RowHeight = ((short)(22));
            this.cmbRegion.SecondaryData = null;
            this.cmbRegion.SelectedData = null;
            this.cmbRegion.SelectedDataID = null;
            this.cmbRegion.SelectionList = null;
            this.cmbRegion.SkipIDColumn = true;
            this.cmbRegion.RequestData += new System.EventHandler(this.cmbRegion_RequestData);
            // 
            // NewStoreDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAddRegion);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.cmbRegion);
            this.Controls.Add(this.btnAddFunctionalityProfile);
            this.Controls.Add(this.btnAddTouchButtonLayout);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbFunctionalityProfile);
            this.Controls.Add(this.cmbTouchButtonLayout);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.btnAddCurrency);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewStoreDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCurrency, 0);
            this.Controls.SetChildIndex(this.btnAddCurrency, 0);
            this.Controls.SetChildIndex(this.lblID, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.cmbTouchButtonLayout, 0);
            this.Controls.SetChildIndex(this.cmbFunctionalityProfile, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btnAddTouchButtonLayout, 0);
            this.Controls.SetChildIndex(this.btnAddFunctionalityProfile, 0);
            this.Controls.SetChildIndex(this.cmbRegion, 0);
            this.Controls.SetChildIndex(this.lblRegion, 0);
            this.Controls.SetChildIndex(this.btnAddRegion, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label1;
        private ContextButton btnAddCurrency;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblID;
        private ContextButton btnAddFunctionalityProfile;
        private ContextButton btnAddTouchButtonLayout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbFunctionalityProfile;
        private DualDataComboBox cmbTouchButtonLayout;
        private ContextButton btnAddRegion;
        private System.Windows.Forms.Label lblRegion;
        private DualDataComboBox cmbRegion;
    }
}