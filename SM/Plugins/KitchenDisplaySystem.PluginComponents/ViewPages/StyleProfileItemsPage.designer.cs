using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class StyleProfileItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileItemsPage));
            this.label6 = new System.Windows.Forms.Label();
            this.btnsRush = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbRush = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnsModified = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnsOnTime = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label11 = new System.Windows.Forms.Label();
            this.btnsDone = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.label12 = new System.Windows.Forms.Label();
            this.btnsVoided = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbVoided = new LSOne.Controls.DualDataComboBox();
            this.btnsDefault = new LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls.KdsStyleButtons();
            this.cmbModified = new LSOne.Controls.DualDataComboBox();
            this.cmbDefault = new LSOne.Controls.DualDataComboBox();
            this.cmbDone = new LSOne.Controls.DualDataComboBox();
            this.cmbOnTime = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // btnsRush
            // 
            this.btnsRush.AddButtonEnabled = true;
            this.btnsRush.BackColor = System.Drawing.Color.Transparent;
            this.btnsRush.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsRush.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsRush, "btnsRush");
            this.btnsRush.Name = "btnsRush";
            this.btnsRush.RemoveButtonEnabled = false;
            // 
            // cmbRush
            // 
            this.cmbRush.AddList = null;
            this.cmbRush.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRush, "cmbRush");
            this.cmbRush.MaxLength = 32767;
            this.cmbRush.Name = "cmbRush";
            this.cmbRush.NoChangeAllowed = false;
            this.cmbRush.OnlyDisplayID = false;
            this.cmbRush.RemoveList = null;
            this.cmbRush.RowHeight = ((short)(22));
            this.cmbRush.SecondaryData = null;
            this.cmbRush.SelectedData = null;
            this.cmbRush.SelectedDataID = null;
            this.cmbRush.SelectionList = null;
            this.cmbRush.SkipIDColumn = true;
            this.cmbRush.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbRush.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnsModified
            // 
            this.btnsModified.AddButtonEnabled = true;
            this.btnsModified.BackColor = System.Drawing.Color.Transparent;
            this.btnsModified.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsModified.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsModified, "btnsModified");
            this.btnsModified.Name = "btnsModified";
            this.btnsModified.RemoveButtonEnabled = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // btnsOnTime
            // 
            this.btnsOnTime.AddButtonEnabled = true;
            this.btnsOnTime.BackColor = System.Drawing.Color.Transparent;
            this.btnsOnTime.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsOnTime.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsOnTime, "btnsOnTime");
            this.btnsOnTime.Name = "btnsOnTime";
            this.btnsOnTime.RemoveButtonEnabled = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // btnsDone
            // 
            this.btnsDone.AddButtonEnabled = true;
            this.btnsDone.BackColor = System.Drawing.Color.Transparent;
            this.btnsDone.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDone.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsDone, "btnsDone");
            this.btnsDone.Name = "btnsDone";
            this.btnsDone.RemoveButtonEnabled = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
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
            // btnsDefault
            // 
            this.btnsDefault.AddButtonEnabled = true;
            this.btnsDefault.BackColor = System.Drawing.Color.Transparent;
            this.btnsDefault.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDefault.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsDefault, "btnsDefault");
            this.btnsDefault.Name = "btnsDefault";
            this.btnsDefault.RemoveButtonEnabled = false;
            // 
            // cmbModified
            // 
            this.cmbModified.AddList = null;
            this.cmbModified.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbModified, "cmbModified");
            this.cmbModified.MaxLength = 32767;
            this.cmbModified.Name = "cmbModified";
            this.cmbModified.NoChangeAllowed = false;
            this.cmbModified.OnlyDisplayID = false;
            this.cmbModified.RemoveList = null;
            this.cmbModified.RowHeight = ((short)(22));
            this.cmbModified.SecondaryData = null;
            this.cmbModified.SelectedData = null;
            this.cmbModified.SelectedDataID = null;
            this.cmbModified.SelectionList = null;
            this.cmbModified.SkipIDColumn = true;
            this.cmbModified.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbModified.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // cmbDefault
            // 
            this.cmbDefault.AddList = null;
            this.cmbDefault.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDefault, "cmbDefault");
            this.cmbDefault.MaxLength = 32767;
            this.cmbDefault.Name = "cmbDefault";
            this.cmbDefault.NoChangeAllowed = false;
            this.cmbDefault.OnlyDisplayID = false;
            this.cmbDefault.RemoveList = null;
            this.cmbDefault.RowHeight = ((short)(22));
            this.cmbDefault.SecondaryData = null;
            this.cmbDefault.SelectedData = null;
            this.cmbDefault.SelectedDataID = null;
            this.cmbDefault.SelectionList = null;
            this.cmbDefault.SkipIDColumn = true;
            this.cmbDefault.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbDefault.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // cmbDone
            // 
            this.cmbDone.AddList = null;
            this.cmbDone.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDone, "cmbDone");
            this.cmbDone.MaxLength = 32767;
            this.cmbDone.Name = "cmbDone";
            this.cmbDone.NoChangeAllowed = false;
            this.cmbDone.OnlyDisplayID = false;
            this.cmbDone.RemoveList = null;
            this.cmbDone.RowHeight = ((short)(22));
            this.cmbDone.SecondaryData = null;
            this.cmbDone.SelectedData = null;
            this.cmbDone.SelectedDataID = null;
            this.cmbDone.SelectionList = null;
            this.cmbDone.SkipIDColumn = true;
            this.cmbDone.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbDone.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // cmbOnTime
            // 
            this.cmbOnTime.AddList = null;
            this.cmbOnTime.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOnTime, "cmbOnTime");
            this.cmbOnTime.MaxLength = 32767;
            this.cmbOnTime.Name = "cmbOnTime";
            this.cmbOnTime.NoChangeAllowed = false;
            this.cmbOnTime.OnlyDisplayID = false;
            this.cmbOnTime.RemoveList = null;
            this.cmbOnTime.RowHeight = ((short)(22));
            this.cmbOnTime.SecondaryData = null;
            this.cmbOnTime.SelectedData = null;
            this.cmbOnTime.SelectedDataID = null;
            this.cmbOnTime.SelectionList = null;
            this.cmbOnTime.SkipIDColumn = true;
            this.cmbOnTime.RequestData += new System.EventHandler(this.cmb_RequestData);
            this.cmbOnTime.RequestClear += new System.EventHandler(this.cmb_RequestClear);
            // 
            // StyleProfileItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnsRush);
            this.Controls.Add(this.cmbRush);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnsModified);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnsOnTime);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnsDone);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnsVoided);
            this.Controls.Add(this.cmbVoided);
            this.Controls.Add(this.btnsDefault);
            this.Controls.Add(this.cmbModified);
            this.Controls.Add(this.cmbDefault);
            this.Controls.Add(this.cmbDone);
            this.Controls.Add(this.cmbOnTime);
            this.Name = "StyleProfileItemsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private KdsStyleButtons btnsRush;
        private LSOne.Controls.DualDataComboBox cmbRush;
        private System.Windows.Forms.Label label8;
        private KdsStyleButtons btnsModified;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private KdsStyleButtons btnsOnTime;
        private System.Windows.Forms.Label label11;
        private KdsStyleButtons btnsDone;
        private System.Windows.Forms.Label label12;
        private KdsStyleButtons btnsVoided;
        private LSOne.Controls.DualDataComboBox cmbVoided;
        private KdsStyleButtons btnsDefault;
        private LSOne.Controls.DualDataComboBox cmbModified;
        private LSOne.Controls.DualDataComboBox cmbDefault;
        private LSOne.Controls.DualDataComboBox cmbDone;
        private LSOne.Controls.DualDataComboBox cmbOnTime;
    }
}
