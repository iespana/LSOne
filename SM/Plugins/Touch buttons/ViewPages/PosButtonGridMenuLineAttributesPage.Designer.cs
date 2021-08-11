using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    partial class PosButtonGridMenuLineAttributesPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosButtonGridMenuLineAttributesPage));
            this.lblGradientMode = new System.Windows.Forms.Label();
            this.cmbGradientMode = new System.Windows.Forms.ComboBox();
            this.lblBackcolor = new System.Windows.Forms.Label();
            this.lblBackcolor2 = new System.Windows.Forms.Label();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.lblShape = new System.Windows.Forms.Label();
            this.chkUseHeaderConfiguration = new System.Windows.Forms.CheckBox();
            this.lblUserConfiguration = new System.Windows.Forms.Label();
            this.cwBackColor = new LSOne.Controls.ColorWell();
            this.cwBackColor2 = new LSOne.Controls.ColorWell();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblGradientMode
            // 
            resources.ApplyResources(this.lblGradientMode, "lblGradientMode");
            this.lblGradientMode.Name = "lblGradientMode";
            // 
            // cmbGradientMode
            // 
            this.cmbGradientMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbGradientMode, "cmbGradientMode");
            this.cmbGradientMode.Name = "cmbGradientMode";
            this.cmbGradientMode.SelectedIndexChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // lblBackcolor
            // 
            resources.ApplyResources(this.lblBackcolor, "lblBackcolor");
            this.lblBackcolor.Name = "lblBackcolor";
            // 
            // lblBackcolor2
            // 
            resources.ApplyResources(this.lblBackcolor2, "lblBackcolor2");
            this.lblBackcolor2.Name = "lblBackcolor2";
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            resources.ApplyResources(this.cmbShape, "cmbShape");
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.SelectedIndexChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // lblShape
            // 
            resources.ApplyResources(this.lblShape, "lblShape");
            this.lblShape.Name = "lblShape";
            // 
            // chkUseHeaderConfiguration
            // 
            resources.ApplyResources(this.chkUseHeaderConfiguration, "chkUseHeaderConfiguration");
            this.chkUseHeaderConfiguration.Name = "chkUseHeaderConfiguration";
            this.chkUseHeaderConfiguration.UseVisualStyleBackColor = true;
            this.chkUseHeaderConfiguration.CheckedChanged += new System.EventHandler(this.chkUseHeaderConfiguration_CheckedChanged);
            // 
            // lblUserConfiguration
            // 
            resources.ApplyResources(this.lblUserConfiguration, "lblUserConfiguration");
            this.lblUserConfiguration.Name = "lblUserConfiguration";
            // 
            // cwBackColor
            // 
            resources.ApplyResources(this.cwBackColor, "cwBackColor");
            this.cwBackColor.Name = "cwBackColor";
            this.cwBackColor.SelectedColor = System.Drawing.Color.White;
            this.cwBackColor.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // cwBackColor2
            // 
            resources.ApplyResources(this.cwBackColor2, "cwBackColor2");
            this.cwBackColor2.Name = "cwBackColor2";
            this.cwBackColor2.SelectedColor = System.Drawing.Color.White;
            this.cwBackColor2.SelectedColorChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PosButtonGridMenuLineAttributesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseHeaderConfiguration);
            this.Controls.Add(this.lblUserConfiguration);
            this.Controls.Add(this.cmbShape);
            this.Controls.Add(this.lblShape);
            this.Controls.Add(this.cwBackColor2);
            this.Controls.Add(this.lblBackcolor2);
            this.Controls.Add(this.cwBackColor);
            this.Controls.Add(this.lblBackcolor);
            this.Controls.Add(this.cmbGradientMode);
            this.Controls.Add(this.lblGradientMode);
            this.DoubleBuffered = true;
            this.Name = "PosButtonGridMenuLineAttributesPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGradientMode;
        private System.Windows.Forms.ComboBox cmbGradientMode;
        private System.Windows.Forms.Label lblBackcolor;
        private ColorWell cwBackColor;
        private ColorWell cwBackColor2;
        private System.Windows.Forms.Label lblBackcolor2;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.Label lblShape;
        private System.Windows.Forms.CheckBox chkUseHeaderConfiguration;
        private System.Windows.Forms.Label lblUserConfiguration;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
