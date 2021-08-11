namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls
{
    partial class KitchenDisplayUIPreview
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
            this.pnlOrder = new KitchenDisplayPanePreview();
            this.pnlHistory = new KitchenDisplayPanePreview();
            this.pnlAggregate = new KitchenDisplayPanePreview();
            this.pnlButton = new KitchenDisplayPanePreview();
            this.pnlHeader = new KitchenDisplayPanePreview();
            this.SuspendLayout();
            // 
            // pnlOrder
            // 
            this.pnlOrder.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrder.Description = "Order";
            this.pnlOrder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlOrder.Location = new System.Drawing.Point(100, 41);
            this.pnlOrder.Name = "pnlOrder";
            this.pnlOrder.Size = new System.Drawing.Size(348, 315);
            this.pnlOrder.TabIndex = 10;
            this.pnlOrder.X = 100;
            this.pnlOrder.Y = 41;
            // 
            // pnlHistory
            // 
            this.pnlHistory.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHistory.Description = "History";
            this.pnlHistory.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlHistory.Location = new System.Drawing.Point(3, 41);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(91, 315);
            this.pnlHistory.TabIndex = 9;
            this.pnlHistory.X = 3;
            this.pnlHistory.Y = 41;
            // 
            // pnlAggregate
            // 
            this.pnlAggregate.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlAggregate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAggregate.Description = "Aggregate";
            this.pnlAggregate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAggregate.Location = new System.Drawing.Point(454, 41);
            this.pnlAggregate.Name = "pnlAggregate";
            this.pnlAggregate.Size = new System.Drawing.Size(91, 315);
            this.pnlAggregate.TabIndex = 8;
            this.pnlAggregate.X = 454;
            this.pnlAggregate.Y = 41;
            // 
            // pnlButton
            // 
            this.pnlButton.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButton.Description = "Button";
            this.pnlButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlButton.Location = new System.Drawing.Point(3, 362);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(543, 32);
            this.pnlButton.TabIndex = 7;
            this.pnlButton.X = 3;
            this.pnlButton.Y = 362;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Description = "Header";
            this.pnlHeader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlHeader.Location = new System.Drawing.Point(2, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(543, 32);
            this.pnlHeader.TabIndex = 6;
            this.pnlHeader.X = 2;
            this.pnlHeader.Y = 3;
            // 
            // KitchenDisplayUIPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlOrder);
            this.Controls.Add(this.pnlHistory);
            this.Controls.Add(this.pnlAggregate);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.pnlHeader);
            this.Name = "KitchenDisplayUIPreview";
            this.Size = new System.Drawing.Size(548, 396);
            this.ResumeLayout(false);

        }

        #endregion

        private KitchenDisplayPanePreview pnlHeader;
        private KitchenDisplayPanePreview pnlButton;
        private KitchenDisplayPanePreview pnlAggregate;
        private KitchenDisplayPanePreview pnlHistory;
        private KitchenDisplayPanePreview pnlOrder;
    }
}
