using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    [DefaultEvent("LocationSelected")]
    public partial class LocationTreeControl : UserControl
    {
        public LocationTreeControl()
        {
            InitializeComponent();
            ilLocations.Images.Add(Properties.Resources.HeadOfficeImage);
            ilLocations.Images.Add(Properties.Resources.HeadOfficeDisabledImage);
            ilLocations.Images.Add(Properties.Resources.store_16);
            ilLocations.Images.Add(Properties.Resources.store_disabled_16);
            ilLocations.Images.Add(Properties.Resources.terminal_disabled_16);
            ilLocations.Images.Add(Properties.Resources.terminal_disabled_16);
            ilLocations.Images.Add(Properties.Resources.LocationImage);
            ilLocations.Images.Add(Properties.Resources.LocationDisabledImage);
        }


        public void LoadData(IEnumerable<JscLocation> locations)
        {
            var locationList = new List<JscLocation>(locations);
            locationList.Sort(CompareLocations);
            Cursor.Current = Cursors.WaitCursor;
            tvLocations.BeginUpdate();
            tvLocations.Nodes.Clear();
            foreach (var location in locationList)
            {
                TreeNode node = CreateNode(location);
                tvLocations.Nodes.Add(node);
            }
            tvLocations.EndUpdate();
        }


        private static int CompareLocations(JscLocation x, JscLocation y)
        {
            int xCompareValue = GetKindCompareValue(x);
            int yCompareValue = GetKindCompareValue(y);

            int compareValue = xCompareValue.CompareTo(yCompareValue);
            if (compareValue == 0)
            {
                compareValue = StringComparer.CurrentCultureIgnoreCase.Compare(x.Text, y.Text);
            }

            return compareValue;
        }

        private static int GetKindCompareValue(JscLocation x)
        {
            int value = 0; 
            switch (x.LocationKind)
            {
                case LocationKind.Undefined:
                    value = 0;
                    break;
                case LocationKind.General:
                    value = 4;
                    break;
                case LocationKind.HeadOffice:
                    value = 1;
                    break;
                case LocationKind.Store:
                    value = 2;
                    break;
                case LocationKind.Terminal:
                    value = 3;
                    break;
                default:
                    value = 9;
                    break;
            }

            return value;
        }

        private TreeNode CreateNode(JscLocation location)
        {
            TreeNode node = new TreeNode();
            node.Text = location.Text;
            node.ImageIndex = GetImageIndex(location);
            node.SelectedImageIndex = node.ImageIndex;
            node.Tag = location;
            node.Nodes.Add(CreateDummyNode());

            return node;
        }

        private TreeNode CreateDummyNode()
        {
            return new TreeNode();
        }

        private bool IsDummyNode(TreeNode treeNode)
        {
            return treeNode.Tag == null;
        }

        private void tvLocations_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Expand && e.Node.Tag is JscLocation && e.Node.Nodes.Count == 1 && IsDummyNode(e.Node.Nodes[0]))
            {
                LoadMemberLocations(e.Node);
            }
        }

        private void LoadMemberLocations(TreeNode treeNode)
        {
            JscLocation location = (JscLocation)treeNode.Tag;

            Cursor.Current = Cursors.WaitCursor;
            tvLocations.BeginUpdate();
            treeNode.Nodes.Clear();
            if (location.MemberLocations != null)
            {
                foreach (var member in location.MemberLocations)
                {
                    TreeNode node = CreateNode(member.Member);
                    treeNode.Nodes.Add(node);
                }
            }
            tvLocations.EndUpdate();
        }

        private int GetImageIndex(JscLocation location)
        {
            int imageIndex = 0;
            switch (location.LocationKind)
            {
                case LocationKind.HeadOffice:
                    imageIndex = 0;
                    break;
                case LocationKind.Store:
                    imageIndex = 2;
                    break;
                case LocationKind.Terminal:
                    imageIndex = 4;
                    break;
                case LocationKind.General:
                    imageIndex = 6;
                    break;
            }

            if (!location.Enabled)
                imageIndex++;

            return imageIndex;

        }

        private void tvLocations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnLocationItemSelected(e.Node);
        }

        private void OnLocationItemSelected(TreeNode treeNode)
        {
            if (LocationSelected != null)
            {
                LocationSelected(this, new LocationSelectedEventArgs { Location = (JscLocation)treeNode.Tag });
            }
        }


        private void tvLocations_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                OnLocationDoubleClicked(e.Node);
            }
        }

        private void OnLocationDoubleClicked(TreeNode treeNode)
        {
            if (LocationDoubleClicked != null)
            {
                LocationDoubleClicked(this, new LocationSelectedEventArgs { Location = (JscLocation)treeNode.Tag });
            }
        }

        public event EventHandler<LocationSelectedEventArgs> LocationSelected;

        public event EventHandler<LocationSelectedEventArgs> LocationDoubleClicked;

        public JscLocation SelectedLocation
        {
            get
            {
                JscLocation result = null;
                if (tvLocations.SelectedNode != null)
                {
                    result = (JscLocation)tvLocations.SelectedNode.Tag;
                }
                return result;
            }
            set
            {
                TreeNode node = null;
                if (value != null)
                {
                    node = FindNode(tvLocations.Nodes, value);
                    if (node != null)
                    {
                        tvLocations.SelectedNode = node;
                    }
                }

                if (node == null)
                {
                    tvLocations.SelectedNode = null;
                }
            }
        }

        public Guid? SelectedLocationId
        {
            get
            {
                var location = SelectedLocation;
                if (location != null)
                {
                    return (Guid)location.ID;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                TreeNode node = null;
                if (value != null)
                {
                    node = FindNode(tvLocations.Nodes, value.Value);
                    if (node != null)
                    {
                        tvLocations.SelectedNode = node;
                    }
                }

                if (node == null)
                {
                    tvLocations.SelectedNode = null;
                }
            }
        }

        private TreeNode FindNode(TreeNodeCollection treeNodeCollection, JscLocation location)
        {
            TreeNode result = null;

            foreach (TreeNode node in treeNodeCollection)
            {
                if ((JscLocation)node.Tag == location)
                {
                    result = node;
                }
                else
                {
                    result = FindNode(node.Nodes, location);
                }

                if (result != null)
                    break;
            }

            return result;
        }

        private TreeNode FindNode(TreeNodeCollection treeNodeCollection, Guid locationId)
        {
            TreeNode result = null;

            foreach (TreeNode node in treeNodeCollection)
            {
                var location = node.Tag as JscLocation;
                if (location != null && location.ID == locationId)
                {
                    result = node;
                }
                else
                {
                    result = FindNode(node.Nodes, locationId);
                }
                if (result != null)
                    break;
            }

            return result;
        }






    }


    public class LocationSelectedEventArgs : EventArgs
    {
        public JscLocation Location { get; set; }
    }
}
