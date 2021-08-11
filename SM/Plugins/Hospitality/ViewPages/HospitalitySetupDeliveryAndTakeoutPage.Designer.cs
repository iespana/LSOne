using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalitySetupDeliveryAndTakeoutPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalitySetupDeliveryAndTakeoutPage));
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
            this.ntbOrderProcessTimeMin = new NumericTextBox();
            this.cmbDeliverySalesType = new DualDataComboBox();
            this.cmbTakeoutSalesType = new DualDataComboBox();
            this.cmbPreOrderSalesType = new DualDataComboBox();
            this.chkPopulateDeliveryInfo = new System.Windows.Forms.CheckBox();
            this.chkAllowPreOrders = new System.Windows.Forms.CheckBox();
            this.chkDisplayTimeAtOrder = new System.Windows.Forms.CheckBox();
            this.ntbAdvPreOrdPrintMin = new NumericTextBox();
            this.cmbPosTerminalPrintPreOrders = new DualDataComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chkCloseTripOnDepart = new System.Windows.Forms.CheckBox();
            this.chkDelProgressStatusInUse = new System.Windows.Forms.CheckBox();
            this.ntbDaysDriverTripsExist = new NumericTextBox();
            this.txtTakeoutNoNameNo = new System.Windows.Forms.TextBox();
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
            // ntbOrderProcessTimeMin
            // 
            this.ntbOrderProcessTimeMin.AllowDecimal = false;
            this.ntbOrderProcessTimeMin.AllowNegative = false;
            this.ntbOrderProcessTimeMin.DecimalLetters = 2;
            this.ntbOrderProcessTimeMin.HasMinValue = false;
            resources.ApplyResources(this.ntbOrderProcessTimeMin, "ntbOrderProcessTimeMin");
            this.ntbOrderProcessTimeMin.MaxValue = 0D;
            this.ntbOrderProcessTimeMin.MinValue = 0D;
            this.ntbOrderProcessTimeMin.Name = "ntbOrderProcessTimeMin";
            this.ntbOrderProcessTimeMin.Value = 0D;
            // 
            // cmbDeliverySalesType
            // 
            resources.ApplyResources(this.cmbDeliverySalesType, "cmbDeliverySalesType");
            this.cmbDeliverySalesType.MaxLength = 32767;
            this.cmbDeliverySalesType.Name = "cmbDeliverySalesType";
            this.cmbDeliverySalesType.SelectedData = null;
            this.cmbDeliverySalesType.SkipIDColumn = true;
            this.cmbDeliverySalesType.RequestData += new System.EventHandler(this.cmbDeliverySalesType_RequestData);
            // 
            // cmbTakeoutSalesType
            // 
            resources.ApplyResources(this.cmbTakeoutSalesType, "cmbTakeoutSalesType");
            this.cmbTakeoutSalesType.MaxLength = 32767;
            this.cmbTakeoutSalesType.Name = "cmbTakeoutSalesType";
            this.cmbTakeoutSalesType.SelectedData = null;
            this.cmbTakeoutSalesType.SkipIDColumn = true;
            this.cmbTakeoutSalesType.RequestData += new System.EventHandler(this.cmbTakeoutSalesType_RequestData);
            // 
            // cmbPreOrderSalesType
            // 
            resources.ApplyResources(this.cmbPreOrderSalesType, "cmbPreOrderSalesType");
            this.cmbPreOrderSalesType.MaxLength = 32767;
            this.cmbPreOrderSalesType.Name = "cmbPreOrderSalesType";
            this.cmbPreOrderSalesType.SelectedData = null;
            this.cmbPreOrderSalesType.SkipIDColumn = true;
            this.cmbPreOrderSalesType.RequestData += new System.EventHandler(this.cmbPreOrderSalesType_RequestData);
            // 
            // chkPopulateDeliveryInfo
            // 
            resources.ApplyResources(this.chkPopulateDeliveryInfo, "chkPopulateDeliveryInfo");
            this.chkPopulateDeliveryInfo.Name = "chkPopulateDeliveryInfo";
            this.chkPopulateDeliveryInfo.UseVisualStyleBackColor = true;
            // 
            // chkAllowPreOrders
            // 
            resources.ApplyResources(this.chkAllowPreOrders, "chkAllowPreOrders");
            this.chkAllowPreOrders.Name = "chkAllowPreOrders";
            this.chkAllowPreOrders.UseVisualStyleBackColor = true;
            // 
            // chkDisplayTimeAtOrder
            // 
            resources.ApplyResources(this.chkDisplayTimeAtOrder, "chkDisplayTimeAtOrder");
            this.chkDisplayTimeAtOrder.Name = "chkDisplayTimeAtOrder";
            this.chkDisplayTimeAtOrder.UseVisualStyleBackColor = true;
            // 
            // ntbAdvPreOrdPrintMin
            // 
            this.ntbAdvPreOrdPrintMin.AllowDecimal = false;
            this.ntbAdvPreOrdPrintMin.AllowNegative = false;
            this.ntbAdvPreOrdPrintMin.DecimalLetters = 2;
            this.ntbAdvPreOrdPrintMin.HasMinValue = false;
            resources.ApplyResources(this.ntbAdvPreOrdPrintMin, "ntbAdvPreOrdPrintMin");
            this.ntbAdvPreOrdPrintMin.MaxValue = 0D;
            this.ntbAdvPreOrdPrintMin.MinValue = 0D;
            this.ntbAdvPreOrdPrintMin.Name = "ntbAdvPreOrdPrintMin";
            this.ntbAdvPreOrdPrintMin.Value = 0D;
            // 
            // cmbPosTerminalPrintPreOrders
            // 
            resources.ApplyResources(this.cmbPosTerminalPrintPreOrders, "cmbPosTerminalPrintPreOrders");
            this.cmbPosTerminalPrintPreOrders.MaxLength = 32767;
            this.cmbPosTerminalPrintPreOrders.Name = "cmbPosTerminalPrintPreOrders";
            this.cmbPosTerminalPrintPreOrders.SelectedData = null;
            this.cmbPosTerminalPrintPreOrders.SkipIDColumn = true;
            this.cmbPosTerminalPrintPreOrders.RequestData += new System.EventHandler(this.cmbPosTerminalPrintPreOrders_RequestData);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // chkCloseTripOnDepart
            // 
            resources.ApplyResources(this.chkCloseTripOnDepart, "chkCloseTripOnDepart");
            this.chkCloseTripOnDepart.Name = "chkCloseTripOnDepart";
            this.chkCloseTripOnDepart.UseVisualStyleBackColor = true;
            // 
            // chkDelProgressStatusInUse
            // 
            resources.ApplyResources(this.chkDelProgressStatusInUse, "chkDelProgressStatusInUse");
            this.chkDelProgressStatusInUse.Name = "chkDelProgressStatusInUse";
            this.chkDelProgressStatusInUse.UseVisualStyleBackColor = true;
            // 
            // ntbDaysDriverTripsExist
            // 
            this.ntbDaysDriverTripsExist.AllowDecimal = false;
            this.ntbDaysDriverTripsExist.AllowNegative = false;
            this.ntbDaysDriverTripsExist.DecimalLetters = 2;
            this.ntbDaysDriverTripsExist.HasMinValue = false;
            resources.ApplyResources(this.ntbDaysDriverTripsExist, "ntbDaysDriverTripsExist");
            this.ntbDaysDriverTripsExist.MaxValue = 0D;
            this.ntbDaysDriverTripsExist.MinValue = 0D;
            this.ntbDaysDriverTripsExist.Name = "ntbDaysDriverTripsExist";
            this.ntbDaysDriverTripsExist.Value = 0D;
            // 
            // txtTakeoutNoNameNo
            // 
            resources.ApplyResources(this.txtTakeoutNoNameNo, "txtTakeoutNoNameNo");
            this.txtTakeoutNoNameNo.Name = "txtTakeoutNoNameNo";
            // 
            // HospitalitySetupDeliveryAndTakeoutPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtTakeoutNoNameNo);
            this.Controls.Add(this.ntbDaysDriverTripsExist);
            this.Controls.Add(this.chkDelProgressStatusInUse);
            this.Controls.Add(this.chkCloseTripOnDepart);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbPosTerminalPrintPreOrders);
            this.Controls.Add(this.ntbAdvPreOrdPrintMin);
            this.Controls.Add(this.chkDisplayTimeAtOrder);
            this.Controls.Add(this.chkAllowPreOrders);
            this.Controls.Add(this.chkPopulateDeliveryInfo);
            this.Controls.Add(this.cmbPreOrderSalesType);
            this.Controls.Add(this.cmbTakeoutSalesType);
            this.Controls.Add(this.cmbDeliverySalesType);
            this.Controls.Add(this.ntbOrderProcessTimeMin);
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
            this.Name = "HospitalitySetupDeliveryAndTakeoutPage";
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
        private NumericTextBox ntbOrderProcessTimeMin;
        private DualDataComboBox cmbDeliverySalesType;
        private DualDataComboBox cmbTakeoutSalesType;
        private DualDataComboBox cmbPreOrderSalesType;
        private System.Windows.Forms.CheckBox chkPopulateDeliveryInfo;
        private System.Windows.Forms.CheckBox chkAllowPreOrders;
        private System.Windows.Forms.CheckBox chkDisplayTimeAtOrder;
        private NumericTextBox ntbAdvPreOrdPrintMin;
        private DualDataComboBox cmbPosTerminalPrintPreOrders;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkCloseTripOnDepart;
        private System.Windows.Forms.CheckBox chkDelProgressStatusInUse;
        private NumericTextBox ntbDaysDriverTripsExist;
        private System.Windows.Forms.TextBox txtTakeoutNoNameNo;
    }
}
