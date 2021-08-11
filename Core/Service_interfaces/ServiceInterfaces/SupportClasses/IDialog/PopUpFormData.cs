using System.Collections.Generic;
using System.Linq;
using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.SupportClasses.IDialog
{
    public class PopUpFormData
    {
        public List<Item> Items { get; set; }
        public List<Group> Groups { get; set; }
        public string HeaderText { get; set; }

        public string Dimension1Selected { get; set; }
        public string Dimension2Selected { get; set; }
        public string Dimension3Selected { get; set; }

        public PopUpFormData()
        {

            HeaderText = "Survey";
            Dimension1Selected = "";
            Dimension2Selected = "";
            Dimension3Selected = "";

            Items = new List<Item>();
            Groups = new List<Group>();

            #region TestData

            //for (int i = 0; i < 26; i++)
            //{
            //    Item item = new Item();
            //    item.GroupId = "";
            //    item.ItemID = "Item" + i.ToString();
            //    item.Text = "Item" + i.ToString();
            //    item.NumberOfClicks = 0;
            //    Items.Add(item);
            //}

            //    for (int j = 0; j < 2; j++)
            //    {
            //        Group group = new Group();
            //        group.Index = j;
            //        group.GroupId = "Group" + j.ToString();
            //        group.Text = "Group" + j.ToString();
            //        group.GroupHeader = "Select " + group.Text;
            //        group.NumberOfClicks = 0;
            //        group.MinSelection = 1;
            //        group.MaxSelection = 3;
            //        Groups.Add(group);
            //    }

            //    for (int j = 2; j < 4; j++)
            //    {
            //        Group group = new Group();
            //        group.Index = j;
            //        group.GroupId = "Group" + j.ToString();
            //        group.Text = "Group" + j.ToString();
            //        group.GroupHeader = "Select " + group.Text;
            //        group.NumberOfClicks = 0;
            //        group.MinSelection = 0;
            //        group.MaxSelection = 0;
            //        Groups.Add(group);
            //    }

            //    for (int i = 0; i < 26; i++)
            //    {
            //        Item item = new Item();
            //        item.Index = i;
            //        item.GroupId = "Group0";
            //        item.ItemID = "Item" + i.ToString();
            //        item.Text = "Item" + i.ToString();
            //        item.NumberOfClicks = 0;
            //        Items.Add(item);
            //    }

            //    for (int i = 0; i < 6; i++)
            //    {
            //        Item item = new Item();
            //        item.Index = i;
            //        item.GroupId = "Group1";
            //        item.ItemID = "Item" + i.ToString();
            //        item.Text = "Item" + i.ToString();
            //        item.NumberOfClicks = 0;
            //        Items.Add(item);
            //    }


            //    for (int i = 0; i < 4; i++)
            //    {
            //        Item item = new Item();
            //        item.Index = i;
            //        item.GroupId = "Group2";
            //        item.ItemID = "Item" + i.ToString();
            //        item.Text = "Item" + i.ToString();
            //        item.NumberOfClicks = 0;
            //        Items.Add(item);
            //    }

            //    for (int i = 0; i < 7; i++)
            //    {
            //        Item item = new Item();
            //        item.Index = i;
            //        item.GroupId = "Group3";
            //        item.ItemID = "Item" + i.ToString();
            //        item.Text = "Item" + i.ToString();
            //        item.NumberOfClicks = 0;
            //        Items.Add(item);
            //    }


            //Group group1 = new Group();
            //group1.Index = 0;
            //group1.GroupId = "";
            //group1.Dimension = Group.Dimensions.Dimension1;
            //group1.Text = "Color";
            //group1.GroupHeader = "Select " + group1.Text;
            //group1.NumberOfClicks = 0;
            //group1.MinSelection = 1;
            //group1.MaxSelection = 1;
            //group1.IsVariantGroup = true;
            //Groups.Add(group1);

            //Group group2 = new Group();
            //group2.Index = 0;
            //group2.GroupId = "";
            //group2.Dimension = Group.Dimensions.Dimension2;
            //group2.Text = "Size";
            //group2.GroupHeader = "Select " + group2.Text;
            //group2.NumberOfClicks = 0;
            //group2.MinSelection = 1;
            //group2.MaxSelection = 1;
            //group2.IsVariantGroup = true;
            //Groups.Add(group2);

            //Group group3 = new Group();
            //group3.Index = 0;
            //group3.GroupId = "";
            //group3.Dimension = Group.Dimensions.Dimension3;
            //group3.Text = "Style";
            //group3.GroupHeader = "Select " + group3.Text;
            //group3.NumberOfClicks = 0;
            //group3.MinSelection = 1;
            //group3.MaxSelection = 1;
            //group3.IsVariantGroup = true;
            //Groups.Add(group3);

            //Item item1 = new Item();
            //item1.Index = 0;
            //item1.GroupId = "";
            //item1.ItemID = "Item" ;
            //item1.Text = "Item BLU 32 FIN";
            //item1.NumberOfClicks = 0;
            //item1.IsVariantItem = true;
            //item1.VariantId = "1";
            //item1.Dimension1 = "BLU";
            //item1.Dimension2 = "32";
            //item1.Dimension3 = "FIN";
            //Items.Add(item1);

            //Item item2 = new Item();
            //item2.Index = 0;
            //item2.GroupId = "";
            //item2.ItemID = "Item";
            //item2.Text = "Item BLU 34 FIN";
            //item2.NumberOfClicks = 0;
            //item2.IsVariantItem = true;
            //item2.VariantId = "1";
            //item2.Dimension1 = "BLU";
            //item2.Dimension2 = "34";
            //item2.Dimension3 = "FIN";
            //Items.Add(item2);

            //Item item3 = new Item();
            //item3.Index = 0;
            //item3.GroupId = "";
            //item3.ItemID = "Item";
            //item3.Text = "Item RED 32 SOL";
            //item3.NumberOfClicks = 0;
            //item3.IsVariantItem = true;
            //item3.VariantId = "1";
            //item3.Dimension1 = "RED";
            //item3.Dimension2 = "32";
            //item3.Dimension3 = "SOL";
            //Items.Add(item3);

            //Item item4 = new Item();
            //item4.Index = 0;
            //item4.GroupId = "";
            //item4.ItemID = "Item";
            //item4.Text = "Item RED 36 SOL";
            //item4.NumberOfClicks = 0;
            //item4.IsVariantItem = true;
            //item4.VariantId = "1";
            //item4.Dimension1 = "RED";
            //item4.Dimension2 = "36";
            //item4.Dimension3 = "SOL";
            //Items.Add(item4);

            //Item item5 = new Item();
            //item5.Index = 0;
            //item5.GroupId = "";
            //item5.ItemID = "Item";
            //item5.Text = "Item GRE 32 FIN";
            //item5.NumberOfClicks = 0;
            //item5.IsVariantItem = true;
            //item5.VariantId = "1";
            //item5.Dimension1 = "GRE";
            //item5.Dimension2 = "32";
            //item5.Dimension3 = "FIN";
            //Items.Add(item5);

            //Item item6 = new Item();
            //item6.Index = 0;
            //item6.GroupId = "";
            //item6.ItemID = "Item";
            //item6.Text = "Item GRE 36 FIN";
            //item6.NumberOfClicks = 0;
            //item6.IsVariantItem = true;
            //item6.VariantId = "1";
            //item6.Dimension1 = "GRE";
            //item6.Dimension2 = "36";
            //item6.Dimension3 = "FIN";
            //Items.Add(item6);

            ////

            //Group group = new Group();
            //group.Index = 0;
            //group.GroupId = "Group";
            //group.Text = "Group Normal";
            //group.GroupHeader = "Select " + group.Text;
            //group.NumberOfClicks = 0;
            //group.MinSelection = 1;
            //group.MaxSelection = 3;
            //Groups.Add(group);


            //for (int i = 0; i < 4; i++)
            //{
            //    Item item = new Item();
            //    item.Index = i;
            //    item.GroupId = "Group";
            //    item.ItemID = "Item" + i.ToString();
            //    item.Text = "Item" + i.ToString();
            //    item.NumberOfClicks = 0;
            //    Items.Add(item);
            //}

            #endregion TestData
        }

        public List<string> GetVariantsItemsDim1(string Dim1, string Dim2, string Dim3)
        {
            var items = (from i in Items
                         where (i.Dimension1.Contains(Dim1))
                            && (i.Dimension2.Contains(Dim2))
                            && (i.Dimension3.Contains(Dim3))
                         select i.Dimension1).Distinct().ToList();

            items.Remove("");

            return items;
        }


        public List<string> GetVariantsItemsDim2(string Dim1, string Dim2, string Dim3)
        {
            var items = (from i in Items
                         where (i.Dimension1.Contains(Dim1))
                            && (i.Dimension2.Contains(Dim2))
                            && (i.Dimension3.Contains(Dim3))
                         select i.Dimension2).Distinct().ToList();

            items.Remove("");

            return items;
        }

        public List<string> GetVariantsItemsDim3(string Dim1, string Dim2, string Dim3)
        {
            var items = (from i in Items
                         where (i.Dimension1.Contains(Dim1))
                            && (i.Dimension2.Contains(Dim2))
                            && (i.Dimension3.Contains(Dim3))
                         select i.Dimension3).Distinct().ToList();

            items.Remove("");

            return items;
        }


        public List<Item> GetItems(string GroupId)
        {
            List<Item> result = new List<Item>();

            foreach (Item item in Items)
            {
                if (item.GroupId == GroupId)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public List<Group> GetGroups()
        {
            return Groups;
        }

        public Group GetGroup(string GroupId)
        {
            Group result = null;
            foreach (Group group in Groups)
            {
                if (group.GroupId == GroupId)
                {
                    result = group;
                    break;
                }
            }
            return result;
        }

        public Item GetItem(string GroupId, string ItemId)
        {
            foreach (Item item in Items)
            {
                if (item.GroupId == GroupId && item.ItemId == ItemId)
                    return item;
            }
            return null;
        }

        public bool AllRequiredItemsSelected()
        {
            foreach (Group group in Groups)
            {
                if (group.NumberOfClicks < group.MinSelection)
                    return false;
            }
            return true;
        }

        public void ClearQty(string GroupId, string ItemId)
        {
            int numberOfClickes = 0;

            foreach (Item item in Items)
            {
                if (item.GroupId == GroupId && item.ItemId == ItemId)
                {
                    numberOfClickes = (item.NumberOfClicks - item.PrevSelection);
                    item.NumberOfClicks = item.PrevSelection;
                    break;
                }
            }

            foreach (Group group in Groups)
            {
                if (group.GroupId == GroupId)
                {
                    group.NumberOfClicks -= numberOfClickes;
                    break;
                }
            }
        }

        /// <summary>
        /// Clear the selection information
        /// </summary>
        /// <param name="Group">The group to clear the seletion for</param>
        public void ClearSelection(Group Group)
        {
            if (Group.IsVariantGroup)
            {
                switch (Group.Dimension)
                {
                    case Group.Dimensions.Dimension1: Dimension1Selected = "";
                        break;
                    case Group.Dimensions.Dimension2: Dimension2Selected = "";
                        break;
                    case Group.Dimensions.Dimension3: Dimension3Selected = "";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                foreach (Group group in Groups)
                {
                    if (group.GroupId == Group.GroupId)
                    {
                        group.NumberOfClicks = group.PrevSelection;
                    }
                }

                foreach (Item item in Items)
                {
                    if (item.GroupId == Group.GroupId)
                    {
                        item.NumberOfClicks = item.PrevSelection;
                    }
                }
            }
        }

        public void ClearAllSelection()
        {
            foreach (Group group in Groups)
            {
                group.NumberOfClicks = 0;
            }

            foreach (Item item in Items)
            {
                item.NumberOfClicks = 0;
            }

            Dimension1Selected = "";
            Dimension2Selected = "";
            Dimension3Selected = "";
        }

        public bool SelectionCompleted()
        {
            foreach (Group group in Groups)
            {
                if (group.SelectionCompleted == false)
                    return false;
            }
            return true;
        }

        public bool ItemSelected(string GroupId, string ItemId, bool DimensionItem)
        {
            if (DimensionItem == true)
            {
                if (GroupId == "Dim1")
                    Dimension1Selected = ItemId;
                else if (GroupId == "Dim2")
                    Dimension2Selected = ItemId;
                else if (GroupId == "Dim3")
                    Dimension3Selected = ItemId;
            }
            else
            {
                int i = 0;
                foreach (Item item in Items)
                {
                    if (item.GroupId == GroupId && item.ItemId == ItemId)
                        break;

                    i++;
                }

                if (Items[i].NumberOfClicks < Items[i].MaxSelection || Items[i].MaxSelection == 0)
                {
                    Items[i].NumberOfClicks++;
                    ItemInGroupSelected(GroupId);
                }
                else
                    return false;
            }

            return true;
        }

        private void ItemInGroupSelected(string GroupId)
        {
            int i = 0;
            foreach (Group group in Groups)
            {
                if (group.GroupId == GroupId)
                    break;

                i++;
            }

            Groups[i].NumberOfClicks++;
        }

        public Group FindNextActionGroup(string GroupId)
        {
            Group group = GetGroup(GroupId);

            if ((Groups.IndexOf(group) + 1) < Groups.Count)
            {
                for (int i = (Groups.IndexOf(group) + 1); i < Groups.Count; i++)
                {
                    if (group.OKPressdAction == OKPressedActions.JumpToNextRequierd && Groups[i].InputRequired == true)
                        return Groups[i];
                    if (group.OKPressdAction == OKPressedActions.JumpToNextUnDisplayed)
                        return Groups[i];
                }
                return Groups[0];
            }
            else
                return Groups[0];
        }

        public void AddNumberOfClicks(string itemId)
        {
            foreach (Item item in Items)
            {
                if (item.ItemId == itemId)
                {
                    item.NumberOfClicks++;
                    item.PrevSelection++;
                    Group group = GetGroup(item.GroupId);
                    group.NumberOfClicks++;
                    group.PrevSelection++;
                    break;
                }
            }
        }
    }
}
