using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemImagePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemImagePage));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.thumbLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.thumbContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.contextButtons = new LSOne.Controls.ContextButtons();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.mainLayout.SuspendLayout();
            this.thumbContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            this.pictureBox.SizeChanged += new System.EventHandler(this.OnPictureBoxSizeChanged);
            // 
            // mainLayout
            // 
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Controls.Add(this.thumbLayout, 0, 0);
            this.mainLayout.Controls.Add(this.pictureBox, 1, 0);
            this.mainLayout.Controls.Add(this.lblImageInfo, 1, 1);
            this.mainLayout.Name = "mainLayout";
            // 
            // thumbLayout
            // 
            resources.ApplyResources(this.thumbLayout, "thumbLayout");
            this.thumbLayout.Name = "thumbLayout";
            this.mainLayout.SetRowSpan(this.thumbLayout, 2);
            // 
            // lblImageInfo
            // 
            resources.ApplyResources(this.lblImageInfo, "lblImageInfo");
            this.lblImageInfo.Name = "lblImageInfo";
            // 
            // thumbContextMenuStrip
            // 
            this.thumbContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator2});
            this.thumbContextMenuStrip.Name = "thumbContextMenuStrip";
            resources.ApplyResources(this.thumbContextMenuStrip, "thumbContextMenuStrip");
            this.thumbContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.thumbContextMenuStrip_Opening);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            resources.ApplyResources(this.moveUpToolStripMenuItem, "moveUpToolStripMenuItem");
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            resources.ApplyResources(this.moveDownToolStripMenuItem, "moveDownToolStripMenuItem");
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnMoveDown
            // 
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // contextButtons
            // 
            this.contextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtons, "contextButtons");
            this.contextButtons.BackColor = System.Drawing.Color.Transparent;
            this.contextButtons.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtons.EditButtonEnabled = false;
            this.contextButtons.Name = "contextButtons";
            this.contextButtons.RemoveButtonEnabled = false;
            this.contextButtons.AddButtonClicked += new System.EventHandler(this.contextButtons_AddButtonClicked);
            this.contextButtons.RemoveButtonClicked += new System.EventHandler(this.contextButtons_RemoveButtonClicked);
            // 
            // ItemImagePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.contextButtons);
            this.Name = "ItemImagePage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.thumbContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private ContextButtons contextButtons;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel thumbLayout;
        private System.Windows.Forms.ContextMenuStrip thumbContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;

    }
}
