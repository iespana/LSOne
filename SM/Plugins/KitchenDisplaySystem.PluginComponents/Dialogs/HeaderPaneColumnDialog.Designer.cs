using LSOne.Controls;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class HeaderPaneColumnDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeaderPaneColumnDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblLineType = new System.Windows.Forms.Label();
            this.cmbColumnType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbColumnAlignment = new System.Windows.Forms.ComboBox();
            this.btnsStyle = new KdsStyleButtons();
            this.cmbStyle = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblLineType
            // 
            resources.ApplyResources(this.lblLineType, "lblLineType");
            this.lblLineType.Name = "lblLineType";
            // 
            // cmbColumnType
            // 
            this.cmbColumnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColumnType.FormattingEnabled = true;
            this.cmbColumnType.Items.AddRange(new object[] {
            resources.GetString("cmbColumnType.Items"),
            resources.GetString("cmbColumnType.Items1"),
            resources.GetString("cmbColumnType.Items2"),
            resources.GetString("cmbColumnType.Items3"),
            resources.GetString("cmbColumnType.Items4"),
            resources.GetString("cmbColumnType.Items5"),
            resources.GetString("cmbColumnType.Items6")});
            resources.ApplyResources(this.cmbColumnType, "cmbColumnType");
            this.cmbColumnType.Name = "cmbColumnType";
            this.cmbColumnType.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbColumnAlignment
            // 
            this.cmbColumnAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColumnAlignment.FormattingEnabled = true;
            this.cmbColumnAlignment.Items.AddRange(new object[] {
            resources.GetString("cmbColumnAlignment.Items"),
            resources.GetString("cmbColumnAlignment.Items1"),
            resources.GetString("cmbColumnAlignment.Items2")});
            resources.ApplyResources(this.cmbColumnAlignment, "cmbColumnAlignment");
            this.cmbColumnAlignment.Name = "cmbColumnAlignment";
            this.cmbColumnAlignment.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // btnsStyle
            // 
            this.btnsStyle.AddButtonEnabled = true;
            this.btnsStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsStyle.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsStyle, "btnsStyle");
            this.btnsStyle.Name = "btnsStyle";
            this.btnsStyle.RemoveButtonEnabled = false;
            // 
            // cmbStyle
            // 
            this.cmbStyle.AddList = null;
            this.cmbStyle.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStyle, "cmbStyle");
            this.cmbStyle.MaxLength = 32767;
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.NoChangeAllowed = false;
            this.cmbStyle.OnlyDisplayID = false;
            this.cmbStyle.RemoveList = null;
            this.cmbStyle.RowHeight = ((short)(22));
            this.cmbStyle.SecondaryData = null;
            this.cmbStyle.SelectedData = null;
            this.cmbStyle.SelectedDataID = null;
            this.cmbStyle.SelectionList = null;
            this.cmbStyle.SkipIDColumn = true;
            this.cmbStyle.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbStyle.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // HeaderPaneColumnDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsStyle);
            this.Controls.Add(this.cmbStyle);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbColumnAlignment);
            this.Controls.Add(this.lblLineType);
            this.Controls.Add(this.cmbColumnType);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "HeaderPaneColumnDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.cmbColumnType, 0);
            this.Controls.SetChildIndex(this.lblLineType, 0);
            this.Controls.SetChildIndex(this.cmbColumnAlignment, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cmbStyle, 0);
            this.Controls.SetChildIndex(this.btnsStyle, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbColumnAlignment;
        private System.Windows.Forms.Label lblLineType;
        private System.Windows.Forms.ComboBox cmbColumnType;
        private Controls.KdsStyleButtons btnsStyle;
        private DualDataComboBox cmbStyle;
        private System.Windows.Forms.Label label7;
    }
}