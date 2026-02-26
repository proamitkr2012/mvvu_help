namespace MVVU_Model.DTO.HelpDesk { 
    

    public class StatusCount
    {
        public int PendingCount { get; set; }
        public int OpenCount { get; set; }
        public int HoldCount { get; set; }
        public int SolvedCount { get; set; }
        public int ClosedCount { get; set; }
    }
}
