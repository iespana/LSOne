using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
	public partial class NewCategoryDialog : DialogBase
	{
		public GroupCategory Category { get; set; }

		private bool editCategory;
		private List<GroupCategory> categories;

		public NewCategoryDialog(GroupCategory category)
		{
			InitializeComponent();

			this.Category = category;
			editCategory = this.Category.ID != RecordIdentifier.Empty;

			Header = editCategory ? Properties.Resources.EditCategoryHeader : Properties.Resources.NewCategoryHeader;

			Text = editCategory ? Properties.Resources.EditCategoryDlgCaption : Properties.Resources.NewCategoryDlgCaption;

			categories = Providers.GroupCategoryData.GetList(PluginEntry.DataModel);

			if (category.ID != RecordIdentifier.Empty)
			{
				tbDescription.Text = category.Text;
			}

			CheckEnabled(this, new EventArgs());
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void CheckEnabled(object sender, EventArgs e)
		{
			btnOK.Enabled = (editCategory && tbDescription.Text != Category.Text) || (!editCategory && tbDescription.Text.Trim() != string.Empty)
								? true 
								: false;
			errorProvider.Clear();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			GroupCategory exists = categories.FirstOrDefault(f => f.Text.ToLower() == tbDescription.Text.ToLower());
			if (!editCategory && exists != null)
			{
				errorProvider.SetError(tbDescription, Properties.Resources.CategoryDescriptionAlreadyExists);
				btnOK.Enabled = false;
				return;
			}

			Category.Text = tbDescription.Text;

			Providers.GroupCategoryData.Save(PluginEntry.DataModel, Category);
			PluginEntry.Framework.ViewController.NotifyDataChanged(this, editCategory ? DataEntityChangeType.Edit : DataEntityChangeType.Add, "Categories", Category.ID, Category);

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
