using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class ReasonCodeSelectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReasonCodeSelectDialog));
            this.touchBanner = new LSOne.Controls.TouchDialogBanner();
            this.lvReasonCodes = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.btnOk = new LSOne.Controls.TouchButton();
            this.SuspendLayout();
            // 
            // touchBanner
            // 
            resources.ApplyResources(this.touchBanner, "touchBanner");
            this.touchBanner.BackColor = System.Drawing.Color.White;
            this.touchBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchBanner.Name = "touchBanner";
            this.touchBanner.TabStop = false;
            // 
            // lvReasonCodes
            // 
            resources.ApplyResources(this.lvReasonCodes, "lvReasonCodes");
            this.lvReasonCodes.ApplyVisualStyles = false;
            this.lvReasonCodes.BackColor = System.Drawing.Color.White;
            this.lvReasonCodes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvReasonCodes.BuddyControl = null;
            this.lvReasonCodes.Columns.Add(this.column1);
            this.lvReasonCodes.Columns.Add(this.column5);
            this.lvReasonCodes.ContentBackColor = System.Drawing.Color.White;
            this.lvReasonCodes.DefaultRowHeight = ((short)(50));
            this.lvReasonCodes.EvenRowColor = System.Drawing.Color.White;
            this.lvReasonCodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvReasonCodes.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvReasonCodes.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvReasonCodes.HeaderHeight = ((short)(30));
            this.lvReasonCodes.HideHorizontalScrollbarWhenDisabled = true;
            this.lvReasonCodes.HideVerticalScrollbarWhenDisabled = true;
            this.lvReasonCodes.Name = "lvReasonCodes";
            this.lvReasonCodes.OddRowColor = System.Drawing.Color.White;
            this.lvReasonCodes.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvReasonCodes.RowLines = true;
            this.lvReasonCodes.SecondarySortColumn = ((short)(-1));
            this.lvReasonCodes.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvReasonCodes.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvReasonCodes.SortSetting = "0:1";
            this.lvReasonCodes.TouchScroll = true;
            this.lvReasonCodes.UseFocusRectangle = false;
            this.lvReasonCodes.VerticalScrollbarValue = 0;
            this.lvReasonCodes.VerticalScrollbarYOffset = 0;
            this.lvReasonCodes.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvReasonCodes.SelectionChanged += new System.EventHandler(this.lvReasonCodes_SelectionChanged);
            this.lvReasonCodes.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvReasonCodes_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            this.column1.FillRemainingWidth = true;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(250));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(250));
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
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(190)))), ((int)(((byte)(140)))));
            this.btnOk.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ReasonCodeSelectDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lvReasonCodes);
            this.Controls.Add(this.touchBanner);
            this.Name = "ReasonCodeSelectDialog";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchBanner;
        private ListView lvReasonCodes;
        private TouchButton btnCancel;
        private TouchButton btnOk;
        private Column column1;
        private Column column5;
    }
}