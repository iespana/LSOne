using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class HospitalityTypeRoutingConnectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeRoutingConnectionDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblConnection = new System.Windows.Forms.Label();
            this.cmbHospitalityType = new LSOne.Controls.DualDataComboBox();
            this.cmbInclude = new System.Windows.Forms.ComboBox();
            this.lblInclude = new System.Windows.Forms.Label();
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
            // lblConnection
            // 
            resources.ApplyResources(this.lblConnection, "lblConnection");
            this.lblConnection.Name = "lblConnection";
            // 
            // cmbHospitalityType
            // 
            this.cmbHospitalityType.AddList = null;
            this.cmbHospitalityType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHospitalityType, "cmbHospitalityType");
            this.cmbHospitalityType.MaxLength = 32767;
            this.cmbHospitalityType.Name = "cmbHospitalityType";
            this.cmbHospitalityType.NoChangeAllowed = false;
            this.cmbHospitalityType.OnlyDisplayID = false;
            this.cmbHospitalityType.RemoveList = null;
            this.cmbHospitalityType.RowHeight = ((short)(22));
            this.cmbHospitalityType.SecondaryData = null;
            this.cmbHospitalityType.SelectedData = null;
            this.cmbHospitalityType.SelectedDataID = null;
            this.cmbHospitalityType.SelectionList = null;
            this.cmbHospitalityType.SkipIDColumn = true;
            this.cmbHospitalityType.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbHospitalityType_DropDown);
            this.cmbHospitalityType.SelectedDataChanged += new System.EventHandler(this.cmbHospitalityType_SelectedDataChanged);
            // 
            // cmbInclude
            // 
            this.cmbInclude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInclude.FormattingEnabled = true;
            this.cmbInclude.Items.AddRange(new object[] {
            resources.GetString("cmbInclude.Items"),
            resources.GetString("cmbInclude.Items1")});
            resources.ApplyResources(this.cmbInclude, "cmbInclude");
            this.cmbInclude.Name = "cmbInclude";
            // 
            // lblInclude
            // 
            resources.ApplyResources(this.lblInclude, "lblInclude");
            this.lblInclude.Name = "lblInclude";
            // 
            // HospitalityTypeRoutingConnectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbInclude);
            this.Controls.Add(this.lblInclude);
            this.Controls.Add(this.cmbHospitalityType);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "HospitalityTypeRoutingConnectionDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblConnection, 0);
            this.Controls.SetChildIndex(this.cmbHospitalityType, 0);
            this.Controls.SetChildIndex(this.lblInclude, 0);
            this.Controls.SetChildIndex(this.cmbInclude, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblConnection;
        private DualDataComboBox cmbHospitalityType;
        private System.Windows.Forms.ComboBox cmbInclude;
        private System.Windows.Forms.Label lblInclude;
    }
}