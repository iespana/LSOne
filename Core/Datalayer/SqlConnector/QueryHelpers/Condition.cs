using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.SqlConnector.QueryHelpers
{
    public class Condition
    {
        public static Condition Empty
        {
            get { return new Condition {Operator = "", ConditionValue = ""}; }
        }
        public string Operator { get; set; }
        public string ConditionValue { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Operator, ConditionValue);
        }
    }
}
