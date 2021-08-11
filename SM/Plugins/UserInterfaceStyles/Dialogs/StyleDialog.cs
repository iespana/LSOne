//style class here

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.UserInterface;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Dialogs
{
    public partial class StyleDialog : DialogBase
    {
        private bool suspendEvents;
        //private bool editingNewPosMenuHeader;
        private bool isNewStyle;

        public RecordIdentifier StyleID { get; private set; }
        public RecordIdentifier copyStyleID;
        public PosStyle Style { get; private set; }
        public UIStyle UIStyle { get; private set; }

        public StyleDialog(RecordIdentifier styleID)
            : this()
        {
            if (styleID != RecordIdentifier.Empty)
            {
                StyleID = styleID;
                //editingNewPosMenuHeader = false;
            }
            else if (styleID.DataType == RecordIdentifier.RecordIdentifierType.Guid)
            {
                string parentStyleDescription = "";

                UIStyle = Providers.UIStyleData.Get(PluginEntry.DataModel, styleID);
                isNewStyle = false;

                tbDescription.Text = UIStyle.Text;

                //tbContext.Text = contextDescription.ToString();

                styleControl.Visible = false;
                contextStyleControl.Visible = true;

                Dictionary<Guid, ContextStyleDescriptor> contexts = PluginEntry.Framework.GetPluginContextStyleDescriptors();

                if (UIStyle.ParentStyleID != RecordIdentifier.Empty && UIStyle.ParentStyleID != null)
                {
                    var parentStyle = Providers.UIStyleData.Get(PluginEntry.DataModel, UIStyle.ParentStyleID);

                    if (parentStyle != null)
                    {
                        parentStyleDescription = parentStyle.Text;
                    }
                }

                contextStyleControl.SetValues(UIStyle, contexts[(Guid)UIStyle.ContextID], false, parentStyleDescription);
            }
        }

        public StyleDialog(PosStyle posStyle)
            : this()
        {
            isNewStyle = true;
            Style = posStyle;
        }

        public StyleDialog(UIStyle uiStyle, ContextStyleDescriptor contextDescription, bool isNew, string parentStyleDescription)
            : this()
        {
            isNewStyle = isNew;
            UIStyle = uiStyle;
            tbDescription.Text = uiStyle.Text;

            styleControl.Visible = false;
            contextStyleControl.Visible = true;

            contextStyleControl.SetValues(uiStyle, contextDescription, isNew, parentStyleDescription);


        }

        public StyleDialog()
        {
            InitializeComponent();

            suspendEvents = false;
            StyleID = RecordIdentifier.Empty;
            copyStyleID = RecordIdentifier.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadData();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }



        // Returns true if the user has made modifications
        private bool IsModified()
        {
            // General
            if (Style != null)
            {
                if (!Style.IsSystemStyle && tbDescription.Text != Style.Text) return true;
                if (styleControl.Changed) return true;
            }
            else
            {
                if (tbDescription.Text != UIStyle.Text) return true;

                if (contextStyleControl.Changed) return true;
            }

            return false;
        }

        // Loads data from the data entity to controls
        private void LoadData()
        {
            ContextStyleDescriptor contextDescription;


            if (Style == null && UIStyle == null && StyleID != RecordIdentifier.Empty)
            {
                if (StyleID.DataType == RecordIdentifier.RecordIdentifierType.Guid)
                {
                    string parentStyleDescription;

                    UIStyle = Providers.UIStyleData.Get(PluginEntry.DataModel, StyleID);

                    isNewStyle = false;
                    tbDescription.Text = UIStyle.Text;

                    styleControl.Visible = false;
                    contextStyleControl.Visible = true;

                    Dictionary<Guid, ContextStyleDescriptor> contexts = PluginEntry.Framework.GetPluginContextStyleDescriptors();

                    if (contexts.ContainsKey((Guid)UIStyle.ContextID))
                    {
                        contextDescription = contexts[(Guid)UIStyle.ContextID];

                        if (UIStyle.ParentStyleID == null || UIStyle.ParentStyleID == RecordIdentifier.Empty)
                        {
                            parentStyleDescription = "";
                        }
                        else
                        {
                            UIStyle parentStyle = Providers.UIStyleData.Get(PluginEntry.DataModel, UIStyle.ParentStyleID);

                            parentStyleDescription = parentStyle.Text;
                        }

                        contextStyleControl.SetValues(UIStyle, contextDescription, false, parentStyleDescription);
                    }
                }
                else
                {
                    Style = Providers.PosStyleData.Get(PluginEntry.DataModel, StyleID);
                    if (Style != null && Style.IsSystemStyle)
                    {
                        tbDescription.Enabled = false;
                        chkSystemStyle.Checked = true;
                    }
                    tbDescription.Text = Style.IsSystemStyle ? Style.StyleTypeString : Style.Text;

                    styleControl.Visible = true;
                    contextStyleControl.Visible = false;
                    styleControl.Style = Style;
                }
            }
            else if (Style != null)
            {
                styleControl.IsNew = true;
                tbDescription.Text = Style.IsSystemStyle ? Style.StyleTypeString : Style.Text;

                styleControl.Visible = true;
                contextStyleControl.Visible = false;
                styleControl.Style = Style;
                CheckEnabled(null, EventArgs.Empty);
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            btnOK.Enabled = IsModified() && tbDescription.Text.Length > 0;
            SetPreviewButton();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Style != null)
            {
                Style = styleControl.Style;
                Style.Text = Style.IsSystemStyle ? Style.StyleType.ToString() : tbDescription.Text;

                Providers.PosStyleData.Save(PluginEntry.DataModel, Style);

                StyleID = Style.ID;
            }
            else
            {
                if (contextStyleControl.ValidateSave())
                {
                    return;
                }

                UIStyle.Text = tbDescription.Text;

                contextStyleControl.PutValuesInStyle(UIStyle.Style);

                Providers.UIStyleData.Save(PluginEntry.DataModel, UIStyle);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, isNewStyle ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "UIStyle", UIStyle.ID, null);
            }



            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetPreviewButton()
        {
            //buttonProperties.ToButton(btnMenuButtonPreview);
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            var styleProfiles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");
            //cmbCopyFrom.SetData(styleProfiles, null);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            //copyStyleID = cmbCopyFrom.SelectedData.ID;
            Style = null;
            LoadData();
        }

        private void cmbCopyFrom_RequestClear(object sender, EventArgs e)
        {
            //cmbCopyFrom.SelectedData = null;
            copyStyleID = RecordIdentifier.Empty;
            LoadData();
        }

        private void OnButtonPropertiesModified(object sender, EventArgs e)
        {
            if (Style != null)
                CheckEnabled(sender, e);
        }

        private void contextStyleControl_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }


    }
}

