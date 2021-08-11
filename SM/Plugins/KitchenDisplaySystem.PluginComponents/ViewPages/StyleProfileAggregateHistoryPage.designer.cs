using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StyleProfileAggregateHistoryPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileAggregateHistoryPage));
            this.cmbAggregateHeader = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbAggregateBody = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnsAggregatePane = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbAggregatePane = new LSOne.Controls.DualDataComboBox();
            this.btnsAggregateBody = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.btnsAggregateHeader = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnsHistoryPane = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbHistoryPane = new LSOne.Controls.DualDataComboBox();
            this.btnsHistoryBody = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbHistoryBody = new LSOne.Controls.DualDataComboBox();
            this.btnsHistoryHeader = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbHistoryHeader = new LSOne.Controls.DualDataComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbAggregateHeader
            // 
            this.cmbAggregateHeader.AddList = null;
            this.cmbAggregateHeader.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAggregateHeader, "cmbAggregateHeader");
            this.cmbAggregateHeader.MaxLength = 32767;
            this.cmbAggregateHeader.Name = "cmbAggregateHeader";
            this.cmbAggregateHeader.NoChangeAllowed = false;
            this.cmbAggregateHeader.OnlyDisplayID = false;
            this.cmbAggregateHeader.RemoveList = null;
            this.cmbAggregateHeader.RowHeight = ((short)(22));
            this.cmbAggregateHeader.SecondaryData = null;
            this.cmbAggregateHeader.SelectedData = null;
            this.cmbAggregateHeader.SelectedDataID = null;
            this.cmbAggregateHeader.SelectionList = null;
            this.cmbAggregateHeader.SkipIDColumn = true;
            this.cmbAggregateHeader.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbAggregateHeader.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbAggregateBody
            // 
            this.cmbAggregateBody.AddList = null;
            this.cmbAggregateBody.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAggregateBody, "cmbAggregateBody");
            this.cmbAggregateBody.MaxLength = 32767;
            this.cmbAggregateBody.Name = "cmbAggregateBody";
            this.cmbAggregateBody.NoChangeAllowed = false;
            this.cmbAggregateBody.OnlyDisplayID = false;
            this.cmbAggregateBody.RemoveList = null;
            this.cmbAggregateBody.RowHeight = ((short)(22));
            this.cmbAggregateBody.SecondaryData = null;
            this.cmbAggregateBody.SelectedData = null;
            this.cmbAggregateBody.SelectedDataID = null;
            this.cmbAggregateBody.SelectionList = null;
            this.cmbAggregateBody.SkipIDColumn = true;
            this.cmbAggregateBody.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbAggregateBody.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnsAggregatePane);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbAggregatePane);
            this.groupBox1.Controls.Add(this.btnsAggregateBody);
            this.groupBox1.Controls.Add(this.btnsAggregateHeader);
            this.groupBox1.Controls.Add(this.cmbAggregateHeader);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbAggregateBody);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnsAggregatePane
            // 
            this.btnsAggregatePane.AddButtonEnabled = true;
            this.btnsAggregatePane.BackColor = System.Drawing.Color.Transparent;
            this.btnsAggregatePane.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsAggregatePane.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsAggregatePane, "btnsAggregatePane");
            this.btnsAggregatePane.Name = "btnsAggregatePane";
            this.btnsAggregatePane.RemoveButtonEnabled = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbAggregatePane
            // 
            this.cmbAggregatePane.AddList = null;
            this.cmbAggregatePane.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbAggregatePane, "cmbAggregatePane");
            this.cmbAggregatePane.MaxLength = 32767;
            this.cmbAggregatePane.Name = "cmbAggregatePane";
            this.cmbAggregatePane.NoChangeAllowed = false;
            this.cmbAggregatePane.OnlyDisplayID = false;
            this.cmbAggregatePane.RemoveList = null;
            this.cmbAggregatePane.RowHeight = ((short)(22));
            this.cmbAggregatePane.SecondaryData = null;
            this.cmbAggregatePane.SelectedData = null;
            this.cmbAggregatePane.SelectedDataID = null;
            this.cmbAggregatePane.SelectionList = null;
            this.cmbAggregatePane.SkipIDColumn = true;
            this.cmbAggregatePane.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbAggregatePane.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // btnsAggregateBody
            // 
            this.btnsAggregateBody.AddButtonEnabled = true;
            this.btnsAggregateBody.BackColor = System.Drawing.Color.Transparent;
            this.btnsAggregateBody.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsAggregateBody.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsAggregateBody, "btnsAggregateBody");
            this.btnsAggregateBody.Name = "btnsAggregateBody";
            this.btnsAggregateBody.RemoveButtonEnabled = false;
            // 
            // btnsAggregateHeader
            // 
            this.btnsAggregateHeader.AddButtonEnabled = true;
            this.btnsAggregateHeader.BackColor = System.Drawing.Color.Transparent;
            this.btnsAggregateHeader.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsAggregateHeader.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsAggregateHeader, "btnsAggregateHeader");
            this.btnsAggregateHeader.Name = "btnsAggregateHeader";
            this.btnsAggregateHeader.RemoveButtonEnabled = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnsHistoryPane);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbHistoryPane);
            this.groupBox2.Controls.Add(this.btnsHistoryBody);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbHistoryBody);
            this.groupBox2.Controls.Add(this.btnsHistoryHeader);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbHistoryHeader);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnsHistoryPane
            // 
            this.btnsHistoryPane.AddButtonEnabled = true;
            this.btnsHistoryPane.BackColor = System.Drawing.Color.Transparent;
            this.btnsHistoryPane.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsHistoryPane.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsHistoryPane, "btnsHistoryPane");
            this.btnsHistoryPane.Name = "btnsHistoryPane";
            this.btnsHistoryPane.RemoveButtonEnabled = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbHistoryPane
            // 
            this.cmbHistoryPane.AddList = null;
            this.cmbHistoryPane.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHistoryPane, "cmbHistoryPane");
            this.cmbHistoryPane.MaxLength = 32767;
            this.cmbHistoryPane.Name = "cmbHistoryPane";
            this.cmbHistoryPane.NoChangeAllowed = false;
            this.cmbHistoryPane.OnlyDisplayID = false;
            this.cmbHistoryPane.RemoveList = null;
            this.cmbHistoryPane.RowHeight = ((short)(22));
            this.cmbHistoryPane.SecondaryData = null;
            this.cmbHistoryPane.SelectedData = null;
            this.cmbHistoryPane.SelectedDataID = null;
            this.cmbHistoryPane.SelectionList = null;
            this.cmbHistoryPane.SkipIDColumn = true;
            this.cmbHistoryPane.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbHistoryPane.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // btnsHistoryBody
            // 
            this.btnsHistoryBody.AddButtonEnabled = true;
            this.btnsHistoryBody.BackColor = System.Drawing.Color.Transparent;
            this.btnsHistoryBody.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsHistoryBody.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsHistoryBody, "btnsHistoryBody");
            this.btnsHistoryBody.Name = "btnsHistoryBody";
            this.btnsHistoryBody.RemoveButtonEnabled = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbHistoryBody
            // 
            this.cmbHistoryBody.AddList = null;
            this.cmbHistoryBody.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHistoryBody, "cmbHistoryBody");
            this.cmbHistoryBody.MaxLength = 32767;
            this.cmbHistoryBody.Name = "cmbHistoryBody";
            this.cmbHistoryBody.NoChangeAllowed = false;
            this.cmbHistoryBody.OnlyDisplayID = false;
            this.cmbHistoryBody.RemoveList = null;
            this.cmbHistoryBody.RowHeight = ((short)(22));
            this.cmbHistoryBody.SecondaryData = null;
            this.cmbHistoryBody.SelectedData = null;
            this.cmbHistoryBody.SelectedDataID = null;
            this.cmbHistoryBody.SelectionList = null;
            this.cmbHistoryBody.SkipIDColumn = true;
            this.cmbHistoryBody.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbHistoryBody.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // btnsHistoryHeader
            // 
            this.btnsHistoryHeader.AddButtonEnabled = true;
            this.btnsHistoryHeader.BackColor = System.Drawing.Color.Transparent;
            this.btnsHistoryHeader.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsHistoryHeader.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsHistoryHeader, "btnsHistoryHeader");
            this.btnsHistoryHeader.Name = "btnsHistoryHeader";
            this.btnsHistoryHeader.RemoveButtonEnabled = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbHistoryHeader
            // 
            this.cmbHistoryHeader.AddList = null;
            this.cmbHistoryHeader.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHistoryHeader, "cmbHistoryHeader");
            this.cmbHistoryHeader.MaxLength = 32767;
            this.cmbHistoryHeader.Name = "cmbHistoryHeader";
            this.cmbHistoryHeader.NoChangeAllowed = false;
            this.cmbHistoryHeader.OnlyDisplayID = false;
            this.cmbHistoryHeader.RemoveList = null;
            this.cmbHistoryHeader.RowHeight = ((short)(22));
            this.cmbHistoryHeader.SecondaryData = null;
            this.cmbHistoryHeader.SelectedData = null;
            this.cmbHistoryHeader.SelectedDataID = null;
            this.cmbHistoryHeader.SelectionList = null;
            this.cmbHistoryHeader.SkipIDColumn = true;
            this.cmbHistoryHeader.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbHistoryHeader.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // StyleProfileAggregateHistoryPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "StyleProfileAggregateHistoryPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.DualDataComboBox cmbAggregateHeader;
        private System.Windows.Forms.Label label7;
        private LSOne.Controls.DualDataComboBox cmbAggregateBody;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.KdsStyleButtons btnsAggregateBody;
        private Controls.KdsStyleButtons btnsAggregateHeader;
        private Controls.KdsStyleButtons btnsAggregatePane;
        private System.Windows.Forms.Label label5;
        private LSOne.Controls.DualDataComboBox cmbAggregatePane;
        private System.Windows.Forms.GroupBox groupBox2;
        private KdsStyleButtons btnsHistoryPane;
        private System.Windows.Forms.Label label4;
        private LSOne.Controls.DualDataComboBox cmbHistoryPane;
        private KdsStyleButtons btnsHistoryBody;
        private System.Windows.Forms.Label label3;
        private LSOne.Controls.DualDataComboBox cmbHistoryBody;
        private KdsStyleButtons btnsHistoryHeader;
        private System.Windows.Forms.Label label2;
        private LSOne.Controls.DualDataComboBox cmbHistoryHeader;
    }
}
