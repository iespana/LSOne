using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.ViewPlugins.TouchButtons.Controls;
using Style = LSOne.DataLayer.BusinessObjects.TouchButtons.Style;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    partial class NewStyle
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStyle));
            LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader posMenuHeader1 = new LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader();
            LSOne.DataLayer.BusinessObjects.TouchButtons.Style style1 = new LSOne.DataLayer.BusinessObjects.TouchButtons.Style();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpPreviewButton = new System.Windows.Forms.GroupBox();
            this.btnMenuButtonPreview = new LSOne.Controls.MenuButton();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.innerLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.pnlCopyFrom = new System.Windows.Forms.Panel();
            this.pnlSystem = new System.Windows.Forms.Panel();
            this.chkSystemStyle = new System.Windows.Forms.CheckBox();
            this.lblSystem = new System.Windows.Forms.Label();
            this.buttonProperties = new LSOne.ViewPlugins.TouchButtons.Controls.ButtonPropertiesControl();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel2.SuspendLayout();
            this.buttonLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpPreviewButton.SuspendLayout();
            this.innerLayout.SuspendLayout();
            this.pnlDescription.SuspendLayout();
            this.pnlCopyFrom.SuspendLayout();
            this.pnlSystem.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.buttonLayout);
            this.panel2.Name = "panel2";
            // 
            // buttonLayout
            // 
            resources.ApplyResources(this.buttonLayout, "buttonLayout");
            this.buttonLayout.Controls.Add(this.btnReset);
            this.buttonLayout.Controls.Add(this.btnOK);
            this.buttonLayout.Controls.Add(this.btnCancel);
            this.buttonLayout.Name = "buttonLayout";
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
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
            this.btnMenuButtonPreview.HeightMM = 677;
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
            this.btnMenuButtonPreview.TabStop = false;
            this.btnMenuButtonPreview.TextAlignmentInt = 32;
            this.btnMenuButtonPreview.TextGradientModeInt = 0;
            this.btnMenuButtonPreview.WidthMM = 1344;
            this.btnMenuButtonPreview.XPos = 3;
            this.btnMenuButtonPreview.XPosMM = 32;
            this.btnMenuButtonPreview.YPos = 34;
            this.btnMenuButtonPreview.YPosMM = 360;
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            this.cmbCopyFrom.EnableTextBox = true;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = false;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_RequestClear);
            // 
            // innerLayout
            // 
            resources.ApplyResources(this.innerLayout, "innerLayout");
            this.innerLayout.Controls.Add(this.pnlDescription, 0, 1);
            this.innerLayout.Controls.Add(this.grpPreviewButton, 0, 0);
            this.innerLayout.Controls.Add(this.pnlCopyFrom, 0, 3);
            this.innerLayout.Controls.Add(this.pnlSystem, 0, 2);
            this.innerLayout.Controls.Add(this.buttonProperties, 0, 4);
            this.innerLayout.Name = "innerLayout";
            // 
            // pnlDescription
            // 
            this.pnlDescription.Controls.Add(this.tbDescription);
            this.pnlDescription.Controls.Add(this.lblDescription);
            resources.ApplyResources(this.pnlDescription, "pnlDescription");
            this.pnlDescription.Name = "pnlDescription";
            // 
            // pnlCopyFrom
            // 
            this.pnlCopyFrom.Controls.Add(this.cmbCopyFrom);
            this.pnlCopyFrom.Controls.Add(this.lblCopyFrom);
            resources.ApplyResources(this.pnlCopyFrom, "pnlCopyFrom");
            this.pnlCopyFrom.Name = "pnlCopyFrom";
            // 
            // pnlSystem
            // 
            this.pnlSystem.Controls.Add(this.chkSystemStyle);
            this.pnlSystem.Controls.Add(this.lblSystem);
            resources.ApplyResources(this.pnlSystem, "pnlSystem");
            this.pnlSystem.Name = "pnlSystem";
            // 
            // chkSystemStyle
            // 
            resources.ApplyResources(this.chkSystemStyle, "chkSystemStyle");
            this.chkSystemStyle.Name = "chkSystemStyle";
            this.chkSystemStyle.UseVisualStyleBackColor = true;
            // 
            // lblSystem
            // 
            this.lblSystem.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSystem, "lblSystem");
            this.lblSystem.Name = "lblSystem";
            // 
            // buttonProperties
            // 
            resources.ApplyResources(this.buttonProperties, "buttonProperties");
            this.buttonProperties.BackColor = System.Drawing.Color.Transparent;
            this.buttonProperties.EnableStyleUse = false;
            this.buttonProperties.FontBold = false;
            this.buttonProperties.FontCharset = 0;
            this.buttonProperties.FontItalic = false;
            this.buttonProperties.FontName = "Tahoma";
            this.buttonProperties.FontSize = 14;
            this.buttonProperties.FontStrikethrough = false;
            this.buttonProperties.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
            this.buttonProperties.Name = "buttonProperties";
            posMenuHeader1.AppliesTo = LSOne.DataLayer.BusinessObjects.TouchButtons.PosMenuHeader.AppliesToEnum.None;
            posMenuHeader1.BackColor = -1;
            posMenuHeader1.BackColor2 = -1;
            posMenuHeader1.BorderColor = -3352871;
            posMenuHeader1.BorderWidth = 1;
            posMenuHeader1.Columns = 0;
            posMenuHeader1.DefaultOperation = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader1.DefaultOperation")));
            posMenuHeader1.DeviceType = LSOne.DataLayer.BusinessObjects.TouchButtons.DeviceTypeEnum.POS;
            posMenuHeader1.FontBold = false;
            posMenuHeader1.FontCharset = 0;
            posMenuHeader1.FontItalic = false;
            posMenuHeader1.FontName = "Tahoma";
            posMenuHeader1.FontSize = 14;
            posMenuHeader1.ForeColor = -16777216;
            posMenuHeader1.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
            posMenuHeader1.Guid = new System.Guid("00000000-0000-0000-0000-000000000000");
            posMenuHeader1.ID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader1.ID")));
            posMenuHeader1.ImportDateTime = null;
            posMenuHeader1.KitchenDisplay = false;
            posMenuHeader1.MainMenu = false;
            posMenuHeader1.Margin = 0;
            posMenuHeader1.MenuColor = 0;
            posMenuHeader1.MenuType = LSOne.DataLayer.BusinessObjects.TouchButtons.MenuTypeEnum.Hospitality;
            posMenuHeader1.Rows = 0;
            posMenuHeader1.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
            posMenuHeader1.StyleID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("posMenuHeader1.StyleID")));
            posMenuHeader1.Text = "";
            posMenuHeader1.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
            posMenuHeader1.UsageIntent = LSOne.Utilities.DataTypes.UsageIntentEnum.Normal;
            posMenuHeader1.UseNavOperation = false;
            this.buttonProperties.PosMenuHeader = posMenuHeader1;
            this.buttonProperties.PosStyle = ((LSOne.DataLayer.BusinessObjects.TouchButtons.PosStyle)(resources.GetObject("buttonProperties.PosStyle")));
            this.buttonProperties.PosStyleID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("buttonProperties.PosStyleID")));
            this.buttonProperties.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
            style1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            style1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            style1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            style1.FontBold = false;
            style1.FontCharset = 0;
            style1.FontItalic = false;
            style1.FontName = "Tahoma";
            style1.FontSize = 14;
            style1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            style1.GradientMode = LSOne.Utilities.Enums.GradientModeEnum.None;
            style1.ID = ((LSOne.Utilities.DataTypes.RecordIdentifier)(resources.GetObject("style1.ID")));
            style1.Shape = LSOne.Utilities.Enums.ShapeEnum.RoundRectangle;
            style1.Text = "";
            style1.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
            style1.UsageIntent = LSOne.Utilities.DataTypes.UsageIntentEnum.Normal;
            this.buttonProperties.Style = style1;
            this.buttonProperties.StyleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonProperties.StyleBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.buttonProperties.StyleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonProperties.TextPosition = LSOne.DataLayer.BusinessObjects.TouchButtons.Position.Center;
            this.buttonProperties.Modified += new System.EventHandler(this.OnButtonPropertiesModified);
            // 
            // mainLayout
            // 
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Controls.Add(this.panel2, 0, 1);
            this.mainLayout.Controls.Add(this.innerLayout, 0, 0);
            this.mainLayout.Name = "mainLayout";
            // 
            // NewStyle
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ControlBox = true;
            this.Controls.Add(this.mainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewStyle";
            this.Controls.SetChildIndex(this.mainLayout, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.buttonLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpPreviewButton.ResumeLayout(false);
            this.innerLayout.ResumeLayout(false);
            this.innerLayout.PerformLayout();
            this.pnlDescription.ResumeLayout(false);
            this.pnlDescription.PerformLayout();
            this.pnlCopyFrom.ResumeLayout(false);
            this.pnlSystem.ResumeLayout(false);
            this.pnlSystem.PerformLayout();
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox grpPreviewButton;
        private System.Windows.Forms.Label lblCopyFrom;
        //private System.Windows.Forms.FontDialog fontDialog1;
        private MenuButton btnMenuButtonPreview;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.TableLayoutPanel innerLayout;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.FlowLayoutPanel buttonLayout;
        private ButtonPropertiesControl buttonProperties;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel pnlDescription;
        private System.Windows.Forms.Panel pnlCopyFrom;
        private System.Windows.Forms.Panel pnlSystem;
        private System.Windows.Forms.CheckBox chkSystemStyle;
        private System.Windows.Forms.Label lblSystem;
    }
}