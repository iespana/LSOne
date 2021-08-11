using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscSubJob
    {
 
        private JscTableDesign jscTableFrom;
        private JscTableDesign jscActionTable;
        private JscFieldDesign jscRepCounterField;
        private JscFieldDesign jscMarkSentRecordsField;
        
        public RecordIdentifier ID { get; set; }
        public string Description { get; set; }
        public RecordIdentifier TableFrom { get; set; }
        public string StoredProcName { get; set; }
        public string TableNameTo { get; set; }
        public ReplicationTypes ReplicationMethod { get; set; }
        public ModeDef WhatToDo { get; set; }
        public bool Enabled { get; set; }
        public bool IncludeFlowFields { get; set; }
        public RecordIdentifier ActionTable { get; set; }
        public int? ActionCounterInterval { get; set; }
        public bool MoveActions { get; set; }
        public bool NoDistributionFilter { get; set; }
        public RecordIdentifier RepCounterField { get; set; }
        public int? RepCounterInterval { get; set; }
        public bool UpdateRepCounter { get; set; }
        public bool UpdateRepCounterOnEmptyInt { get; set; }
        public RecordIdentifier MarkSentRecordsField { get; set; }
        public List<JscJobSubjob> JscJobSubjobs { get; set; }
        public List<JscSubJobFromTableFilter> JscSubJobFromTableFilters { get; set; }
        
        public JscTableDesign JscTableFrom
        {
            get { return jscTableFrom; }
            set
            { 
                jscTableFrom = value;
                TableFrom = value == null ? TableFrom : value.ID;
            }
        }

        public JscTableDesign JscActionTable
        {
            get { return jscActionTable; }
            set
            {
                jscActionTable = value;
                ActionTable = value == null ? ActionTable : value.ID;
            }
        }

        public JscFieldDesign JscRepCounterField
        {
            get { return jscRepCounterField; }
            set
            {
                jscRepCounterField = value;
                RepCounterField = value == null ? RepCounterField : value.ID;
            }
        }

        public JscFieldDesign JscMarkSentRecordsField
        {
            get { return jscMarkSentRecordsField; }
            set
            {
                jscMarkSentRecordsField = value;
                MarkSentRecordsField = value == null ? MarkSentRecordsField : value.ID;
            }
        }

        internal string ObjectName
        {
            get
            {
                string objectName = string.Empty;

                if (ReplicationMethod != ReplicationTypes.Procedure)
                {
                    if (JscTableFrom != null)
                    {
                        objectName = JscTableFrom.TableName;
                    }
                }
                else
                {
                    objectName = StoredProcName;
                }

                return objectName;
            }
        }
    }
}