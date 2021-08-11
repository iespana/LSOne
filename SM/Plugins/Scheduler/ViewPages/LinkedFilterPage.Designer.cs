using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class LinkedFilterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LinkedFilterPage));
            this.label4 = new System.Windows.Forms.Label();
            this.grpHeader = new LSOne.Controls.GroupPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tbToDesign = new System.Windows.Forms.TextBox();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvFilters = new LSOne.Controls.ListView();
            this.colFromField = new LSOne.Controls.Columns.Column();
            this.colToField = new LSOne.Controls.Columns.Column();
            this.grpHeader.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.grpHeader, "grpHeader");
            this.grpHeader.Name = "grpHeader";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // tbToDesign
            // 
            resources.ApplyResources(this.tbToDesign, "tbToDesign");
            this.tbToDesign.Name = "tbToDesign";
            this.tbToDesign.ReadOnly = true;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnsEditAddRemove);
            resources.ApplyResources(this.pnlActions, "pnlActions");
            this.pnlActions.Name = "pnlActions";
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbToDesign);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.grpHeader);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lvFilters);
            this.panel2.Controls.Add(this.pnlActions);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lvFilters
            // 
            resources.ApplyResources(this.lvFilters, "lvFilters");
            this.lvFilters.BuddyControl = null;
            this.lvFilters.Columns.Add(this.colFromField);
            this.lvFilters.Columns.Add(this.colToField);
            this.lvFilters.ContentBackColor = System.Drawing.Color.White;
            this.lvFilters.DefaultRowHeight = ((short)(22));
            this.lvFilters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFilters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFilters.HeaderHeight = ((short)(25));
            this.lvFilters.Name = "lvFilters";
            this.lvFilters.OddRowColor = System.Drawing.Color.White;
            this.lvFilters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFilters.SecondarySortColumn = ((short)(-1));
            this.lvFilters.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvFilters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvFilters.SortSetting = "0:1";
            this.lvFilters.SelectionChanged += new System.EventHandler(this.lvFilters_SelectionChanged);
            this.lvFilters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvFilters_RowDoubleClick);
            // 
            // colFromField
            // 
            this.colFromField.AutoSize = true;
            this.colFromField.DefaultStyle = null;
            resources.ApplyResources(this.colFromField, "colFromField");
            this.colFromField.InternalSort = true;
            this.colFromField.MaximumWidth = ((short)(0));
            this.colFromField.MinimumWidth = ((short)(10));
            this.colFromField.SecondarySortColumn = ((short)(-1));
            this.colFromField.Sizable = false;
            this.colFromField.Tag = null;
            this.colFromField.Width = ((short)(50));
            // 
            // colToField
            // 
            this.colToField.AutoSize = true;
            this.colToField.DefaultStyle = null;
            resources.ApplyResources(this.colToField, "colToField");
            this.colToField.InternalSort = true;
            this.colToField.MaximumWidth = ((short)(0));
            this.colToField.MinimumWidth = ((short)(10));
            this.colToField.SecondarySortColumn = ((short)(-1));
            this.colToField.Sizable = false;
            this.colToField.Tag = null;
            this.colToField.Width = ((short)(50));
            // 
            // LinkedFilterPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "LinkedFilterPage";
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private GroupPanel grpHeader;
        private System.Windows.Forms.TextBox tbToDesign;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ListView lvFilters;
        private LSOne.Controls.Columns.Column colFromField;
        private LSOne.Controls.Columns.Column colToField;
        private ContextButtons btnsEditAddRemove;
    }
}
