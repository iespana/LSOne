using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class DimensionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DimensionDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.lvDimensions = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.lblNoVariantsAvailable = new System.Windows.Forms.Label();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lvDimensions
            // 
            resources.ApplyResources(this.lvDimensions, "lvDimensions");
            this.lvDimensions.ApplyVisualStyles = false;
            this.lvDimensions.BackColor = System.Drawing.Color.White;
            this.lvDimensions.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvDimensions.BuddyControl = null;
            this.lvDimensions.Columns.Add(this.column1);
            this.lvDimensions.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensions.DefaultRowHeight = ((short)(50));
            this.lvDimensions.EvenRowColor = System.Drawing.Color.White;
            this.lvDimensions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvDimensions.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvDimensions.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvDimensions.HeaderHeight = ((short)(30));
            this.lvDimensions.HideVerticalScrollbarWhenDisabled = true;
            this.lvDimensions.Name = "lvDimensions";
            this.lvDimensions.OddRowColor = System.Drawing.Color.White;
            this.lvDimensions.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvDimensions.RowLines = true;
            this.lvDimensions.SecondarySortColumn = ((short)(-1));
            this.lvDimensions.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvDimensions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensions.SortSetting = "0:1";
            this.lvDimensions.TouchScroll = true;
            this.lvDimensions.UseFocusRectangle = false;
            this.lvDimensions.VerticalScrollbarValue = 0;
            this.lvDimensions.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvDimensions.CellAction += new LSOne.Controls.CellActionDelegate(this.lvDimensions_CellAction);
            this.lvDimensions.RowClick += new LSOne.Controls.RowClickDelegate(this.lvDimensions_RowClick);
            this.lvDimensions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvDimensions_MouseDown);
            this.lvDimensions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvDimensions_MouseUp);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(1000));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(1000));
            // 
            // lblNoVariantsAvailable
            // 
            resources.ApplyResources(this.lblNoVariantsAvailable, "lblNoVariantsAvailable");
            this.lblNoVariantsAvailable.BackColor = System.Drawing.Color.Transparent;
            this.lblNoVariantsAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblNoVariantsAvailable.Name = "lblNoVariantsAvailable";
            this.lblNoVariantsAvailable.Paint += new System.Windows.Forms.PaintEventHandler(this.lblNoVariantsAvailable_Paint);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // DimensionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblNoVariantsAvailable);
            this.Controls.Add(this.lvDimensions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "DimensionDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchDialogBanner touchDialogBanner1;
        private Controls.TouchButton btnCancel;
        private Controls.ListView lvDimensions;
        private Controls.Columns.Column column1;
        private System.Windows.Forms.Label lblNoVariantsAvailable;
        private Controls.TouchButton btnOK;
    }
}