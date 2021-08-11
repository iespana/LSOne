using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class EditRestaurantDiningTableDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRestaurantDiningTableDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ntbTableNo = new NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbScreenNo = new DualDataComboBox();
            this.btnEditScreen = new ContextButton();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescriptionOnButton = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkNonSmoking = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbAvailability = new System.Windows.Forms.ComboBox();
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ntbTableNo
            // 
            this.ntbTableNo.AllowDecimal = false;
            this.ntbTableNo.AllowNegative = false;
            this.ntbTableNo.DecimalLetters = 2;
            this.ntbTableNo.HasMinValue = false;
            resources.ApplyResources(this.ntbTableNo, "ntbTableNo");
            this.ntbTableNo.MaxValue = 0D;
            this.ntbTableNo.MinValue = 0D;
            this.ntbTableNo.Name = "ntbTableNo";
            this.ntbTableNo.ReadOnly = true;
            this.ntbTableNo.Value = 0D;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbScreenNo
            // 
            resources.ApplyResources(this.cmbScreenNo, "cmbScreenNo");
            this.cmbScreenNo.MaxLength = 32767;
            this.cmbScreenNo.Name = "cmbScreenNo";
            this.cmbScreenNo.SelectedData = null;
            this.cmbScreenNo.SkipIDColumn = true;
            this.cmbScreenNo.RequestData += new System.EventHandler(this.cmbScreenNo_RequestData);
            this.cmbScreenNo.SelectedDataChanged += new System.EventHandler(this.CheckOkEnabled);
            this.cmbScreenNo.RequestClear += new System.EventHandler(this.cmbScreenNo_RequestClear);
            // 
            // btnEditScreen
            // 
            this.btnEditScreen.BackColor = System.Drawing.Color.Transparent;
            this.btnEditScreen.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditScreen, "btnEditScreen");
            this.btnEditScreen.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnEditScreen.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnEditScreen.Name = "btnEditScreen";
            this.btnEditScreen.Click += new System.EventHandler(this.btnEditScreen_Click);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckOkEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbDescriptionOnButton
            // 
            resources.ApplyResources(this.tbDescriptionOnButton, "tbDescriptionOnButton");
            this.tbDescriptionOnButton.Name = "tbDescriptionOnButton";
            this.tbDescriptionOnButton.ReadOnly = true;
            this.tbDescriptionOnButton.TextChanged += new System.EventHandler(this.CheckOkEnabled);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkNonSmoking
            // 
            resources.ApplyResources(this.chkNonSmoking, "chkNonSmoking");
            this.chkNonSmoking.Name = "chkNonSmoking";
            this.chkNonSmoking.UseVisualStyleBackColor = true;
            this.chkNonSmoking.CheckedChanged += new System.EventHandler(this.CheckOkEnabled);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbAvailability
            // 
            this.cmbAvailability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAvailability.FormattingEnabled = true;
            this.cmbAvailability.Items.AddRange(new object[] {
            resources.GetString("cmbAvailability.Items"),
            resources.GetString("cmbAvailability.Items1")});
            resources.ApplyResources(this.cmbAvailability, "cmbAvailability");
            this.cmbAvailability.Name = "cmbAvailability";
            // 
            // EditRestaurantDiningTableDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbAvailability);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkNonSmoking);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDescriptionOnButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.btnEditScreen);
            this.Controls.Add(this.cmbScreenNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ntbTableNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "EditRestaurantDiningTableDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.ntbTableNo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbScreenNo, 0);
            this.Controls.SetChildIndex(this.btnEditScreen, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbDescriptionOnButton, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.chkNonSmoking, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cmbAvailability, 0);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbTableNo;
        private DualDataComboBox cmbScreenNo;
        private ContextButton btnEditScreen;
        private System.Windows.Forms.CheckBox chkNonSmoking;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDescriptionOnButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbAvailability;
    }
}