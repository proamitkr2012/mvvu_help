namespace MVVU_Model.DTO.HelpDesk { 
    public class FormResponse
    {
        public int ResponseCode { get; set; }

        public string? ResponseMessage { get; set; }

        public string? ResponseMessageCode { get; set; }

        public object? ResponseId { get; set; }
        
    }

    public class EnrolmentUpdateResponse
    {
        public int Id { get; set; }
        public string? OldEnNo { get; set; }
        public string? NewEnNo { get; set; }
        public string? Message { get; set; }
    }
}
