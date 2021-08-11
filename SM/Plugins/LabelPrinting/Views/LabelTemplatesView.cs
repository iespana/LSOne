using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.LabelPrinting.Views
{
    public partial class LabelTemplatesView : ListBaseView<LabelTemplate>
    {
        private static readonly Guid sortSettingID = new Guid("1C37155B-4DC9-47BD-8D0A-C89AE8E4EADA");
        private readonly LabelTemplate.ContextEnum context;

        public LabelTemplatesView()
        {
            if (!DesignMode)
            { 
                InitializeComponent();
            }
        }

        public LabelTemplatesView(RecordIdentifier selectedID, LabelTemplate.ContextEnum context)
            : base(selectedID)
        {
            if (!DesignMode)
            { 
                InitializeComponent();
                this.context = context;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return context.ToString(); // We want one view to be able to exist for customer and one for items
            }
        }

        protected override IConnectionManager Connection
        {
            get { return PluginEntry.DataModel; }
        }

        protected override IApplicationCallbacks Framework
        {
            get { return PluginEntry.Framework; }
        }

        protected override bool AllowsView
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesView);
            }
        }

        protected override bool AllowsEdit
        {
            get
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit);
            }
        }

        protected override Guid SortSettingID
        {
            get { return sortSettingID; }
        }

        protected override Guid SearchBarSettingID
        {
            get { return Guid.Empty; }
        }

        protected override string ListKey
        {
            get { return "LabelTemplate"; }
        }

        protected override string ListName
        {
            get 
            { 
                return context == LabelTemplate.ContextEnum.Items ? Properties.Resources.ItemLabelTemplates : Properties.Resources.CustomerLableTemplates; 
            }
        }

        protected override Image ListIcon
        {
            get { return null; }
        }

        protected override void AddColumns()
        {
            AddColumn(Properties.Resources.ColID, 75, true, true);
            AddColumn(Properties.Resources.ColName, 250, true, true);
            AddColumn(Properties.Resources.ColDescription, 350, true, true);
            AddColumn(Properties.Resources.ColContext, 100, true, true);
            AddColumn(Properties.Resources.ColCodepage, 50, true, true);
        }

        protected override string[] Columns
        {
            get { return new[] {"LABELID", "NAME", "DESCRIPTION", "CONTEXT", "CODEPAGE"}; }
        }

        protected override List<LabelTemplate> LoadData(string sortColumn)
        {
            return Providers.LabelTemplateData.GetList(PluginEntry.DataModel, context, sortColumn);
        }

        protected override void PopulateRow(Row row, LabelTemplate template)
        {
            row.AddText((string)template.ID);
            row.AddText(template.Text);
            row.AddText(template.Description);
            row.AddText(template.ContextString);
            row.AddText(template.CodePage);
        }

        protected override void AddNew()
        {
            PluginOperations.NewLabelTemplate(context);
        }

        protected override void Edit(RecordIdentifier id)
        {
            PluginOperations.ShowLabelTemplateSheet(id);
        }

        protected override bool Delete(RecordIdentifier id)
        {
            return PluginOperations.DeleteLabelTemplate(id);
        }

        protected override bool Delete(List<RecordIdentifier> ids)
        {
            return PluginOperations.DeleteLabelTemplates(ids);
        }

        protected override void Edit(LabelTemplate record)
        {
            // Not used
        }

        protected override bool Delete(LabelTemplate record)
        {
            // Not used
            return false;
        }

        protected override bool Delete(List<LabelTemplate> records)
        {
            // Not used
            return false;
        }

        protected override void NotifyFramework(ContextMenuStrip contextMenuStrip, ListView listView)
        {
            PluginEntry.Framework.ContextMenuNotify("LabelTemplateMenuList", contextMenuStrip, listView);
        }
    }
}
