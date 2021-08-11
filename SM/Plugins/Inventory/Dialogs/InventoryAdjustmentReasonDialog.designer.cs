using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class InventoryAdjustmentReasonDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryAdjustmentReasonDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsAddEdit = new LSOne.Controls.ContextButtons();
            this.lvReasons = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Name = "panel2";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnSave);
            this.groupPanel1.Controls.Add(this.lblName);
            this.groupPanel1.Controls.Add(this.tbDescription);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckSaveEnabled);
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnsAddEdit
            // 
            this.btnsAddEdit.AddButtonEnabled = true;
            this.btnsAddEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddEdit.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsAddEdit.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsAddEdit, "btnsAddEdit");
            this.btnsAddEdit.Name = "btnsAddEdit";
            this.btnsAddEdit.RemoveButtonEnabled = false;
            this.btnsAddEdit.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsAddEdit.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvReasons
            // 
            resources.ApplyResources(this.lvReasons, "lvReasons");
            this.lvReasons.BuddyControl = null;
            this.lvReasons.Columns.Add(this.column1);
            this.lvReasons.ContentBackColor = System.Drawing.Color.White;
            this.lvReasons.DefaultRowHeight = ((short)(22));
            this.lvReasons.DimSelectionWhenDisabled = true;
            this.lvReasons.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvReasons.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvReasons.HeaderHeight = ((short)(25));
            this.lvReasons.Name = "lvReasons";
            this.lvReasons.OddRowColor = System.Drawing.Color.White;
            this.lvReasons.RowLineColor = System.Drawing.Color.LightGray;
            this.lvReasons.SecondarySortColumn = ((short)(-1));
            this.lvReasons.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvReasons.SortSetting = "0:1";
            this.lvReasons.SelectionChanged += new System.EventHandler(this.lvReasons_SelectionChanged);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 100;
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // InventoryAdjustmentReasonDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lvReasons);
            this.Controls.Add(this.btnsAddEdit);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "InventoryAdjustmentReasonDialog";
            this.Load += new System.EventHandler(this.InventoryAdjustmentReasonDialog_Load);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.btnsAddEdit, 0);
            this.Controls.SetChildIndex(this.lvReasons, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Button btnCancel;
        private ContextButtons btnsAddEdit;
        private ListView lvReasons;
        private Controls.Columns.Column column1;
    }
}