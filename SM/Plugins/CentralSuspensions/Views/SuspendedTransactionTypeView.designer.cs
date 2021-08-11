using LSOne.Controls;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
    partial class SuspendedTransactionTypeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuspendedTransactionTypeView));
            this.label5 = new System.Windows.Forms.Label();
            this.lvAddinfo = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsEditAddRemove = new ContextButtons();
            this.cmbAllowEOD = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.btnUp = new ContextButton();
            this.btnDown = new ContextButton();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvAddinfo);
            this.pnlBottom.Controls.Add(this.btnDown);
            this.pnlBottom.Controls.Add(this.btnUp);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.cmbAllowEOD);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.label5);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lvAddinfo
            // 
            resources.ApplyResources(this.lvAddinfo, "lvAddinfo");
            this.lvAddinfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvAddinfo.FullRowSelect = true;
            this.lvAddinfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvAddinfo.HideSelection = false;
            this.lvAddinfo.LockDrawing = false;
            this.lvAddinfo.Name = "lvAddinfo";
            this.lvAddinfo.SortColumn = -1;
            this.lvAddinfo.SortedBackwards = false;
            this.lvAddinfo.UseCompatibleStateImageBehavior = false;
            this.lvAddinfo.UseEveryOtherRowColoring = true;
            this.lvAddinfo.View = System.Windows.Forms.View.Details;
            this.lvAddinfo.SelectedIndexChanged += new System.EventHandler(this.lvAddinfo_SelectedIndexChanged);
            this.lvAddinfo.DoubleClick += new System.EventHandler(this.lvAddinfo_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // cmbAllowEOD
            // 
            this.cmbAllowEOD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllowEOD.FormattingEnabled = true;
            this.cmbAllowEOD.Items.AddRange(new object[] {
            resources.GetString("cmbAllowEOD.Items"),
            resources.GetString("cmbAllowEOD.Items1"),
            resources.GetString("cmbAllowEOD.Items2"),
            resources.GetString("cmbAllowEOD.Items3")});
            resources.ApplyResources(this.cmbAllowEOD, "cmbAllowEOD");
            this.cmbAllowEOD.Name = "cmbAllowEOD";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Context = ButtonType.MoveUp;
            this.btnUp.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnUp.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnUp.Name = "btnUp";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Context = ButtonType.MoveDown;
            this.btnDown.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnDown.MinimumSize = new System.Drawing.Size(23, 23);
            this.btnDown.Name = "btnDown";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // SuspendedTransactionTypeView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SuspendedTransactionTypeView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private ExtendedListView lvAddinfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private ContextButtons btnsEditAddRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAllowEOD;
        private System.Windows.Forms.TextBox tbDescription;
        private ContextButton btnDown;
        private ContextButton btnUp;
    }
}
