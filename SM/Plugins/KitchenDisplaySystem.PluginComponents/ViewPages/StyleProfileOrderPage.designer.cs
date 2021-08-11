using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StyleProfileOrderPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileOrderPage));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvTimeStyles = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.linkFields1 = new LSOne.Controls.LinkFields();
            this.cwSeparatorColor = new LSOne.Controls.ColorWell();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.cmbDoneChitOverlay = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnsMarkedForAlert = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbMarkedForAlert = new LSOne.Controls.DualDataComboBox();
            this.btnsOrderPane = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.btnsOrder = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbOrder = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbOrderPane = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.lvTimeStyles);
            this.groupBox3.Controls.Add(this.btnsEditAddRemove);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lvTimeStyles
            // 
            resources.ApplyResources(this.lvTimeStyles, "lvTimeStyles");
            this.lvTimeStyles.BorderColor = System.Drawing.Color.DarkGray;
            this.lvTimeStyles.BuddyControl = null;
            this.lvTimeStyles.Columns.Add(this.column1);
            this.lvTimeStyles.Columns.Add(this.column2);
            this.lvTimeStyles.ContentBackColor = System.Drawing.Color.White;
            this.lvTimeStyles.DefaultRowHeight = ((short)(22));
            this.lvTimeStyles.EvenRowColor = System.Drawing.Color.White;
            this.lvTimeStyles.HeaderBackColor = System.Drawing.Color.White;
            this.lvTimeStyles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTimeStyles.HeaderHeight = ((short)(25));
            this.lvTimeStyles.Name = "lvTimeStyles";
            this.lvTimeStyles.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTimeStyles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTimeStyles.SecondarySortColumn = ((short)(-1));
            this.lvTimeStyles.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvTimeStyles.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTimeStyles.SortSetting = "0:1";
            this.lvTimeStyles.VerticalScrollbarValue = 0;
            this.lvTimeStyles.VerticalScrollbarYOffset = 0;
            this.lvTimeStyles.SelectionChanged += new System.EventHandler(this.lvTimeStyles_SelectedIndexChanged);
            this.lvTimeStyles.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvTimeStyles_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
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
            // linkFields1
            // 
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.Name = "linkFields1";
            this.linkFields1.TabStop = false;
            // 
            // cwSeparatorColor
            // 
            resources.ApplyResources(this.cwSeparatorColor, "cwSeparatorColor");
            this.cwSeparatorColor.Name = "cwSeparatorColor";
            this.cwSeparatorColor.SelectedColor = System.Drawing.Color.White;
            this.cwSeparatorColor.SelectedColorChanged += new System.EventHandler(this.cwSeparatorColor_SelectedColorChanged);
            // 
            // lblSeparator
            // 
            this.lblSeparator.BackColor = System.Drawing.Color.Transparent;
            this.lblSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.lblSeparator, "lblSeparator");
            this.lblSeparator.Name = "lblSeparator";
            // 
            // cmbDoneChitOverlay
            // 
            this.cmbDoneChitOverlay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoneChitOverlay.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDoneChitOverlay, "cmbDoneChitOverlay");
            this.cmbDoneChitOverlay.Name = "cmbDoneChitOverlay";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnsMarkedForAlert
            // 
            this.btnsMarkedForAlert.AddButtonEnabled = true;
            this.btnsMarkedForAlert.BackColor = System.Drawing.Color.Transparent;
            this.btnsMarkedForAlert.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsMarkedForAlert.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsMarkedForAlert, "btnsMarkedForAlert");
            this.btnsMarkedForAlert.Name = "btnsMarkedForAlert";
            this.btnsMarkedForAlert.RemoveButtonEnabled = false;
            // 
            // cmbMarkedForAlert
            // 
            this.cmbMarkedForAlert.AddList = null;
            this.cmbMarkedForAlert.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbMarkedForAlert, "cmbMarkedForAlert");
            this.cmbMarkedForAlert.MaxLength = 32767;
            this.cmbMarkedForAlert.Name = "cmbMarkedForAlert";
            this.cmbMarkedForAlert.NoChangeAllowed = false;
            this.cmbMarkedForAlert.OnlyDisplayID = false;
            this.cmbMarkedForAlert.RemoveList = null;
            this.cmbMarkedForAlert.RowHeight = ((short)(22));
            this.cmbMarkedForAlert.SecondaryData = null;
            this.cmbMarkedForAlert.SelectedData = null;
            this.cmbMarkedForAlert.SelectedDataID = null;
            this.cmbMarkedForAlert.SelectionList = null;
            this.cmbMarkedForAlert.SkipIDColumn = true;
            this.cmbMarkedForAlert.RequestData += new System.EventHandler(this.cmb_RequestData);
            // 
            // btnsOrderPane
            // 
            this.btnsOrderPane.AddButtonEnabled = true;
            this.btnsOrderPane.BackColor = System.Drawing.Color.Transparent;
            this.btnsOrderPane.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsOrderPane.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsOrderPane, "btnsOrderPane");
            this.btnsOrderPane.Name = "btnsOrderPane";
            this.btnsOrderPane.RemoveButtonEnabled = false;
            // 
            // btnsOrder
            // 
            this.btnsOrder.AddButtonEnabled = true;
            this.btnsOrder.BackColor = System.Drawing.Color.Transparent;
            this.btnsOrder.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsOrder.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsOrder, "btnsOrder");
            this.btnsOrder.Name = "btnsOrder";
            this.btnsOrder.RemoveButtonEnabled = false;
            // 
            // cmbOrder
            // 
            this.cmbOrder.AddList = null;
            this.cmbOrder.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOrder, "cmbOrder");
            this.cmbOrder.MaxLength = 32767;
            this.cmbOrder.Name = "cmbOrder";
            this.cmbOrder.NoChangeAllowed = false;
            this.cmbOrder.OnlyDisplayID = false;
            this.cmbOrder.RemoveList = null;
            this.cmbOrder.RowHeight = ((short)(22));
            this.cmbOrder.SecondaryData = null;
            this.cmbOrder.SelectedData = null;
            this.cmbOrder.SelectedDataID = null;
            this.cmbOrder.SelectionList = null;
            this.cmbOrder.SkipIDColumn = true;
            this.cmbOrder.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbOrder.SelectedDataChanged += new System.EventHandler(this.cmbOrder_SelectedDataChanged);
            this.cmbOrder.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbOrderPane
            // 
            this.cmbOrderPane.AddList = null;
            this.cmbOrderPane.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOrderPane, "cmbOrderPane");
            this.cmbOrderPane.MaxLength = 32767;
            this.cmbOrderPane.Name = "cmbOrderPane";
            this.cmbOrderPane.NoChangeAllowed = false;
            this.cmbOrderPane.OnlyDisplayID = false;
            this.cmbOrderPane.RemoveList = null;
            this.cmbOrderPane.RowHeight = ((short)(22));
            this.cmbOrderPane.SecondaryData = null;
            this.cmbOrderPane.SelectedData = null;
            this.cmbOrderPane.SelectedDataID = null;
            this.cmbOrderPane.SelectionList = null;
            this.cmbOrderPane.SkipIDColumn = true;
            this.cmbOrderPane.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbOrderPane.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // StyleProfileOrderPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.cwSeparatorColor);
            this.Controls.Add(this.lblSeparator);
            this.Controls.Add(this.cmbDoneChitOverlay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnsMarkedForAlert);
            this.Controls.Add(this.cmbMarkedForAlert);
            this.Controls.Add(this.btnsOrderPane);
            this.Controls.Add(this.btnsOrder);
            this.Controls.Add(this.cmbOrder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbOrderPane);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Name = "StyleProfileOrderPage";
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private LSOne.Controls.ContextButtons btnsEditAddRemove;
        private LSOne.Controls.ListView lvTimeStyles;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
        private LSOne.Controls.LinkFields linkFields1;
        private LSOne.Controls.ColorWell cwSeparatorColor;
        private System.Windows.Forms.Label lblSeparator;
        private System.Windows.Forms.ComboBox cmbDoneChitOverlay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private KdsStyleButtons btnsMarkedForAlert;
        private LSOne.Controls.DualDataComboBox cmbMarkedForAlert;
        private KdsStyleButtons btnsOrderPane;
        private KdsStyleButtons btnsOrder;
        private LSOne.Controls.DualDataComboBox cmbOrder;
        private System.Windows.Forms.Label label7;
        private LSOne.Controls.DualDataComboBox cmbOrderPane;
        private System.Windows.Forms.Label label1;
    }
}
