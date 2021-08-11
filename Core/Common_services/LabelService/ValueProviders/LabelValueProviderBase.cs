using System;
using System.Collections.Generic;
using System.Threading;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.ValueProviders
{
    public abstract class LabelValueProviderBase
    {
        abstract public List<string> SupportedMacros { get; }

        private void AddBaseMacros(List<string> macros)
        {
            macros.Add("${STORE}");
            macros.Add("${TERMINAL}");
            macros.Add("${USER}");
            macros.Add("${DATETIME}");
            macros.Add("${DATE}");
            macros.Add("${TIME}");
            macros.Add("${NUMLABELS}");
        }

        public virtual string ApplyMacros(IConnectionManager entry, int numLabels, string form, IDataEntity entity)
        {
            form = ApplyMacro(form, "${NUMLABELS}", numLabels.ToString());

            if (form.IndexOf("${STORE") >= 0)
            {
                var store = Providers.StoreData.Get(entry, entry.CurrentStoreID);
                form = ApplyMacro(form, "${STORE}", store.Text);
            }
            if (form.IndexOf("${TERMINAL") >= 0)
            {
                var terminal = Providers.TerminalData.Get(entry, entry.CurrentTerminalID, entry.CurrentStoreID);
                form = ApplyMacro(form, "${TERMINAL}", terminal.Text);
            }
            if (form.IndexOf("${USER") >= 0)
            {
                form = ApplyMacro(form, "${USER}", entry.CurrentUser == null ? "" : entry.CurrentUser.UserName);
            }
            if (form.IndexOf("${DATETIME") >= 0)
            {
                form = ApplyMacro(form, "${DATETIME}", DateTime.Now,
                                  Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern + " " +
                                  Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortTimePattern);
            }
            if (form.IndexOf("${DATE") >= 0)
            {
                form = ApplyMacro(form, "${DATE}", DateTime.Now,
                                  Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern);
            }
            if (form.IndexOf("${TIME") >= 0)
            {
                form = ApplyMacro(form, "${TIME}", DateTime.Now,
                                  Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortTimePattern);
            }

            return form;
        }

        protected string ApplyMacro(string template, string macro, string value)
        {
            if (value == null)
                value = "";

            string res = template;
            
            // See if macro is present with length-limiter
            int pos = res.IndexOf(macro.TrimEnd('}'));
            while (pos >= 0)
            {
                int endPos = res.IndexOf('}', pos);
                string macroForm = res.Substring(pos, endPos - pos);
                var items = macroForm.Split(',', ';');
                int maxLength = Int32.MaxValue;
                if (items.Length > 1)
                    Int32.TryParse(items[1], out maxLength);
                if (value.Length > maxLength)
                    value = value.Substring(0, maxLength);

                res = res.Replace(macroForm + '}', value);

                pos = res.IndexOf(macro.TrimEnd('}'), pos + 1);
            }

            return res;
        }

        protected string ApplyMacro(string template, string macro, DateTime value, string defaultFormat)
        {
            string res = template;

            // See if macro is present with format
            int pos = res.IndexOf(macro.TrimEnd('}'));
            while (pos >= 0)
            {
                int endPos = res.IndexOf('}', pos);
                string macroForm = res.Substring(pos, endPos - pos);
                var items = macroForm.Split(',', ';');
                if (items.Length > 1)
                    defaultFormat = items[1];

                res = res.Replace(macroForm + '}', value.ToString(defaultFormat));

                pos = res.IndexOf(macro.TrimEnd('}'), pos + 1);
            }

            return res;
        }

        protected string ApplyMacro(string template, string macro, decimal value, string defaultFormat)
        {
            string res = template;

            // See if macro is present with format
            int pos = res.IndexOf(macro.TrimEnd('}'));
            while (pos >= 0)
            {
                int endPos = res.IndexOf('}', pos);
                string macroForm = res.Substring(pos, endPos - pos);
                var items = macroForm.Split(',', ';');
                if (items.Length > 1)
                    defaultFormat = items[1];

                res = res.Replace(macroForm + '}', value.ToString(defaultFormat));

                pos = res.IndexOf(macro.TrimEnd('}'), pos + 1);
            }

            return res;
        }

        protected IList<string> SortMacros(List<string> macroList)
        {
            AddBaseMacros(macroList);

            var sorted = new SortedList<string, string>();
            foreach (var macro in macroList)
            {
                sorted.Add(macro, macro);
            }
            return sorted.Values;
        }
    }
}
