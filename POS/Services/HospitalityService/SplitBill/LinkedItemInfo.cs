namespace LSOne.Services.SplitBill
{
    public class LinkedItemInfo
    {
        public int OrgLineId { get; set; }
        public int OrgLinkedLineId { get; set; }
        public int NewLineId { get; set; }
        public int NewLinkedLineId { get; set; }
        public bool IsInfocodeItem { get; set; }
        public bool IsLinkedItem { get; set; }
        public bool IsSplitItem { get; set; }


        public LinkedItemInfo()
        {
            OrgLineId = 0;
            OrgLinkedLineId = 0;
            NewLineId = 0;
            NewLinkedLineId = 0;
            IsInfocodeItem = false;
            IsLinkedItem = false;
            IsSplitItem = false;
        }
    }
}
