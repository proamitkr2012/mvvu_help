using MVVU_Model.CustomAttribute;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVVU_Model.DTO.HelpDesk
{
    public class TblHelpDeskDocuments_AM
    {
        public long EntryID { get; set; }
        public long? RemarkID { get; set; }

        public string? FilePath { get; set; }
        public bool? IsActive { get; set; }
        public string? Name { get; set; }


    }


}
