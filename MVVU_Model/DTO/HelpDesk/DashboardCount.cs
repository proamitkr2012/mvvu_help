namespace MVVU_Model.DTO.HelpDesk { 
    

    public class DashboardCount
    {
        public List<ComplaintTypeList> DashBoardList { get; set; }
    }

    public class ComplaintTypeList
    {
        public int CTypeID { get; set; }
        public string? TypeName { get; set; }
        public string? Details { get; set; }
        public bool? IsActive { get; set; }
        public int? Sequence { get; set; }       


        public int PendingCount { get; set; }
        public int OpenCount { get; set; }
        public int HoldCount { get; set; }
        public int SolvedCount { get; set; }
        public int ClosedCount { get; set; }
    }
}
