using System;
using System.Security;
using System.Windows.Forms;

using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SiteService.Properties;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
	public partial class AuthorizationDialog : DialogBase
	{
		/// <summary>
		/// Site Service host name or IP.
		/// </summary>
		private string ssHost;

		/// <summary>
		/// Site Service port.
		/// </summary>
		private ushort ssPort;

		/// <summary>
		/// Gets or sets the Site Service administrative hashed password.
		/// </summary>
		public SecureString AdministrativeSecureHash { get; private set; }

		public AuthorizationDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new <see cref="AuthorizationDialog"/>.
		/// </summary>
		/// <param name="host">Site Service host.</param>
		/// <param name="port">Site Service port.</param>
		/// <exception cref="ArgumentNullException">If host is null or empty string.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If port is a negative number or zero.</exception>
		public AuthorizationDialog(string host, ushort port)
		: this()
		{
			if(string.IsNullOrEmpty(host)) throw new ArgumentNullException(nameof(host));
			if(port <= 0) throw new ArgumentOutOfRangeException(nameof(port));

			ssHost = host;
			ssPort = port;
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void AuthenticationDialog_Shown(object sender, EventArgs e)
		{
			mtxtPassword.Focus();
		}

		private void mtxtPassword_TextChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = !string.IsNullOrEmpty(mtxtPassword.Text);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(mtxtPassword.Text))
			{
				DialogResult = DialogResult.None;
				return;
			}

			ShowProgress(
				(sender1, e1) =>PluginOperations.AdministrateSiteService(
					() =>
					{
						string administrativeHash = CalculateAdministrativeHash(mtxtPassword.Text.Trim());
						string timestamp = PluginOperations.ValidateSiteServiceAdministrativePassword(ssHost, ssPort, administrativeHash);
						//if timestamp is empty then the password is incorrect so don't close the dialog
						if (string.IsNullOrEmpty(timestamp))
						{
							DialogResult = DialogResult.None;
						}
						else
						{
							AdministrativeSecureHash = SecureStringHelper.FromString(CalculateAdministrativeHash(mtxtPassword.Text.Trim(), timestamp));
							DialogResult = DialogResult.OK;
						}
					},
					() =>
					{
						HideProgress();
					},
					() => { 
						HideProgress();
						mtxtPassword.Text = string.Empty;
					}
				), 
				Resources.SiteServiceLogin);
		}

		private string CalculateAdministrativeHash(string plainPassword)
		{
			string hash = SimpleHash.ComputePassword(plainPassword, 
													PluginEntry.HashAlgorithm, 
													PluginEntry.HashSalt.GetBytes());


			return Cipher.Encrypt(hash, PluginEntry.ConfigurationCryptoKey);
		}

		private string CalculateAdministrativeHash(string plainPassword, string timestamp)
		{
			string hash = SimpleHash.ComputePassword(plainPassword, 
				PluginEntry.HashAlgorithm, 
				PluginEntry.HashSalt.GetBytes());


			return Cipher.Encrypt(hash, PluginEntry.ConfigurationCryptoKey) + PluginEntry.AdministrativePasswordDelimiter + timestamp;
		}
	}
}
