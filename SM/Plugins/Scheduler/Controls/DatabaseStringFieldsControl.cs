using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class DatabaseStringFieldsControl : UserControl
    {
        private List<DatabaseDriverType> driverTypes;
        private bool enableSync = true;

        public DatabaseStringFieldsControl()
        {
            InitializeComponent();

            // Tag controls with the field they represent
            tbCompany.Tag = DatabaseStringFields.Company;
            tbUserId.Tag = DatabaseStringFields.UserId;
            tbPassword.Tag = DatabaseStringFields.Password;
            tbDBServerHost.Tag = DatabaseStringFields.DBServerHost;
            tbDBPathName.Tag = DatabaseStringFields.DBPathName;
            cmbConnectionType.Tag = DatabaseStringFields.ConnectionType;

            cmbConnectionType.Items.Add(new DataSelector { Code = null, Text = Properties.Resources.ConnectionTypeDefault });
            cmbConnectionType.Items.Add(new DataSelector { Code = "dbnmpntw", Text = "Named Pipes" });
            cmbConnectionType.Items.Add(new DataSelector { Code = "dbmslpcn", Text = "Shared Memory" });
            cmbConnectionType.Items.Add(new DataSelector { Code = "dbmssocn", Text = "TCP/IP" });
        }

        public List<DatabaseDriverType> DatabaseDriverTypes
        {
            get { return driverTypes; }
            set
            {
                driverTypes = value;
                cmbDBDriverType.Items.Clear();
                if (driverTypes != null)
                {
                    foreach (var driverType in driverTypes)
                    {
                        cmbDBDriverType.Items.Add(driverType);
                    }
                }
            }
        }


        public bool IsDDString
        {
            get { return databaseStringControl.IsDDString; }
            set { databaseStringControl.IsDDString = value; }
        }

        public DatabaseString DatabaseString
        {
            get
            {
                return databaseStringControl.DatabaseString;
            }
            set
            {
                SelectMatchingDriverType(value);
                databaseStringControl.DatabaseString = value;
            }
        }

        [DefaultValue(true)]
        public bool TestButtonVisible
        {
            get { return btnTest.Visible; }
            set { btnTest.Visible = value; }
        }


        public event EventHandler DatabaseStringChanged;
        public event EventHandler<DatabaseStringEventArgs> TestClicked;


        private void cmbDBDriverType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabledFields();
            UpdateDatabaseString();
        }

        private void UpdateEnabledFields()
        {
            var driverType = (DatabaseDriverType)cmbDBDriverType.SelectedItem;
            foreach (Control control in gbFields.Controls)
            {
                if (control.Tag is DatabaseStringFields)
                {
                    bool enabled = false;
                    if (driverType != null)
                    {
                        foreach (var field in driverType.EnabledFields)
                        {
                            if ((DatabaseStringFields)control.Tag == field)
                            {
                                enabled = true;
                                break;
                            }
                        }
                    }
                    control.Enabled = enabled;
                }
            }
        }

        private void tbCompany_TextChanged(object sender, EventArgs e)
        {
            UpdateDatabaseString();
        }

        private void tbUserId_TextChanged(object sender, EventArgs e)
        {
            UpdateDatabaseString();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateDatabaseString();
        }

        private void tbDBServerHost_TextChanged(object sender, EventArgs e)
        {
            UpdateDatabaseString();
        }

        private void tbDBPathName_TextChanged(object sender, EventArgs e)
        {
            UpdateDatabaseString();
        }

        private void cmbConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(cmbConnectionType.SelectedIndex);
            UpdateDatabaseString();
        }

        private void UpdateDatabaseString()
        {
            if (!enableSync)
            {
                return;
            }

            var driverType = (DatabaseDriverType)cmbDBDriverType.SelectedItem; 
            if (driverType == null)
            {
                return;
            }

            string format = driverType.Format;
            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            foreach (DatabaseStringFields field in Enum.GetValues(typeof(DatabaseStringFields)))
            {
                string newValue = null;
                var control = GetControlOfField(field);
                if (control != null)
                {
                    if (control is TextBox)
                    {
                        newValue = control.Text;
                    }
                    else if (control is ComboBox)
                    {
                        newValue = ComboUtils.GetComboSelectionCode(cmbConnectionType);
                    }
                }
                else
                {
                    if (IsDDString)
                    {
                        if (field == DatabaseStringFields.DatabaseServerType)
                        {
                            newValue = DataServerType.DataSrvTypeToString(driverType.DataSrvType);
                        }
                        else if (field == DatabaseStringFields.DatabaseParams)
                        {
                            if (driverType.DatabaseParams != null)
                            {
                                newValue = driverType.DatabaseParams;
                            }
                            else
                            {
                                newValue = DatabaseString.DatabaseParamsNullText;
                            }
                        }
                    }
                }
                format = ResolvePropertyAssignment(format, GetFieldPlaceholder(field), newValue);
            }

            format = format.Trim();

            // Strip trailing '|' chars if not DD string
            if (!IsDDString)
            {
                int index = format.Length - 1;
                while (index >= 0 && format[index] == '|')
                {
                    index--;
                }

                if (index >= 0)
                {
                    format = format.Substring(0, index + 1);
                }
            }

            enableSync = false;
            databaseStringControl.DatabaseString = new DatabaseString(IsDDString, format);
            enableSync = true;
        }


        private Control GetControlOfField(DatabaseStringFields field)
        {
            foreach (Control control in gbFields.Controls)
            {
                if (control.Tag is DatabaseStringFields)
                {
                    if ((DatabaseStringFields)control.Tag == field)
                    {
                        return control;
                    }
                }
            }

            return null;
        }


        private string ResolvePropertyAssignment(string target, string oldValue, string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                // A simple replace
                return target.Replace(oldValue, newValue);
            }
            else
            {
                // New value is empty, we want to remove the property and equals sign as well
                int valueIndex = target.IndexOf(oldValue);
                if (valueIndex == -1)
                    return target;

                // Make sure this is a property assignment
                int assignIndex = FindTokenBackwards(target, '=', valueIndex - 1);
                if (assignIndex == -1)
                {
                    // Not a property assignment, simply remove the old value
                    return target.Replace(oldValue, newValue);
                }

                string result;

                // Find property name
                int nameIndex = target.LastIndexOf(';', assignIndex - 1);
                if (nameIndex == -1)
                {
                    // property name is at start of text
                    result = target.Substring(valueIndex + oldValue.Length);
                }
                else
                {
                    result = target.Substring(0, nameIndex + 1);

                    int tailIndex = valueIndex + oldValue.Length + 1;
                    if (tailIndex < target.Length)
                        result += target.Substring(tailIndex);
                }

                // Strip leading and trailing ';'
                if (result.StartsWith(";"))
                    result = result.Substring(1);
                if (result.EndsWith(";"))
                    result = result.Substring(0, result.Length - 1);

                return result;
            }
        }

        private int FindTokenBackwards(string target, char token, int index)
        {
            while (target[index] != token && char.IsWhiteSpace(target[index]) && index >= 0)
            {
                index--;
            }

            if (index < 0 || target[index] != token)
            {
                index = -1;
            }

            return index;
        }

        private void databaseStringControl_DatabaseStringChanged(object sender, EventArgs e)
        {
            if (enableSync)
            {
                var driverType = (DatabaseDriverType)cmbDBDriverType.SelectedItem;
                if (driverType == null)
                {
                    return;
                }

                var formatDatabaseString = new DatabaseString(IsDDString, driverType.Format);

                enableSync = false;

                var databaseString = databaseStringControl.DatabaseString;
                foreach (DatabaseStringFields field in Enum.GetValues(typeof(DatabaseStringFields)))
                {
                    var propertyName = formatDatabaseString.GetPropertyNameOfValue(GetFieldPlaceholder(field));
                    if (propertyName != null)
                    {
                        var control = GetControlOfField(field);
                        if (control != null)
                        {
                            string value;
                            if (databaseString.Properties.TryGetValue(propertyName, out value))
                            {
                                if (control is TextBox)
                                {
                                    ((TextBox)control).Text = value;
                                }
                                else if (control is ComboBox)
                                {
                                    ComboUtils.SetComboSelection((ComboBox)control, value);
                                }
                            }
                            else
                            {
                                if (control is TextBox)
                                {
                                    control.Text = null;
                                }
                                else if (control is ComboBox)
                                {
                                    ((ComboBox)control).SelectedIndex = control.Enabled ? 0 : -1;
                                }
                            }
                        }
                    }
                }

                enableSync = true;
            }

            OnDatabaseStringChanged();

        }


        //private void tbDatabaseString_TextChanged(object sender, EventArgs e)
        //{
        //    if (enableSync)
        //    {
        //        var driverType = (DatabaseDriverType)cmbDBDriverType.SelectedItem;
        //        if (driverType == null)
        //        {
        //            return;
        //        }

        //        var formatDatabaseString = new DatabaseString(IsDDString, driverType.Format);

        //        enableSync = false;

        //        var databaseString = new DatabaseString(IsDDString, tbDatabaseString.Text);
        //        foreach (DatabaseStringFields field in Enum.GetValues(typeof(DatabaseStringFields)))
        //        {
        //            var propertyName = formatDatabaseString.GetPropertyNameOfValue(GetFieldPlaceholder(field));
        //            if (propertyName != null)
        //            {
        //                var control = GetControlOfField(field);
        //                if (control != null)
        //                {
        //                    string value;
        //                    if (databaseString.Properties.TryGetValue(propertyName, out value))
        //                    {
        //                        if (control is TextBox)
        //                        {
        //                            ((TextBox)control).Text = value;
        //                        }
        //                        else if (control is ComboBox)
        //                        {
        //                            ComboUtils.SetComboSelection((ComboBox)control, value);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (control is TextBox)
        //                        {
        //                            control.Text = null;
        //                        }
        //                        else if (control is ComboBox)
        //                        {
        //                            ((ComboBox)control).SelectedIndex = control.Enabled ? 0 : -1;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        enableSync = true;
        //    }

        //    OnDatabaseStringChanged();
        //}

        private void OnDatabaseStringChanged()
        {
            if (DatabaseStringChanged != null)
            {
                DatabaseStringChanged(this, EventArgs.Empty);
            }
        }

        private static string GetFieldPlaceholder(DatabaseStringFields field)
        {
            return "{" + field.ToString() + "}";
        }


        private void SelectMatchingDriverType(DatabaseString databaseString)
        {
            enableSync = false;

            int selectedIndex = -1;
            int bestMatchScore = 0;

            if (databaseString != null)
            {
                for (int i = 0; i < cmbDBDriverType.Items.Count; i++)
                {
                    var driverType = (DatabaseDriverType)cmbDBDriverType.Items[i];
                    var matchScore = CalculateMatchScore(databaseString, driverType);
                    if (matchScore > bestMatchScore)
                    {
                        selectedIndex = i;
                        bestMatchScore = matchScore;
                    }
                }
            }

            cmbDBDriverType.SelectedIndex = selectedIndex;

            enableSync = true;
        }

        private int CalculateMatchScore(DatabaseString databaseString, DatabaseDriverType driverType)
        {
            int score = 0;

            if (databaseString.IsDDString)
            {
                if (databaseString.DataSrvType == driverType.DataSrvType)
                {
                    score += 10;
                }

                if (databaseString.DatabaseParams == driverType.DatabaseParams)
                {
                    score += 10;
                }
            }

            var formatDatabaseString = new DatabaseString(databaseString.IsDDString, driverType.Format);
            foreach (var property in formatDatabaseString.Properties)
            {
                if (databaseString.Properties.ContainsKey(property.Key))
                {
                    score += 1;
                }
                else
                {
                    score -= 1;
                }
            }

            return score;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            OnTestClicked();
        }

        private void OnTestClicked()
        {
            if (TestClicked != null)
            {
                TestClicked(this, new DatabaseStringEventArgs { DatabaseString = this.DatabaseString });
            }
        }




    }
}
