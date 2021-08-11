using System;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Utils
{
    public static class Utils
    {
        public static ImageList GetImageList()
        {
            ImageList im = new ImageList();
            im.Images.Add(Properties.Resources.SortAscendingImage);
            im.Images.Add(Properties.Resources.SortDescendingimage);
            im.Images.Add(Properties.Resources.ClearImage);
            im.Images.Add(Properties.Resources.Disabled12);
            return im;
        }

        public static string ListViewToString(ListView listView)
        {
            const string clipboardSeperator = "\t";

            var sb = new StringBuilder();
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(clipboardSeperator);
                }
                sb.Append(listView.Columns[i].Text);
            }
            sb.AppendLine();

            foreach (ListViewItem item in listView.SelectedItems)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(clipboardSeperator);
                    }
                    sb.Append(item.SubItems[i].Text);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static int[] GetSelectedIndices(ListView listView)
        {
            int[] selectedIndices = null;
            if (listView.SelectedIndices.Count > 0)
            {
                selectedIndices = new int[listView.SelectedIndices.Count];
                listView.SelectedIndices.CopyTo(selectedIndices, 0);
            }
            return selectedIndices;
        }

        public static void SetSelectedIndices(ListView listView, int[] selectedIndices)
        {
            if (selectedIndices == null)
                return;

            listView.SelectedIndices.Clear();
            for (int i = 0; i < selectedIndices.Length; i++)
            {
                var index = selectedIndices[i];
                if (index < listView.Items.Count)
                {
                    listView.SelectedIndices.Add(index);
                }
            }
        }
        
        public static bool TestDatabaseConnection(IWin32Window messageDialogOwner, DatabaseString databaseString, string testTableName)
        {
            if (databaseString == null)
                return false;

            bool result = false;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(databaseString.ToString(true)))
                {
                    sqlcon.Open();
                    using (SqlCommand sqlcmd = new SqlCommand("SELECT Count(*) FROM " + testTableName, sqlcon))
                    {
                        sqlcmd.ExecuteScalar();
                    }
                    result = true;
                    MessageUtils.ShowInfo(messageDialogOwner, Properties.Resources.TestDatabaseConnectionOK, Properties.Resources.TestDatabaseConnectionHeader);
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is SqlException || ex is ArgumentException)
                {
                    MessageUtils.ShowError(messageDialogOwner, Properties.Resources.TestDatabaseConnectionFailed, Properties.Resources.TestDatabaseConnectionHeader, ex);
                }
                else
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a resource string for the specified enumeration value using the current thread's CurrentUICulture.
        /// </summary>
        /// <remarks>
        /// The resource string must have a name of the form type_member where type is the un-namespeced name
        /// of the enumeration type and member is the name of the enumeration member.
        /// Example: JobValidationMessage_DescriptionMissing.
        /// </remarks>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="resourceManager">The ResourceManager where the string is fetched from.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>The resource string for the enumeration value or null if none was found.</returns>
        public static string EnumResourceString<T>(System.Resources.ResourceManager resourceManager, T value)
        {
            return EnumResourceString(resourceManager, typeof(T), value, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Gets a resource string for the specified enumeration value.
        /// </summary>
        /// <remarks>
        /// The resource string must have a name of the form type_member where type is the un-namespeced name
        /// of the enumeration type and member is the name of the enumeration member.
        /// Example: JobValidationMessage_DescriptionMissing.
        /// </remarks>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="resourceManager">The ResourceManager where the string is fetched from.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="resourceCulture">The CultureInfo of the string to fetch.</param>
        /// <returns>The resource string for the enumeration value or null if none was found.</returns>
        public static string EnumResourceString<T>(System.Resources.ResourceManager resourceManager, T value, System.Globalization.CultureInfo resourceCulture)
        {
            return EnumResourceString(resourceManager, typeof(T), value, resourceCulture);
        }

        /// <summary>
        /// Gets a resource string for the specified enumeration value using the current thread's CurrentUICulture.
        /// </summary>
        /// <remarks>
        /// The resource string must have a name of the form type_member where type is the un-namespeced name
        /// of the enumeration type and member is the name of the enumeration member.
        /// Example: JobValidationMessage_DescriptionMissing.
        /// </remarks>
        /// <param name="resourceManager">The ResourceManager where the string is fetched from.</param>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <returns>The resource string for the enumeration value or null if none was found.</returns>
        public static string EnumResourceString(System.Resources.ResourceManager resourceManager, Type enumType, object value)
        {
            return EnumResourceString(resourceManager, enumType, value, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Gets a resource string for the specified enumeration value.
        /// </summary>
        /// <remarks>
        /// The resource string must have a name of the form type_member where type is the un-namespeced name
        /// of the enumeration type and member is the name of the enumeration member.
        /// Example: JobValidationMessage_DescriptionMissing.
        /// </remarks>
        /// <param name="resourceManager">The ResourceManager where the string is fetched from.</param>
        /// <param name="enumType">The enum type.</param>
        /// <param name="value">The enum value.</param>
        /// <param name="resourceCulture">The CultureInfo of the string to fetch.</param>
        /// <returns>The resource string for the enumeration value or null if none was found.</returns>
        public static string EnumResourceString(System.Resources.ResourceManager resourceManager, Type enumType, object value, System.Globalization.CultureInfo resourceCulture)
        {
            if (resourceManager == null)
                throw new ArgumentNullException("resourceManager");

            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (value == null)
                throw new ArgumentNullException("value");

            string name = enumType.Name + "_" + Enum.GetName(enumType, value);
            return resourceManager.GetString(name, resourceCulture);
        }

    }
}
