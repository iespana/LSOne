namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class VisualProfileLayoutPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfileLayoutPage));
            this.cmbHeaderProfile = new LSOne.Controls.DualDataComboBox();
            this.btnsHeaderProfile = new LSOne.Controls.ContextButtons();
            this.lblKitchenDisplayProfile = new System.Windows.Forms.Label();
            this.tbHeaderHeight = new LSOne.Controls.NumericTextBox();
            this.lblHeaderHeight = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbAggregateColumns = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAggregateSize = new LSOne.Controls.NumericTextBox();
            this.cbAggregatePosition = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAggregateVisable = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbButtonSize = new LSOne.Controls.NumericTextBox();
            this.cmbButtonPosistion = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkButtonVisable = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbHistoryHorizon = new LSOne.Controls.NumericTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbHistoryRows = new LSOne.Controls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbHistorySize = new LSOne.Controls.NumericTextBox();
            this.cmbHistoryPosition = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkHistoryVisable = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbHeaderProfile
            // 
            this.cmbHeaderProfile.AddList = null;
            this.cmbHeaderProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHeaderProfile, "cmbHeaderProfile");
            this.cmbHeaderProfile.MaxLength = 32767;
            this.cmbHeaderProfile.Name = "cmbHeaderProfile";
            this.cmbHeaderProfile.NoChangeAllowed = false;
            this.cmbHeaderProfile.OnlyDisplayID = false;
            this.cmbHeaderProfile.RemoveList = null;
            this.cmbHeaderProfile.RowHeight = ((short)(22));
            this.cmbHeaderProfile.SecondaryData = null;
            this.cmbHeaderProfile.SelectedData = null;
            this.cmbHeaderProfile.SelectedDataID = null;
            this.cmbHeaderProfile.SelectionList = null;
            this.cmbHeaderProfile.SkipIDColumn = true;
            this.cmbHeaderProfile.RequestData += new System.EventHandler(this.cmbHeaderProfile_RequestData);
            // 
            // btnsHeaderProfile
            // 
            this.btnsHeaderProfile.AddButtonEnabled = true;
            this.btnsHeaderProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnsHeaderProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsHeaderProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsHeaderProfile, "btnsHeaderProfile");
            this.btnsHeaderProfile.Name = "btnsHeaderProfile";
            this.btnsHeaderProfile.RemoveButtonEnabled = false;
            this.btnsHeaderProfile.EditButtonClicked += new System.EventHandler(this.btnsHeaderProfile_EditButtonClicked);
            this.btnsHeaderProfile.AddButtonClicked += new System.EventHandler(this.btnsHeaderProfile_AddButtonClicked);
            // 
            // lblKitchenDisplayProfile
            // 
            resources.ApplyResources(this.lblKitchenDisplayProfile, "lblKitchenDisplayProfile");
            this.lblKitchenDisplayProfile.Name = "lblKitchenDisplayProfile";
            // 
            // tbHeaderHeight
            // 
            this.tbHeaderHeight.AllowDecimal = false;
            this.tbHeaderHeight.AllowNegative = false;
            this.tbHeaderHeight.CultureInfo = null;
            this.tbHeaderHeight.DecimalLetters = 0;
            this.tbHeaderHeight.ForeColor = System.Drawing.Color.Black;
            this.tbHeaderHeight.HasMinValue = false;
            resources.ApplyResources(this.tbHeaderHeight, "tbHeaderHeight");
            this.tbHeaderHeight.MaxValue = 100D;
            this.tbHeaderHeight.MinValue = 0D;
            this.tbHeaderHeight.Name = "tbHeaderHeight";
            this.tbHeaderHeight.Value = 0D;
            this.tbHeaderHeight.Leave += new System.EventHandler(this.UpdatePreview);
            // 
            // lblHeaderHeight
            // 
            this.lblHeaderHeight.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblHeaderHeight, "lblHeaderHeight");
            this.lblHeaderHeight.Name = "lblHeaderHeight";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbHeaderProfile);
            this.groupBox1.Controls.Add(this.lblKitchenDisplayProfile);
            this.groupBox1.Controls.Add(this.btnsHeaderProfile);
            this.groupBox1.Controls.Add(this.lblHeaderHeight);
            this.groupBox1.Controls.Add(this.tbHeaderHeight);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbAggregateColumns);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbAggregateSize);
            this.groupBox2.Controls.Add(this.cbAggregatePosition);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkAggregateVisable);
            this.groupBox2.Controls.Add(this.label7);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tbAggregateColumns
            // 
            this.tbAggregateColumns.AllowDecimal = false;
            this.tbAggregateColumns.AllowNegative = false;
            this.tbAggregateColumns.CultureInfo = null;
            this.tbAggregateColumns.DecimalLetters = 0;
            this.tbAggregateColumns.ForeColor = System.Drawing.Color.Black;
            this.tbAggregateColumns.HasMinValue = false;
            resources.ApplyResources(this.tbAggregateColumns, "tbAggregateColumns");
            this.tbAggregateColumns.MaxValue = 100D;
            this.tbAggregateColumns.MinValue = 1D;
            this.tbAggregateColumns.Name = "tbAggregateColumns";
            this.tbAggregateColumns.Value = 1D;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbAggregateSize
            // 
            this.tbAggregateSize.AllowDecimal = false;
            this.tbAggregateSize.AllowNegative = false;
            this.tbAggregateSize.CultureInfo = null;
            this.tbAggregateSize.DecimalLetters = 0;
            this.tbAggregateSize.ForeColor = System.Drawing.Color.Black;
            this.tbAggregateSize.HasMinValue = false;
            resources.ApplyResources(this.tbAggregateSize, "tbAggregateSize");
            this.tbAggregateSize.MaxValue = 100D;
            this.tbAggregateSize.MinValue = 0D;
            this.tbAggregateSize.Name = "tbAggregateSize";
            this.tbAggregateSize.Value = 0D;
            this.tbAggregateSize.Leave += new System.EventHandler(this.UpdatePreview);
            // 
            // cbAggregatePosition
            // 
            this.cbAggregatePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAggregatePosition.FormattingEnabled = true;
            this.cbAggregatePosition.Items.AddRange(new object[] {
            resources.GetString("cbAggregatePosition.Items"),
            resources.GetString("cbAggregatePosition.Items1"),
            resources.GetString("cbAggregatePosition.Items2"),
            resources.GetString("cbAggregatePosition.Items3")});
            resources.ApplyResources(this.cbAggregatePosition, "cbAggregatePosition");
            this.cbAggregatePosition.Name = "cbAggregatePosition";
            this.cbAggregatePosition.SelectedIndexChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // chkAggregateVisable
            // 
            resources.ApplyResources(this.chkAggregateVisable, "chkAggregateVisable");
            this.chkAggregateVisable.Name = "chkAggregateVisable";
            this.chkAggregateVisable.UseVisualStyleBackColor = true;
            this.chkAggregateVisable.CheckedChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbButtonSize);
            this.groupBox3.Controls.Add(this.cmbButtonPosistion);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.chkButtonVisable);
            this.groupBox3.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbButtonSize
            // 
            this.tbButtonSize.AllowDecimal = false;
            this.tbButtonSize.AllowNegative = false;
            this.tbButtonSize.CultureInfo = null;
            this.tbButtonSize.DecimalLetters = 0;
            this.tbButtonSize.ForeColor = System.Drawing.Color.Black;
            this.tbButtonSize.HasMinValue = false;
            resources.ApplyResources(this.tbButtonSize, "tbButtonSize");
            this.tbButtonSize.MaxValue = 100D;
            this.tbButtonSize.MinValue = 0D;
            this.tbButtonSize.Name = "tbButtonSize";
            this.tbButtonSize.Value = 0D;
            this.tbButtonSize.Leave += new System.EventHandler(this.UpdatePreview);
            // 
            // cmbButtonPosistion
            // 
            this.cmbButtonPosistion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbButtonPosistion.FormattingEnabled = true;
            this.cmbButtonPosistion.Items.AddRange(new object[] {
            resources.GetString("cmbButtonPosistion.Items"),
            resources.GetString("cmbButtonPosistion.Items1"),
            resources.GetString("cmbButtonPosistion.Items2"),
            resources.GetString("cmbButtonPosistion.Items3")});
            resources.ApplyResources(this.cmbButtonPosistion, "cmbButtonPosistion");
            this.cmbButtonPosistion.Name = "cmbButtonPosistion";
            this.cmbButtonPosistion.SelectedIndexChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkButtonVisable
            // 
            resources.ApplyResources(this.chkButtonVisable, "chkButtonVisable");
            this.chkButtonVisable.Name = "chkButtonVisable";
            this.chkButtonVisable.UseVisualStyleBackColor = true;
            this.chkButtonVisable.CheckedChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbHistoryHorizon);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.tbHistoryRows);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.tbHistorySize);
            this.groupBox4.Controls.Add(this.cmbHistoryPosition);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.chkHistoryVisable);
            this.groupBox4.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbHistoryHorizon
            // 
            this.tbHistoryHorizon.AllowDecimal = false;
            this.tbHistoryHorizon.AllowNegative = false;
            this.tbHistoryHorizon.CultureInfo = null;
            this.tbHistoryHorizon.DecimalLetters = 0;
            this.tbHistoryHorizon.ForeColor = System.Drawing.Color.Black;
            this.tbHistoryHorizon.HasMinValue = false;
            resources.ApplyResources(this.tbHistoryHorizon, "tbHistoryHorizon");
            this.tbHistoryHorizon.MaxValue = 180D;
            this.tbHistoryHorizon.MinValue = 1D;
            this.tbHistoryHorizon.Name = "tbHistoryHorizon";
            this.tbHistoryHorizon.Value = 60D;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbHistoryRows
            // 
            this.tbHistoryRows.AllowDecimal = false;
            this.tbHistoryRows.AllowNegative = false;
            this.tbHistoryRows.CultureInfo = null;
            this.tbHistoryRows.DecimalLetters = 0;
            this.tbHistoryRows.ForeColor = System.Drawing.Color.Black;
            this.tbHistoryRows.HasMinValue = false;
            resources.ApplyResources(this.tbHistoryRows, "tbHistoryRows");
            this.tbHistoryRows.MaxValue = 100D;
            this.tbHistoryRows.MinValue = 1D;
            this.tbHistoryRows.Name = "tbHistoryRows";
            this.tbHistoryRows.Value = 5D;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbHistorySize
            // 
            this.tbHistorySize.AllowDecimal = false;
            this.tbHistorySize.AllowNegative = false;
            this.tbHistorySize.CultureInfo = null;
            this.tbHistorySize.DecimalLetters = 0;
            this.tbHistorySize.ForeColor = System.Drawing.Color.Black;
            this.tbHistorySize.HasMinValue = false;
            resources.ApplyResources(this.tbHistorySize, "tbHistorySize");
            this.tbHistorySize.MaxValue = 100D;
            this.tbHistorySize.MinValue = 0D;
            this.tbHistorySize.Name = "tbHistorySize";
            this.tbHistorySize.Value = 0D;
            this.tbHistorySize.Leave += new System.EventHandler(this.UpdatePreview);
            // 
            // cmbHistoryPosition
            // 
            this.cmbHistoryPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistoryPosition.FormattingEnabled = true;
            this.cmbHistoryPosition.Items.AddRange(new object[] {
            resources.GetString("cmbHistoryPosition.Items"),
            resources.GetString("cmbHistoryPosition.Items1")});
            resources.ApplyResources(this.cmbHistoryPosition, "cmbHistoryPosition");
            this.cmbHistoryPosition.Name = "cmbHistoryPosition";
            this.cmbHistoryPosition.SelectedIndexChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkHistoryVisable
            // 
            resources.ApplyResources(this.chkHistoryVisable, "chkHistoryVisable");
            this.chkHistoryVisable.Name = "chkHistoryVisable";
            this.chkHistoryVisable.UseVisualStyleBackColor = true;
            this.chkHistoryVisable.CheckedChanged += new System.EventHandler(this.UpdatePreview);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // pnlPreview
            // 
            resources.ApplyResources(this.pnlPreview, "pnlPreview");
            this.pnlPreview.ForeColor = System.Drawing.Color.Purple;
            this.pnlPreview.Name = "pnlPreview";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // VisualProfileLayoutPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "VisualProfileLayoutPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private LSOne.Controls.DualDataComboBox cmbHeaderProfile;
        private LSOne.Controls.ContextButtons btnsHeaderProfile;
        private System.Windows.Forms.Label lblKitchenDisplayProfile;
        private LSOne.Controls.NumericTextBox tbHeaderHeight;
        private System.Windows.Forms.Label lblHeaderHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private LSOne.Controls.NumericTextBox tbAggregateColumns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private LSOne.Controls.NumericTextBox tbAggregateSize;
        private System.Windows.Forms.ComboBox cbAggregatePosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAggregateVisable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private LSOne.Controls.NumericTextBox tbButtonSize;
        private System.Windows.Forms.ComboBox cmbButtonPosistion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkButtonVisable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private LSOne.Controls.NumericTextBox tbHistoryHorizon;
        private System.Windows.Forms.Label label12;
        private LSOne.Controls.NumericTextBox tbHistoryRows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private LSOne.Controls.NumericTextBox tbHistorySize;
        private System.Windows.Forms.ComboBox cmbHistoryPosition;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkHistoryVisable;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.Label label13;
    }
}
