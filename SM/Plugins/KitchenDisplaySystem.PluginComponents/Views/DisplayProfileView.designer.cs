using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class DisplayProfileView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayProfileView));
            this.tbProfileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlChit = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnsEditAddRemoveChitDisplayLines = new LSOne.Controls.ContextButtons();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.btnChitMoveDown = new LSOne.Controls.ContextButton();
            this.btnChitMoveUp = new LSOne.Controls.ContextButton();
            this.btnsEditAddRemoveChitDisplayColumns = new LSOne.Controls.ContextButtons();
            this.label2 = new System.Windows.Forms.Label();
            this.lvChitDisplayColumns = new LSOne.Controls.ExtendedListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.lvChitDisplayLines = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlLine = new System.Windows.Forms.Panel();
            this.btnLineMoveDown = new LSOne.Controls.ContextButton();
            this.btnLineMoveUp = new LSOne.Controls.ContextButton();
            this.btnsEditAddRemoveLineDisplayColumns = new LSOne.Controls.ContextButtons();
            this.label6 = new System.Windows.Forms.Label();
            this.lvLineDisplayColumns = new LSOne.Controls.ExtendedListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmbDisplayMode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.pnlChit.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.pnlLine.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlLine);
            this.pnlBottom.Controls.Add(this.cmbDisplayMode);
            this.pnlBottom.Controls.Add(this.label11);
            this.pnlBottom.Controls.Add(this.pnlChit);
            this.pnlBottom.Controls.Add(this.tbProfileName);
            this.pnlBottom.Controls.Add(this.label3);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // tbProfileName
            // 
            resources.ApplyResources(this.tbProfileName, "tbProfileName");
            this.tbProfileName.Name = "tbProfileName";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // pnlChit
            // 
            resources.ApplyResources(this.pnlChit, "pnlChit");
            this.pnlChit.Controls.Add(this.label1);
            this.pnlChit.Controls.Add(this.btnsEditAddRemoveChitDisplayLines);
            this.pnlChit.Controls.Add(this.groupPanel2);
            this.pnlChit.Controls.Add(this.lvChitDisplayLines);
            this.pnlChit.Name = "pnlChit";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // btnsEditAddRemoveChitDisplayLines
            // 
            this.btnsEditAddRemoveChitDisplayLines.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveChitDisplayLines, "btnsEditAddRemoveChitDisplayLines");
            this.btnsEditAddRemoveChitDisplayLines.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveChitDisplayLines.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsEditAddRemoveChitDisplayLines.EditButtonEnabled = false;
            this.btnsEditAddRemoveChitDisplayLines.Name = "btnsEditAddRemoveChitDisplayLines";
            this.btnsEditAddRemoveChitDisplayLines.RemoveButtonEnabled = true;
            this.btnsEditAddRemoveChitDisplayLines.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayLines_EditButtonClicked);
            this.btnsEditAddRemoveChitDisplayLines.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayLines_AddButtonClicked);
            this.btnsEditAddRemoveChitDisplayLines.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayLines_RemoveButtonClicked);
            // 
            // groupPanel2
            // 
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Controls.Add(this.btnChitMoveDown);
            this.groupPanel2.Controls.Add(this.btnChitMoveUp);
            this.groupPanel2.Controls.Add(this.btnsEditAddRemoveChitDisplayColumns);
            this.groupPanel2.Controls.Add(this.label2);
            this.groupPanel2.Controls.Add(this.lvChitDisplayColumns);
            this.groupPanel2.Controls.Add(this.label4);
            this.groupPanel2.Name = "groupPanel2";
            // 
            // btnChitMoveDown
            // 
            resources.ApplyResources(this.btnChitMoveDown, "btnChitMoveDown");
            this.btnChitMoveDown.BackColor = System.Drawing.Color.Transparent;
            this.btnChitMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnChitMoveDown.Name = "btnChitMoveDown";
            this.btnChitMoveDown.Click += new System.EventHandler(this.btnChitMoveDown_Click);
            // 
            // btnChitMoveUp
            // 
            resources.ApplyResources(this.btnChitMoveUp, "btnChitMoveUp");
            this.btnChitMoveUp.BackColor = System.Drawing.Color.Transparent;
            this.btnChitMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnChitMoveUp.Name = "btnChitMoveUp";
            this.btnChitMoveUp.Click += new System.EventHandler(this.btnChitMoveUp_Click);
            // 
            // btnsEditAddRemoveChitDisplayColumns
            // 
            this.btnsEditAddRemoveChitDisplayColumns.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveChitDisplayColumns, "btnsEditAddRemoveChitDisplayColumns");
            this.btnsEditAddRemoveChitDisplayColumns.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveChitDisplayColumns.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveChitDisplayColumns.EditButtonEnabled = true;
            this.btnsEditAddRemoveChitDisplayColumns.Name = "btnsEditAddRemoveChitDisplayColumns";
            this.btnsEditAddRemoveChitDisplayColumns.RemoveButtonEnabled = true;
            this.btnsEditAddRemoveChitDisplayColumns.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayColumns_EditButtonClicked);
            this.btnsEditAddRemoveChitDisplayColumns.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayColumns_AddButtonClicked);
            this.btnsEditAddRemoveChitDisplayColumns.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveChitDisplayColumns_RemoveButtonClicked);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // lvChitDisplayColumns
            // 
            resources.ApplyResources(this.lvChitDisplayColumns, "lvChitDisplayColumns");
            this.lvChitDisplayColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.lvChitDisplayColumns.FullRowSelect = true;
            this.lvChitDisplayColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvChitDisplayColumns.HideSelection = false;
            this.lvChitDisplayColumns.LockDrawing = false;
            this.lvChitDisplayColumns.MultiSelect = false;
            this.lvChitDisplayColumns.Name = "lvChitDisplayColumns";
            this.lvChitDisplayColumns.SortColumn = -1;
            this.lvChitDisplayColumns.SortedBackwards = false;
            this.lvChitDisplayColumns.UseCompatibleStateImageBehavior = false;
            this.lvChitDisplayColumns.UseEveryOtherRowColoring = false;
            this.lvChitDisplayColumns.View = System.Windows.Forms.View.Details;
            this.lvChitDisplayColumns.SelectedIndexChanged += new System.EventHandler(this.lvChitDisplayColumns_SelectedIndexChanged);
            this.lvChitDisplayColumns.DoubleClick += new System.EventHandler(this.lvChitDisplayColumns_DoubleClick);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // columnHeader9
            // 
            resources.ApplyResources(this.columnHeader9, "columnHeader9");
            // 
            // columnHeader10
            // 
            resources.ApplyResources(this.columnHeader10, "columnHeader10");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Name = "label4";
            // 
            // lvChitDisplayLines
            // 
            this.lvChitDisplayLines.Activation = System.Windows.Forms.ItemActivation.OneClick;
            resources.ApplyResources(this.lvChitDisplayLines, "lvChitDisplayLines");
            this.lvChitDisplayLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvChitDisplayLines.FullRowSelect = true;
            this.lvChitDisplayLines.HideSelection = false;
            this.lvChitDisplayLines.LockDrawing = false;
            this.lvChitDisplayLines.MultiSelect = false;
            this.lvChitDisplayLines.Name = "lvChitDisplayLines";
            this.lvChitDisplayLines.SortColumn = -1;
            this.lvChitDisplayLines.SortedBackwards = false;
            this.lvChitDisplayLines.UseCompatibleStateImageBehavior = false;
            this.lvChitDisplayLines.UseEveryOtherRowColoring = true;
            this.lvChitDisplayLines.View = System.Windows.Forms.View.Details;
            this.lvChitDisplayLines.SelectedIndexChanged += new System.EventHandler(this.lvChitDisplayLines_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // pnlLine
            // 
            resources.ApplyResources(this.pnlLine, "pnlLine");
            this.pnlLine.Controls.Add(this.btnLineMoveDown);
            this.pnlLine.Controls.Add(this.btnLineMoveUp);
            this.pnlLine.Controls.Add(this.btnsEditAddRemoveLineDisplayColumns);
            this.pnlLine.Controls.Add(this.label6);
            this.pnlLine.Controls.Add(this.lvLineDisplayColumns);
            this.pnlLine.Name = "pnlLine";
            // 
            // btnLineMoveDown
            // 
            resources.ApplyResources(this.btnLineMoveDown, "btnLineMoveDown");
            this.btnLineMoveDown.BackColor = System.Drawing.Color.Transparent;
            this.btnLineMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnLineMoveDown.Name = "btnLineMoveDown";
            this.btnLineMoveDown.Click += new System.EventHandler(this.btnLineMoveDown_Click);
            // 
            // btnLineMoveUp
            // 
            resources.ApplyResources(this.btnLineMoveUp, "btnLineMoveUp");
            this.btnLineMoveUp.BackColor = System.Drawing.Color.Transparent;
            this.btnLineMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnLineMoveUp.Name = "btnLineMoveUp";
            this.btnLineMoveUp.Click += new System.EventHandler(this.btnLineMoveUp_Click);
            // 
            // btnsEditAddRemoveLineDisplayColumns
            // 
            this.btnsEditAddRemoveLineDisplayColumns.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveLineDisplayColumns, "btnsEditAddRemoveLineDisplayColumns");
            this.btnsEditAddRemoveLineDisplayColumns.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveLineDisplayColumns.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveLineDisplayColumns.EditButtonEnabled = true;
            this.btnsEditAddRemoveLineDisplayColumns.Name = "btnsEditAddRemoveLineDisplayColumns";
            this.btnsEditAddRemoveLineDisplayColumns.RemoveButtonEnabled = true;
            this.btnsEditAddRemoveLineDisplayColumns.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveLineDisplayColumns_EditButtonClicked);
            this.btnsEditAddRemoveLineDisplayColumns.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveLineDisplayColumns_AddButtonClicked);
            this.btnsEditAddRemoveLineDisplayColumns.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveLineDisplayColumns_RemoveButtonClicked);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // lvLineDisplayColumns
            // 
            resources.ApplyResources(this.lvLineDisplayColumns, "lvLineDisplayColumns");
            this.lvLineDisplayColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.lvLineDisplayColumns.FullRowSelect = true;
            this.lvLineDisplayColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLineDisplayColumns.HideSelection = false;
            this.lvLineDisplayColumns.LockDrawing = false;
            this.lvLineDisplayColumns.MultiSelect = false;
            this.lvLineDisplayColumns.Name = "lvLineDisplayColumns";
            this.lvLineDisplayColumns.SortColumn = -1;
            this.lvLineDisplayColumns.SortedBackwards = false;
            this.lvLineDisplayColumns.UseCompatibleStateImageBehavior = false;
            this.lvLineDisplayColumns.UseEveryOtherRowColoring = false;
            this.lvLineDisplayColumns.View = System.Windows.Forms.View.Details;
            this.lvLineDisplayColumns.SelectedIndexChanged += new System.EventHandler(this.lvLineDisplayColumns_SelectedIndexChanged);
            this.lvLineDisplayColumns.DoubleClick += new System.EventHandler(this.lvLineDisplayColumns_DoubleClick);
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader11
            // 
            resources.ApplyResources(this.columnHeader11, "columnHeader11");
            // 
            // columnHeader12
            // 
            resources.ApplyResources(this.columnHeader12, "columnHeader12");
            // 
            // columnHeader13
            // 
            resources.ApplyResources(this.columnHeader13, "columnHeader13");
            // 
            // columnHeader14
            // 
            resources.ApplyResources(this.columnHeader14, "columnHeader14");
            // 
            // columnHeader15
            // 
            resources.ApplyResources(this.columnHeader15, "columnHeader15");
            // 
            // columnHeader16
            // 
            resources.ApplyResources(this.columnHeader16, "columnHeader16");
            // 
            // cmbDisplayMode
            // 
            this.cmbDisplayMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplayMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDisplayMode, "cmbDisplayMode");
            this.cmbDisplayMode.Name = "cmbDisplayMode";
            this.cmbDisplayMode.SelectedIndexChanged += new System.EventHandler(this.cmbDisplayMode_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // DisplayProfileView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 50;
            this.Name = "DisplayProfileView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlChit.ResumeLayout(false);
            this.pnlChit.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.pnlLine.ResumeLayout(false);
            this.pnlLine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbProfileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlChit;
        private System.Windows.Forms.ComboBox cmbDisplayMode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.ContextButtons btnsEditAddRemoveChitDisplayLines;
        private LSOne.Controls.GroupPanel groupPanel2;
        private LSOne.Controls.ContextButtons btnsEditAddRemoveChitDisplayColumns;
        private System.Windows.Forms.Label label2;
        private LSOne.Controls.ExtendedListView lvChitDisplayColumns;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.Label label4;
        private LSOne.Controls.ExtendedListView lvChitDisplayLines;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel pnlLine;
        private LSOne.Controls.ContextButtons btnsEditAddRemoveLineDisplayColumns;
        private System.Windows.Forms.Label label6;
        private LSOne.Controls.ExtendedListView lvLineDisplayColumns;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private LSOne.Controls.ContextButton btnLineMoveDown;
        private LSOne.Controls.ContextButton btnLineMoveUp;
        private LSOne.Controls.ContextButton btnChitMoveDown;
        private LSOne.Controls.ContextButton btnChitMoveUp;
    }
}
