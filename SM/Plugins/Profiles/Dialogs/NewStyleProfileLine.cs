using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    /// <summary>
    /// A dialog that creates a style profile line combination for a specific style profile
    /// </summary>
    public partial class NewStyleProfileLine : DialogBase
    {        
        RecordIdentifier styleID = "";
        RecordIdentifier menuID = "";
        RecordIdentifier contextID = "";
        bool styleProfileLineEdit = false;
        private PosContext profileContextName;

        WeakReference editMenuDialog;
        WeakReference addMenuDialog;
        WeakReference editStyleDialog;
        WeakReference addStyleDialog;

        /// <summary>
        /// The unique ID of the profile line combination being created and/or edited
        /// </summary>
        public RecordIdentifier ProfileLineID { get; private set; }       
        /// <summary>
        /// The unique ID of the profile the profile line combination belongs to
        /// </summary>
        public RecordIdentifier ProfileID { get; private set;}                

        /// <summary>
        /// Default constructor for the dialog. All dialog variables are initalized
        /// </summary>
        public NewStyleProfileLine()
        {
            InitializeComponent();
            IPlugin addMenuplugin = PluginEntry.Framework.FindImplementor(this, "NewPosMenuDialog", null);
            IPlugin editMenuplugin = PluginEntry.Framework.FindImplementor(this, "EditPosMenuDialog", null);

            IPlugin addStylePlugin = PluginEntry.Framework.FindImplementor(this, "NewStyleMenuDialog", null);
            IPlugin editStylePlugin = PluginEntry.Framework.FindImplementor(this, "EditStyleMenuDialog", null);

            editMenuDialog = editMenuplugin != null ? new WeakReference(editMenuplugin) : null;
            addMenuDialog = editMenuplugin != null ? new WeakReference(addMenuplugin) : null;

            addStyleDialog = addStylePlugin != null ? new WeakReference(addStylePlugin) : null;
            editStyleDialog = editStylePlugin != null ? new WeakReference(editStylePlugin) : null;

            menuAdd.Visible = (addMenuDialog != null);
            menuEdit.Visible = (editMenuDialog != null);
            styleAdd.Visible = (addStyleDialog != null);
            styleEdit.Visible = (editStyleDialog != null);

            profileContextName = new PosContext();
            profileContextName.MenuRequired = false;
            
            this.Header = Properties.Resources.NewStyleProfileLine;
            this.Text = Properties.Resources.NewStyleProfileLine;            
        }

        /// <summary>
        /// Constructor used to create a new <see cref="PosStyleProfileLine"/> for a specific <see cref="PosStyleProfile"/>
        /// </summary>
        /// <param name="profileID">The unique ID of the style profile</param>
        public NewStyleProfileLine(RecordIdentifier profileID)
            : this()
        {
            ProfileID = profileID;
            btnOK.Enabled = false;

            contextEdit.Enabled = false;
            menuEdit.Enabled = false;
            styleEdit.Enabled = false;            
        }

        /// <summary>
        /// Constructor used for editing a profile line for a specific profile
        /// </summary>
        /// <param name="profileID">The unique ID of the style profile</param>
        /// <param name="profileLineID">The unique ID of the style profile line to be edited</param>
        public NewStyleProfileLine(RecordIdentifier profileID, RecordIdentifier profileLineID)
            :this()
        {
            ProfileID = profileID;
            ProfileLineID = profileLineID;

            this.Header = Properties.Resources.EditStyleProfileLine;
            this.Text = Properties.Resources.EditStyleProfileLine;

            LoadData();                        
        }

        private void CheckEnabled()
        {
            if (cmbContext.SelectedData != null)
            {
                profileContextName = Providers.PosContextData.Get(PluginEntry.DataModel, ((DataEntity)cmbContext.SelectedData).ID);
                if (profileContextName.MenuRequired == true)
                {
                    if (cmbMenu.SelectedData != null)
                    {
                        btnOK.Enabled = ((cmbContext.SelectedData.ID != contextID) || (cmbContext.SelectedData.ID != ""));
                    }
                    else
                        btnOK.Enabled = false;
                }
                else if (profileContextName.MenuRequired == false)
                {
                    btnOK.Enabled = true;
                }
                else
                    btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = false;
            }

            contextEdit.Enabled = cmbContext.SelectedData != null;
            menuEdit.Enabled = cmbMenu.SelectedData != null;
            styleEdit.Enabled = cmbStyle.SelectedData != null;
        }

        private void LoadData()
        {
            List<PosContext> contextProfiles;
            List<PosMenuHeader> menuProfiles;
            List<PosStyle> styleProfiles;

            contextProfiles = Providers.PosContextData.GetList(PluginEntry.DataModel);
            menuProfiles = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel);
            styleProfiles = Providers.PosStyleData.GetList(PluginEntry.DataModel);

            PosStyleProfileLine posStyleLine = new PosStyleProfileLine();
            posStyleLine = Providers.PosStyleProfileLineData.GetProfileLine(PluginEntry.DataModel, ProfileLineID, ProfileID);
            styleProfileLineEdit = true;

            DataEntity profile = contextProfiles.FirstOrDefault(f => f.Text == posStyleLine.ContextDescription);
            if (profile != null)
            {
                cmbContext.SelectedData = profile;
            }

            profile = menuProfiles.FirstOrDefault(f => f.Text == posStyleLine.MenuDescription);
            if (profile != null)
            {
                cmbMenu.SelectedData = profile;
            }

            profile = styleProfiles.FirstOrDefault(f => f.Text == posStyleLine.StyleDescription);
            if (profile != null)
            {
                cmbStyle.SelectedData = profile;
            }

            CheckEnabled();
            btnOK.Enabled = false;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            PosStyleProfileLine profile = new PosStyleProfileLine();

            PosContext profileContextName;
            if (cmbContext.SelectedData != null)
            {
                profileContextName = Providers.PosContextData.Get(PluginEntry.DataModel, ((DataEntity)cmbContext.SelectedData).ID);
                profile.Text = profileContextName.Text;
                profile.System = false;
                profile.ContextID = profileContextName.ID;
            }

            PosMenuHeader profileMenu;
            if (cmbMenu.SelectedData != null)
            {
                profileMenu = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, ((DataEntity)cmbMenu.SelectedData).ID);
                profile.MenuID = profileMenu.ID;
            }

            PosStyle profileStyle;
            if (cmbStyle.SelectedData != null)
            {
                profileStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, ((DataEntity)cmbStyle.SelectedData).ID);
                profile.StyleID = profileStyle.ID;
            }

            if (SaveInfo(PluginEntry.DataModel, profile))
            {

                ProfileLineID = profile.PosStyleProfileLineId;

                DialogResult = DialogResult.OK;
                Close();
            }
        }        

        private bool SaveInfo(IConnectionManager entry, PosStyleProfileLine posStyleProfileLine)
        {
            if (Providers.PosStyleProfileLineData.ProfileLineExists(entry, posStyleProfileLine, ProfileID))
            {
                MessageDialog.Show(Properties.Resources.StyleProfileLineAlreadyExists, Properties.Resources.NewStyleProfileLine);
                return false;
            }

            if (styleProfileLineEdit)
            {
                posStyleProfileLine.PosStyleProfileLineId = ProfileLineID;
                posStyleProfileLine.ProfileID = ProfileID;
                Providers.PosStyleProfileLineData.Save(PluginEntry.DataModel, posStyleProfileLine);
            }
            else
            {
                posStyleProfileLine.PosStyleProfileLineId = RecordIdentifier.Empty;
                posStyleProfileLine.ProfileID = ProfileID;
                Providers.PosStyleProfileLineData.Save(PluginEntry.DataModel, posStyleProfileLine);
            }

            return true;
        }        

        private void contextEdit_Click(object sender, EventArgs e)
        {            
            if (cmbContext.SelectedData != null)
            {
                PosContext newContext = PluginOperations.EditPosContext(((DataEntity)cmbContext.SelectedData).ID);
                if (newContext != null)
                {
                    cmbContext.SelectedData = new DataEntity(newContext.ID, newContext.Text);
                }
            }
            CheckEnabled();
        }

        private void contextAdd_Click(object sender, EventArgs e)
        {
            PosContext newContext = PluginOperations.NewPosContext();
            if (newContext != null)
            {
                cmbContext.SelectedData = new DataEntity(newContext.ID, newContext.Text);
            }
            CheckEnabled();
        }

        private void styleAdd_Click(Object sender, EventArgs e)
        {                       
            if (addStyleDialog.IsAlive)
            {
                RecordIdentifier styleID = (RecordIdentifier)((IPlugin)addStyleDialog.Target).Message(this, "NewStyleSetup", RecordIdentifier.Empty);
                if (styleID != RecordIdentifier.Empty)
                {
                    PosStyle style = Providers.PosStyleData.Get(PluginEntry.DataModel, styleID);
                    cmbStyle.SelectedData = new DataEntity(style.ID, style.Text);                                            
                }
            }
            CheckEnabled();
        }

        private void styleEdit_Click(object sender, EventArgs e)
        {
            if (cmbStyle.SelectedData != null)
            {
                if (editStyleDialog.IsAlive)
                {
                    RecordIdentifier styleID = (RecordIdentifier)((IPlugin)editStyleDialog.Target).Message(this, "EditStyleSetup", ((DataEntity)cmbStyle.SelectedData).ID);
                    if (styleID != RecordIdentifier.Empty)
                    {
                        PosStyle style = Providers.PosStyleData.Get(PluginEntry.DataModel, styleID);
                        cmbStyle.SelectedData = new DataEntity(style.ID, style.Text);                        
                    }
                }
            }
            CheckEnabled();
        }

        private void menuAdd_Click(Object sender, EventArgs e)
        {

            if (addMenuDialog.IsAlive)
            {
                RecordIdentifier menuID = (RecordIdentifier)((IPlugin)addMenuDialog.Target).Message(this, "AddPosMenuHeader", RecordIdentifier.Empty);
                if (menuID != RecordIdentifier.Empty)
                {
                    PosMenuHeader menuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, menuID);
                    cmbMenu.SelectedData = new DataEntity(menuHeader.ID, menuHeader.Text); 
                }                   
            }
            CheckEnabled();
        }        

        private void menuEdit_Click(Object sender, EventArgs e)
        {
            if ((cmbMenu.SelectedData) != null)
            {
                if (editMenuDialog.IsAlive)
                {
                    RecordIdentifier menuID = (RecordIdentifier)((IPlugin)editMenuDialog.Target).Message(this, "EditPosMenuHeader", ((DataEntity)cmbMenu.SelectedData).ID);
                    if (menuID != RecordIdentifier.Empty)
                    {
                        PosMenuHeader menuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, menuID);
                        cmbMenu.SelectedData = new DataEntity(menuHeader.ID, menuHeader.Text);
                    }
                }
            }
            CheckEnabled();
        }        

        private void cmbContext_RequestData(object sender, EventArgs e)
        {
            cmbContext.SetData(Providers.PosContextData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbMenu_RequestData(object sender, EventArgs e)
        {
            cmbMenu.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStyle_RequestData(object sender, EventArgs e)
        {
            cmbStyle.SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel), null);
        }       

        private void cmbContext_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbMenu_SelectedDataChanged(object sender, EventArgs e)
        {            
            CheckEnabled();
        }

        private void cmbStyle_SelectedDataChanged(object sender, EventArgs e)
        {            
            CheckEnabled();
        }        
    }
}
