using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class DatabaseDesignTree : UserControl
    {
        private bool filterOnlyLinked;
        private bool filterIncludeDisabled;
        private string filterTableName;

        public enum ItemType
        {
            DatabaseDesign,
            TableDesign,
            LinkedTable,
        }


        public enum FilterLevelType
        {
            Databases,
            DatabasesAndTables,
            DatabasesAndTablesAndLinkedTables
        }

        public class Item
        {
            public ItemType ItemType { get; set; }
            public object Object { get; set; }
        }

        public DatabaseDesignTree()
        {
            this.FilterLevel = FilterLevelType.DatabasesAndTablesAndLinkedTables;
            InitializeComponent();
        }


        [Browsable(true)]
        [DefaultValue(typeof(FilterLevelType), "DatabasesAndTablesAndLinkedTables")]
        [Category("Behavior")]
        public FilterLevelType FilterLevel { get; set; }

        [Browsable(false)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return tvMain.ContextMenuStrip;
            }
        }


        [Browsable(true)]
        [DefaultValue("")]
        [Category("Behavior")]
        public string FilterTableName
        {
            get
            {
                return filterTableName;
            }

            set
            {
                if (filterTableName != value)
                {
                    filterTableName = value;

                    // Filter on selected node and subnodes. Use first node if none selected.
                    TreeNode node = tvMain.SelectedNode;
                    if (node == null && tvMain.Nodes.Count > 0)
                    {
                        node = tvMain.Nodes[0];
                    }

                    if (node == null)
                    {
                        return;
                    }

                    // Make sure we refresh the table list if needed
                    Item item = (Item)node.Tag;
                    if (item.ItemType == ItemType.DatabaseDesign)
                    {
                        if (!NeedsExpandItems(node))
                        {
                            // Already expanded, must refresh
                            RefreshTableNodes(node);
                        }
                    }
                    else if (item.ItemType == ItemType.TableDesign)
                    {
                        // Refresh parent
                        RefreshTableNodes(node.Parent);
                    }
                    else if (item.ItemType == ItemType.LinkedTable)
                    {
                        // Refresh grandparent
                        RefreshTableNodes(node.Parent.Parent);
                    }
                }
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [Category("Behavior")]
        public bool SelectNodeOnExpand { get; set; }

        [Browsable(true)]
        [DefaultValue("")]
        [Category("Behavior")]
        public bool ExpandIfSingleRoot { get; set; }

        public event EventHandler<CancelEventArgs> SelectedItemChanging;
        public event EventHandler<EventArgs> SelectedItemChanged;
        public event EventHandler<EventArgs> ItemDoubleClicked;
        public event EventHandler<CancelEventArgs> OpeningContextMenuStrip;


        public Item SelectedItem
        {
            get
            {
                TreeNode node = tvMain.SelectedNode;
                if (node != null)
                    return (Item)node.Tag;
                else
                    return null;
            }
        }

        public void LoadData()
        {
            RefreshTreeData();
        }

        private void RefreshTreeData()
        {
            Cursor.Current = Cursors.WaitCursor;
            tvMain.BeginUpdate();
            tvMain.Nodes.Clear();
            foreach (var databaseDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesigns(PluginEntry.DataModel, filterIncludeDisabled))
            {
                tvMain.Nodes.Add(CreateDatabaseNode(databaseDesign));
            }
            if (tvMain.Nodes.Count == 1 && ExpandIfSingleRoot)
            {
                tvMain.Nodes[0].Expand();
            }
            lblNoDesignsFound.Visible = tvMain.Nodes.Count == 0;
            tvMain.EndUpdate();
        }


        private TreeNode CreateDatabaseNode(JscDatabaseDesign databaseDesign)
        {
            TreeNode node = new TreeNode();
            node.Tag = new Item { ItemType = DatabaseDesignTree.ItemType.DatabaseDesign, Object = databaseDesign };
            SyncDatabaseNode(node);
            if (FilterLevel >= FilterLevelType.DatabasesAndTables)
            {
                node.Nodes.Add(CreateDummyNode());
            }

            return node;
        }

        private static void SyncDatabaseNode(TreeNode node)
        {
            var databaseDesign = (JscDatabaseDesign)((Item)node.Tag).Object;
            node.Text = databaseDesign.Description;
            if (databaseDesign.Enabled)
            {
                node.ImageKey = "DatabaseDesign";
            }
            else
            {
                node.ImageKey = "DataDisabled";
            }
            node.SelectedImageKey = node.ImageKey;
        }

        private TreeNode CreateDummyNode()
        {
            return new TreeNode();
        }

        private void tvMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand && NeedsExpandItems(e.Node))
            {
                Item item = (Item)e.Node.Tag;

                switch (item.ItemType)
                {
                    case ItemType.DatabaseDesign:
                        RefreshTableNodes(e.Node);
                        break;
                    case ItemType.TableDesign:
                        RefreshTableChildNodes(e.Node);
                        break;
                    case ItemType.LinkedTable:
                        break;
                    default:
                        break;
                }
            }
        }

        private bool NeedsExpandItems(TreeNode treeNode)
        {
            return treeNode.Nodes.Count == 1 && treeNode.Nodes[0].Tag == null;
        }

        private void RefreshTableNodes(TreeNode treeNode)
        {
            Cursor.Current = Cursors.WaitCursor;
            JscDatabaseDesign databaseDesign = (JscDatabaseDesign)(((Item)treeNode.Tag).Object);
            tvMain.BeginUpdate();
            treeNode.Nodes.Clear();
            foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(PluginEntry.DataModel, databaseDesign.ID, FilterOnlyLinked))
            {
                if (filterTableName == null || tableDesign.TableName.IndexOf(filterTableName, StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    treeNode.Nodes.Add(CreateTableNode(tableDesign));
                }
            }
            tvMain.EndUpdate();
        }



        private TreeNode CreateTableNode(JscTableDesign tableDesign)
        {
            TreeNode node = new TreeNode();
            node.Text = tableDesign.TableName;
            node.Tag = new Item { ItemType = ItemType.TableDesign, Object = tableDesign };
            if (tableDesign.Enabled)
            {
                node.ImageKey = "TableDesign";
            }
            else
            {
                node.ImageKey = "TableDesignDisabled";
            }
            node.SelectedImageKey = node.ImageKey;

            if (FilterLevel >= FilterLevelType.DatabasesAndTablesAndLinkedTables)
            {
                node.Nodes.Add(CreateDummyNode());
            }

            return node;
        }


        private TreeNode CreateLinkedTableNode(JscLinkedTable linkedTable)
        {
            TreeNode node = new TreeNode();
            node.Text = linkedTable.ToJscTableDesign.TableName;
            node.Tag = new Item { ItemType = ItemType.LinkedTable, Object = linkedTable };
            node.ImageKey = "LinkedTable";
            node.SelectedImageKey = node.ImageKey;

            return node;
        }




        private void RefreshTableChildNodes(TreeNode treeNode)
        {
            Cursor.Current = Cursors.WaitCursor;
            tvMain.BeginUpdate();
            JscTableDesign tableDesign = (JscTableDesign)((Item)treeNode.Tag).Object;
            treeNode.Nodes.Clear();
            foreach (var linkedTable in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetLinkedTables(PluginEntry.DataModel, tableDesign.ID))
            {
                treeNode.Nodes.Add(CreateLinkedTableNode(linkedTable));
            }
            tvMain.EndUpdate();
        }


        private void tvMain_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (SelectedItem != (Item)e.Node.Tag)
            {
                OnSelectedItemChanging(e);
            }
        }

        private void OnSelectedItemChanging(TreeViewCancelEventArgs e)
        {
            if (SelectedItemChanging != null)
            {
                CancelEventArgs eventArgs = new CancelEventArgs { Cancel = e.Cancel };
                SelectedItemChanging(this, eventArgs);
                e.Cancel = eventArgs.Cancel;
            }
        }

    
        private void tvMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectedItem != null)
            {
                OnSelectedItemChanged();
            }
        }

        private void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, EventArgs.Empty);
            }
        }

        private void tvMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = tvMain.GetNodeAt(e.Location);
                if (node != null)
                {
                    tvMain.SelectedNode = node;
                }
            }
        }


        public JscDatabaseDesign GetDatabaseDesignOfSelection()
        {
            JscDatabaseDesign databaseDesign = null;
            TreeNode node = tvMain.SelectedNode;
            while (node != null && databaseDesign == null)
            {
                Item item = (Item)node.Tag;
                if (item.ItemType == ItemType.DatabaseDesign)
                {
                    databaseDesign = (JscDatabaseDesign)item.Object;
                }
                else
                {
                    node = node.Parent;
                }
            }

            return databaseDesign;
        }


        public JscTableDesign GetTableDesignOfSelection()
        {
            JscTableDesign tableDesign = null;
            TreeNode node = tvMain.SelectedNode;
            while (node != null && tableDesign == null)
            {
                Item item = (Item)node.Tag;
                if (item.ItemType == ItemType.TableDesign)
                {
                    tableDesign = (JscTableDesign)item.Object;
                }
                else
                {
                    node = node.Parent;
                }
            }

            return tableDesign;
        }

        
        public void Add(JscLinkedTable linkedTable)
        {
            Cursor.Current = Cursors.WaitCursor;
            tvMain.BeginUpdate();
            TreeNode parentNode = FindTableNode(linkedTable.FromJscTableDesign);
            if (parentNode == null)
            {
                throw new ArgumentException("The specified linked table does not have any parent data in the tree view");
            }

            TreeNode newNode;

            if (NeedsExpandItems(parentNode))
            {
                RefreshTableChildNodes(parentNode);
                newNode = FindLinkedTableNode(parentNode, linkedTable);
            }
            else
            {
                newNode = CreateLinkedTableNode(linkedTable);
                parentNode.Nodes.Add(newNode);
            }
                
            // Make sure that the new node is visible and selected
            if (!parentNode.IsExpanded)
            {
                parentNode.Expand();
            }

            if (newNode != null)
            {
                tvMain.SelectedNode = newNode;
            }
            tvMain.EndUpdate();
        }



        private TreeNode FindTableNode(JscTableDesign tableDesign)
        {
            TreeNode databaseNode = FindDatabaseNode((Guid) tableDesign.JscDatabaseDesign.ID);
            if (databaseNode == null)
                return null;

            foreach (TreeNode tableNode in databaseNode.Nodes)
            {
                if (((JscTableDesign)((Item)tableNode.Tag).Object).ID == tableDesign.ID)
                {
                    return tableNode;
                }
            }

            return null;
        }

        private TreeNode FindLinkedTableNode(TreeNode tableNode, JscLinkedTable linkedTable)
        {
            foreach (TreeNode linkedTableNode in tableNode.Nodes)
            {
                if (((JscLinkedTable)((Item)linkedTableNode.Tag).Object).ID == linkedTable.ID)
                {
                    return linkedTableNode;
                }
            }

            return null;
        }


        private TreeNode FindDatabaseNode(Guid databaseDesignId)
        {
            foreach (TreeNode databaseNode in tvMain.Nodes)
            {
                if (((JscDatabaseDesign)((Item)databaseNode.Tag).Object).ID == databaseDesignId)
                {
                    return databaseNode;
                }
            }

            return null;
        }


        public bool FilterOnlyLinked
        {
            get { return filterOnlyLinked; }
            set
            {
                if (filterOnlyLinked != value)
                {
                    filterOnlyLinked = value;
                    RefreshTreeData();
                }
            }
        }

        public bool FilterIncludeDisabled
        {
            get { return filterIncludeDisabled; }
            set
            {
                if (filterIncludeDisabled != value)
                {
                    filterIncludeDisabled = value;
                    RefreshTreeData();
                }
            }
        }


        public void DeleteItem(Item item)
        {
            TreeNode treeNode = FindTreeNode(tvMain.Nodes, item);
            if (treeNode != null)
                treeNode.Remove();
        }

        private TreeNode FindTreeNode(TreeNodeCollection treeNodeCollection, Item item)
        {
            TreeNode result = null;

            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (object.ReferenceEquals(treeNode.Tag, item))
                {
                    result = treeNode;
                    break;
                }
                else if (treeNode.Nodes.Count > 0)
                {
                    result = FindTreeNode(treeNode.Nodes, item);
                    if (result != null)
                        break;
                }
            }

            return result;
        }




        /// <summary>
        /// Refreshes the information showed in the tree about the specified database design.
        /// </summary>
        /// <param name="id"></param>
        public void RefreshDatabaseDesign(Guid id)
        {
            TreeNode treeNode = FindDatabaseNode(id);
            if (treeNode != null)
            {
                SyncDatabaseNode(treeNode);
            }
        }

        private void tvMain_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (SelectNodeOnExpand)
            {
                tvMain.SelectedNode = e.Node;
            }
        }



        public bool SelectItem(Guid databaseDesignId, Guid tableDesignId)
        {
            var databaseNode = FindDatabaseNode(databaseDesignId);
            if (databaseNode == null)
            {
                return false;
            }

            databaseNode.Expand();

            foreach (TreeNode treeNode in databaseNode.Nodes)
            {
                var item = (Item)treeNode.Tag;
                if (item.ItemType == ItemType.TableDesign && tableDesignId == ((JscTableDesign)item.Object).ID)
                {
                    tvMain.SelectedNode = treeNode;
                    tvMain.Focus();
                    return true;
                }
            }

            return false;
        }

        private void tvMain_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (ItemDoubleClicked != null)
            {
                ItemDoubleClicked(this, EventArgs.Empty);
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip.Items.Clear();

            ExtendedMenuItem item;

            if (tvMain.SelectedNode != null)
            {
                if (!tvMain.SelectedNode.IsExpanded)
                {
                    item = new ExtendedMenuItem(
                            Properties.Resources.ExpandNode,
                            100,
                            new EventHandler(ExpandSelectedItem));
                }
                else
                {
                    item = new ExtendedMenuItem(
                            Properties.Resources.CollapseNode,
                            100,
                            new EventHandler(CollapseSelectedItem));
                }
                item.Default = true;
                contextMenuStrip.Items.Add(item);
            }

            OnOpeningContextMenuStrip(e);
        }


        private void ExpandSelectedItem(object sender, EventArgs e)
        {
            if (tvMain.SelectedNode != null && !tvMain.SelectedNode.IsExpanded)
            {
                tvMain.SelectedNode.Expand();
            }
        }


        private void CollapseSelectedItem(object sender, EventArgs e)
        {
            if (tvMain.SelectedNode != null && tvMain.SelectedNode.IsExpanded)
            {
                tvMain.SelectedNode.Collapse();
            }
        }

        private void OnOpeningContextMenuStrip(CancelEventArgs e)
        {
            if (OpeningContextMenuStrip != null)
            {
                OpeningContextMenuStrip(this, e);
            }
        }


        private void tvMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Forward key press events from tree to KeyPress event on control
            base.OnKeyPress(e);
        }

    }



}
