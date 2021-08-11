using LSOne.Controls;

namespace LSOne.ViewPlugins.Infocodes.Dialogs
{
    partial class InfocodeSpecificDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfocodeSpecificDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbInfocodeLink = new LSOne.Controls.DualDataComboBox();
            this.cmbInfocodeType = new System.Windows.Forms.ComboBox();
            this.cmbSalesType = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTriggering = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblUOM = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.chkInputRequired = new System.Windows.Forms.CheckBox();
            this.lblInputRequired = new System.Windows.Forms.Label();
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
            // cmbInfocodeLink
            // 
            this.cmbInfocodeLink.AddList = null;
            this.cmbInfocodeLink.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbInfocodeLink, "cmbInfocodeLink");
            this.cmbInfocodeLink.MaxLength = 32767;
            this.cmbInfocodeLink.Name = "cmbInfocodeLink";
            this.cmbInfocodeLink.NoChangeAllowed = false;
            this.cmbInfocodeLink.OnlyDisplayID = false;
            this.cmbInfocodeLink.RemoveList = null;
            this.cmbInfocodeLink.RowHeight = ((short)(22));
            this.cmbInfocodeLink.SecondaryData = null;
            this.cmbInfocodeLink.SelectedData = null;
            this.cmbInfocodeLink.SelectedDataID = null;
            this.cmbInfocodeLink.SelectionList = null;
            this.cmbInfocodeLink.SkipIDColumn = true;
            this.cmbInfocodeLink.RequestData += new System.EventHandler(this.cmbInfocodeLink_RequestData);
            this.cmbInfocodeLink.SelectedDataChanged += new System.EventHandler(this.cmbInfocodeLink_SelectedDataChanged);
            this.cmbInfocodeLink.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbInfocodeType
            // 
            this.cmbInfocodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInfocodeType.FormattingEnabled = true;
            this.cmbInfocodeType.Items.AddRange(new object[] {
            resources.GetString("cmbInfocodeType.Items"),
            resources.GetString("cmbInfocodeType.Items1")});
            resources.ApplyResources(this.cmbInfocodeType, "cmbInfocodeType");
            this.cmbInfocodeType.Name = "cmbInfocodeType";
            this.cmbInfocodeType.SelectedIndexChanged += new System.EventHandler(this.cmbInfocodeType_SelectedDataChanged);
            // 
            // cmbSalesType
            // 
            this.cmbSalesType.AddList = null;
            this.cmbSalesType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesType, "cmbSalesType");
            this.cmbSalesType.MaxLength = 32767;
            this.cmbSalesType.Name = "cmbSalesType";
            this.cmbSalesType.NoChangeAllowed = false;
            this.cmbSalesType.OnlyDisplayID = false;
            this.cmbSalesType.RemoveList = null;
            this.cmbSalesType.RowHeight = ((short)(22));
            this.cmbSalesType.SecondaryData = null;
            this.cmbSalesType.SelectedData = null;
            this.cmbSalesType.SelectedDataID = null;
            this.cmbSalesType.SelectionList = null;
            this.cmbSalesType.SkipIDColumn = true;
            this.cmbSalesType.RequestData += new System.EventHandler(this.cmbSalesType_RequestData);
            this.cmbSalesType.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbTriggering
            // 
            this.cmbTriggering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbTriggering, "cmbTriggering");
            this.cmbTriggering.FormattingEnabled = true;
            this.cmbTriggering.Items.AddRange(new object[] {
            resources.GetString("cmbTriggering.Items"),
            resources.GetString("cmbTriggering.Items1")});
            this.cmbTriggering.Name = "cmbTriggering";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblUOM
            // 
            resources.ApplyResources(this.lblUOM, "lblUOM");
            this.lblUOM.Name = "lblUOM";
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            // 
            // chkInputRequired
            // 
            resources.ApplyResources(this.chkInputRequired, "chkInputRequired");
            this.chkInputRequired.Name = "chkInputRequired";
            this.chkInputRequired.UseVisualStyleBackColor = true;
            // 
            // lblInputRequired
            // 
            resources.ApplyResources(this.lblInputRequired, "lblInputRequired");
            this.lblInputRequired.Name = "lblInputRequired";
            // 
            // InfocodeSpecificDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkInputRequired);
            this.Controls.Add(this.lblInputRequired);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblUOM);
            this.Controls.Add(this.cmbTriggering);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbSalesType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbInfocodeType);
            this.Controls.Add(this.cmbInfocodeLink);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "InfocodeSpecificDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbInfocodeLink, 0);
            this.Controls.SetChildIndex(this.cmbInfocodeType, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbSalesType, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cmbTriggering, 0);
            this.Controls.SetChildIndex(this.lblUOM, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblInputRequired, 0);
            this.Controls.SetChildIndex(this.chkInputRequired, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbInfocodeLink;
        private System.Windows.Forms.ComboBox cmbInfocodeType;
        private DualDataComboBox cmbSalesType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTriggering;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblUOM;
        private DualDataComboBox cmbUnit;
        private System.Windows.Forms.CheckBox chkInputRequired;
        private System.Windows.Forms.Label lblInputRequired;
    }
}