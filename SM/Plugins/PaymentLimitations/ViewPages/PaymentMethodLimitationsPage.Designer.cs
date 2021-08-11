using LSOne.Controls;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    partial class PaymentMethodLimitationsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentMethodLimitationsPage));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvLimitations = new LSOne.Controls.ListView();
            this.colCode = new LSOne.Controls.Columns.Column();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.colPaymentAllowed = new LSOne.Controls.Columns.Column();
            this.colTaxExempt = new LSOne.Controls.Columns.Column();
            this.btnCopyFrom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvLimitations
            // 
            resources.ApplyResources(this.lvLimitations, "lvLimitations");
            this.lvLimitations.BuddyControl = null;
            this.lvLimitations.Columns.Add(this.colCode);
            this.lvLimitations.Columns.Add(this.colType);
            this.lvLimitations.Columns.Add(this.colDescription);
            this.lvLimitations.Columns.Add(this.colVariant);
            this.lvLimitations.Columns.Add(this.colPaymentAllowed);
            this.lvLimitations.Columns.Add(this.colTaxExempt);
            this.lvLimitations.ContentBackColor = System.Drawing.Color.White;
            this.lvLimitations.DefaultRowHeight = ((short)(22));
            this.lvLimitations.DimSelectionWhenDisabled = true;
            this.lvLimitations.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLimitations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLimitations.HeaderHeight = ((short)(25));
            this.lvLimitations.Name = "lvLimitations";
            this.lvLimitations.OddRowColor = System.Drawing.Color.White;
            this.lvLimitations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLimitations.SecondarySortColumn = ((short)(-1));
            this.lvLimitations.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvLimitations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLimitations.SortSetting = "0:1";
            this.lvLimitations.SelectionChanged += new System.EventHandler(this.lvLimitations_SelectedIndexChanged);
            this.lvLimitations.DoubleClick += new System.EventHandler(this.lvLimitations_DoubleClick);
            // 
            // colCode
            // 
            this.colCode.AutoSize = true;
            this.colCode.DefaultStyle = null;
            resources.ApplyResources(this.colCode, "colCode");
            this.colCode.InternalSort = true;
            this.colCode.MaximumWidth = ((short)(0));
            this.colCode.MinimumWidth = ((short)(100));
            this.colCode.SecondarySortColumn = ((short)(-1));
            this.colCode.Tag = null;
            this.colCode.Width = ((short)(200));
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.InternalSort = true;
            this.colType.MaximumWidth = ((short)(0));
            this.colType.MinimumWidth = ((short)(100));
            this.colType.SecondarySortColumn = ((short)(-1));
            this.colType.Tag = null;
            this.colType.Width = ((short)(200));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(200));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(250));
            // 
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
            this.colVariant.InternalSort = true;
            this.colVariant.MaximumWidth = ((short)(0));
            this.colVariant.MinimumWidth = ((short)(5));
            this.colVariant.NoTextWhenSmall = true;
            this.colVariant.SecondarySortColumn = ((short)(-1));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(100));
            // 
            // colPaymentAllowed
            // 
            this.colPaymentAllowed.AutoSize = true;
            this.colPaymentAllowed.DefaultStyle = null;
            resources.ApplyResources(this.colPaymentAllowed, "colPaymentAllowed");
            this.colPaymentAllowed.InternalSort = true;
            this.colPaymentAllowed.MaximumWidth = ((short)(0));
            this.colPaymentAllowed.MinimumWidth = ((short)(50));
            this.colPaymentAllowed.SecondarySortColumn = ((short)(-1));
            this.colPaymentAllowed.Tag = null;
            this.colPaymentAllowed.Width = ((short)(100));
            // 
            // colTaxExempt
            // 
            this.colTaxExempt.AutoSize = true;
            this.colTaxExempt.DefaultStyle = null;
            resources.ApplyResources(this.colTaxExempt, "colTaxExempt");
            this.colTaxExempt.InternalSort = true;
            this.colTaxExempt.MaximumWidth = ((short)(0));
            this.colTaxExempt.MinimumWidth = ((short)(10));
            this.colTaxExempt.SecondarySortColumn = ((short)(-1));
            this.colTaxExempt.Tag = null;
            this.colTaxExempt.Width = ((short)(50));
            // 
            // btnCopyFrom
            // 
            resources.ApplyResources(this.btnCopyFrom, "btnCopyFrom");
            this.btnCopyFrom.Name = "btnCopyFrom";
            this.btnCopyFrom.UseVisualStyleBackColor = true;
            this.btnCopyFrom.Click += new System.EventHandler(this.btnCopyFrom_Click);
            // 
            // PaymentMethodLimitationsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnCopyFrom);
            this.Controls.Add(this.lvLimitations);
            this.Controls.Add(this.btnsEditAddRemove);
            this.DoubleBuffered = true;
            this.Name = "PaymentMethodLimitationsPage";
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private ListView lvLimitations;
        private Controls.Columns.Column colCode;
        private Controls.Columns.Column colType;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colPaymentAllowed;
        private Controls.Columns.Column colVariant;
        private System.Windows.Forms.Button btnCopyFrom;
        private Controls.Columns.Column colTaxExempt;
    }
}
