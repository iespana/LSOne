using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    partial class TouchButtonView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TouchButtonView));
			this.pnlHost = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.tbID = new System.Windows.Forms.TextBox();
			this.lnkRevert = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkSave = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkClose = new LSOne.Controls.ExtendedLinkLabel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.imgEdit = new System.Windows.Forms.PictureBox();
			this.imgDelete = new System.Windows.Forms.PictureBox();
			this.imgRevert = new System.Windows.Forms.PictureBox();
			this.imgClose = new System.Windows.Forms.PictureBox();
			this.lnkDelete = new LSOne.Controls.ExtendedLinkLabel();
			this.imgSave = new System.Windows.Forms.PictureBox();
			this.lnkChangeDesign = new LSOne.Controls.ExtendedLinkLabel();
			this.lnkButtonGridMenus = new LSOne.Controls.ExtendedLinkLabel();
			this.pnlBottom.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgDelete)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRevert)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgClose)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgSave)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.tableLayoutPanel1);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.label2);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.pnlHost);
			// 
			// pnlHost
			// 
			resources.ApplyResources(this.pnlHost, "pnlHost");
			this.pnlHost.BackColor = System.Drawing.Color.White;
			this.pnlHost.Name = "pnlHost";
			this.pnlHost.SizeChanged += new System.EventHandler(this.pnlHost_SizeChanged);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// lnkRevert
			// 
			resources.ApplyResources(this.lnkRevert, "lnkRevert");
			this.lnkRevert.BackColor = System.Drawing.Color.Transparent;
			this.lnkRevert.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkRevert.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkRevert.Name = "lnkRevert";
			this.lnkRevert.TabStop = true;
			this.lnkRevert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRevert_LinkClicked);
			// 
			// lnkSave
			// 
			resources.ApplyResources(this.lnkSave, "lnkSave");
			this.lnkSave.BackColor = System.Drawing.Color.Transparent;
			this.lnkSave.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkSave.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkSave.Name = "lnkSave";
			this.lnkSave.TabStop = true;
			this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
			// 
			// lnkClose
			// 
			resources.ApplyResources(this.lnkClose, "lnkClose");
			this.lnkClose.BackColor = System.Drawing.Color.Transparent;
			this.lnkClose.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkClose.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkClose.Name = "lnkClose";
			this.lnkClose.TabStop = true;
			this.lnkClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClose_LinkClicked);
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.Controls.Add(this.imgEdit, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.imgDelete, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.imgRevert, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.imgClose, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkDelete, 7, 1);
			this.tableLayoutPanel1.Controls.Add(this.lnkClose, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkRevert, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.lnkSave, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.imgSave, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lnkChangeDesign, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lnkButtonGridMenus, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// imgEdit
			// 
			resources.ApplyResources(this.imgEdit, "imgEdit");
			this.imgEdit.Name = "imgEdit";
			this.imgEdit.TabStop = false;
			// 
			// imgDelete
			// 
			resources.ApplyResources(this.imgDelete, "imgDelete");
			this.imgDelete.Name = "imgDelete";
			this.imgDelete.TabStop = false;
			// 
			// imgRevert
			// 
			resources.ApplyResources(this.imgRevert, "imgRevert");
			this.imgRevert.Name = "imgRevert";
			this.imgRevert.TabStop = false;
			// 
			// imgClose
			// 
			resources.ApplyResources(this.imgClose, "imgClose");
			this.imgClose.Name = "imgClose";
			this.imgClose.TabStop = false;
			// 
			// lnkDelete
			// 
			resources.ApplyResources(this.lnkDelete, "lnkDelete");
			this.lnkDelete.BackColor = System.Drawing.Color.Transparent;
			this.lnkDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkDelete.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkDelete.Name = "lnkDelete";
			this.lnkDelete.TabStop = true;
			this.lnkDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDelete_LinkClicked);
			// 
			// imgSave
			// 
			resources.ApplyResources(this.imgSave, "imgSave");
			this.imgSave.Name = "imgSave";
			this.imgSave.TabStop = false;
			// 
			// lnkChangeDesign
			// 
			resources.ApplyResources(this.lnkChangeDesign, "lnkChangeDesign");
			this.lnkChangeDesign.BackColor = System.Drawing.Color.Transparent;
			this.lnkChangeDesign.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkChangeDesign.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkChangeDesign.Name = "lnkChangeDesign";
			this.lnkChangeDesign.TabStop = true;
			this.lnkChangeDesign.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeDesign_LinkClicked);
			// 
			// lnkButtonGridMenus
			// 
			resources.ApplyResources(this.lnkButtonGridMenus, "lnkButtonGridMenus");
			this.lnkButtonGridMenus.BackColor = System.Drawing.Color.Transparent;
			this.lnkButtonGridMenus.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lnkButtonGridMenus.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
			this.lnkButtonGridMenus.Name = "lnkButtonGridMenus";
			this.lnkButtonGridMenus.TabStop = true;
			this.lnkButtonGridMenus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkButtonGridMenus_LinkClicked);
			// 
			// TouchButtonView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 60;
			this.Name = "TouchButtonView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgDelete)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRevert)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgClose)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgSave)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbID;
        private ExtendedLinkLabel lnkRevert;
        private ExtendedLinkLabel lnkSave;
        private ExtendedLinkLabel lnkClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExtendedLinkLabel lnkDelete;
        private System.Windows.Forms.PictureBox imgSave;
        private System.Windows.Forms.PictureBox imgDelete;
        private System.Windows.Forms.PictureBox imgRevert;
        private System.Windows.Forms.PictureBox imgClose;
        private System.Windows.Forms.PictureBox imgEdit;
        private ExtendedLinkLabel lnkChangeDesign;
        private ExtendedLinkLabel lnkButtonGridMenus;

    }
}
