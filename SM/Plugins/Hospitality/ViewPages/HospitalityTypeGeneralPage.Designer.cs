using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityTypeGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeGeneralPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbOverview = new System.Windows.Forms.ComboBox();
            this.cmbStaffTakeoverInTransaction = new System.Windows.Forms.ComboBox();
            this.cmbManagerTakeoverInTransaction = new System.Windows.Forms.ComboBox();
            this.chkStayInPosAfterTransaction = new System.Windows.Forms.CheckBox();
            this.cmbStationPrinting = new System.Windows.Forms.ComboBox();
            this.chkPrintTrainingTransaction = new System.Windows.Forms.CheckBox();
            this.btnEditTouchButtons = new LSOne.Controls.ContextButton();
            this.btnsEditAddTopPosMenu = new LSOne.Controls.ContextButtons();
            this.btnsEditAddPosLogonMenu = new LSOne.Controls.ContextButtons();
            this.chkDefaultType = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbAccessToOtherRestaurant = new LSOne.Controls.DualDataComboBox();
            this.cmbTouchButtonsID = new LSOne.Controls.DualDataComboBox();
            this.cmbTopPosMenuID = new LSOne.Controls.DualDataComboBox();
            this.cmbPosLogonMenuID = new LSOne.Controls.DualDataComboBox();
            this.chkSendVoidedItemsToStation = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkSendTransfersToStation = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkSendSuspensionsToStation = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.chkSendOrderNoToStation = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbOverview
            // 
            this.cmbOverview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbOverview, "cmbOverview");
            this.cmbOverview.FormattingEnabled = true;
            this.cmbOverview.Items.AddRange(new object[] {
            resources.GetString("cmbOverview.Items"),
            resources.GetString("cmbOverview.Items1")});
            this.cmbOverview.Name = "cmbOverview";
            this.cmbOverview.TabStop = false;
            this.cmbOverview.SelectedIndexChanged += new System.EventHandler(this.cmbOverview_SelectedIndexChanged);
            // 
            // cmbStaffTakeoverInTransaction
            // 
            this.cmbStaffTakeoverInTransaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStaffTakeoverInTransaction.FormattingEnabled = true;
            this.cmbStaffTakeoverInTransaction.Items.AddRange(new object[] {
            resources.GetString("cmbStaffTakeoverInTransaction.Items"),
            resources.GetString("cmbStaffTakeoverInTransaction.Items1"),
            resources.GetString("cmbStaffTakeoverInTransaction.Items2")});
            resources.ApplyResources(this.cmbStaffTakeoverInTransaction, "cmbStaffTakeoverInTransaction");
            this.cmbStaffTakeoverInTransaction.Name = "cmbStaffTakeoverInTransaction";
            // 
            // cmbManagerTakeoverInTransaction
            // 
            this.cmbManagerTakeoverInTransaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbManagerTakeoverInTransaction, "cmbManagerTakeoverInTransaction");
            this.cmbManagerTakeoverInTransaction.FormattingEnabled = true;
            this.cmbManagerTakeoverInTransaction.Items.AddRange(new object[] {
            resources.GetString("cmbManagerTakeoverInTransaction.Items"),
            resources.GetString("cmbManagerTakeoverInTransaction.Items1"),
            resources.GetString("cmbManagerTakeoverInTransaction.Items2")});
            this.cmbManagerTakeoverInTransaction.Name = "cmbManagerTakeoverInTransaction";
            this.cmbManagerTakeoverInTransaction.TabStop = false;
            // 
            // chkStayInPosAfterTransaction
            // 
            resources.ApplyResources(this.chkStayInPosAfterTransaction, "chkStayInPosAfterTransaction");
            this.chkStayInPosAfterTransaction.Name = "chkStayInPosAfterTransaction";
            this.chkStayInPosAfterTransaction.UseVisualStyleBackColor = true;
            // 
            // cmbStationPrinting
            // 
            this.cmbStationPrinting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationPrinting.FormattingEnabled = true;
            this.cmbStationPrinting.Items.AddRange(new object[] {
            resources.GetString("cmbStationPrinting.Items"),
            resources.GetString("cmbStationPrinting.Items1"),
            resources.GetString("cmbStationPrinting.Items2"),
            resources.GetString("cmbStationPrinting.Items3"),
            resources.GetString("cmbStationPrinting.Items4"),
            resources.GetString("cmbStationPrinting.Items5")});
            resources.ApplyResources(this.cmbStationPrinting, "cmbStationPrinting");
            this.cmbStationPrinting.Name = "cmbStationPrinting";
            // 
            // chkPrintTrainingTransaction
            // 
            resources.ApplyResources(this.chkPrintTrainingTransaction, "chkPrintTrainingTransaction");
            this.chkPrintTrainingTransaction.Name = "chkPrintTrainingTransaction";
            this.chkPrintTrainingTransaction.TabStop = false;
            this.chkPrintTrainingTransaction.UseVisualStyleBackColor = true;
            // 
            // btnEditTouchButtons
            // 
            this.btnEditTouchButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnEditTouchButtons.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditTouchButtons, "btnEditTouchButtons");
            this.btnEditTouchButtons.Name = "btnEditTouchButtons";
            this.btnEditTouchButtons.Click += new System.EventHandler(this.btnEditVisualProfile_Click);
            // 
            // btnsEditAddTopPosMenu
            // 
            this.btnsEditAddTopPosMenu.AddButtonEnabled = true;
            this.btnsEditAddTopPosMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddTopPosMenu.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddTopPosMenu.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddTopPosMenu, "btnsEditAddTopPosMenu");
            this.btnsEditAddTopPosMenu.Name = "btnsEditAddTopPosMenu";
            this.btnsEditAddTopPosMenu.RemoveButtonEnabled = false;
            this.btnsEditAddTopPosMenu.EditButtonClicked += new System.EventHandler(this.btnsEditAddTopPosMenu_EditButtonClicked);
            this.btnsEditAddTopPosMenu.AddButtonClicked += new System.EventHandler(this.btnsEditAddTopPosMenu_AddButtonClicked);
            this.btnsEditAddTopPosMenu.Load += new System.EventHandler(this.btnsEditAddTopPosMenu_Load);
            // 
            // btnsEditAddPosLogonMenu
            // 
            this.btnsEditAddPosLogonMenu.AddButtonEnabled = true;
            this.btnsEditAddPosLogonMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddPosLogonMenu.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddPosLogonMenu.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddPosLogonMenu, "btnsEditAddPosLogonMenu");
            this.btnsEditAddPosLogonMenu.Name = "btnsEditAddPosLogonMenu";
            this.btnsEditAddPosLogonMenu.RemoveButtonEnabled = false;
            this.btnsEditAddPosLogonMenu.EditButtonClicked += new System.EventHandler(this.btnsEditAddPosLogonMenu_EditButtonClicked);
            this.btnsEditAddPosLogonMenu.AddButtonClicked += new System.EventHandler(this.btnsEditAddPosLogonMenu_AddButtonClicked);
            // 
            // chkDefaultType
            // 
            resources.ApplyResources(this.chkDefaultType, "chkDefaultType");
            this.chkDefaultType.Name = "chkDefaultType";
            this.chkDefaultType.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbAccessToOtherRestaurant
            // 
            this.cmbAccessToOtherRestaurant.AddList = null;
            this.cmbAccessToOtherRestaurant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAccessToOtherRestaurant, "cmbAccessToOtherRestaurant");
            this.cmbAccessToOtherRestaurant.MaxLength = 32767;
            this.cmbAccessToOtherRestaurant.Name = "cmbAccessToOtherRestaurant";
            this.cmbAccessToOtherRestaurant.NoChangeAllowed = false;
            this.cmbAccessToOtherRestaurant.OnlyDisplayID = false;
            this.cmbAccessToOtherRestaurant.RemoveList = null;
            this.cmbAccessToOtherRestaurant.RowHeight = ((short)(22));
            this.cmbAccessToOtherRestaurant.SecondaryData = null;
            this.cmbAccessToOtherRestaurant.SelectedData = null;
            this.cmbAccessToOtherRestaurant.SelectedDataID = null;
            this.cmbAccessToOtherRestaurant.SelectionList = null;
            this.cmbAccessToOtherRestaurant.SkipIDColumn = true;
            this.cmbAccessToOtherRestaurant.TabStop = false;
            this.cmbAccessToOtherRestaurant.RequestData += new System.EventHandler(this.cmbAccessToOtherRestaurant_RequestData);
            this.cmbAccessToOtherRestaurant.SelectedDataChanged += new System.EventHandler(this.cmbAccessToOtherRestaurant_SelectedDataChanged);
            this.cmbAccessToOtherRestaurant.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbTouchButtonsID
            // 
            this.cmbTouchButtonsID.AddList = null;
            this.cmbTouchButtonsID.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTouchButtonsID, "cmbTouchButtonsID");
            this.cmbTouchButtonsID.MaxLength = 32767;
            this.cmbTouchButtonsID.Name = "cmbTouchButtonsID";
            this.cmbTouchButtonsID.NoChangeAllowed = false;
            this.cmbTouchButtonsID.OnlyDisplayID = false;
            this.cmbTouchButtonsID.RemoveList = null;
            this.cmbTouchButtonsID.RowHeight = ((short)(22));
            this.cmbTouchButtonsID.SecondaryData = null;
            this.cmbTouchButtonsID.SelectedData = null;
            this.cmbTouchButtonsID.SelectedDataID = null;
            this.cmbTouchButtonsID.SelectionList = null;
            this.cmbTouchButtonsID.SkipIDColumn = true;
            this.cmbTouchButtonsID.RequestData += new System.EventHandler(this.cmbVisualProfile_RequestData);
            this.cmbTouchButtonsID.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbTopPosMenuID
            // 
            this.cmbTopPosMenuID.AddList = null;
            this.cmbTopPosMenuID.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTopPosMenuID, "cmbTopPosMenuID");
            this.cmbTopPosMenuID.MaxLength = 32767;
            this.cmbTopPosMenuID.Name = "cmbTopPosMenuID";
            this.cmbTopPosMenuID.NoChangeAllowed = false;
            this.cmbTopPosMenuID.OnlyDisplayID = false;
            this.cmbTopPosMenuID.RemoveList = null;
            this.cmbTopPosMenuID.RowHeight = ((short)(22));
            this.cmbTopPosMenuID.SecondaryData = null;
            this.cmbTopPosMenuID.SelectedData = null;
            this.cmbTopPosMenuID.SelectedDataID = null;
            this.cmbTopPosMenuID.SelectionList = null;
            this.cmbTopPosMenuID.SkipIDColumn = true;
            this.cmbTopPosMenuID.RequestData += new System.EventHandler(this.cmbTopPosMenuID_RequestData);
            this.cmbTopPosMenuID.SelectedDataChanged += new System.EventHandler(this.cmbTopPosMenuID_SelectedDataChanged);
            this.cmbTopPosMenuID.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // cmbPosLogonMenuID
            // 
            this.cmbPosLogonMenuID.AddList = null;
            this.cmbPosLogonMenuID.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPosLogonMenuID, "cmbPosLogonMenuID");
            this.cmbPosLogonMenuID.MaxLength = 32767;
            this.cmbPosLogonMenuID.Name = "cmbPosLogonMenuID";
            this.cmbPosLogonMenuID.NoChangeAllowed = false;
            this.cmbPosLogonMenuID.OnlyDisplayID = false;
            this.cmbPosLogonMenuID.RemoveList = null;
            this.cmbPosLogonMenuID.RowHeight = ((short)(22));
            this.cmbPosLogonMenuID.SecondaryData = null;
            this.cmbPosLogonMenuID.SelectedData = null;
            this.cmbPosLogonMenuID.SelectedDataID = null;
            this.cmbPosLogonMenuID.SelectionList = null;
            this.cmbPosLogonMenuID.SkipIDColumn = true;
            this.cmbPosLogonMenuID.RequestData += new System.EventHandler(this.cmbPosLogonMenuID_RequestData);
            this.cmbPosLogonMenuID.SelectedDataChanged += new System.EventHandler(this.cmbPosLogonMenuID_SelectedDataChanged);
            this.cmbPosLogonMenuID.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // chkSendVoidedItemsToStation
            // 
            resources.ApplyResources(this.chkSendVoidedItemsToStation, "chkSendVoidedItemsToStation");
            this.chkSendVoidedItemsToStation.Name = "chkSendVoidedItemsToStation";
            this.chkSendVoidedItemsToStation.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // chkSendTransfersToStation
            // 
            resources.ApplyResources(this.chkSendTransfersToStation, "chkSendTransfersToStation");
            this.chkSendTransfersToStation.Name = "chkSendTransfersToStation";
            this.chkSendTransfersToStation.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // chkSendSuspensionsToStation
            // 
            resources.ApplyResources(this.chkSendSuspensionsToStation, "chkSendSuspensionsToStation");
            this.chkSendSuspensionsToStation.Name = "chkSendSuspensionsToStation";
            this.chkSendSuspensionsToStation.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // chkSendOrderNoToStation
            // 
            resources.ApplyResources(this.chkSendOrderNoToStation, "chkSendOrderNoToStation");
            this.chkSendOrderNoToStation.Name = "chkSendOrderNoToStation";
            this.chkSendOrderNoToStation.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // HospitalityTypeGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkSendOrderNoToStation);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chkSendSuspensionsToStation);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.chkSendTransfersToStation);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chkSendVoidedItemsToStation);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkDefaultType);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnsEditAddPosLogonMenu);
            this.Controls.Add(this.btnsEditAddTopPosMenu);
            this.Controls.Add(this.btnEditTouchButtons);
            this.Controls.Add(this.chkPrintTrainingTransaction);
            this.Controls.Add(this.cmbStationPrinting);
            this.Controls.Add(this.chkStayInPosAfterTransaction);
            this.Controls.Add(this.cmbPosLogonMenuID);
            this.Controls.Add(this.cmbTopPosMenuID);
            this.Controls.Add(this.cmbTouchButtonsID);
            this.Controls.Add(this.cmbManagerTakeoverInTransaction);
            this.Controls.Add(this.cmbStaffTakeoverInTransaction);
            this.Controls.Add(this.cmbAccessToOtherRestaurant);
            this.Controls.Add(this.cmbOverview);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityTypeGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbOverview;
        private DualDataComboBox cmbAccessToOtherRestaurant;
        private System.Windows.Forms.ComboBox cmbStaffTakeoverInTransaction;
        private System.Windows.Forms.ComboBox cmbManagerTakeoverInTransaction;
        private DualDataComboBox cmbTouchButtonsID;
        private DualDataComboBox cmbTopPosMenuID;
        private DualDataComboBox cmbPosLogonMenuID;
        private System.Windows.Forms.CheckBox chkStayInPosAfterTransaction;
        private System.Windows.Forms.ComboBox cmbStationPrinting;
        private System.Windows.Forms.CheckBox chkPrintTrainingTransaction;
        private ContextButton btnEditTouchButtons;
        private ContextButtons btnsEditAddTopPosMenu;
        private ContextButtons btnsEditAddPosLogonMenu;
        private System.Windows.Forms.CheckBox chkDefaultType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkSendVoidedItemsToStation;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkSendTransfersToStation;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkSendSuspensionsToStation;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkSendOrderNoToStation;
        private System.Windows.Forms.Label label15;
    }
}
