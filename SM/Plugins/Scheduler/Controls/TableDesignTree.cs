using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class TableDesignTree : UserControl
    {

        public TableDesignTree()
        {
            InitializeComponent();
        }


        public void LoadData()
        {
         
            tvDesigns.BeginUpdate();
            tvDesigns.Nodes.Clear();

            foreach (var databaseDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesigns(PluginEntry.DataModel, false))
            {
                tvDesigns.Nodes.Add(CreateDatabaseNode(databaseDesign));
            }

            tvDesigns.EndUpdate();
            lblNoDesignsFound.Visible = tvDesigns.Nodes.Count == 0;
        }


        public JscDatabaseDesign SelectedDatabaseDesign
        {
            get
            {
                JscDatabaseDesign result;

                TreeNode node = tvDesigns.SelectedNode;
                if (node != null)
                {
                    result = node.Tag as JscDatabaseDesign;
                }
                else
                {
                    result = null;
                }

                return result;
            }
        }


        public JscTableDesign SelectedTableDesign
        {
            get
            {
                JscTableDesign result;

                TreeNode node = tvDesigns.SelectedNode;
                if (node != null)
                {
                    result = node.Tag as JscTableDesign;
                }
                else
                {
                    result = null;
                }

                return result;
            }
        }


        public bool CheckBoxes
        {
            get { return tvDesigns.CheckBoxes; }
            set { tvDesigns.CheckBoxes = value; }
        }

        public List<JscTableDesign> GetCheckedTableDesigns()
        {
            List<JscTableDesign> tables = new List<JscTableDesign>();
            CollectCheckedTableDesigns(tables, tvDesigns.Nodes);

            return tables;
        }


        public event EventHandler<DatabaseDesignEventArgs> DatabaseDesignSelected;

        public event EventHandler<TableDesignEventArgs> TableDesignSelected;

        public event EventHandler<ItemCheckChangedEventArgs> ItemCheckedChanged;


        private void tvDesigns_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand)
            {
                AssertTableNodes(e.Node);
            }
        }

        private TreeNode CreateDatabaseNode(JscDatabaseDesign databaseDesign)
        {
            TreeNode node = new TreeNode();
            node.Text = databaseDesign.Description;
            node.Tag = databaseDesign;
            node.ImageIndex = 0;
            node.SelectedImageIndex = node.ImageIndex;
            node.Nodes.Add(CreateDummyNode());

            return node;
        }

        private TreeNode CreateTableNode(JscTableDesign tableDesign)
        {
            TreeNode node = new TreeNode();
            node.Text = tableDesign.TableName;
            node.Tag = tableDesign;
            node.ImageIndex = 1;
            node.SelectedImageIndex = node.ImageIndex;

            return node;
        }

        private TreeNode CreateDummyNode()
        {
            return new TreeNode();
        }

        private bool NeedsTableNodes(TreeNode treeNode)
        {
            return treeNode.Nodes.Count == 1 && treeNode.Nodes[0].Tag == null;
        }

        private void AssertTableNodes(TreeNode treeNode)
        {
            if (NeedsTableNodes(treeNode))
            {
                tvDesigns.BeginUpdate();
                treeNode.Nodes.Clear();
                JscDatabaseDesign databaseDesign = (JscDatabaseDesign)treeNode.Tag;
                foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(PluginEntry.DataModel, databaseDesign.ID, false))
                {
                    treeNode.Nodes.Add(CreateTableNode(tableDesign));
                }
                tvDesigns.EndUpdate();
            }
        }

        private void tvDesigns_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is JscDatabaseDesign)
            {
                OnDatabaseDesignSelected((JscDatabaseDesign)e.Node.Tag);
            }
            else if (e.Node.Tag is JscTableDesign)
            {
                OnTableDesignSelected((JscTableDesign)e.Node.Tag);
            }
        }

        private void OnDatabaseDesignSelected(JscDatabaseDesign databaseDesign)
        {
            if (DatabaseDesignSelected != null)
            {
                DatabaseDesignSelected(this, new DatabaseDesignEventArgs { DatabaseDesign = databaseDesign });
            }
        }

        private void OnTableDesignSelected(JscTableDesign tableDesign)
        {
            if (TableDesignSelected != null)
            {
                TableDesignSelected(this, new TableDesignEventArgs { TableDesign = tableDesign });
            }
        }

        private void OnItemChecked()
        {
            if (ItemCheckedChanged != null)
            {
                ItemCheckChangedEventArgs e = new ItemCheckChangedEventArgs { HasCheckedItems = this.HasCheckedItems(tvDesigns.Nodes) };
                ItemCheckedChanged(this, e);
            }
        }

        private bool HasCheckedItems(TreeNodeCollection nodes)
        {
            bool result = false;

            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    result = true;
                    break;
                }
                else
                {
                    result = HasCheckedItems(node.Nodes);
                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public bool FindTableDesign(string tableName)
        {
            bool result = false;

            TreeNode treeNode = tvDesigns.SelectedNode;
            if (treeNode.Tag is JscTableDesign)
            {
                treeNode = treeNode.Parent;
            }
            else
            {
                // A database design node is selected. If it isn't expand it we must do so now.
                AssertTableNodes(treeNode);
            }

            foreach (TreeNode node in treeNode.Nodes)
            {
                JscTableDesign tableDesign = (JscTableDesign)node.Tag;
                if (StringComparer.InvariantCultureIgnoreCase.Compare(tableName, tableDesign.TableName) == 0)
                {
                    tvDesigns.SelectedNode = node;
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void tvDesigns_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                e.Graphics.DrawString(e.Node.Text, this.Font, SystemBrushes.HighlightText, e.Bounds.Location);
                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
            }
        }


        private void CollectCheckedTableDesigns(List<JscTableDesign> tables, TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (treeNode.Checked)
                {
                    JscTableDesign table = treeNode.Tag as JscTableDesign;
                    if (table != null)
                    {
                        tables.Add(table);
                    }
                }

                CollectCheckedTableDesigns(tables, treeNode.Nodes);
            }
        }


        private void tvDesigns_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (!e.Node.IsExpanded && e.Node.Nodes.Count > 0)
            {
                e.Node.Expand();
            }
        }

        private void tvDesigns_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                SetChildNodesChecked(e.Node.Nodes, e.Node.Checked);
            }
            OnItemChecked();
        }

        private void SetChildNodesChecked(TreeNodeCollection treeNodeCollection, bool checkedValue)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                treeNode.Checked = checkedValue;
            }
        }

        private void TableDesignTree_Load(object sender, EventArgs e)
        {

        }


    }


    public class DatabaseDesignEventArgs : EventArgs
    {
        public JscDatabaseDesign DatabaseDesign { get; set; }
    }


    public class TableDesignEventArgs : EventArgs
    {
        public JscTableDesign TableDesign { get; set; }
    }

    public class ItemCheckChangedEventArgs : EventArgs
    {
        public bool HasCheckedItems { get; set; }
    }

}
