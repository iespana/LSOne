using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class PosColorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosColorDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvColors = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new GroupPanel();
            this.colorWellColorValue = new ColorWell();
            this.lblColorValue = new System.Windows.Forms.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.lblBold = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsAddRemove = new ContextButtons();
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
            // lvColors
            // 
            this.lvColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvColors.FullRowSelect = true;
            this.lvColors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvColors.HideSelection = false;
            resources.ApplyResources(this.lvColors, "lvColors");
            this.lvColors.LockDrawing = false;
            this.lvColors.MultiSelect = false;
            this.lvColors.Name = "lvColors";
            this.lvColors.SortColumn = -1;
            this.lvColors.SortedBackwards = false;
            this.lvColors.UseCompatibleStateImageBehavior = false;
            this.lvColors.UseEveryOtherRowColoring = true;
            this.lvColors.View = System.Windows.Forms.View.Details;
            this.lvColors.SelectedIndexChanged += new System.EventHandler(this.lvColors_SelectedIndexChanged);
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
            this.groupPanel1.Controls.Add(this.colorWellColorValue);
            this.groupPanel1.Controls.Add(this.lblColorValue);
            this.groupPanel1.Controls.Add(this.chkBold);
            this.groupPanel1.Controls.Add(this.lblBold);
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnSave);
            this.groupPanel1.Controls.Add(this.lblName);
            this.groupPanel1.Controls.Add(this.tbName);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // colorWellColorValue
            // 
            resources.ApplyResources(this.colorWellColorValue, "colorWellColorValue");
            this.colorWellColorValue.Name = "colorWellColorValue";
            this.colorWellColorValue.SelectedColor = System.Drawing.Color.White;
            this.colorWellColorValue.SelectedColorChanged += new System.EventHandler(this.CheckSaveEnabled);
            this.colorWellColorValue.Load += new System.EventHandler(this.colorWellColorValue_Load);
            // 
            // lblColorValue
            // 
            this.lblColorValue.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblColorValue, "lblColorValue");
            this.lblColorValue.Name = "lblColorValue";
            // 
            // chkBold
            // 
            resources.ApplyResources(this.chkBold, "chkBold");
            this.chkBold.BackColor = System.Drawing.Color.Transparent;
            this.chkBold.Name = "chkBold";
            this.chkBold.UseVisualStyleBackColor = false;
            this.chkBold.CheckedChanged += new System.EventHandler(this.CheckSaveEnabled);
            // 
            // lblBold
            // 
            this.lblBold.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBold, "lblBold");
            this.lblBold.Name = "lblBold";
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
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // tbName
            // 
            resources.ApplyResources(this.tbName, "tbName");
            this.tbName.Name = "tbName";
            this.tbName.TextChanged += new System.EventHandler(this.CheckSaveEnabled);
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
            this.btnsAddRemove.Context = ButtonTypes.AddRemove;
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
            // PosColorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsAddRemove);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.lvColors);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "PosColorDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvColors, 0);
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
        private ExtendedListView lvColors;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblNoSelection;
        private ColorWell colorWellColorValue;
        private System.Windows.Forms.Label lblColorValue;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.Label lblBold;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}