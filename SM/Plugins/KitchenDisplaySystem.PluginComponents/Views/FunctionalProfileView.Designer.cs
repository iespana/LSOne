using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class FunctionalProfileView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileView));
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAutoBump = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDoneOrdersAppear = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbButtons = new LSOne.Controls.DualDataComboBox();
            this.buttonLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new LSOne.Controls.ContextButton();
            this.btnEdit = new LSOne.Controls.ContextButton();
            this.cmbBumpPossible = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRecalledOrdersAppear = new System.Windows.Forms.ComboBox();
            this.chkSoundOnNewOrder = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.buttonLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.label7);
            this.pnlBottom.Controls.Add(this.tbAutoBump);
            this.pnlBottom.Controls.Add(this.label6);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.cmbDoneOrdersAppear);
            this.pnlBottom.Controls.Add(this.label4);
            this.pnlBottom.Controls.Add(this.label9);
            this.pnlBottom.Controls.Add(this.cmbButtons);
            this.pnlBottom.Controls.Add(this.buttonLayout);
            this.pnlBottom.Controls.Add(this.cmbBumpPossible);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.cmbRecalledOrdersAppear);
            this.pnlBottom.Controls.Add(this.chkSoundOnNewOrder);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.tbProfileName);
            this.pnlBottom.Controls.Add(this.label3);
            // 
            // tbProfileName
            // 
            resources.ApplyResources(this.tbProfileName, "tbProfileName");
            this.tbProfileName.Name = "tbProfileName";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbAutoBump
            // 
            this.tbAutoBump.AllowDecimal = false;
            this.tbAutoBump.AllowNegative = false;
            this.tbAutoBump.CultureInfo = null;
            this.tbAutoBump.DecimalLetters = 0;
            this.tbAutoBump.ForeColor = System.Drawing.Color.Black;
            this.tbAutoBump.HasMinValue = false;
            resources.ApplyResources(this.tbAutoBump, "tbAutoBump");
            this.tbAutoBump.MaxValue = 9999D;
            this.tbAutoBump.MinValue = 0D;
            this.tbAutoBump.Name = "tbAutoBump";
            this.tbAutoBump.Value = 0D;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbDoneOrdersAppear
            // 
            this.cmbDoneOrdersAppear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoneOrdersAppear.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDoneOrdersAppear, "cmbDoneOrdersAppear");
            this.cmbDoneOrdersAppear.Name = "cmbDoneOrdersAppear";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbButtons
            // 
            this.cmbButtons.AddList = null;
            this.cmbButtons.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtons, "cmbButtons");
            this.cmbButtons.MaxLength = 32767;
            this.cmbButtons.Name = "cmbButtons";
            this.cmbButtons.NoChangeAllowed = false;
            this.cmbButtons.OnlyDisplayID = false;
            this.cmbButtons.RemoveList = null;
            this.cmbButtons.RowHeight = ((short)(22));
            this.cmbButtons.SecondaryData = null;
            this.cmbButtons.SelectedData = null;
            this.cmbButtons.SelectedDataID = null;
            this.cmbButtons.SelectionList = null;
            this.cmbButtons.SkipIDColumn = true;
            this.cmbButtons.RequestData += new System.EventHandler(this.cmbButtons_RequestData);
            // 
            // buttonLayout
            // 
            resources.ApplyResources(this.buttonLayout, "buttonLayout");
            this.buttonLayout.Controls.Add(this.btnAdd);
            this.buttonLayout.Controls.Add(this.btnEdit);
            this.buttonLayout.Name = "buttonLayout";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cmbBumpPossible
            // 
            this.cmbBumpPossible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBumpPossible.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBumpPossible, "cmbBumpPossible");
            this.cmbBumpPossible.Name = "cmbBumpPossible";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbRecalledOrdersAppear
            // 
            this.cmbRecalledOrdersAppear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecalledOrdersAppear.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRecalledOrdersAppear, "cmbRecalledOrdersAppear");
            this.cmbRecalledOrdersAppear.Name = "cmbRecalledOrdersAppear";
            // 
            // chkSoundOnNewOrder
            // 
            resources.ApplyResources(this.chkSoundOnNewOrder, "chkSoundOnNewOrder");
            this.chkSoundOnNewOrder.Name = "chkSoundOnNewOrder";
            this.chkSoundOnNewOrder.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // FunctionalProfileView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 50;
            this.Name = "FunctionalProfileView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.buttonLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Label label3;
        private NumericTextBox tbAutoBump;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDoneOrdersAppear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private DualDataComboBox cmbButtons;
        private System.Windows.Forms.FlowLayoutPanel buttonLayout;
        private ContextButton btnAdd;
        private ContextButton btnEdit;
        private System.Windows.Forms.ComboBox cmbBumpPossible;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRecalledOrdersAppear;
        private System.Windows.Forms.CheckBox chkSoundOnNewOrder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
    }
}
