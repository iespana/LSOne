using System.Windows.Forms;
using DevExpress.XtraEditors;
using LSOne.POS.Processes.WinControls;

namespace LSOne.Services.EFT.Common.Keyboard
{
    public partial class frmPayCard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayCard));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.lblExpirationDate = new System.Windows.Forms.Label();
            this.lblAmountToPay = new System.Windows.Forms.Label();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layAmount = new DevExpress.XtraLayout.LayoutControlItem();
            this.layExpireDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layCardNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.stringPad1 = new StringPad();
            this.lblCard = new System.Windows.Forms.Label();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layExpireDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layCardNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // keyboardButtonControl
            // 
            resources.ApplyResources(this.keyboardButtonControl, "keyboardButtonControl");
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl2);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.layoutControl1);
            this.panelControl2.Controls.Add(this.stringPad1);
            this.panelControl2.Controls.Add(this.lblCard);
            this.panelControl2.Controls.Add(this.pictureEdit1);
            resources.ApplyResources(this.panelControl2, "panelControl2");
            this.panelControl2.Name = "panelControl2";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.lblCardNumber);
            this.layoutControl1.Controls.Add(this.lblExpirationDate);
            this.layoutControl1.Controls.Add(this.lblAmountToPay);
            this.layoutControl1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.TabStop = false;
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lblCardNumber, "lblCardNumber");
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Tag = "H1";
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lblExpirationDate, "lblExpirationDate");
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Tag = "H1";
            // 
            // lblAmountToPay
            // 
            this.lblAmountToPay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lblAmountToPay, "lblAmountToPay");
            this.lblAmountToPay.Name = "lblAmountToPay";
            this.lblAmountToPay.Tag = "H1";
            // 
            // layoutControlItem2
            // 
            resources.ApplyResources(this.layoutControlItem2, "layoutControlItem2");
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(50, 20);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlGroup1
            // 
            resources.ApplyResources(this.layoutControlGroup1, "layoutControlGroup1");
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layAmount,
            this.layExpireDate,
            this.layCardNumber});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(238, 120);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layAmount
            // 
            this.layAmount.Control = this.lblAmountToPay;
            resources.ApplyResources(this.layAmount, "layAmount");
            this.layAmount.Location = new System.Drawing.Point(0, 79);
            this.layAmount.Name = "layAmount";
            this.layAmount.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layAmount.Size = new System.Drawing.Size(236, 39);
            this.layAmount.TextSize = new System.Drawing.Size(62, 13);
            // 
            // layExpireDate
            // 
            this.layExpireDate.Control = this.lblExpirationDate;
            resources.ApplyResources(this.layExpireDate, "layExpireDate");
            this.layExpireDate.Location = new System.Drawing.Point(0, 39);
            this.layExpireDate.Name = "layoutControlItem2";
            this.layExpireDate.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layExpireDate.Size = new System.Drawing.Size(236, 40);
            this.layExpireDate.TextSize = new System.Drawing.Size(62, 13);
            // 
            // layCardNumber
            // 
            this.layCardNumber.Control = this.lblCardNumber;
            resources.ApplyResources(this.layCardNumber, "layCardNumber");
            this.layCardNumber.Location = new System.Drawing.Point(0, 0);
            this.layCardNumber.Name = "layCardNumber";
            this.layCardNumber.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layCardNumber.Size = new System.Drawing.Size(236, 39);
            this.layCardNumber.TextSize = new System.Drawing.Size(62, 13);
            // 
            // stringPad1
            // 
            this.stringPad1.EnteredValue = "";
            this.stringPad1.EntryType = LSOne.POS.Processes.Enums.StringPadEntryTypes.BarcodeOrQuantity;
            this.stringPad1.Location = new System.Drawing.Point(103, 98);
            this.stringPad1.MaskChar = "";
            this.stringPad1.MaskInterval = 0;
            this.stringPad1.MaxNumberOfDigits = 20;
            this.stringPad1.Name = "stringPad1";
            this.stringPad1.NegativeMode = false;
            this.stringPad1.NumberOfDecimals = 2;
            this.stringPad1.ShortcutKeysActive = false;
            this.stringPad1.Size = new System.Drawing.Size(345, 86);
            this.stringPad1.TabIndex = 39;
            this.stringPad1.TimerEnabled = true;
            this.stringPad1.EnterButtonPressed += new StringPad.EnterButtonHandler(this.StringPad1_EnterButtonPressed);
            this.stringPad1.CardSwept += new StringPad.CardSwipedHandler(this.StringPad1_CardSwept);
            this.stringPad1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAmount_KeyDown_1);
            // 
            // lblCard
            // 
            resources.ApplyResources(this.lblCard, "lblCard");
            this.lblCard.Name = "lblCard";
            // 
            // pictureEdit1
            // 
            resources.ApplyResources(this.pictureEdit1, "pictureEdit1");
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.EditValueChanged += new System.EventHandler(this.pictureEdit1_EditValueChanged);
            // 
            // frmPayCard
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelControl1);
            this.Name = "frmPayCard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPayCard_FormClosed);
            this.Load += new System.EventHandler(this.frmPayCard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAmount_KeyDown_1);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layExpireDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layCardNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private PanelControl panelControl1;
        private PictureEdit pictureEdit1;
        private Label lblCard;

        private AmountViewer amtCardAmounts = new AmountViewer();
        private StringPad stringPad1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private Label lblCardNumber;
        private Label lblExpirationDate;
        private Label lblAmountToPay;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layAmount;
        private DevExpress.XtraLayout.LayoutControlItem layExpireDate;
        private DevExpress.XtraLayout.LayoutControlItem layCardNumber;
        private PanelControl panelControl2;

    }
}
