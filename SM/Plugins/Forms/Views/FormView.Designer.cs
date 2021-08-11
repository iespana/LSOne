using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Forms.Views
{
    partial class FormView
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormView));
			this.label2 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.imgDelete = new System.Windows.Forms.PictureBox();
			this.imgRevert = new System.Windows.Forms.PictureBox();
			this.imgClose = new System.Windows.Forms.PictureBox();
			this.lnkDelete = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkClose = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkRevert = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkSave = new LSOne.Controls.ExtendedLinkLabel();
			this.imgSave = new System.Windows.Forms.PictureBox();
			this.spinSlipLineCount = new DevExpress.XtraEditors.SpinEdit();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.chkIsSlip = new System.Windows.Forms.CheckBox();
			this.reportDesigner = new LSOne.Controls.ReportDesign();
			this.label5 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnEditDeviceName = new LSOne.Controls.ContextButton();
			this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
			this.chkWindowsPrinter = new System.Windows.Forms.CheckBox();
			this.optAskCustomer = new System.Windows.Forms.RadioButton();
			this.optNotPrint = new System.Windows.Forms.RadioButton();
			this.optAlwaysPrint = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.pnlBottom.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgDelete)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRevert)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgSave)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinSlipLineCount.Properties)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.panel1);
			this.pnlBottom.Controls.Add(this.reportDesigner);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.tableLayoutPanel1);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.spinSlipLineCount);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.chkIsSlip);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.label2);
			this.pnlBottom.Controls.Add(this.label4);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.Controls.Add(this.imgDelete, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.imgRevert, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.imgClose, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkDelete, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.lnkClose, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkRevert, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkSave, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.imgSave, 0, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// imgDelete
			// 
			resources.ApplyResources(this.imgDelete, "imgDelete");
			this.imgDelete.Name = "imgDelete";
			this.imgDelete.TabStop = false;
			// 
			// imgRevert
			// 
			resources.ApplyResources(this.imgRevert, "imgRevert");
			this.imgRevert.Name = "imgRevert";
			this.imgRevert.TabStop = false;
			// 
			// imgClose
			// 
			resources.ApplyResources(this.imgClose, "imgClose");
			this.imgClose.Name = "imgClose";
			this.imgClose.TabStop = false;
			// 
			// lnkDelete
			// 
			resources.ApplyResources(this.lnkDelete, "lnkDelete");
			this.lnkDelete.BackColor = System.Drawing.Color.Transparent;
			this.lnkDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkDelete.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkDelete.Name = "lnkDelete";
			this.lnkDelete.TabStop = true;
			this.lnkDelete.Click += new System.EventHandler(this.lnkDelete_LinkClicked);
			// 
			// lnkClose
			// 
			resources.ApplyResources(this.lnkClose, "lnkClose");
			this.lnkClose.BackColor = System.Drawing.Color.Transparent;
			this.lnkClose.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkClose.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkClose.Name = "lnkClose";
			this.lnkClose.TabStop = true;
			this.lnkClose.Click += new System.EventHandler(this.lnkClose_LinkClicked);
			// 
			// lnkRevert
			// 
			resources.ApplyResources(this.lnkRevert, "lnkRevert");
			this.lnkRevert.BackColor = System.Drawing.Color.Transparent;
			this.lnkRevert.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkRevert.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkRevert.Name = "lnkRevert";
			this.lnkRevert.TabStop = true;
			this.lnkRevert.Click += new System.EventHandler(this.lnkRevert_LinkClicked);
			// 
			// lnkSave
			// 
			resources.ApplyResources(this.lnkSave, "lnkSave");
			this.lnkSave.BackColor = System.Drawing.Color.Transparent;
			this.lnkSave.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkSave.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkSave.Name = "lnkSave";
			this.lnkSave.TabStop = true;
			this.lnkSave.Click += new System.EventHandler(this.lnkSave_LinkClicked);
			// 
			// imgSave
			// 
			resources.ApplyResources(this.imgSave, "imgSave");
			this.imgSave.Name = "imgSave";
			this.imgSave.TabStop = false;
			// 
			// spinSlipLineCount
			// 
			resources.ApplyResources(this.spinSlipLineCount, "spinSlipLineCount");
			this.spinSlipLineCount.Name = "spinSlipLineCount";
			// 
			// 
			// 
			this.spinSlipLineCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinSlipLineCount.Properties.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// chkIsSlip
			// 
			resources.ApplyResources(this.chkIsSlip, "chkIsSlip");
			this.chkIsSlip.BackColor = System.Drawing.Color.Transparent;
			this.chkIsSlip.Name = "chkIsSlip";
			this.chkIsSlip.UseVisualStyleBackColor = false;
			// 
			// reportDesigner
			// 
			resources.ApplyResources(this.reportDesigner, "reportDesigner");
			this.reportDesigner.FormID = null;
			this.reportDesigner.Name = "reportDesigner";
			this.reportDesigner.ReceiptChanged = false;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(245)))), ((int)(((byte)(241)))));
			this.panel1.Controls.Add(this.btnEditDeviceName);
			this.panel1.Controls.Add(this.cmbDeviceName);
			this.panel1.Controls.Add(this.chkWindowsPrinter);
			this.panel1.Controls.Add(this.optAskCustomer);
			this.panel1.Controls.Add(this.optNotPrint);
			this.panel1.Controls.Add(this.optAlwaysPrint);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Name = "panel1";
			// 
			// btnEditDeviceName
			// 
			this.btnEditDeviceName.BackColor = System.Drawing.Color.Transparent;
			this.btnEditDeviceName.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditDeviceName, "btnEditDeviceName");
			this.btnEditDeviceName.Name = "btnEditDeviceName";
			this.btnEditDeviceName.Click += new System.EventHandler(this.BtnEditDeviceName_Click);
			// 
			// cmbDeviceName
			// 
			this.cmbDeviceName.AddList = null;
			this.cmbDeviceName.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
			this.cmbDeviceName.MaxLength = 32767;
			this.cmbDeviceName.Name = "cmbDeviceName";
			this.cmbDeviceName.NoChangeAllowed = false;
			this.cmbDeviceName.OnlyDisplayID = false;
			this.cmbDeviceName.RemoveList = null;
			this.cmbDeviceName.RowHeight = ((short)(22));
			this.cmbDeviceName.SecondaryData = null;
			this.cmbDeviceName.SelectedData = null;
			this.cmbDeviceName.SelectedDataID = null;
			this.cmbDeviceName.SelectionList = null;
			this.cmbDeviceName.SkipIDColumn = true;
			this.cmbDeviceName.RequestData += new System.EventHandler(this.CmbDeviceName_RequestData);
			this.cmbDeviceName.SelectedDataChanged += new System.EventHandler(this.CmbDeviceName_SelectedDataChanged);
			this.cmbDeviceName.RequestClear += new System.EventHandler(this.CmbDeviceName_RequestClear);
			this.cmbDeviceName.SelectedDataCleared += new System.EventHandler(this.CmbDeviceName_SelectedDataCleared);
			// 
			// chkWindowsPrinter
			// 
			resources.ApplyResources(this.chkWindowsPrinter, "chkWindowsPrinter");
			this.chkWindowsPrinter.Name = "chkWindowsPrinter";
			this.chkWindowsPrinter.UseVisualStyleBackColor = true;
			this.chkWindowsPrinter.CheckedChanged += new System.EventHandler(this.chkWindowsPrinter_CheckedChanged);
			// 
			// optAskCustomer
			// 
			resources.ApplyResources(this.optAskCustomer, "optAskCustomer");
			this.optAskCustomer.Name = "optAskCustomer";
			this.optAskCustomer.TabStop = true;
			this.optAskCustomer.UseVisualStyleBackColor = true;
			// 
			// optNotPrint
			// 
			resources.ApplyResources(this.optNotPrint, "optNotPrint");
			this.optNotPrint.Name = "optNotPrint";
			this.optNotPrint.TabStop = true;
			this.optNotPrint.UseVisualStyleBackColor = true;
			// 
			// optAlwaysPrint
			// 
			resources.ApplyResources(this.optAlwaysPrint, "optAlwaysPrint");
			this.optAlwaysPrint.Name = "optAlwaysPrint";
			this.optAlwaysPrint.TabStop = true;
			this.optAlwaysPrint.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// FormView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 60;
			this.Name = "FormView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgDelete)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRevert)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgSave)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinSlipLineCount.Properties)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox imgDelete;
        private System.Windows.Forms.PictureBox imgRevert;
        private System.Windows.Forms.PictureBox imgClose;
        private ExtendedLinkLabel lnkDelete;
        private ExtendedLinkLabel lnkClose;
        private ExtendedLinkLabel lnkRevert;
        private ExtendedLinkLabel lnkSave;
        private System.Windows.Forms.PictureBox imgSave;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit spinSlipLineCount;
        private System.Windows.Forms.CheckBox chkIsSlip;
        private System.Windows.Forms.Label label4;
        private ReportDesign reportDesigner;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkWindowsPrinter;
        private System.Windows.Forms.RadioButton optAskCustomer;
        private System.Windows.Forms.RadioButton optNotPrint;
        private System.Windows.Forms.RadioButton optAlwaysPrint;
        private DualDataComboBox cmbDeviceName;
        private ContextButton btnEditDeviceName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
