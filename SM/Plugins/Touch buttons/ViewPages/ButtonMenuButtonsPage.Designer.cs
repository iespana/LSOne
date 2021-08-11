namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class ButtonMenuButtonsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonMenuButtonsPage));
            this.lvPosMenuLines = new LSOne.Controls.ListView();
            this.clmKeyNo = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmOperation = new LSOne.Controls.Columns.Column();
            this.clmParameter = new LSOne.Controls.Columns.Column();
            this.clmStyle = new LSOne.Controls.Columns.Column();
            this.btnDown = new LSOne.Controls.ContextButton();
            this.btnUp = new LSOne.Controls.ContextButton();
            this.btnsEditAddRemovePosMenuLine = new LSOne.Controls.ContextButtons();
            this.SuspendLayout();
            // 
            // lvPosMenuLines
            // 
            resources.ApplyResources(this.lvPosMenuLines, "lvPosMenuLines");
            this.lvPosMenuLines.BuddyControl = null;
            this.lvPosMenuLines.Columns.Add(this.clmKeyNo);
            this.lvPosMenuLines.Columns.Add(this.clmDescription);
            this.lvPosMenuLines.Columns.Add(this.clmOperation);
            this.lvPosMenuLines.Columns.Add(this.clmParameter);
            this.lvPosMenuLines.Columns.Add(this.clmStyle);
            this.lvPosMenuLines.ContentBackColor = System.Drawing.Color.White;
            this.lvPosMenuLines.DefaultRowHeight = ((short)(22));
            this.lvPosMenuLines.DimSelectionWhenDisabled = true;
            this.lvPosMenuLines.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPosMenuLines.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPosMenuLines.HeaderHeight = ((short)(25));
            this.lvPosMenuLines.Name = "lvPosMenuLines";
            this.lvPosMenuLines.OddRowColor = System.Drawing.Color.White;
            this.lvPosMenuLines.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPosMenuLines.SecondarySortColumn = ((short)(-1));
            this.lvPosMenuLines.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvPosMenuLines.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPosMenuLines.SortSetting = "0:1";
            this.lvPosMenuLines.SelectionChanged += new System.EventHandler(this.lvPosMenuLines_SelectionChanged);
            this.lvPosMenuLines.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPosMenuLines_RowDoubleClick);
            // 
            // clmKeyNo
            // 
            this.clmKeyNo.AutoSize = true;
            this.clmKeyNo.Clickable = false;
            this.clmKeyNo.DefaultStyle = null;
            resources.ApplyResources(this.clmKeyNo, "clmKeyNo");
            this.clmKeyNo.MaximumWidth = ((short)(0));
            this.clmKeyNo.MinimumWidth = ((short)(10));
            this.clmKeyNo.SecondarySortColumn = ((short)(-1));
            this.clmKeyNo.Tag = null;
            this.clmKeyNo.Width = ((short)(100));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.Clickable = false;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(50));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(150));
            // 
            // clmOperation
            // 
            this.clmOperation.AutoSize = true;
            this.clmOperation.Clickable = false;
            this.clmOperation.DefaultStyle = null;
            resources.ApplyResources(this.clmOperation, "clmOperation");
            this.clmOperation.MaximumWidth = ((short)(0));
            this.clmOperation.MinimumWidth = ((short)(10));
            this.clmOperation.SecondarySortColumn = ((short)(-1));
            this.clmOperation.Tag = null;
            this.clmOperation.Width = ((short)(150));
            // 
            // clmParameter
            // 
            this.clmParameter.AutoSize = true;
            this.clmParameter.Clickable = false;
            this.clmParameter.DefaultStyle = null;
            resources.ApplyResources(this.clmParameter, "clmParameter");
            this.clmParameter.MaximumWidth = ((short)(0));
            this.clmParameter.MinimumWidth = ((short)(10));
            this.clmParameter.SecondarySortColumn = ((short)(-1));
            this.clmParameter.Tag = null;
            this.clmParameter.Width = ((short)(150));
            // 
            // clmStyle
            // 
            this.clmStyle.AutoSize = true;
            this.clmStyle.Clickable = false;
            this.clmStyle.DefaultStyle = null;
            resources.ApplyResources(this.clmStyle, "clmStyle");
            this.clmStyle.MaximumWidth = ((short)(0));
            this.clmStyle.MinimumWidth = ((short)(10));
            this.clmStyle.SecondarySortColumn = ((short)(-1));
            this.clmStyle.Tag = null;
            this.clmStyle.Width = ((short)(200));
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnDown.Name = "btnDown";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnUp.Name = "btnUp";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnsEditAddRemovePosMenuLine
            // 
            this.btnsEditAddRemovePosMenuLine.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemovePosMenuLine, "btnsEditAddRemovePosMenuLine");
            this.btnsEditAddRemovePosMenuLine.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemovePosMenuLine.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemovePosMenuLine.EditButtonEnabled = false;
            this.btnsEditAddRemovePosMenuLine.Name = "btnsEditAddRemovePosMenuLine";
            this.btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = false;
            this.btnsEditAddRemovePosMenuLine.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_EditButtonClicked);
            this.btnsEditAddRemovePosMenuLine.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_AddButtonClicked);
            this.btnsEditAddRemovePosMenuLine.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemovePosMenuLine_RemoveButtonClicked);
            // 
            // ButtonMenuButtonsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvPosMenuLines);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnsEditAddRemovePosMenuLine);
            this.Name = "ButtonMenuButtonsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.ListView lvPosMenuLines;
        private LSOne.Controls.ContextButton btnDown;
        private LSOne.Controls.ContextButton btnUp;
        private LSOne.Controls.ContextButtons btnsEditAddRemovePosMenuLine;
        private LSOne.Controls.Columns.Column clmKeyNo;
        private LSOne.Controls.Columns.Column clmDescription;
        private LSOne.Controls.Columns.Column clmOperation;
        private LSOne.Controls.Columns.Column clmParameter;
        private LSOne.Controls.Columns.Column clmStyle;
    }
}
