using LSOne.Controls;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class TimeStyleDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeStyleDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbOrderStyle = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ntbSecondsUntilActive = new LSOne.Controls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsOrderStyle = new KdsStyleButtons();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbOrderStyle
            // 
            this.cmbOrderStyle.AddList = null;
            this.cmbOrderStyle.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOrderStyle, "cmbOrderStyle");
            this.cmbOrderStyle.MaxLength = 32767;
            this.cmbOrderStyle.Name = "cmbOrderStyle";
            this.cmbOrderStyle.NoChangeAllowed = false;
            this.cmbOrderStyle.OnlyDisplayID = false;
            this.cmbOrderStyle.RemoveList = null;
            this.cmbOrderStyle.RowHeight = ((short)(22));
            this.cmbOrderStyle.SecondaryData = null;
            this.cmbOrderStyle.SelectedData = null;
            this.cmbOrderStyle.SelectedDataID = null;
            this.cmbOrderStyle.SelectionList = null;
            this.cmbOrderStyle.SkipIDColumn = true;
            this.cmbOrderStyle.RequestData += new System.EventHandler(this.cmbOrderStyle_RequestData);
            this.cmbOrderStyle.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ntbSecondsUntilActive
            // 
            this.ntbSecondsUntilActive.AllowDecimal = false;
            this.ntbSecondsUntilActive.AllowNegative = false;
            this.ntbSecondsUntilActive.CultureInfo = null;
            this.ntbSecondsUntilActive.DecimalLetters = 0;
            this.ntbSecondsUntilActive.ForeColor = System.Drawing.Color.Black;
            this.ntbSecondsUntilActive.HasMinValue = false;
            resources.ApplyResources(this.ntbSecondsUntilActive, "ntbSecondsUntilActive");
            this.ntbSecondsUntilActive.MaxValue = 0D;
            this.ntbSecondsUntilActive.MinValue = 0D;
            this.ntbSecondsUntilActive.Name = "ntbSecondsUntilActive";
            this.ntbSecondsUntilActive.Value = 0D;
            this.ntbSecondsUntilActive.ValueChanged += new System.EventHandler(this.CheckEnabled);
            this.ntbSecondsUntilActive.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnsOrderStyle
            // 
            this.btnsOrderStyle.AddButtonEnabled = true;
            this.btnsOrderStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnsOrderStyle.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsOrderStyle.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsOrderStyle, "btnsOrderStyle");
            this.btnsOrderStyle.Name = "btnsOrderStyle";
            this.btnsOrderStyle.RemoveButtonEnabled = false;
            // 
            // TimeStyleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsOrderStyle);
            this.Controls.Add(this.ntbSecondsUntilActive);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbOrderStyle);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "TimeStyleDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbOrderStyle, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.ntbSecondsUntilActive, 0);
            this.Controls.SetChildIndex(this.btnsOrderStyle, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbOrderStyle;
        private System.Windows.Forms.Label label2;
        private NumericTextBox ntbSecondsUntilActive;
        private System.Windows.Forms.Label label3;
        private Controls.KdsStyleButtons btnsOrderStyle;
    }
}