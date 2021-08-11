using System;
using System.Collections.Generic;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.SiteService.ViewPages
{
	public partial class KitchenDisplaySiteServicePage : UserControl, ITabView
	{
		private SiteServiceProfile profile;

		public KitchenDisplaySiteServicePage()
		{
			InitializeComponent();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new KitchenDisplaySiteServicePage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			profile = (SiteServiceProfile)internalContext;
			tbUrl.Text = profile.KDSWebServiceUrl;
		}

		public bool DataIsModified()
		{
			return false;
		}

		public bool SaveData()
		{
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

		private void btnCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(tbUrl.Text);
		}
	}
}