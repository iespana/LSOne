using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityPosMenuLineAttributesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityPosMenuLineAttributesPage));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGradientMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cwBackColor = new ColorWell();
            this.cwBackColor2 = new ColorWell();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkUseHeaderConfiguration = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbGradientMode
            // 
            this.cmbGradientMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbGradientMode, "cmbGradientMode");
            this.cmbGradientMode.Name = "cmbGradientMode";
            this.cmbGradientMode.SelectedIndexChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbShape
            // 
            this.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShape.FormattingEnabled = true;
            resources.ApplyResources(this.cmbShape, "cmbShape");
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.SelectedIndexChanged += new System.EventHandler(this.notifyPreviewMenuButtonChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkUseHeaderConfiguration
            // 
            resources.ApplyResources(this.chkUseHeaderConfiguration, "chkUseHeaderConfiguration");
            this.chkUseHeaderConfiguration.Name = "chkUseHeaderConfiguration";
            this.chkUseHeaderConfiguration.UseVisualStyleBackColor = true;
            this.chkUseHeaderConfiguration.CheckedChanged += new System.EventHandler(this.chkUseHeaderConfiguration_CheckedChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // HospitalityPosMenuLineAttributesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkUseHeaderConfiguration);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbShape);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cwBackColor2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cwBackColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbGradientMode);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityPosMenuLineAttributesPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbGradientMode;
        private System.Windows.Forms.Label label2;
        private ColorWell cwBackColor;
        private ColorWell cwBackColor2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkUseHeaderConfiguration;
        private System.Windows.Forms.Label label9;

    }
}
