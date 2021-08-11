using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
    partial class LoyaltySchemesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltySchemesView));
            this.groupPanel1 = new GroupPanel();
            this.gridLines = new ListView();
            this.columnCalcType = new Column();
            this.columnTypeRelation = new Column();
            this.columnTypeName = new Column();
            this.columnStartingDate = new Column();
            this.columnEndingDate = new Column();
            this.columnQtyAmt = new Column();
            this.columnMultType = new Column();
            this.columnLoyPts = new Column();
            this.btnsContextButtonsItems = new ContextButtons();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsContextButtons = new ContextButtons();
            this.gridObjects = new ListView();
            this.columnObjID = new Column();
            this.columnObjDesc = new Column();
            this.columnObjExpTimeUnit = new Column();
            this.columnObjPtsUseLimit = new Column();
            this.columnObjMultType = new Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.gridObjects);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.gridLines);
            this.groupPanel1.Controls.Add(this.btnsContextButtonsItems);
            this.groupPanel1.Controls.Add(this.lblGroupHeader);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // gridLines
            // 
            resources.ApplyResources(this.gridLines, "gridLines");
            this.gridLines.AutoSelectOnFocus = true;
            this.gridLines.BackColor = System.Drawing.Color.White;
            this.gridLines.BuddyControl = null;
            this.gridLines.Columns.Add(this.columnCalcType);
            this.gridLines.Columns.Add(this.columnTypeRelation);
            this.gridLines.Columns.Add(this.columnTypeName);
            this.gridLines.Columns.Add(this.columnStartingDate);
            this.gridLines.Columns.Add(this.columnEndingDate);
            this.gridLines.Columns.Add(this.columnQtyAmt);
            this.gridLines.Columns.Add(this.columnMultType);
            this.gridLines.Columns.Add(this.columnLoyPts);
            this.gridLines.ContentBackColor = System.Drawing.Color.White;
            this.gridLines.DefaultRowHeight = ((short)(22));
            this.gridLines.DimSelectionWhenDisabled = true;
            this.gridLines.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.gridLines.ForeColor = System.Drawing.Color.Black;
            this.gridLines.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.gridLines.HeaderHeight = ((short)(25));
            this.gridLines.HorizontalScrollbar = true;
            this.gridLines.Name = "gridLines";
            this.gridLines.OddRowColor = System.Drawing.Color.Transparent;
            this.gridLines.RowLineColor = System.Drawing.Color.LightGray;
            this.gridLines.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.gridLines.SortSetting = "-1:1";
            this.gridLines.SelectionChanged += new System.EventHandler(this.gridLines_SelectionChanged);
            this.gridLines.RowDoubleClick += new RowClickDelegate(this.gridLines_RowDoubleClick);
            // 
            // columnCalcType
            // 
            this.columnCalcType.AutoSize = true;
            this.columnCalcType.Clickable = false;
            this.columnCalcType.DefaultStyle = null;
            resources.ApplyResources(this.columnCalcType, "columnCalcType");
            this.columnCalcType.MaximumWidth = ((short)(0));
            this.columnCalcType.MinimumWidth = ((short)(10));
            this.columnCalcType.Sizable = true;
            this.columnCalcType.Tag = null;
            this.columnCalcType.Width = ((short)(90));
            // 
            // columnTypeRelation
            // 
            this.columnTypeRelation.AutoSize = true;
            this.columnTypeRelation.Clickable = false;
            this.columnTypeRelation.DefaultStyle = null;
            resources.ApplyResources(this.columnTypeRelation, "columnTypeRelation");
            this.columnTypeRelation.MaximumWidth = ((short)(0));
            this.columnTypeRelation.MinimumWidth = ((short)(10));
            this.columnTypeRelation.Sizable = true;
            this.columnTypeRelation.Tag = null;
            this.columnTypeRelation.Width = ((short)(80));
            // 
            // columnTypeName
            // 
            this.columnTypeName.AutoSize = true;
            this.columnTypeName.Clickable = false;
            this.columnTypeName.DefaultStyle = null;
            resources.ApplyResources(this.columnTypeName, "columnTypeName");
            this.columnTypeName.MaximumWidth = ((short)(0));
            this.columnTypeName.MinimumWidth = ((short)(10));
            this.columnTypeName.Sizable = true;
            this.columnTypeName.Tag = null;
            this.columnTypeName.Width = ((short)(70));
            // 
            // columnStartingDate
            // 
            this.columnStartingDate.AutoSize = true;
            this.columnStartingDate.Clickable = false;
            this.columnStartingDate.DefaultStyle = null;
            resources.ApplyResources(this.columnStartingDate, "columnStartingDate");
            this.columnStartingDate.MaximumWidth = ((short)(0));
            this.columnStartingDate.MinimumWidth = ((short)(10));
            this.columnStartingDate.Sizable = true;
            this.columnStartingDate.Tag = null;
            this.columnStartingDate.Width = ((short)(80));
            // 
            // columnEndingDate
            // 
            this.columnEndingDate.AutoSize = true;
            this.columnEndingDate.Clickable = false;
            this.columnEndingDate.DefaultStyle = null;
            resources.ApplyResources(this.columnEndingDate, "columnEndingDate");
            this.columnEndingDate.MaximumWidth = ((short)(0));
            this.columnEndingDate.MinimumWidth = ((short)(10));
            this.columnEndingDate.Sizable = true;
            this.columnEndingDate.Tag = null;
            this.columnEndingDate.Width = ((short)(70));
            // 
            // columnQtyAmt
            // 
            this.columnQtyAmt.AutoSize = true;
            this.columnQtyAmt.Clickable = false;
            this.columnQtyAmt.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnQtyAmt.DefaultStyle = null;
            resources.ApplyResources(this.columnQtyAmt, "columnQtyAmt");
            this.columnQtyAmt.MaximumWidth = ((short)(0));
            this.columnQtyAmt.MinimumWidth = ((short)(10));
            this.columnQtyAmt.Sizable = true;
            this.columnQtyAmt.Tag = null;
            this.columnQtyAmt.Width = ((short)(90));
            // 
            // columnMultType
            // 
            this.columnMultType.AutoSize = true;
            this.columnMultType.Clickable = false;
            this.columnMultType.DefaultStyle = null;
            resources.ApplyResources(this.columnMultType, "columnMultType");
            this.columnMultType.MaximumWidth = ((short)(0));
            this.columnMultType.MinimumWidth = ((short)(10));
            this.columnMultType.Sizable = true;
            this.columnMultType.Tag = null;
            this.columnMultType.Width = ((short)(90));
            // 
            // columnLoyPts
            // 
            this.columnLoyPts.AutoSize = true;
            this.columnLoyPts.Clickable = false;
            this.columnLoyPts.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnLoyPts.DefaultStyle = null;
            resources.ApplyResources(this.columnLoyPts, "columnLoyPts");
            this.columnLoyPts.MaximumWidth = ((short)(0));
            this.columnLoyPts.MinimumWidth = ((short)(10));
            this.columnLoyPts.Sizable = true;
            this.columnLoyPts.Tag = null;
            this.columnLoyPts.Width = ((short)(90));
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.EditButtonClicked += new System.EventHandler(this.btnEditLine_Click);
            this.btnsContextButtonsItems.AddButtonClicked += new System.EventHandler(this.btnAddLine_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveLine_Click);
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // gridObjects
            // 
            resources.ApplyResources(this.gridObjects, "gridObjects");
            this.gridObjects.BackColor = System.Drawing.Color.White;
            this.gridObjects.BuddyControl = null;
            this.gridObjects.Columns.Add(this.columnObjID);
            this.gridObjects.Columns.Add(this.columnObjDesc);
            this.gridObjects.Columns.Add(this.columnObjExpTimeUnit);
            this.gridObjects.Columns.Add(this.columnObjPtsUseLimit);
            this.gridObjects.Columns.Add(this.columnObjMultType);
            this.gridObjects.ContentBackColor = System.Drawing.Color.White;
            this.gridObjects.DefaultRowHeight = ((short)(22));
            this.gridObjects.DimSelectionWhenDisabled = true;
            this.gridObjects.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.gridObjects.ForeColor = System.Drawing.Color.Black;
            this.gridObjects.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.gridObjects.HeaderHeight = ((short)(25));
            this.gridObjects.HorizontalScrollbar = true;
            this.gridObjects.Name = "gridObjects";
            this.gridObjects.OddRowColor = System.Drawing.Color.Transparent;
            this.gridObjects.RowLineColor = System.Drawing.Color.LightGray;
            this.gridObjects.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.gridObjects.SortSetting = "-1:1";
            this.gridObjects.SelectionChanged += new System.EventHandler(this.gridObjects_SelectionChanged);
            this.gridObjects.RowDoubleClick += new RowClickDelegate(this.gridObjects_RowDoubleClick);
            // 
            // columnObjID
            // 
            this.columnObjID.AutoSize = true;
            this.columnObjID.Clickable = false;
            this.columnObjID.DefaultStyle = null;
            resources.ApplyResources(this.columnObjID, "columnObjID");
            this.columnObjID.MaximumWidth = ((short)(0));
            this.columnObjID.MinimumWidth = ((short)(10));
            this.columnObjID.Sizable = true;
            this.columnObjID.Tag = null;
            this.columnObjID.Width = ((short)(50));
            // 
            // columnObjDesc
            // 
            this.columnObjDesc.AutoSize = true;
            this.columnObjDesc.Clickable = false;
            this.columnObjDesc.DefaultStyle = null;
            resources.ApplyResources(this.columnObjDesc, "columnObjDesc");
            this.columnObjDesc.MaximumWidth = ((short)(0));
            this.columnObjDesc.MinimumWidth = ((short)(10));
            this.columnObjDesc.Sizable = true;
            this.columnObjDesc.Tag = null;
            this.columnObjDesc.Width = ((short)(70));
            // 
            // columnObjExpTimeUnit
            // 
            this.columnObjExpTimeUnit.AutoSize = true;
            this.columnObjExpTimeUnit.Clickable = false;
            this.columnObjExpTimeUnit.DefaultStyle = null;
            resources.ApplyResources(this.columnObjExpTimeUnit, "columnObjExpTimeUnit");
            this.columnObjExpTimeUnit.MaximumWidth = ((short)(0));
            this.columnObjExpTimeUnit.MinimumWidth = ((short)(10));
            this.columnObjExpTimeUnit.Sizable = true;
            this.columnObjExpTimeUnit.Tag = null;
            this.columnObjExpTimeUnit.Width = ((short)(100));
            // 
            // columnObjPtsUseLimit
            // 
            this.columnObjPtsUseLimit.AutoSize = true;
            this.columnObjPtsUseLimit.Clickable = false;
            this.columnObjPtsUseLimit.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            this.columnObjPtsUseLimit.DefaultStyle = null;
            resources.ApplyResources(this.columnObjPtsUseLimit, "columnObjPtsUseLimit");
            this.columnObjPtsUseLimit.MaximumWidth = ((short)(0));
            this.columnObjPtsUseLimit.MinimumWidth = ((short)(10));
            this.columnObjPtsUseLimit.Sizable = true;
            this.columnObjPtsUseLimit.Tag = null;
            this.columnObjPtsUseLimit.Width = ((short)(80));
            // 
            // columnObjMultType
            // 
            this.columnObjMultType.AutoSize = true;
            this.columnObjMultType.Clickable = false;
            this.columnObjMultType.DefaultStyle = null;
            resources.ApplyResources(this.columnObjMultType, "columnObjMultType");
            this.columnObjMultType.MaximumWidth = ((short)(0));
            this.columnObjMultType.MinimumWidth = ((short)(10));
            this.columnObjMultType.Sizable = true;
            this.columnObjMultType.Tag = null;
            this.columnObjMultType.Width = ((short)(110));
            // 
            // LoyaltySchemesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LoyaltySchemesView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private GroupPanel groupPanel1;
		private System.Windows.Forms.Label lblGroupHeader;
		private System.Windows.Forms.Label lblNoSelection;
        private ContextButtons btnsContextButtons;
		private ContextButtons btnsContextButtonsItems;
		private ListView gridObjects;
		private ListView gridLines;
		private Column columnCalcType;
		private Column columnTypeRelation;
		private Column columnTypeName;
		private Column columnStartingDate;
		private Column columnEndingDate;
		private Column columnQtyAmt;
		private Column columnMultType;
		private Column columnLoyPts;
		private Column columnObjDesc;
        private Column columnObjExpTimeUnit;
		private Column columnObjPtsUseLimit;
		private Column columnObjMultType;
		private Column columnObjID;

    }
}
