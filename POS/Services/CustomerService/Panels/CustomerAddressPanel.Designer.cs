using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerAddressPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerAddressPanel));
            this.addressControlTouch = new LSOne.Controls.AddressControlTouch();
            this.touchErrorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // addressControlTouch
            // 
            this.addressControlTouch.AddressFormat = LSOne.Utilities.DataTypes.Address.AddressFormatEnum.US;
            this.addressControlTouch.AddressFormatChangeable = true;
            this.addressControlTouch.BackColor = System.Drawing.Color.White;
            this.addressControlTouch.DataModel = null;
            this.addressControlTouch.FocusedText = "";
            resources.ApplyResources(this.addressControlTouch, "addressControlTouch");
            this.addressControlTouch.Name = "addressControlTouch";
            this.addressControlTouch.States = null;
            this.addressControlTouch.ValueChanged += new System.EventHandler(this.addressControlTouch_ValueChanged);
            // 
            // touchErrorProvider
            // 
            resources.ApplyResources(this.touchErrorProvider, "touchErrorProvider");
            this.touchErrorProvider.BackColor = System.Drawing.Color.White;
            this.touchErrorProvider.Name = "touchErrorProvider";
            // 
            // CustomerAddressPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.touchErrorProvider);
            this.Controls.Add(this.addressControlTouch);
            this.DoubleBuffered = true;
            this.Name = "CustomerAddressPanel";
            this.ResumeLayout(false);

        }

        #endregion
        private AddressControlTouch addressControlTouch;
        private Controls.Dialogs.TouchErrorProvider touchErrorProvider;
    }
}
