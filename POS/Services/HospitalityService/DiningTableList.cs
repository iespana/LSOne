using System.Collections.Generic;

namespace LSOne.Services
{
    class DiningTableList
    {
        public string DiningTableLayoutId { get; set; }
        public List<DiningTable> TableList { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public string HospitalitySalesType { get; set; }
        public string Description { get; set; }

        public DiningTableList()
        {
            TableList = new List<DiningTable>();
        }
    }
}
