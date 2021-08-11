namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class WindowsPrinterConfigurationsView
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
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvPrinterConfigurations = new LSOne.Controls.ListView();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmPrinter = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvPrinterConfigurations);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Size = new System.Drawing.Size(730, 514);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            this.btnsContextButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Location = new System.Drawing.Point(441, 487);
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.Size = new System.Drawing.Size(84, 24);
            this.btnsContextButtons.TabIndex = 1;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // lvPrinterConfigurations
            // 
            this.lvPrinterConfigurations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvPrinterConfigurations.BuddyControl = null;
            this.lvPrinterConfigurations.Columns.Add(this.clmID);
            this.lvPrinterConfigurations.Columns.Add(this.clmDescription);
            this.lvPrinterConfigurations.Columns.Add(this.clmPrinter);
            this.lvPrinterConfigurations.ContentBackColor = System.Drawing.Color.White;
            this.lvPrinterConfigurations.DefaultRowHeight = ((short)(22));
            this.lvPrinterConfigurations.DimSelectionWhenDisabled = true;
            this.lvPrinterConfigurations.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPrinterConfigurations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPrinterConfigurations.HeaderHeight = ((short)(25));
            this.lvPrinterConfigurations.Location = new System.Drawing.Point(3, 3);
            this.lvPrinterConfigurations.Name = "lvPrinterConfigurations";
            this.lvPrinterConfigurations.OddRowColor = System.Drawing.Color.White;
            this.lvPrinterConfigurations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPrinterConfigurations.SecondarySortColumn = ((short)(-1));
            this.lvPrinterConfigurations.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvPrinterConfigurations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPrinterConfigurations.Size = new System.Drawing.Size(522, 478);
            this.lvPrinterConfigurations.SortSetting = "0:1";
            this.lvPrinterConfigurations.TabIndex = 0;
            this.lvPrinterConfigurations.SelectionChanged += new System.EventHandler(this.lvPrinterConfigurations_SelectionChanged);
            this.lvPrinterConfigurations.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPrinterConfigurations_RowDoubleClick);
            // 
            // clmID
            // 
            this.clmID.AutoSize = true;
            this.clmID.DefaultStyle = null;
            this.clmID.HeaderText = "ID";
            this.clmID.InternalSort = true;
            this.clmID.MaximumWidth = ((short)(0));
            this.clmID.MinimumWidth = ((short)(10));
            this.clmID.SecondarySortColumn = ((short)(-1));
            this.clmID.Tag = null;
            this.clmID.Width = ((short)(100));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            this.clmDescription.HeaderText = "Description";
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(250));
            // 
            // clmPrinter
            // 
            this.clmPrinter.AutoSize = true;
            this.clmPrinter.DefaultStyle = null;
            this.clmPrinter.HeaderText = "Printer";
            this.clmPrinter.InternalSort = true;
            this.clmPrinter.MaximumWidth = ((short)(0));
            this.clmPrinter.MinimumWidth = ((short)(10));
            this.clmPrinter.SecondarySortColumn = ((short)(-1));
            this.clmPrinter.Tag = null;
            this.clmPrinter.Width = ((short)(250));
            // 
            // WindowsPrinterConfigurationsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.HeaderText = "Windows printer configurations";
            this.Name = "WindowsPrinterConfigurationsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButtons btnsContextButtons;
        private Controls.ListView lvPrinterConfigurations;
        private Controls.Columns.Column clmID;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmPrinter;
    }
}
