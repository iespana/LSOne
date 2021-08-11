using System;
using System.Collections.Generic;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class LocationSelectorDialog : DialogBase
    {
        private LocationSelectorDialog()
        {
            InitializeComponent();
        }

        public LocationSelectorDialog(List<JscLocation> locations ):this()
        {
            foreach (var jscLocation in locations)
            {
                comboBox1.Items.Add(jscLocation);
            }
        }
        public JscLocation LocationItem { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LocationItem = null;

        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            LocationItem = (JscLocation) comboBox1.SelectedItem;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

    }

}
