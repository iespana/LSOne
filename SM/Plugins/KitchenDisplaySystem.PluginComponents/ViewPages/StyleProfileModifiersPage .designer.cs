using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StyleProfileModifiersPage 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileModifiersPage));
            this.btnsVoided = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.btnsComment = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.btnsIncrease = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVoided = new LSOne.Controls.DualDataComboBox();
            this.cmbInfocode = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbComment = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // btnsVoided
            // 
            this.btnsVoided.AddButtonEnabled = true;
            this.btnsVoided.BackColor = System.Drawing.Color.Transparent;
            this.btnsVoided.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsVoided.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsVoided, "btnsVoided");
            this.btnsVoided.Name = "btnsVoided";
            this.btnsVoided.RemoveButtonEnabled = false;
            // 
            // btnsComment
            // 
            this.btnsComment.AddButtonEnabled = true;
            this.btnsComment.BackColor = System.Drawing.Color.Transparent;
            this.btnsComment.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsComment.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsComment, "btnsComment");
            this.btnsComment.Name = "btnsComment";
            this.btnsComment.RemoveButtonEnabled = false;
            // 
            // btnsIncrease
            // 
            this.btnsIncrease.AddButtonEnabled = true;
            this.btnsIncrease.BackColor = System.Drawing.Color.Transparent;
            this.btnsIncrease.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsIncrease.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsIncrease, "btnsIncrease");
            this.btnsIncrease.Name = "btnsIncrease";
            this.btnsIncrease.RemoveButtonEnabled = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbVoided
            // 
            this.cmbVoided.AddList = null;
            this.cmbVoided.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVoided, "cmbVoided");
            this.cmbVoided.MaxLength = 32767;
            this.cmbVoided.Name = "cmbVoided";
            this.cmbVoided.NoChangeAllowed = false;
            this.cmbVoided.OnlyDisplayID = false;
            this.cmbVoided.RemoveList = null;
            this.cmbVoided.RowHeight = ((short)(22));
            this.cmbVoided.SecondaryData = null;
            this.cmbVoided.SelectedData = null;
            this.cmbVoided.SelectedDataID = null;
            this.cmbVoided.SelectionList = null;
            this.cmbVoided.SkipIDColumn = true;
            this.cmbVoided.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbVoided.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // cmbInfocode
            // 
            this.cmbInfocode.AddList = null;
            this.cmbInfocode.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbInfocode, "cmbInfocode");
            this.cmbInfocode.MaxLength = 32767;
            this.cmbInfocode.Name = "cmbInfocode";
            this.cmbInfocode.NoChangeAllowed = false;
            this.cmbInfocode.OnlyDisplayID = false;
            this.cmbInfocode.RemoveList = null;
            this.cmbInfocode.RowHeight = ((short)(22));
            this.cmbInfocode.SecondaryData = null;
            this.cmbInfocode.SelectedData = null;
            this.cmbInfocode.SelectedDataID = null;
            this.cmbInfocode.SelectionList = null;
            this.cmbInfocode.SkipIDColumn = true;
            this.cmbInfocode.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbInfocode.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbComment
            // 
            this.cmbComment.AddList = null;
            this.cmbComment.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbComment, "cmbComment");
            this.cmbComment.MaxLength = 32767;
            this.cmbComment.Name = "cmbComment";
            this.cmbComment.NoChangeAllowed = false;
            this.cmbComment.OnlyDisplayID = false;
            this.cmbComment.RemoveList = null;
            this.cmbComment.RowHeight = ((short)(22));
            this.cmbComment.SecondaryData = null;
            this.cmbComment.SelectedData = null;
            this.cmbComment.SelectedDataID = null;
            this.cmbComment.SelectionList = null;
            this.cmbComment.SkipIDColumn = true;
            this.cmbComment.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbComment.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // StyleProfileModifiersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsVoided);
            this.Controls.Add(this.btnsComment);
            this.Controls.Add(this.btnsIncrease);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbVoided);
            this.Controls.Add(this.cmbInfocode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbComment);
            this.Name = "StyleProfileModifiersPage";
            this.ResumeLayout(false);

        }

        #endregion

        private KdsStyleButtons btnsVoided;
        private KdsStyleButtons btnsComment;
        private KdsStyleButtons btnsIncrease;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.DualDataComboBox cmbVoided;
        private LSOne.Controls.DualDataComboBox cmbInfocode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private LSOne.Controls.DualDataComboBox cmbComment;
    }
}
