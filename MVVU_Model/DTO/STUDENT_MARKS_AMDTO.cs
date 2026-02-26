using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVVU_Model.DTO
{
    public class STUDENT_MARKS_AMDTO
    {
        public string? ROLL_NO { get; set; }
        public string? PCODE { get; set; }
        public int? MARKS_OBT { get; set; }
        public int? PRAC_OBT { get; set; }
        public int? INT_OBT { get; set; }
        public int? SES_OBT { get; set; }
        public int? TOT_INT_SES_OBT { get; set; }
        public int? PAPER_TOT_MARKS_OBT { get; set; }
        public string? SUBJECT_NAME { get; set; }
        public string? CODE { get; set; }
        public string? LOT_NO { get; set; }
        public string? GRACE { get; set; }
        public string? REMARKS { get; set; }
        public string? SUBJ_GRACE { get; set; }
        public string? THEORY_GRACE { get; set; }
        public string? UFM { get; set; }
        public string? COURSE_ID { get; set; }
        public string? EXAM_TYPE { get; set; }
        public string? SEM_NO { get; set; }
        public string? PAPER_RESULT { get; set; }
        public string? SUBJECT_RESULT { get; set; }
        public string? COURSE_NAME { get; set; }
        public string? Grade { get; set; }
        public int? Credit { get; set; }
        public decimal? EarnCredit { get; set; }
        public decimal? Point { get; set; }
        public int? IsNotOk { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UPDATEON { get; set; }
        public string? PCODE_N { get; set; }
        public decimal? GradePoint { get; set; }
        public int? MARKS_OBT_NEW { get; set; }
        public int? MaxInt { get; set; }
        public int? MaxExt { get; set; }
        public int? MaxTot { get; set; }
        public int? MinInt { get; set; }
        public int? MinExt { get; set; }
        public int? MinTot { get; set; }
        public bool? IsStarTh { get; set; }
        public bool? IsStarInt { get; set; }

        //-------med-----------
        //public int? P1_MED { get; set; }
        //public int? P2_MED { get; set; }
        //public int? PTOT_MED { get; set; }
        //public int? PP1_MED { get; set; }
        //public int? TOT_MED { get; set; }
    }
}