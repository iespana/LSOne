using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class RemoteHostsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteHostsDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvRemoteHosts = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.ntbPort = new LSOne.Controls.NumericTextBox();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsAddRemove = new LSOne.Controls.ContextButtons();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Name = "panel2";
            // 
            // lvRemoteHosts
            // 
            this.lvRemoteHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvRemoteHosts.FullRowSelect = true;
            this.lvRemoteHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRemoteHosts.HideSelection = false;
            resources.ApplyResources(this.lvRemoteHosts, "lvRemoteHosts");
            this.lvRemoteHosts.LockDrawing = false;
            this.lvRemoteHosts.MultiSelect = false;
            this.lvRemoteHosts.Name = "lvRemoteHosts";
            this.lvRemoteHosts.SortColumn = -1;
            this.lvRemoteHosts.SortedBackwards = false;
            this.lvRemoteHosts.UseCompatibleStateImageBehavior = false;
            this.lvRemoteHosts.UseEveryOtherRowColoring = true;
            this.lvRemoteHosts.View = System.Windows.Forms.View.Details;
            this.lvRemoteHosts.SelectedIndexChanged += new System.EventHandler(this.lvRemoteHosts_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.ntbPort);
            this.groupPanel1.Controls.Add(this.tbAddress);
            this.groupPanel1.Controls.Add(this.lblPort);
            this.groupPanel1.Controls.Add(this.lblAddress);
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnSave);
            this.groupPanel1.Controls.Add(this.lblDescription);
            this.groupPanel1.Controls.Add(this.tbDescription);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // ntbPort
            // 
            this.ntbPort.AllowDecimal = false;
            this.ntbPort.AllowNegative = false;
            this.ntbPort.CultureInfo = null;
            this.ntbPort.DecimalLetters = 2;
            this.ntbPort.ForeColor = System.Drawing.Color.Black;
            this.ntbPort.HasMinValue = false;
            resources.ApplyResources(this.ntbPort, "ntbPort");
            this.ntbPort.MaxValue = 32768D;
            this.ntbPort.MinValue = 0D;
            this.ntbPort.Name = "ntbPort";
            this.ntbPort.Value = 0D;
            this.ntbPort.TextChanged += new System.EventHandler(this.CheckSaveEnabled);
            // 
            // tbAddress
            // 
            resources.ApplyResources(this.tbAddress, "tbAddress");
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.TextChanged += new System.EventHandler(this.tbAddress_TextChanged);
            // 
            // lblPort
            // 
            this.lblPort.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPort, "lblPort");
            this.lblPort.Name = "lblPort";
            // 
            // lblAddress
            // 
            this.lblAddress.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.Name = "lblAddress";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckSaveEnabled);
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsAddRemove.EditButtonEnabled = false;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = false;
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.btnsAddRemove_AddButtonClicked);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsAddRemove_RemoveButtonClicked);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // RemoteHostsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsAddRemove);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.lvRemoteHosts);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "RemoteHostsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvRemoteHosts, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.btnsAddRemove, 0);
            this.panel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private ExtendedListView lvRemoteHosts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblAddress;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbAddress;
        private NumericTextBox ntbPort;
    }
}