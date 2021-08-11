using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class StatusDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusDialog));
            this.lblMessage = new System.Windows.Forms.Label();
            this.tdbBanner = new LSOne.Controls.TouchDialogBanner();
            this.buttonLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            resources.ApplyResources(this.lblMessage, "lblMessage");
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblMessage.Name = "lblMessage";
            // 
            // tdbBanner
            // 
            resources.ApplyResources(this.tdbBanner, "tdbBanner");
            this.tdbBanner.BackColor = System.Drawing.Color.White;
            this.tdbBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbBanner.Name = "tdbBanner";
            this.tdbBanner.TabStop = false;
            // 
            // buttonLayout
            // 
            resources.ApplyResources(this.buttonLayout, "buttonLayout");
            this.buttonLayout.Name = "buttonLayout";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::LSOne.Services.Properties.Resources.LS_One_spinner;
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // StatusDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonLayout);
            this.Controls.Add(this.tdbBanner);
            this.Controls.Add(this.lblMessage);
            this.Name = "StatusDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private TouchDialogBanner tdbBanner;
        private System.Windows.Forms.FlowLayoutPanel buttonLayout;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}