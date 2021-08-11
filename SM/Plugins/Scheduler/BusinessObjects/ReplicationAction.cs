using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public enum ActionType
    {
        Insert = 0,
        Update = 1,
        Delete = 2
    };

    public class ReplicationAction
    {
        public RecordIdentifier ActionId { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionTarget { get; set; }
        public List<string> Parameters { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
