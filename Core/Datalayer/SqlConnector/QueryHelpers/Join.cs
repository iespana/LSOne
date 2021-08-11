using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.SqlConnector.QueryHelpers
{
    public class Join
    {
        public string JoinType { get; set; }
        public string Table { get; set; }
        public string Condition { get; set; }
        public string TableAlias { get; set; }
        public bool UseApply { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                if(UseApply)
                {
                    return string.Format("{0} APPLY {1}", JoinType, Condition);
                }

                return string.Format("{0} JOIN {1} {2} ON {3}", JoinType, Table, TableAlias, Condition);
            }
            return string.Format("{0} JOIN {1} {2}", JoinType, Table, TableAlias);

        }
    }
}
