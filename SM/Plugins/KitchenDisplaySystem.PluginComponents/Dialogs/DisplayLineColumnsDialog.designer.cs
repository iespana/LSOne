using LSOne.Controls;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class DisplayLineColumnsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayLineColumnsDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCaption = new System.Windows.Forms.TextBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblAlignment = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblColumnType = new System.Windows.Forms.Label();
            this.lblFieldData = new System.Windows.Forms.Label();
            this.cmbColumnStyle = new LSOne.Controls.DualDataComboBox();
            this.lblStyle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRelativeSize = new LSOne.Controls.NumericTextBox();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.cmbDataType = new LSOne.Controls.DualDataComboBox();
            this.cmbFieldData = new LSOne.Controls.DualDataComboBox();
            this.cmbAlignment = new LSOne.Controls.DualDataComboBox();
            this.btnsColumnStyle = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
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
            // tbCaption
            // 
            resources.ApplyResources(this.tbCaption, "tbCaption");
            this.tbCaption.Name = "tbCaption";
            this.tbCaption.TextChanged += new System.EventHandler(this.OnControlValuesChanged);
            // 
            // lblCaption
            // 
            resources.ApplyResources(this.lblCaption, "lblCaption");
            this.lblCaption.Name = "lblCaption";
            // 
            // lblAlignment
            // 
            resources.ApplyResources(this.lblAlignment, "lblAlignment");
            this.lblAlignment.Name = "lblAlignment";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblColumnType
            // 
            resources.ApplyResources(this.lblColumnType, "lblColumnType");
            this.lblColumnType.Name = "lblColumnType";
            // 
            // lblFieldData
            // 
            resources.ApplyResources(this.lblFieldData, "lblFieldData");
            this.lblFieldData.Name = "lblFieldData";
            // 
            // cmbColumnStyle
            // 
            this.cmbColumnStyle.AddList = null;
            this.cmbColumnStyle.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbColumnStyle, "cmbColumnStyle");
            this.cmbColumnStyle.MaxLength = 32767;
            this.cmbColumnStyle.Name = "cmbColumnStyle";
            this.cmbColumnStyle.NoChangeAllowed = false;
            this.cmbColumnStyle.OnlyDisplayID = false;
            this.cmbColumnStyle.RemoveList = null;
            this.cmbColumnStyle.RowHeight = ((short)(22));
            this.cmbColumnStyle.SecondaryData = null;
            this.cmbColumnStyle.SelectedData = null;
            this.cmbColumnStyle.SelectedDataID = null;
            this.cmbColumnStyle.SelectionList = null;
            this.cmbColumnStyle.SkipIDColumn = true;
            this.cmbColumnStyle.RequestData += new System.EventHandler(this.cmbColumnStyle_RequestData);
            this.cmbColumnStyle.SelectedDataChanged += new System.EventHandler(this.OnControlValuesChanged);
            // 
            // lblStyle
            // 
            resources.ApplyResources(this.lblStyle, "lblStyle");
            this.lblStyle.Name = "lblStyle";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbRelativeSize
            // 
            this.tbRelativeSize.AllowDecimal = true;
            this.tbRelativeSize.AllowNegative = false;
            this.tbRelativeSize.CultureInfo = null;
            this.tbRelativeSize.DecimalLetters = 0;
            this.tbRelativeSize.ForeColor = System.Drawing.Color.Black;
            this.tbRelativeSize.HasMinValue = false;
            resources.ApplyResources(this.tbRelativeSize, "tbRelativeSize");
            this.tbRelativeSize.MaxValue = 100D;
            this.tbRelativeSize.MinValue = 0D;
            this.tbRelativeSize.Name = "tbRelativeSize";
            this.tbRelativeSize.Value = 0D;
            this.tbRelativeSize.TextChanged += new System.EventHandler(this.OnControlValuesChanged);
            // 
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // cmbDataType
            // 
            this.cmbDataType.AddList = null;
            this.cmbDataType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDataType, "cmbDataType");
            this.cmbDataType.MaxLength = 32767;
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.NoChangeAllowed = false;
            this.cmbDataType.OnlyDisplayID = false;
            this.cmbDataType.RemoveList = null;
            this.cmbDataType.RowHeight = ((short)(22));
            this.cmbDataType.SecondaryData = null;
            this.cmbDataType.SelectedData = null;
            this.cmbDataType.SelectedDataID = null;
            this.cmbDataType.SelectionList = null;
            this.cmbDataType.SkipIDColumn = true;
            this.cmbDataType.RequestData += new System.EventHandler(this.cmbDataType_RequestData);
            this.cmbDataType.SelectedDataChanged += new System.EventHandler(this.cmbDataType_SelectedDataChanged);
            // 
            // cmbFieldData
            // 
            this.cmbFieldData.AddList = null;
            this.cmbFieldData.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFieldData, "cmbFieldData");
            this.cmbFieldData.MaxLength = 32767;
            this.cmbFieldData.Name = "cmbFieldData";
            this.cmbFieldData.NoChangeAllowed = false;
            this.cmbFieldData.OnlyDisplayID = false;
            this.cmbFieldData.RemoveList = null;
            this.cmbFieldData.RowHeight = ((short)(22));
            this.cmbFieldData.SecondaryData = null;
            this.cmbFieldData.SelectedData = null;
            this.cmbFieldData.SelectedDataID = null;
            this.cmbFieldData.SelectionList = null;
            this.cmbFieldData.SkipIDColumn = true;
            this.cmbFieldData.RequestData += new System.EventHandler(this.cmbFieldData_RequestData);
            this.cmbFieldData.SelectedDataChanged += new System.EventHandler(this.cmbFieldData_SelectedDataChanged);
            // 
            // cmbAlignment
            // 
            this.cmbAlignment.AddList = null;
            this.cmbAlignment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAlignment, "cmbAlignment");
            this.cmbAlignment.MaxLength = 32767;
            this.cmbAlignment.Name = "cmbAlignment";
            this.cmbAlignment.NoChangeAllowed = false;
            this.cmbAlignment.OnlyDisplayID = false;
            this.cmbAlignment.RemoveList = null;
            this.cmbAlignment.RowHeight = ((short)(22));
            this.cmbAlignment.SecondaryData = null;
            this.cmbAlignment.SelectedData = null;
            this.cmbAlignment.SelectedDataID = null;
            this.cmbAlignment.SelectionList = null;
            this.cmbAlignment.SkipIDColumn = true;
            this.cmbAlignment.RequestData += new System.EventHandler(this.cmbAlignment_RequestData);
            // 
            // btnsColumnStyle
            // 
            this.btnsColumnStyle.AddButtonEnabled = true;
            this.btnsColumnStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsColumnStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsColumnStyle.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsColumnStyle, "btnsColumnStyle");
            this.btnsColumnStyle.Name = "btnsColumnStyle";
            this.btnsColumnStyle.RemoveButtonEnabled = false;
            // 
            // DisplayLineColumnsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbAlignment);
            this.Controls.Add(this.cmbFieldData);
            this.Controls.Add(this.cmbDataType);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.tbRelativeSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.btnsColumnStyle);
            this.Controls.Add(this.cmbColumnStyle);
            this.Controls.Add(this.lblColumnType);
            this.Controls.Add(this.lblFieldData);
            this.Controls.Add(this.lblAlignment);
            this.Controls.Add(this.tbCaption);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "DisplayLineColumnsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblCaption, 0);
            this.Controls.SetChildIndex(this.tbCaption, 0);
            this.Controls.SetChildIndex(this.lblAlignment, 0);
            this.Controls.SetChildIndex(this.lblFieldData, 0);
            this.Controls.SetChildIndex(this.lblColumnType, 0);
            this.Controls.SetChildIndex(this.cmbColumnStyle, 0);
            this.Controls.SetChildIndex(this.btnsColumnStyle, 0);
            this.Controls.SetChildIndex(this.lblStyle, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbRelativeSize, 0);
            this.Controls.SetChildIndex(this.linkFields1, 0);
            this.Controls.SetChildIndex(this.cmbDataType, 0);
            this.Controls.SetChildIndex(this.cmbFieldData, 0);
            this.Controls.SetChildIndex(this.cmbAlignment, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbCaption;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblAlignment;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblColumnType;
        private System.Windows.Forms.Label lblFieldData;
        private System.Windows.Forms.Label lblStyle;
        private Controls.KdsStyleButtons btnsColumnStyle;
        private DualDataComboBox cmbColumnStyle;
        private System.Windows.Forms.Label label1;
        private NumericTextBox tbRelativeSize;
        private LinkFields linkFields1;
        private DualDataComboBox cmbFieldData;
        private DualDataComboBox cmbDataType;
        private DualDataComboBox cmbAlignment;
    }
}