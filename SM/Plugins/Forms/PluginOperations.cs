using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Forms.Dialogs;
using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;

namespace LSOne.ViewPlugins.Forms
{
    internal class PluginOperations
    {
        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Profiles.Views.FormProfilesView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.FormProfileView))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.FormLayouts, new ContextbarClickEventHandler(PluginOperations.ShowFormsSheet)), 300);
                }
            }
        }

        public static void ShowFormsSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FormsView());
        }

        public static void ShowFormTypesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FormTypesView());
        }

        public static void ShowFormSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FormView(id));
        }

        public static void ShowNewFormSheet(RecordIdentifier id, int formWidth)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FormView(id, formWidth));
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsView))
                {
                    args.Add(new Item(Properties.Resources.Forms, "Forms", null), 1000);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Forms")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsView))
                {
                    args.Add(
                        new ItemButton(Properties.Resources.FormLayouts, Properties.Resources.FormLayoutsDescription,
                                       new EventHandler(ShowFormsSheet)), 10);
                    args.Add(new ItemButton(Properties.Resources.FormTypes, Properties.Resources.FormTypesDescription,new EventHandler(ShowFormTypesSheet)), 11);
                }
            }
        }

        public static bool DeleteForm(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteFormQuestion, Properties.Resources.DeleteForm) == DialogResult.Yes)
                {
                    Providers.FormData.Delete(PluginEntry.DataModel, id);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "Form", id, null);
                    return true;
                }
            }

            return false;
        }

        public static bool DeleteForms(List<RecordIdentifier> ids)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteFormsQuestion, Properties.Resources.DeleteForms) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier id in ids)
                    {
                        Providers.FormData.Delete(PluginEntry.DataModel, id);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "Form", null, ids);
                    return true;
                }
            }

            return false;
        }

        public static bool DeleteFormType(RecordIdentifier formTypeId)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                var formType = Providers.FormTypeData.Get(PluginEntry.DataModel, formTypeId);
                if (formType.SystemType == 0)
                {
                    if (QuestionDialog.Show(Properties.Resources.DeleteFormTypeQuestion, Properties.Resources.DeleteFormType) ==
                        DialogResult.Yes)
                    {
                        Providers.FormTypeData.Delete(PluginEntry.DataModel, formTypeId);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "FormType",
                                                                               formTypeId, null);
                        return true;
                    }
                }
                else
                {
                    MessageDialog.Show(Properties.Resources.SystemTypesCannotBeDeleted, Properties.Resources.DeleteFormType);
                }
            }

            return false;
        }

        public static bool DeleteFormTypes(List<RecordIdentifier> formTypeIds)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                foreach (var formTypeId in formTypeIds)
                {
                    var formType = Providers.FormTypeData.Get(PluginEntry.DataModel, formTypeId);
                    if (formType.SystemType != 0)
                    {
                        MessageDialog.Show(Properties.Resources.SystemTypesCannotBeDeleted, Properties.Resources.DeleteFormTypes);
                        return false;
                    }
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteFormTypesQuestion, Properties.Resources.DeleteFormTypes) == DialogResult.Yes)
                {
                    var provider = Providers.FormTypeData;
                    foreach (RecordIdentifier id in formTypeIds)
                    {
                        provider.Delete(PluginEntry.DataModel, id);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "Form", null, formTypeIds);
                    return true;
                }
            }

            return false;
        }

        public static RecordIdentifier NewForm()
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                Dialogs.NewFormDialog dlg = new Dialogs.NewFormDialog();

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.FormID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", selectedId, null);
                    ShowNewFormSheet(selectedId, dlg.FormWidth);
                }
            }

            return selectedId;
        }

        public static RecordIdentifier NewForm(RecordIdentifier formTypeID)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                Dialogs.NewFormDialog dlg = new Dialogs.NewFormDialog(formTypeID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.FormID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", selectedId, null);
                    ShowNewFormSheet(selectedId, dlg.FormWidth);
                }
            }

            return selectedId;
        }

        public static RecordIdentifier NewFormForProfileLine()
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                Dialogs.NewFormDialog dlg = new Dialogs.NewFormDialog();

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.FormID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", selectedId, null);
                }
            }

            return selectedId;
        }

        public static RecordIdentifier NewFormForProfileLine(RecordIdentifier formTypeID)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                Dialogs.NewFormDialog dlg = new Dialogs.NewFormDialog(formTypeID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.FormID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", selectedId, null);
                }
            }

            return selectedId;
        }

        public static RecordIdentifier EditFormForProfileLine(RecordIdentifier formTypeID, RecordIdentifier formID)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                Dialogs.NewFormDialog dlg = new Dialogs.NewFormDialog(formTypeID, formID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.FormID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", selectedId, null);
                }
            }

            return selectedId;
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Sites, "Sites"), 700);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                args.Add(new PageCategory(Properties.Resources.FormsAndLabels, "Forms"), 500);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "Forms")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsView))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.FormLayouts,
                            Properties.Resources.FormLayouts,
                            Properties.Resources.FormLayoutsTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button ,
                            null,
                            Properties.Resources.form_layouts_32,
                            new EventHandler(ShowFormsSheet),
                            "ShowForms"), 10);

                    args.Add(new CategoryItem(
                            Properties.Resources.FormTypes,
                            Properties.Resources.FormTypes,
                            Properties.Resources.FormTypesTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            new EventHandler(ShowFormTypesSheet),
                            "ShowFormTypesSheet"), 30);
                }
            }
        }

        public static RecordIdentifier NewFormType()
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                FormTypeDialog dlg = new FormTypeDialog();

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedId = dlg.FormTypeID;
                }
            }
            return selectedId;
        }

        public static RecordIdentifier NewFormType(RecordIdentifier profileId)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                FormTypeDialog dlg = new FormTypeDialog();

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedId = dlg.FormTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FormProfile", profileId, null);
                }
            }
            return selectedId;
        }

        public static RecordIdentifier EditFormType(RecordIdentifier formTypeId)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                FormTypeDialog dlg = new FormTypeDialog(formTypeId);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedId = dlg.FormTypeID;
                }
            }
            return selectedId;
        }

        public static RecordIdentifier EditFormType(RecordIdentifier profileId, RecordIdentifier formTypeId)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit))
            {
                FormTypeDialog dlg = new FormTypeDialog(formTypeId);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedId = dlg.FormTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "FormProfile", profileId, null);
                }
            }
            return selectedId;
        }

        public static void ExportLayouts(List<RecordIdentifier> formIds, bool selectedFormNotExported)
        {
            if (formIds.Count == 0 || selectedFormNotExported)
            {
                MessageDialog.Show(Properties.Resources.AllSelectFormsOfUserDefinedType);
                return;
            }
            var dlg = new SaveFileDialog
            {
                Filter = "Form" + " (*.form)|*.form",
                DefaultExt = ".form"
            };

            var dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement rootNode = new XElement("Root");
                doc.Add(rootNode);

                foreach (var formId in formIds)
                {
                    var form = Providers.FormData.Get(PluginEntry.DataModel, formId);
                    var formXml = form.ToXML();
                    doc.Element("Root").Add(formXml);
                }

                doc.Save(dlg.FileName);
            }
        }

        public static void ImportLayouts()
        {
            var dlg = new OpenFileDialog { CheckFileExists = true, Multiselect = false, Filter = "Form" + " (*.form)|*.form" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                XDocument doc = XDocument.Load(dlg.FileName);
                XElement root = doc.Root;

                IEnumerable<XElement> formElements = root.Elements("receiptLayout");
                var forms = new List<Form>();
                foreach (var xForm in formElements)
                {
                    var tmp = new Form();
                    tmp.ToClass(xForm);
                    forms.Add(tmp);
                }

                foreach (var form in forms.Where(f => Providers.FormData.Exists(PluginEntry.DataModel, f.ID)))
                {
                    form.ID = RecordIdentifier.Empty;
                }
                foreach (var form in forms)
                {
                    Providers.FormData.Save(PluginEntry.DataModel, form);
                }
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Form", null, null);
            }
        }
    }
}
