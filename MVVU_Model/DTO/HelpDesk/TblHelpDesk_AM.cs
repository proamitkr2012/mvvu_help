using MVVU_Model.CustomAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVVU_Model.DTO.HelpDesk
{
    public class TblHelpDesk_AM
    {
        public long EntryID { get; set; }
        public string? HelpID { get; set; }
        public string? TicketID { get; set; }
        public string? Subject { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? FName { get; set; }
        public string? Mobile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? Enroll_Roll { get; set; }
        public string? Remarks { get; set; }
        public bool? IsUser { get; set; }
        public int? CTypeID { get; set; }
        public string? TypeName { get; set; }
        public string? IpAddress { get; set; }
        public int? FlagTypeID { get; set; }
        public string? FlagTypeName { get; set; }
        public string? Color { get; set; }
        public int? ForwardCTypeID { get; set; }
        public int? ForwardBy { get; set; }
        public DateTime? ForwardDateTime { get; set; }

        public string? ProfilePic { get; set; }
        
        public List<TblHelpFile_AM> FileList { get; set; }

        public string? IsReplied { get; set; }
        public string? Roll { get; set; }
        public string? ApplicationNo { get; set; }
        

    }
    public class TblHelpFile_AM
    {
        public string? FilePath { get; set; }
        public string? Name { get; set; }
    }

}
