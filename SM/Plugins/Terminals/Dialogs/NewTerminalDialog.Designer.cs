using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.Dialogs
{
    partial class NewTerminalDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTerminalDialog));
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.tbID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddVisualProfile = new LSOne.Controls.ContextButton();
            this.cmbHardwareProfile = new LSOne.Controls.DualDataComboBox();
            this.cmbVisualProfile = new LSOne.Controls.DualDataComboBox();
            this.btnAddHardwareProfile = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
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
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            // 
            // lblStore
            // 
            resources.ApplyResources(this.lblStore, "lblStore");
            this.lblStore.Name = "lblStore";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnAddVisualProfile
            // 
            this.btnAddVisualProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnAddVisualProfile.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddVisualProfile, "btnAddVisualProfile");
            this.btnAddVisualProfile.Name = "btnAddVisualProfile";
            this.btnAddVisualProfile.Click += new System.EventHandler(this.btnAddVisualProfile_Click);
            // 
            // cmbHardwareProfile
            // 
            this.cmbHardwareProfile.AddList = null;
            this.cmbHardwareProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHardwareProfile, "cmbHardwareProfile");
            this.cmbHardwareProfile.MaxLength = 32767;
            this.cmbHardwareProfile.Name = "cmbHardwareProfile";
            this.cmbHardwareProfile.NoChangeAllowed = false;
            this.cmbHardwareProfile.OnlyDisplayID = false;
            this.cmbHardwareProfile.RemoveList = null;
            this.cmbHardwareProfile.RowHeight = ((short)(22));
            this.cmbHardwareProfile.SecondaryData = null;
            this.cmbHardwareProfile.SelectedData = null;
            this.cmbHardwareProfile.SelectedDataID = null;
            this.cmbHardwareProfile.SelectionList = null;
            this.cmbHardwareProfile.SkipIDColumn = true;
            this.cmbHardwareProfile.RequestData += new System.EventHandler(this.cmbHardwareProfile_RequestData);
            this.cmbHardwareProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbVisualProfile
            // 
            this.cmbVisualProfile.AddList = null;
            this.cmbVisualProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVisualProfile, "cmbVisualProfile");
            this.cmbVisualProfile.MaxLength = 32767;
            this.cmbVisualProfile.Name = "cmbVisualProfile";
            this.cmbVisualProfile.NoChangeAllowed = false;
            this.cmbVisualProfile.OnlyDisplayID = false;
            this.cmbVisualProfile.RemoveList = null;
            this.cmbVisualProfile.RowHeight = ((short)(22));
            this.cmbVisualProfile.SecondaryData = null;
            this.cmbVisualProfile.SelectedData = null;
            this.cmbVisualProfile.SelectedDataID = null;
            this.cmbVisualProfile.SelectionList = null;
            this.cmbVisualProfile.SkipIDColumn = true;
            this.cmbVisualProfile.RequestData += new System.EventHandler(this.cmbVisualProfile_RequestData);
            this.cmbVisualProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnAddHardwareProfile
            // 
            this.btnAddHardwareProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnAddHardwareProfile.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddHardwareProfile, "btnAddHardwareProfile");
            this.btnAddHardwareProfile.Name = "btnAddHardwareProfile";
            this.btnAddHardwareProfile.Click += new System.EventHandler(this.btnAddHardwareProfile_Click);
            // 
            // NewTerminalDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.btnAddVisualProfile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbHardwareProfile);
            this.Controls.Add(this.cmbVisualProfile);
            this.Controls.Add(this.btnAddHardwareProfile);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewTerminalDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.lblStore, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.lblID, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.btnAddHardwareProfile, 0);
            this.Controls.SetChildIndex(this.cmbVisualProfile, 0);
            this.Controls.SetChildIndex(this.cmbHardwareProfile, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.btnAddVisualProfile, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblStore;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ContextButton btnAddVisualProfile;
        private DualDataComboBox cmbHardwareProfile;
        private DualDataComboBox cmbVisualProfile;
        private ContextButton btnAddHardwareProfile;
    }
}