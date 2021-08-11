using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Peripherals.Dialogs
{
    partial class DetectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetectionDialog));
            this.btnExit = new LSOne.Controls.TouchButton();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lvSelection = new LSOne.Controls.ListView();
            this.colDevice = new LSOne.Controls.Columns.Column();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnExit.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LSOne.Peripherals.Properties.Resources.LS_One_spinner;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // lvSelection
            // 
            resources.ApplyResources(this.lvSelection, "lvSelection");
            this.lvSelection.ApplyVisualStyles = false;
            this.lvSelection.BackColor = System.Drawing.Color.White;
            this.lvSelection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.BuddyControl = null;
            this.lvSelection.Columns.Add(this.colDevice);
            this.lvSelection.ContentBackColor = System.Drawing.Color.White;
            this.lvSelection.DefaultRowHeight = ((short)(50));
            this.lvSelection.EvenRowColor = System.Drawing.Color.White;
            this.lvSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvSelection.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvSelection.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvSelection.HeaderHeight = ((short)(30));
            this.lvSelection.HideHorizontalScrollbarWhenDisabled = true;
            this.lvSelection.HideVerticalScrollbarWhenDisabled = true;
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.OddRowColor = System.Drawing.Color.White;
            this.lvSelection.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.RowLines = true;
            this.lvSelection.SecondarySortColumn = ((short)(-1));
            this.lvSelection.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvSelection.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSelection.SortSetting = "0:1";
            this.lvSelection.TouchScroll = true;
            this.lvSelection.UseFocusRectangle = false;
            this.lvSelection.VerticalScrollbarValue = 0;
            this.lvSelection.VerticalScrollbarYOffset = 0;
            this.lvSelection.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            // 
            // colDevice
            // 
            this.colDevice.AutoSize = true;
            this.colDevice.Clickable = false;
            this.colDevice.DefaultStyle = null;
            resources.ApplyResources(this.colDevice, "colDevice");
            this.colDevice.MaximumWidth = ((short)(0));
            this.colDevice.MinimumWidth = ((short)(505));
            this.colDevice.RelativeSize = 100;
            this.colDevice.SecondarySortColumn = ((short)(-1));
            this.colDevice.Tag = null;
            this.colDevice.Width = ((short)(505));
            // 
            // DetectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.btnExit);
            this.Name = "DetectionDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchButton btnExit;
        private TouchDialogBanner touchDialogBanner1;
        private ListView lvSelection;
        private Controls.Columns.Column colDevice;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}