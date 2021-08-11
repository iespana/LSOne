using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
    partial class SelectLoyaltyCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectLoyaltyCardDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbLoyaltyCard = new DualDataComboBox();
            this.laScheme = new System.Windows.Forms.Label();
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
            // cmbLoyaltyCard
            // 
            this.cmbLoyaltyCard.AddList = null;
            this.cmbLoyaltyCard.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLoyaltyCard, "cmbLoyaltyCard");
            this.cmbLoyaltyCard.MaxLength = 32767;
            this.cmbLoyaltyCard.Name = "cmbLoyaltyCard";
            this.cmbLoyaltyCard.RemoveList = null;
            this.cmbLoyaltyCard.RowHeight = ((short)(22));
            this.cmbLoyaltyCard.SelectedData = null;
            this.cmbLoyaltyCard.SelectionList = null;
            this.cmbLoyaltyCard.SkipIDColumn = false;
            this.cmbLoyaltyCard.DropDown += new DropDownEventHandler(this.dcbScheme_DropDown);
            this.cmbLoyaltyCard.SelectedDataChanged += new System.EventHandler(this.cmbLoyaltyCard_SelectedDataChanged);
            this.cmbLoyaltyCard.RequestClear += new System.EventHandler(this.DualDataComboBox_RequestClear);
            // 
            // laScheme
            // 
            this.laScheme.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laScheme, "laScheme");
            this.laScheme.Name = "laScheme";
            // 
            // SelectLoyaltyCardDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbLoyaltyCard);
            this.Controls.Add(this.laScheme);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "SelectLoyaltyCardDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.laScheme, 0);
            this.Controls.SetChildIndex(this.cmbLoyaltyCard, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
		private DualDataComboBox cmbLoyaltyCard;
        private System.Windows.Forms.Label laScheme;
    }
}