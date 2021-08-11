using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class DiningTableLayoutScreensDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiningTableLayoutScreensDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvScreens = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.ntbScreenNo = new LSOne.Controls.NumericTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblNumber = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsAddRemove = new LSOne.Controls.ContextButtons();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Name = "panel2";
            // 
            // lvScreens
            // 
            this.lvScreens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvScreens.FullRowSelect = true;
            this.lvScreens.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvScreens.HideSelection = false;
            resources.ApplyResources(this.lvScreens, "lvScreens");
            this.lvScreens.LockDrawing = false;
            this.lvScreens.MultiSelect = false;
            this.lvScreens.Name = "lvScreens";
            this.lvScreens.SortColumn = -1;
            this.lvScreens.SortedBackwards = false;
            this.lvScreens.UseCompatibleStateImageBehavior = false;
            this.lvScreens.UseEveryOtherRowColoring = true;
            this.lvScreens.View = System.Windows.Forms.View.Details;
            this.lvScreens.SelectedIndexChanged += new System.EventHandler(this.lvScreens_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.ntbScreenNo);
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnSave);
            this.groupPanel1.Controls.Add(this.lblDescription);
            this.groupPanel1.Controls.Add(this.tbDescription);
            this.groupPanel1.Controls.Add(this.lblNumber);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // ntbScreenNo
            // 
            this.ntbScreenNo.AllowDecimal = false;
            this.ntbScreenNo.AllowNegative = false;
            this.ntbScreenNo.CultureInfo = null;
            this.ntbScreenNo.DecimalLetters = 2;
            this.ntbScreenNo.ForeColor = System.Drawing.Color.Black;
            this.ntbScreenNo.HasMinValue = false;
            resources.ApplyResources(this.ntbScreenNo, "ntbScreenNo");
            this.ntbScreenNo.MaxValue = 0D;
            this.ntbScreenNo.MinValue = 0D;
            this.ntbScreenNo.Name = "ntbScreenNo";
            this.ntbScreenNo.Value = 0D;
            this.ntbScreenNo.TextChanged += new System.EventHandler(this.tbID_TextChanged);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckSaveEnabled);
            // 
            // lblNumber
            // 
            this.lblNumber.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblNumber, "lblNumber");
            this.lblNumber.Name = "lblNumber";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsAddRemove.EditButtonEnabled = false;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = false;
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.btnsAddRemove_AddButtonClicked);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsAddRemove_RemoveButtonClicked);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // DiningTableLayoutScreensDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsAddRemove);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.lvScreens);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "DiningTableLayoutScreensDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvScreens, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.btnsAddRemove, 0);
            this.panel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private ExtendedListView lvScreens;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Label lblNoSelection;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbScreenNo;
    }
}