using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SiteService.Properties;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
	public partial class AccessTokenDialog : DialogBase
	{
		private AccessToken token;
		private List<DataEntity> userList;
		private List<User> users;
		IProfileSettings settings;
		private User user;
		private Store store;

		public AccessToken Token
		{
			get { return token; }
		}

		public AccessTokenDialog(AccessToken token)
			: this()
		{
			this.token = token;
			user = Providers.UserData.Get(PluginEntry.DataModel, (Guid)token.UserID);
			store = Providers.StoreData.Get(PluginEntry.DataModel, token.StoreID);
			tbDescription.Text = token.Description;
			tbSender.Text = token.SenderDNS;
			cmbStore.SelectedData  = new DataEntity(store.ID, store.Text);
			cmbUser.SelectedData = new DataEntity(user.Login, settings.NameFormatter.Format(user.Name));
			cmbStore.Enabled = cmbUser.Enabled = !token.Active;
			if (token.Active)
			{
				btnGenerate.Text = Resources.Revoke;
			}
			tbSender.Enabled = tbDescription.Enabled = false;
			CheckEnabled();
			btnOK.Enabled = false;
		}

		public AccessTokenDialog()
		{
			InitializeComponent();
			settings = PluginEntry.DataModel.Settings;
			token = new AccessToken();
			userList = new List<DataEntity>();
			PopulateUserList();
			cmbStore.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
			cmbUser.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
			btnGenerate.Enabled = false;
			HideTokenGroupPanel();
		}

		private void PopulateUserList()
		{
			users = Providers.UserData.AllUsers(PluginEntry.DataModel);
			foreach (User localUser in users)
			{
				if (localUser.IsDomainUser) continue;
				
				userList.Add(new DataEntity(new RecordIdentifier(localUser.Login, localUser.ID )
						   , settings.NameFormatter.Format(localUser.Name)));
			}
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private AccessToken AssembleToken()
		{
			token = new AccessToken();
			token.Description = tbDescription.Text;
			token.SenderDNS = tbSender.Text;
			token.UserID = cmbUser.SelectedData.ID.SecondaryID == null ? users.First(p => p.Login == cmbUser.SelectedData.ID).ID : cmbUser.SelectedData.ID.SecondaryID;
			token.StoreID = cmbStore.SelectedData.ID;
			token.TimeStamp = DateTime.Now;
			token.Active = tbSecurityToken.Text.Length > 0;
			token.Token = tbSecurityToken.Text.Length > 0 ? SimpleHash.ComputeHash(tbSecurityToken.Text, "SHA256", null) : "";
			return token;
		}

		private void cmbUser_SelectedDataChanged(object sender, EventArgs e)
		{
			CheckEnabled();
		}

		private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
		{
			CheckEnabled();
		}

		private void CheckEnabled()
		{
			var enabled = (tbSender.Text.Length > 0) && (tbDescription.Text.Length > 0);
			enabled = enabled && (cmbStore.SelectedData != null) && ((string)cmbStore.SelectedData.ID != "");
			enabled = enabled && (cmbUser.SelectedData != null) && ((string)cmbUser.SelectedData.ID != "");
			enabled = enabled && tokenModified();
			btnOK.Enabled = btnGenerate.Enabled = enabled;
		}

		private bool tokenModified()
		{
			return token.UserID != cmbUser.SelectedData.ID ||
				   token.StoreID != cmbStore.SelectedData.ID ||
				   token.Description != tbDescription.Text;
		}

		private void cmbStore_RequestData(object sender, EventArgs e)
		{
			cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
		}

		private void cmbUser_RequestData(object sender, EventArgs e)
		{ }

		private void tbSender_TextChanged(object sender, EventArgs e)
		{
			errorProvider.Clear();
			CheckEnabled();
		}

		private void tbDescription_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled();
		}

		private void cmbUser_DropDown(object sender, DropDownEventArgs e)
		{
			e.ControlToEmbed =  new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (!token.Active)
			{
				string jwtToken = JwtToken.WriteJwtSecurityToken(cmbUser.SelectedData.ID.StringValue, cmbStore.SelectedData.ID.StringValue, DateTime.Now.Ticks);
				tbSecurityToken.Text = jwtToken;
				grpToken.Visible = true;
				txtCopyToken.Visible = true;
				btnGenerate.Enabled = false;
				ShowTokenGroupPanel();
				btnCancel.Enabled = true;
				btnOK.Enabled = true;
				btnOK.Text = Resources.Activate;
			}
			else
			{
				if (QuestionDialog.Show(Resources.RevokeTokenQuestion + "\n" + Resources.CannotCancel, Resources.RevokeToken) == DialogResult.Yes)
				{
					Providers.AccessTokenData.RevokeAccessToken(PluginEntry.DataModel, token.SenderDNS);
					token.Active = false;
					cmbStore.Enabled = cmbUser.Enabled = true;
					btnGenerate.Text = Resources.Generate;
					btnCancel.Enabled = false;
					btnOK.Enabled = true;

					token = AssembleToken();
					DialogResult = DialogResult.OK;
					Close();
				}

			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if(token.SenderDNS == null && Providers.AccessTokenData.Exists(PluginEntry.DataModel, tbSender.Text)) //Creating new token
			{
				errorProvider.SetError(tbSender, Properties.Resources.SenderDNSAlreadyExists);
				tbSender.Focus();
				return;
			}

			if (btnOK.Text == Resources.Activate)
			{
				if (QuestionDialog.Show(Resources.DeleteTokenQuestion + "\n" + Resources.WontSeeTokenAgain, Resources.CopyToken) == DialogResult.Yes)
				{
					token = AssembleToken();
					DialogResult = DialogResult.OK;
					Close();
				}
			}
			else
			{
				token = AssembleToken();
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(tbSecurityToken.Text);
			btnOK.Enabled = true;
		}

		private void ShowTokenGroupPanel()
		{
			Size = new Size(Size.Width, 365);
			grpToken.Visible = true;
		}

		private void HideTokenGroupPanel()
		{
			grpToken.Visible = false;
			Size = new Size(Size.Width, 280);
		}
	}
}
