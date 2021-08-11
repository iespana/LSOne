using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Profiles.Properties;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    /// <summary>
    /// A dialog to create and/or edit a <see cref="PosContext"/>
    /// </summary>
    public partial class NewContext : DialogBase
    {
        RecordIdentifier contextID = "";

        /// <summary>
        /// The <see cref="PosContext"/> being edited and/or created
        /// </summary>
        public PosContext Context { get; private set; } 

        /// <summary>
        /// Default constructor for the dialog. All variables are initialized
        /// </summary>
        public NewContext()
        {
            InitializeComponent();
            Context = new PosContext();
            contextID = RecordIdentifier.Empty;

            this.Header = Resources.NewContext;
            this.Text = Resources.NewContext;
        }

        /// <summary>
        /// Constructor for editing an existing <see cref="PosContext"/>
        /// </summary>
        /// <param name="id">The unique ID of the <see cref="PosContext"/> being edited</param>
        public NewContext(RecordIdentifier id) : this()
        {
            if (id != RecordIdentifier.Empty)
            {
                Context = Providers.PosContextData.Get(PluginEntry.DataModel, id);
                contextID = Context.ID;

                txtDescription.Text = Context.Text;
                txtDescription.Focus();

                if (Context.MenuRequired == true)
                {
                    chkMenuRequired.Checked = true;
                }

                this.Header = Properties.Resources.EditContext;
                this.Text = Properties.Resources.EditContext;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Context.ID = contextID;
            Context.Text = txtDescription.Text;
            if (chkMenuRequired.Checked)
            {
                Context.MenuRequired = true;
            }

            Providers.PosContextData.Save(PluginEntry.DataModel, Context);

            contextID = Context.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (txtDescription.Text.Length > 0);
        }
    }
}
