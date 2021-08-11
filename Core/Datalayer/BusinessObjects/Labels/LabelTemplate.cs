using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Labels
{
	public class LabelTemplate : DataEntity
	{
	    public enum ContextEnum
	    {
		    Items = 0,
		    Customers = 1,
	    };

        public string DataAreaID { get; set; }
        public ContextEnum Context { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public string CodePage { get; set; }
        public Image SampleImage { get; set; }

        // Accessors
	    public string ContextString
	    {
	        get { return FromContext(Context); }
            set { Context = ToContext(value); }
	    }

		/// <summary>
        /// Initializes a new instance of the <see cref="LabelTemplate" /> class.
		/// </summary>
        public LabelTemplate()
		{
            Context = ContextEnum.Items;
            DataAreaID = "";
		    Description = Template = "";
		}

        public static string FromContext(ContextEnum context)
        {
            switch (context)
            {
                case ContextEnum.Items:
                    return Properties.Resources.LabelTemplateContextItems;
                case ContextEnum.Customers:
                    return Properties.Resources.LabelTemplateContextCustomers;
            }

            return Properties.Resources.LabelTemplateContextItems;
        }

	    public static ContextEnum ToContext(string value)
	    {
	        if (value == Properties.Resources.LabelTemplateContextCustomers)
	            return ContextEnum.Customers;
	        return ContextEnum.Items;
	    }

	    public static string[] ContextStrings
	    {
	        get
	        {
	            var list = new SortedList<string, string>
	                {
	                    {Properties.Resources.LabelTemplateContextCustomers, Properties.Resources.LabelTemplateContextCustomers},
	                    {Properties.Resources.LabelTemplateContextItems, Properties.Resources.LabelTemplateContextItems}
	                };

	            var arr = new List<string>();
	            arr.AddRange(list.Values);
	            return arr.ToArray();
	        }
	    }

        public static Encoding ParseCodepage(string codepage)
        {
            Encoding encoding = null;
            int codepageNum;
            if (Int32.TryParse(codepage, out codepageNum))
            {
                try
                {
                    encoding = Encoding.GetEncoding(codepageNum);
                }
                catch {}
            }
            if (encoding == null)
            {
                try
                {
                    encoding = Encoding.GetEncoding(codepage);
                }
                catch {}
            }

            return encoding;
        }

        public string Translate(string text)
        {
            if (string.IsNullOrEmpty(CodePage))
                return text;
            var encoding = ParseCodepage(CodePage);
            if (encoding == null)
                return text;
         
            // Read old file
            var s = new MemoryStream(Encoding.Unicode.GetBytes(text));
            var sr = new StreamReader(s, Encoding.Unicode);

            // Write to new file
            var sout = new MemoryStream();
            var sw = new StreamWriter(sout, encoding);

            var inString = sr.ReadToEnd();
            sw.Write(inString);
            sw.Flush();

            sout.Position = 0;
            var sr2 = new StreamReader(sout, encoding);
            string translated = sr2.ReadToEnd();

            sw.Close();
            sr.Close();
            sr2.Close();

            return translated;
        }
    }
}

