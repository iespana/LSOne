namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class CSVImportProfileView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSVImportProfileView));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvCSVImportProfileLines = new LSOne.Controls.ListView();
            this.clmColumnName = new LSOne.Controls.Columns.Column();
            this.clmDataType = new LSOne.Controls.Columns.Column();
            this.contextButtonMoveUp = new LSOne.Controls.ContextButton();
            this.contextButtonMoveDown = new LSOne.Controls.ContextButton();
            this.cmbImportType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.chkHasHeaders);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.cmbImportType);
            this.pnlBottom.Controls.Add(this.contextButtonMoveDown);
            this.pnlBottom.Controls.Add(this.contextButtonMoveUp);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvCSVImportProfileLines);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.label2);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // lvCSVImportProfileLines
            // 
            resources.ApplyResources(this.lvCSVImportProfileLines, "lvCSVImportProfileLines");
            this.lvCSVImportProfileLines.BuddyControl = null;
            this.lvCSVImportProfileLines.Columns.Add(this.clmColumnName);
            this.lvCSVImportProfileLines.Columns.Add(this.clmDataType);
            this.lvCSVImportProfileLines.ContentBackColor = System.Drawing.Color.White;
            this.lvCSVImportProfileLines.DefaultRowHeight = ((short)(22));
            this.lvCSVImportProfileLines.DimSelectionWhenDisabled = true;
            this.lvCSVImportProfileLines.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCSVImportProfileLines.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCSVImportProfileLines.HeaderHeight = ((short)(25));
            this.lvCSVImportProfileLines.HorizontalScrollbar = true;
            this.lvCSVImportProfileLines.Name = "lvCSVImportProfileLines";
            this.lvCSVImportProfileLines.OddRowColor = System.Drawing.Color.White;
            this.lvCSVImportProfileLines.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCSVImportProfileLines.SecondarySortColumn = ((short)(-1));
            this.lvCSVImportProfileLines.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCSVImportProfileLines.SortSetting = "0:1";
            this.lvCSVImportProfileLines.SelectionChanged += new System.EventHandler(this.lvCSVImportProfileLines_SelectionChanged);
            this.lvCSVImportProfileLines.DoubleClick += new System.EventHandler(this.lvCSVImportProfileLines_DoubleClick);
            // 
            // clmColumnName
            // 
            this.clmColumnName.AutoSize = true;
            this.clmColumnName.DefaultStyle = null;
            resources.ApplyResources(this.clmColumnName, "clmColumnName");
            this.clmColumnName.MaximumWidth = ((short)(0));
            this.clmColumnName.MinimumWidth = ((short)(10));
            this.clmColumnName.SecondarySortColumn = ((short)(-1));
            this.clmColumnName.Tag = null;
            this.clmColumnName.Width = ((short)(150));
            // 
            // clmDataType
            // 
            this.clmDataType.AutoSize = true;
            this.clmDataType.DefaultStyle = null;
            resources.ApplyResources(this.clmDataType, "clmDataType");
            this.clmDataType.MaximumWidth = ((short)(0));
            this.clmDataType.MinimumWidth = ((short)(10));
            this.clmDataType.SecondarySortColumn = ((short)(-1));
            this.clmDataType.Tag = null;
            this.clmDataType.Width = ((short)(150));
            // 
            // contextButtonMoveUp
            // 
            resources.ApplyResources(this.contextButtonMoveUp, "contextButtonMoveUp");
            this.contextButtonMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.contextButtonMoveUp.Name = "contextButtonMoveUp";
            this.contextButtonMoveUp.Click += new System.EventHandler(this.contextButtonMoveUp_Click);
            // 
            // contextButtonMoveDown
            // 
            resources.ApplyResources(this.contextButtonMoveDown, "contextButtonMoveDown");
            this.contextButtonMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.contextButtonMoveDown.Name = "contextButtonMoveDown";
            this.contextButtonMoveDown.Click += new System.EventHandler(this.contextButtonMoveDown_Click);
            // 
            // cmbImportType
            // 
            this.cmbImportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbImportType, "cmbImportType");
            this.cmbImportType.FormattingEnabled = true;
            this.cmbImportType.Name = "cmbImportType";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkHasHeaders
            // 
            resources.ApplyResources(this.chkHasHeaders, "chkHasHeaders");
            this.chkHasHeaders.BackColor = System.Drawing.Color.Transparent;
            this.chkHasHeaders.Checked = true;
            this.chkHasHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.UseVisualStyleBackColor = false;
            // 
            // CSVImportProfileView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 72;
            this.Name = "CSVImportProfileView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Controls.ContextButtons btnsContextButtons;
        private Controls.ListView lvCSVImportProfileLines;
        private Controls.ContextButton contextButtonMoveDown;
        private Controls.ContextButton contextButtonMoveUp;
        private Controls.Columns.Column clmColumnName;
        private Controls.Columns.Column clmDataType;
        private System.Windows.Forms.ComboBox cmbImportType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHasHeaders;
    }
}
