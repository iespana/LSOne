using LSOne.Controls;

namespace LSOne.ViewPlugins.LabelPrinting.ViewPages
{
    partial class LabelTemplatePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelTemplatePage));
            this.lblDescription = new System.Windows.Forms.Label();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetSample = new ContextButton();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.picSample = new System.Windows.Forms.PictureBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblSample = new System.Windows.Forms.Label();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.btnGetMacros = new ContextButton();
            this.lblCodepage = new System.Windows.Forms.Label();
            this.txtCodepage = new System.Windows.Forms.TextBox();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSample)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Name = "lblDescription";
            // 
            // mainLayout
            // 
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Controls.Add(this.btnSetSample, 2, 3);
            this.mainLayout.Controls.Add(this.lblTemplate, 0, 2);
            this.mainLayout.Controls.Add(this.picSample, 1, 3);
            this.mainLayout.Controls.Add(this.lblDescription, 0, 0);
            this.mainLayout.Controls.Add(this.txtDescription, 1, 0);
            this.mainLayout.Controls.Add(this.lblSample, 0, 3);
            this.mainLayout.Controls.Add(this.txtTemplate, 1, 2);
            this.mainLayout.Controls.Add(this.btnGetMacros, 2, 2);
            this.mainLayout.Controls.Add(this.lblCodepage, 0, 1);
            this.mainLayout.Controls.Add(this.txtCodepage, 1, 1);
            this.mainLayout.Name = "mainLayout";
            // 
            // btnSetSample
            // 
            resources.ApplyResources(this.btnSetSample, "btnSetSample");
            this.btnSetSample.Context = ButtonType.Edit;
            this.btnSetSample.Name = "btnSetSample";
            this.btnSetSample.Click += new System.EventHandler(this.btnSetSample_Click);
            // 
            // lblTemplate
            // 
            resources.ApplyResources(this.lblTemplate, "lblTemplate");
            this.lblTemplate.BackColor = System.Drawing.Color.Transparent;
            this.lblTemplate.Name = "lblTemplate";
            // 
            // picSample
            // 
            resources.ApplyResources(this.picSample, "picSample");
            this.picSample.Name = "picSample";
            this.picSample.TabStop = false;
            // 
            // txtDescription
            // 
            this.mainLayout.SetColumnSpan(this.txtDescription, 2);
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            // 
            // lblSample
            // 
            resources.ApplyResources(this.lblSample, "lblSample");
            this.lblSample.BackColor = System.Drawing.Color.Transparent;
            this.lblSample.Name = "lblSample";
            // 
            // txtTemplate
            // 
            resources.ApplyResources(this.txtTemplate, "txtTemplate");
            this.txtTemplate.Name = "txtTemplate";
            // 
            // btnGetMacros
            // 
            resources.ApplyResources(this.btnGetMacros, "btnGetMacros");
            this.btnGetMacros.Context = ButtonType.Edit;
            this.btnGetMacros.ImageOverride = global::LSOne.ViewPlugins.LabelPrinting.Properties.Resources.Label_16;
            this.btnGetMacros.Name = "btnGetMacros";
            this.btnGetMacros.Click += new System.EventHandler(this.btnGetMacros_Click);
            // 
            // lblCodepage
            // 
            resources.ApplyResources(this.lblCodepage, "lblCodepage");
            this.lblCodepage.Name = "lblCodepage";
            // 
            // tbCodepage
            // 
            resources.ApplyResources(this.txtCodepage, "txtCodepage");
            this.txtCodepage.Name = "txtCodepage";
            // 
            // LabelTemplatePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.mainLayout);
            this.DoubleBuffered = true;
            this.Name = "LabelTemplatePage";
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.PictureBox picSample;
        private ContextButton btnSetSample;
        private ContextButton btnGetMacros;
        private System.Windows.Forms.Label lblCodepage;
        private System.Windows.Forms.TextBox txtCodepage;

    }
}
