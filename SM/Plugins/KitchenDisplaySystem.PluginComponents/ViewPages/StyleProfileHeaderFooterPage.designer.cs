using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StyleProfileHeaderFooterPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileHeaderFooterPage));
            this.btnsTransactComment = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTransactComment = new LSOne.Controls.DualDataComboBox();
            this.btnsDefaultFooter = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.btnsDefaultHeader = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbDefaultHeader = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDefaultFooter = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // btnsTransactComment
            // 
            this.btnsTransactComment.AddButtonEnabled = true;
            this.btnsTransactComment.BackColor = System.Drawing.Color.Transparent;
            this.btnsTransactComment.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsTransactComment.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsTransactComment, "btnsTransactComment");
            this.btnsTransactComment.Name = "btnsTransactComment";
            this.btnsTransactComment.RemoveButtonEnabled = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbTransactComment
            // 
            this.cmbTransactComment.AddList = null;
            this.cmbTransactComment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTransactComment, "cmbTransactComment");
            this.cmbTransactComment.MaxLength = 32767;
            this.cmbTransactComment.Name = "cmbTransactComment";
            this.cmbTransactComment.NoChangeAllowed = false;
            this.cmbTransactComment.OnlyDisplayID = false;
            this.cmbTransactComment.RemoveList = null;
            this.cmbTransactComment.RowHeight = ((short)(22));
            this.cmbTransactComment.SecondaryData = null;
            this.cmbTransactComment.SelectedData = null;
            this.cmbTransactComment.SelectedDataID = null;
            this.cmbTransactComment.SelectionList = null;
            this.cmbTransactComment.SkipIDColumn = true;
            this.cmbTransactComment.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbTransactComment.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // btnsDefaultFooter
            // 
            this.btnsDefaultFooter.AddButtonEnabled = true;
            this.btnsDefaultFooter.BackColor = System.Drawing.Color.Transparent;
            this.btnsDefaultFooter.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDefaultFooter.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsDefaultFooter, "btnsDefaultFooter");
            this.btnsDefaultFooter.Name = "btnsDefaultFooter";
            this.btnsDefaultFooter.RemoveButtonEnabled = false;
            // 
            // btnsDefaultHeader
            // 
            this.btnsDefaultHeader.AddButtonEnabled = true;
            this.btnsDefaultHeader.BackColor = System.Drawing.Color.Transparent;
            this.btnsDefaultHeader.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDefaultHeader.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsDefaultHeader, "btnsDefaultHeader");
            this.btnsDefaultHeader.Name = "btnsDefaultHeader";
            this.btnsDefaultHeader.RemoveButtonEnabled = false;
            // 
            // cmbDefaultHeader
            // 
            this.cmbDefaultHeader.AddList = null;
            this.cmbDefaultHeader.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDefaultHeader, "cmbDefaultHeader");
            this.cmbDefaultHeader.MaxLength = 32767;
            this.cmbDefaultHeader.Name = "cmbDefaultHeader";
            this.cmbDefaultHeader.NoChangeAllowed = false;
            this.cmbDefaultHeader.OnlyDisplayID = false;
            this.cmbDefaultHeader.RemoveList = null;
            this.cmbDefaultHeader.RowHeight = ((short)(22));
            this.cmbDefaultHeader.SecondaryData = null;
            this.cmbDefaultHeader.SelectedData = null;
            this.cmbDefaultHeader.SelectedDataID = null;
            this.cmbDefaultHeader.SelectionList = null;
            this.cmbDefaultHeader.SkipIDColumn = true;
            this.cmbDefaultHeader.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbDefaultHeader.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbDefaultFooter
            // 
            this.cmbDefaultFooter.AddList = null;
            this.cmbDefaultFooter.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDefaultFooter, "cmbDefaultFooter");
            this.cmbDefaultFooter.MaxLength = 32767;
            this.cmbDefaultFooter.Name = "cmbDefaultFooter";
            this.cmbDefaultFooter.NoChangeAllowed = false;
            this.cmbDefaultFooter.OnlyDisplayID = false;
            this.cmbDefaultFooter.RemoveList = null;
            this.cmbDefaultFooter.RowHeight = ((short)(22));
            this.cmbDefaultFooter.SecondaryData = null;
            this.cmbDefaultFooter.SelectedData = null;
            this.cmbDefaultFooter.SelectedDataID = null;
            this.cmbDefaultFooter.SelectionList = null;
            this.cmbDefaultFooter.SkipIDColumn = true;
            this.cmbDefaultFooter.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbDefaultFooter.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // StyleProfileHeaderFooterPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsTransactComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbTransactComment);
            this.Controls.Add(this.btnsDefaultFooter);
            this.Controls.Add(this.btnsDefaultHeader);
            this.Controls.Add(this.cmbDefaultHeader);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDefaultFooter);
            this.Name = "StyleProfileHeaderFooterPage";
            this.ResumeLayout(false);

        }

        #endregion

        private KdsStyleButtons btnsTransactComment;
        private System.Windows.Forms.Label label2;
        private LSOne.Controls.DualDataComboBox cmbTransactComment;
        private KdsStyleButtons btnsDefaultFooter;
        private KdsStyleButtons btnsDefaultHeader;
        private LSOne.Controls.DualDataComboBox cmbDefaultHeader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.DualDataComboBox cmbDefaultFooter;
    }
}
