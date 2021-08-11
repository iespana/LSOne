﻿using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    partial class ButtonView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbKeyNo = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbOperation = new LSOne.Controls.DualDataComboBox();
			this.lblParameter1 = new System.Windows.Forms.Label();
			this.cmbParameter = new LSOne.Controls.DualDataComboBox();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblParameter2 = new System.Windows.Forms.Label();
			this.cmbParameter2 = new LSOne.Controls.DualDataComboBox();
			this.grpPreviewButton = new System.Windows.Forms.GroupBox();
			this.btnMenuButtonPreview = new LSOne.Controls.MenuButton();
			this.cmbStyle = new LSOne.Controls.DualDataComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlBottom.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.grpPreviewButton.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.cmbStyle);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.grpPreviewButton);
			this.pnlBottom.Controls.Add(this.cmbOperation);
			this.pnlBottom.Controls.Add(this.groupBox1);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbKeyNo);
			this.pnlBottom.Controls.Add(this.label2);
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbKeyNo
			// 
			this.tbKeyNo.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbKeyNo, "tbKeyNo");
			this.tbKeyNo.Name = "tbKeyNo";
			this.tbKeyNo.ReadOnly = true;
			this.tbKeyNo.BackColor = ColorPalette.BackgroundColor;
			this.tbKeyNo.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// cmbOperation
			// 
			this.cmbOperation.AddList = null;
			this.cmbOperation.AllowKeyboardSelection = false;
			this.cmbOperation.EnableTextBox = true;
			resources.ApplyResources(this.cmbOperation, "cmbOperation");
			this.cmbOperation.MaxLength = 32767;
			this.cmbOperation.Name = "cmbOperation";
			this.cmbOperation.NoChangeAllowed = false;
			this.cmbOperation.OnlyDisplayID = false;
			this.cmbOperation.RemoveList = null;
			this.cmbOperation.RowHeight = ((short)(22));
			this.cmbOperation.SecondaryData = null;
			this.cmbOperation.SelectedData = null;
			this.cmbOperation.SelectedDataID = null;
			this.cmbOperation.SelectionList = null;
			this.cmbOperation.ShowDropDownOnTyping = true;
			this.cmbOperation.SkipIDColumn = true;
			this.cmbOperation.RequestData += new System.EventHandler(this.cmbOperation_RequestData);
			this.cmbOperation.SelectedDataChanged += new System.EventHandler(this.cmbOperation_SelectedDataChanged);
			// 
			// lblParameter1
			// 
			this.lblParameter1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblParameter1, "lblParameter1");
			this.lblParameter1.Name = "lblParameter1";
			// 
			// cmbParameter
			// 
			this.cmbParameter.AddList = null;
			this.cmbParameter.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbParameter, "cmbParameter");
			this.cmbParameter.EnableTextBox = true;
			this.cmbParameter.MaxLength = 500;
			this.cmbParameter.Name = "cmbParameter";
			this.cmbParameter.NoChangeAllowed = false;
			this.cmbParameter.OnlyDisplayID = false;
			this.cmbParameter.RemoveList = null;
			this.cmbParameter.RowHeight = ((short)(22));
			this.cmbParameter.SecondaryData = null;
			this.cmbParameter.SelectedData = null;
			this.cmbParameter.SelectedDataID = null;
			this.cmbParameter.SelectionList = null;
			this.cmbParameter.SkipIDColumn = true;
			this.cmbParameter.RequestData += new System.EventHandler(this.cmbParameter_RequestData);
			this.cmbParameter.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbParameter_DropDown);
			this.cmbParameter.SelectedDataChanged += new System.EventHandler(this.cmbParameter_SelectedDataChanged);
			this.cmbParameter.RequestClear += new System.EventHandler(this.cmbParameter_RequestClear);
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblParameter2);
			this.groupBox1.Controls.Add(this.cmbParameter2);
			this.groupBox1.Controls.Add(this.lblParameter1);
			this.groupBox1.Controls.Add(this.cmbParameter);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// lblParameter2
			// 
			this.lblParameter2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblParameter2, "lblParameter2");
			this.lblParameter2.Name = "lblParameter2";
			// 
			// cmbParameter2
			// 
			this.cmbParameter2.AddList = null;
			this.cmbParameter2.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbParameter2, "cmbParameter2");
			this.cmbParameter2.MaxLength = 500;
			this.cmbParameter2.Name = "cmbParameter2";
			this.cmbParameter2.NoChangeAllowed = false;
			this.cmbParameter2.OnlyDisplayID = false;
			this.cmbParameter2.RemoveList = null;
			this.cmbParameter2.RowHeight = ((short)(22));
			this.cmbParameter2.SecondaryData = null;
			this.cmbParameter2.SelectedData = null;
			this.cmbParameter2.SelectedDataID = null;
			this.cmbParameter2.SelectionList = null;
			this.cmbParameter2.SkipIDColumn = true;
			this.cmbParameter2.RequestData += new System.EventHandler(this.cmbParameter2_RequestData);
			this.cmbParameter2.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbParameter2_FormatData);
			this.cmbParameter2.SelectedDataChanged += new System.EventHandler(this.cmbParameter2_SelectedDataChanged);
			this.cmbParameter2.RequestClear += new System.EventHandler(this.cmbParameter_RequestClear);
			// 
			// grpPreviewButton
			// 
			this.grpPreviewButton.Controls.Add(this.btnMenuButtonPreview);
			resources.ApplyResources(this.grpPreviewButton, "grpPreviewButton");
			this.grpPreviewButton.Name = "grpPreviewButton";
			this.grpPreviewButton.TabStop = false;
			// 
			// btnMenuButtonPreview
			// 
			this.btnMenuButtonPreview.AllowDrop = true;
			this.btnMenuButtonPreview.BackColor = System.Drawing.Color.Transparent;
			this.btnMenuButtonPreview.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.btnMenuButtonPreview.BorderColorInt = -4934476;
			this.btnMenuButtonPreview.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.btnMenuButtonPreview.ButtonColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
			this.btnMenuButtonPreview.ButtonColor2Int = -1842205;
			this.btnMenuButtonPreview.ButtonColorInt = -986896;
			this.btnMenuButtonPreview.ButtonFacingInt = 1;
			this.btnMenuButtonPreview.ButtonImagePositionInt = 0;
			this.btnMenuButtonPreview.ButtonKey = -1;
			this.btnMenuButtonPreview.ColumnIndex = -1;
			this.btnMenuButtonPreview.Designing = false;
			resources.ApplyResources(this.btnMenuButtonPreview, "btnMenuButtonPreview");
			this.btnMenuButtonPreview.DragDropEnabled = false;
			this.btnMenuButtonPreview.FontSize = 11F;
			this.btnMenuButtonPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.ForeColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.ForeColor2Int = -16777216;
			this.btnMenuButtonPreview.ForeColorInt = -16777216;
			this.btnMenuButtonPreview.Glyph1Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.Glyph1ColorInt = -16777216;
			this.btnMenuButtonPreview.Glyph1Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnMenuButtonPreview.Glyph1FontName = "Microsoft Sans Serif";
			this.btnMenuButtonPreview.Glyph1FontSize = 8.25F;
			this.btnMenuButtonPreview.Glyph2Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.Glyph2ColorInt = -16777216;
			this.btnMenuButtonPreview.Glyph2Font = null;
			this.btnMenuButtonPreview.Glyph2FontName = "";
			this.btnMenuButtonPreview.Glyph2FontSize = 8F;
			this.btnMenuButtonPreview.Glyph3Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.Glyph3ColorInt = -16777216;
			this.btnMenuButtonPreview.Glyph3Font = null;
			this.btnMenuButtonPreview.Glyph3FontName = "";
			this.btnMenuButtonPreview.Glyph3FontSize = 8F;
			this.btnMenuButtonPreview.Glyph4Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnMenuButtonPreview.Glyph4ColorInt = -16777216;
			this.btnMenuButtonPreview.Glyph4Font = null;
			this.btnMenuButtonPreview.Glyph4FontName = "";
			this.btnMenuButtonPreview.Glyph4FontSize = 8F;
			this.btnMenuButtonPreview.GradientModeInt = 2;
			this.btnMenuButtonPreview.HeightMM = 2170;
			this.btnMenuButtonPreview.Highlighted = false;
			this.btnMenuButtonPreview.HotKey = System.Windows.Forms.Keys.None;
			this.btnMenuButtonPreview.Image = null;
			this.btnMenuButtonPreview.ImageFont = null;
			this.btnMenuButtonPreview.ImageText = "";
			this.btnMenuButtonPreview.ImageTextColor = System.Drawing.Color.Black;
			this.btnMenuButtonPreview.IsDirty = false;
			this.btnMenuButtonPreview.MenuID = null;
			this.btnMenuButtonPreview.MenuName = null;
			this.btnMenuButtonPreview.Name = "btnMenuButtonPreview";
			this.btnMenuButtonPreview.PushEffectInt = 0;
			this.btnMenuButtonPreview.Resizable = false;
			this.btnMenuButtonPreview.RowIndex = -1;
			this.btnMenuButtonPreview.ShapeInt = 0;
			this.btnMenuButtonPreview.SubTextFont = new System.Drawing.Font("Segoe UI", 9F);
			this.btnMenuButtonPreview.SubTextFontName = "Segoe UI";
			this.btnMenuButtonPreview.SubTextFontSize = 9F;
			this.btnMenuButtonPreview.TextAlignmentInt = 32;
			this.btnMenuButtonPreview.TextGradientModeInt = 0;
			this.btnMenuButtonPreview.WidthMM = 3625;
			this.btnMenuButtonPreview.XPos = 3;
			this.btnMenuButtonPreview.XPosMM = 79;
			this.btnMenuButtonPreview.YPos = 16;
			this.btnMenuButtonPreview.YPosMM = 423;
			// 
			// cmbStyle
			// 
			this.cmbStyle.AddList = null;
			this.cmbStyle.AllowKeyboardSelection = false;
			this.cmbStyle.EnableTextBox = true;
			resources.ApplyResources(this.cmbStyle, "cmbStyle");
			this.cmbStyle.MaxLength = 32767;
			this.cmbStyle.Name = "cmbStyle";
			this.cmbStyle.NoChangeAllowed = false;
			this.cmbStyle.OnlyDisplayID = false;
			this.cmbStyle.RemoveList = null;
			this.cmbStyle.RowHeight = ((short)(22));
			this.cmbStyle.SecondaryData = null;
			this.cmbStyle.SelectedData = null;
			this.cmbStyle.SelectedDataID = null;
			this.cmbStyle.SelectionList = null;
			this.cmbStyle.ShowDropDownOnTyping = true;
			this.cmbStyle.SkipIDColumn = true;
			this.cmbStyle.RequestData += new System.EventHandler(this.cmbStyle_RequestData);
			this.cmbStyle.SelectedDataChanged += new System.EventHandler(this.cmbStyle_SelectedDataChanged);
			this.cmbStyle.RequestClear += new System.EventHandler(this.cmbStyle_RequestClear);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// ButtonView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "ButtonView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.grpPreviewButton.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbKeyNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbOperation;
        private System.Windows.Forms.Label lblParameter1;
        private DualDataComboBox cmbParameter;
        //private Controls.TabControl tabSheetTabs;
        private TabControlLeft tabSheetTabs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpPreviewButton;
        private MenuButton btnMenuButtonPreview;
        private System.Windows.Forms.Label lblParameter2;
        private DualDataComboBox cmbParameter2;
        private DualDataComboBox cmbStyle;
        private System.Windows.Forms.Label label1;
    }
}
