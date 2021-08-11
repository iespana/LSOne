using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class PrintersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintersView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvKitchenPrinters = new LSOne.Controls.ListView();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvKitchenPrinters);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
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
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvKitchenPrinters
            // 
            resources.ApplyResources(this.lvKitchenPrinters, "lvKitchenPrinters");
            this.lvKitchenPrinters.BuddyControl = null;
            this.lvKitchenPrinters.Columns.Add(this.column3);
            this.lvKitchenPrinters.Columns.Add(this.column4);
            this.lvKitchenPrinters.Columns.Add(this.column1);
            this.lvKitchenPrinters.Columns.Add(this.column2);
            this.lvKitchenPrinters.ContentBackColor = System.Drawing.Color.White;
            this.lvKitchenPrinters.DefaultRowHeight = ((short)(22));
            this.lvKitchenPrinters.DimSelectionWhenDisabled = true;
            this.lvKitchenPrinters.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvKitchenPrinters.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvKitchenPrinters.HeaderHeight = ((short)(25));
            this.lvKitchenPrinters.Name = "lvKitchenPrinters";
            this.lvKitchenPrinters.OddRowColor = System.Drawing.Color.White;
            this.lvKitchenPrinters.RowLineColor = System.Drawing.Color.LightGray;
            this.lvKitchenPrinters.SecondarySortColumn = ((short)(-1));
            this.lvKitchenPrinters.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvKitchenPrinters.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvKitchenPrinters.SortSetting = "2:1";
            this.lvKitchenPrinters.SelectionChanged += new System.EventHandler(this.lvKitchenPrinters_SelectionChanged);
            this.lvKitchenPrinters.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvKitchenPrinters_RowDoubleClick);
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // PrintersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrintersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemove;
        private ListView lvKitchenPrinters;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
        private LSOne.Controls.Columns.Column column3;
        private LSOne.Controls.Columns.Column column4;

    }
}