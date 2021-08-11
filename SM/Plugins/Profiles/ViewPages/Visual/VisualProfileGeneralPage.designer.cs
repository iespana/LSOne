using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    partial class VisualProfileGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfileGeneralPage));
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblResolution = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTerminalType = new System.Windows.Forms.ComboBox();
            this.chkCursorHide = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbResolution = new System.Windows.Forms.ComboBox();
            this.innerFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbScreenNumber = new System.Windows.Forms.ComboBox();
            this.lnkIdentify = new System.Windows.Forms.LinkLabel();
            this.layoutPanel.SuspendLayout();
            this.innerFlowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            resources.ApplyResources(this.layoutPanel, "layoutPanel");
            this.layoutPanel.Controls.Add(this.lblResolution, 0, 0);
            this.layoutPanel.Controls.Add(this.label1, 0, 1);
            this.layoutPanel.Controls.Add(this.label4, 0, 2);
            this.layoutPanel.Controls.Add(this.cmbTerminalType, 1, 1);
            this.layoutPanel.Controls.Add(this.chkCursorHide, 1, 2);
            this.layoutPanel.Controls.Add(this.label6, 0, 4);
            this.layoutPanel.Controls.Add(this.cmbResolution, 1, 0);
            this.layoutPanel.Controls.Add(this.innerFlowLayout, 1, 4);
            this.layoutPanel.Name = "layoutPanel";
            // 
            // lblResolution
            // 
            resources.ApplyResources(this.lblResolution, "lblResolution");
            this.lblResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblResolution.Name = "lblResolution";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // cmbTerminalType
            // 
            this.cmbTerminalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTerminalType.FormattingEnabled = true;
            this.cmbTerminalType.Items.AddRange(new object[] {
            resources.GetString("cmbTerminalType.Items"),
            resources.GetString("cmbTerminalType.Items1")});
            resources.ApplyResources(this.cmbTerminalType, "cmbTerminalType");
            this.cmbTerminalType.Name = "cmbTerminalType";
            // 
            // chkCursorHide
            // 
            resources.ApplyResources(this.chkCursorHide, "chkCursorHide");
            this.chkCursorHide.Name = "chkCursorHide";
            this.chkCursorHide.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // cmbResolution
            // 
            this.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResolution.FormattingEnabled = true;
            resources.ApplyResources(this.cmbResolution, "cmbResolution");
            this.cmbResolution.Name = "cmbResolution";
            // 
            // innerFlowLayout
            // 
            resources.ApplyResources(this.innerFlowLayout, "innerFlowLayout");
            this.innerFlowLayout.Controls.Add(this.cmbScreenNumber);
            this.innerFlowLayout.Controls.Add(this.lnkIdentify);
            this.innerFlowLayout.Name = "innerFlowLayout";
            // 
            // cmbScreenNumber
            // 
            this.cmbScreenNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScreenNumber.FormattingEnabled = true;
            resources.ApplyResources(this.cmbScreenNumber, "cmbScreenNumber");
            this.cmbScreenNumber.Name = "cmbScreenNumber";
            // 
            // lnkIdentify
            // 
            resources.ApplyResources(this.lnkIdentify, "lnkIdentify");
            this.lnkIdentify.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkIdentify.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkIdentify.Name = "lnkIdentify";
            this.lnkIdentify.TabStop = true;
            this.lnkIdentify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // VisualProfileGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.layoutPanel);
            this.DoubleBuffered = true;
            this.Name = "VisualProfileGeneralPage";
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.innerFlowLayout.ResumeLayout(false);
            this.innerFlowLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbTerminalType;
        private System.Windows.Forms.CheckBox chkCursorHide;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbResolution;
        private System.Windows.Forms.FlowLayoutPanel innerFlowLayout;
        private System.Windows.Forms.ComboBox cmbScreenNumber;
        private System.Windows.Forms.LinkLabel lnkIdentify;
    }
}
