using System;
using System.Resources;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Utils
{
    public static class ComboUtils
    {
        public static void PopulateComboBoxItems<T>(ComboBox comboBox, ResourceManager resourceManager, Func<T, bool> predicate)
            where T : IConvertible
        {
            if (comboBox == null)
                throw new ArgumentNullException("comboBox");

            Type type = typeof(T);

            comboBox.Items.Clear();
            foreach (T value in Enum.GetValues(type))
            {
                var intValue = ((IConvertible)value).ToInt32(System.Globalization.CultureInfo.InvariantCulture);
                if (predicate == null || predicate(value))
                {
                    string text = null;
                    if (resourceManager != null)
                    {
                        text = Utils.EnumResourceString(resourceManager, type, intValue);
                    }
                    if (text == null)
                    {
                        text = Enum.GetName(type, intValue);
                    }
                    comboBox.Items.Add
                    (
                        new DataSelector { IntId = intValue, Text = text }
                    );
                }
            }
        }


        public static void PopulateComboBoxItems<T>(ComboBox comboBox, ResourceManager resourceManager)
            where T : IConvertible
        {
            PopulateComboBoxItems<T>(comboBox, resourceManager, null);
        }


        public static void PopulateComboBoxItems<T>(ComboBox comboBox)
            where T : IConvertible
        {
            PopulateComboBoxItems<T>(comboBox, null, null);
        }


        public static void SetComboSelection(ComboBox comboBox, int value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((DataSelector)comboBox.Items[i]).IntId == value)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }


        public static void SetComboSelection(ComboBox comboBox, Guid value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((DataSelector)comboBox.Items[i]).GuidId == value)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }



        public static void SetComboSelection(ComboBox comboBox, string code)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((DataSelector)comboBox.Items[i]).Code == code)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        public static int GetComboSelectionInt(ComboBox comboBox)
        {
            int result = -1;
            DataSelector dataSelector = comboBox.SelectedItem as DataSelector;
            if (dataSelector != null)
            {
                result = dataSelector.IntId;
            }

            return result;
        }

        public static Guid GetComboSelectionGuid(ComboBox comboBox)
        {
            DataSelector dataSelector = comboBox.SelectedItem as DataSelector;
            if (dataSelector != null && dataSelector.GuidId != null)
            {
                return (Guid) dataSelector.GuidId;
            }

            return Guid.Empty;
        }


        public static string GetComboSelectionCode(ComboBox comboBox)
        {
            string result = null;
            DataSelector dataSelector = comboBox.SelectedItem as DataSelector;
            if (dataSelector != null)
            {
                result = dataSelector.Code;
            }

            return result;
        }


        /// <summary>
        /// Sets the item in the combo box as selected that has the Id property of its DataSelector
        /// value equal to the specified value. If value is null it will be matched agains an Id value of 0.
        /// </summary>
        public static void SetComboSelectionNullableInt(ComboBox comboBox, int? value)
        {
            int actualValue = value ?? 0;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((DataSelector)comboBox.Items[i]).IntId == actualValue)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }


        /// <summary>
        /// Sets the item in the combo box as selected that has the Id property of its DataSelector
        /// value equal to the specified value. If value is null it will be matched against an Id value of Guid.Empty.
        /// </summary>
        public static void SetComboSelectionNullableGuid(ComboBox comboBox, Guid? value)
        {
            Guid actualValue = value ?? Guid.Empty;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (((DataSelector)comboBox.Items[i]).GuidId == actualValue)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the Id property of the DataSelector of the selected item in the
        /// combo box. An Id value of 0 and will be returned as null. If no value is selected
        /// null is also returned.
        /// </summary>
        public static int? GetComboSelectionNullableInt(ComboBox comboBox)
        {
            int? result = null;
            DataSelector dataSelector = comboBox.SelectedItem as DataSelector;
            if (dataSelector != null)
            {
                result = dataSelector.IntId;
                if (result.Value == 0)
                    result = null;
            }

            return result;
        }

        /// <summary>
        /// Returns the Id property of the DataSelector of the selected item in the
        /// combo box. An Id value of Guid.Empty and will be returned as null. If no value is selected
        /// null is also returned.
        /// </summary>
        public static Guid? GetComboSelectionNullableGuid(ComboBox comboBox)
        {
            DataSelector dataSelector = comboBox.SelectedItem as DataSelector;
            if (dataSelector != null)
            {
                if (dataSelector.GuidId.IsEmpty || ((Guid)dataSelector.GuidId) == Guid.Empty )
                    return null;
                
                return (Guid)dataSelector.GuidId;
                
            }

            return null;
        }



    }
}
