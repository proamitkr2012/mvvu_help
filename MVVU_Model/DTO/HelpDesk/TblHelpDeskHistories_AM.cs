using MVVU_Model.CustomAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVVU_Model.DTO.HelpDesk
{
    public class TblHelpDeskHistories_AM
    {
        public long EntryID { get; set; }
        public string? TicketID { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
       
        public int? Updatedby { get; set; }
        public string? UpdatedbyName { get; set; }
        public int? CTypeID { get; set; }
        public string? IpAddress { get; set; }

        public List<TblHelpFile_AM> FileList { get; set; }
        public string? Status { get; set; }
        public string? CreatedDateDisplay { get; set; }
        public string? TypeName { get; set; }


        public int? ForwardCTypeID { get; set; }
        public int? ForwardBy { get; set; }
        public DateTime? ForwardDateTime { get; set; }

    }
    
}
