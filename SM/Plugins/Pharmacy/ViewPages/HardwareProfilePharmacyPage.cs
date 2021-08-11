using System;
using System.Collections.Generic;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Pharmacy.ViewPages
{
	public partial class HardwareProfilePharmacyPage : UserControl, ITabView
	{
		private HardwareProfile profile;

		public HardwareProfilePharmacyPage()
		{
			InitializeComponent();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new HardwareProfilePharmacyPage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			profile = (HardwareProfile)internalContext;

			chkPharmacyActive.Checked = profile.PharmacyActive;
			tbHost.Text = profile.PharmacyHost;
			ntbPort.Value = profile.PharmacyPort;

			chkPharmacyActive_CheckedChanged(this, EventArgs.Empty);
		}

		public bool DataIsModified()
		{
			if (chkPharmacyActive.Checked != profile.PharmacyActive) return true;
			if (tbHost.Text != profile.PharmacyHost) return true;
			if (ntbPort.Value != profile.PharmacyPort) return true;

			return false;
		}

		public bool SaveData()
		{
			profile.PharmacyActive = chkPharmacyActive.Checked;
			profile.PharmacyHost = tbHost.Text.Trim();
			profile.PharmacyPort = (int)ntbPort.Value;

			return true;
		}

		public void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
		}

		public void OnClose()
		{
		}

		public void SaveUserInterface()
		{
		}

		#endregion

		private void chkPharmacyActive_CheckedChanged(object sender, EventArgs e)
		{
			tbHost.Enabled = ntbPort.Enabled = chkPharmacyActive.Checked;
		}
	}
}
