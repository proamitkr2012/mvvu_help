using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVVU_Model.DTO
{
    public class STUDENT_RESULT_AMDTO
    {
        public string? EXAM_NAME { get; set; }
        public string? EXAM_TYPE { get; set; }
        public string? CCODE { get; set; }
        public string? ROLL_NO { get; set; }
        public string? RESULT { get; set; }
        public string? DIVISION { get; set; }
        public string? MARKS_OBT { get; set; }
        public int? MAX_MARKS { get; set; }
        public int? MIN_MARKS { get; set; }
        public string? REMARKS { get; set; }
        public string? RESULT_DECL_DATE { get; set; }
        public DateTime? CREATEON { get; set; }
        public DateTime? UPDATEON { get; set; }
        public string? COURSE_NAME { get; set; }
        public string? SEM_NO { get; set; }
        public int COURSE_ID { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? TotalEarnCredit { get; set; }
        public decimal? TotalPoint { get; set; }
        public decimal? SGPA { get; set; }
        public int? TheoryObtP { get; set; }
        public int? SessionalObtP { get; set; }
        public decimal? TotalGradePointP { get; set; }
        public decimal? TotalCreditGradePointP { get; set; }
        public int? TheoryObt { get; set; }
        public int? SessionalObt { get; set; }
        public decimal? TotalGradePoint { get; set; }
        public decimal? TotalCreditGradePoint { get; set; }
        public int? GTCredit { get; set; }
        public decimal? GTCreditGradePoint { get; set; }
        public int? TotalCreditP { get; set; }
        public int? TotalObtP { get; set; }
        public int? TotalObt { get; set; }
        public string? OBT_MARKS_1 { get; set; }
        public string? MIN_MARKS_1 { get; set; }
        public string? MAX_MARKS_1 { get; set; }
        public int? TOTAL_CREDIT_1 { get; set; }
        public int? EARN_CREDIT_1 { get; set; }
        public decimal? TOTAL_GP_1 { get; set; }
        public decimal? SGPA_1 { get; set; }
        public string? RESULT_1 { get; set; }
        public string? OBT_MARKS_2 { get; set; }
        public string? MIN_MARKS_2 { get; set; }
        public string? MAX_MARKS_2 { get; set; }
        public int? TOTAL_CREDIT_2 { get; set; }
        public int? EARN_CREDIT_2 { get; set; }
        public decimal? TOTAL_GP_2 { get; set; }
        public decimal? SGPA_2 { get; set; }
        public string? RESULT_2 { get; set; }
        public string? OBT_MARKS_3 { get; set; }
        public string? MIN_MARKS_3 { get; set; }
        public string? MAX_MARKS_3 { get; set; }
        public int? TOTAL_CREDIT_3 { get; set; }
        public int? EARN_CREDIT_3 { get; set; }
        public decimal? TOTAL_GP_3 { get; set; }
        public decimal? SGPA_3 { get; set; }
        public string? RESULT_3 { get; set; }
        public string? OBT_MARKS_4 { get; set; }
        public string? MIN_MARKS_4 { get; set; }
        public string? MAX_MARKS_4 { get; set; }
        public int? TOTAL_CREDIT_4 { get; set; }
        public int? EARN_CREDIT_4 { get; set; }
        public decimal? TOTAL_GP_4 { get; set; }
        public decimal? SGPA_4 { get; set; }
        public string? RESULT_4 { get; set; }
        public string? OBT_MARKS_5 { get; set; }
        public string? MIN_MARKS_5 { get; set; }
        public string? MAX_MARKS_5 { get; set; }
        public int? TOTAL_CREDIT_5 { get; set; }
        public int? EARN_CREDIT_5 { get; set; }
        public decimal? TOTAL_GP_5 { get; set; }
        public decimal? SGPA_5 { get; set; }
        public string? RESULT_5 { get; set; }
        public string? OBT_MARKS_6 { get; set; }
        public string? MIN_MARKS_6 { get; set; }
        public string? MAX_MARKS_6 { get; set; }
        public int? TOTAL_CREDIT_6 { get; set; }
        public int? EARN_CREDIT_6 { get; set; }
        public decimal? TOTAL_GP_6 { get; set; }
        public decimal? SGPA_6 { get; set; }
        public string? RESULT_6 { get; set; }
        public string? OBT_MARKS_7 { get; set; }
        public string? MIN_MARKS_7 { get; set; }
        public string? MAX_MARKS_7 { get; set; }
        public int? TOTAL_CREDIT_7 { get; set; }
        public int? EARN_CREDIT_7 { get; set; }
        public decimal? TOTAL_GP_7 { get; set; }
        public decimal? SGPA_7 { get; set; }
        public string? RESULT_7 { get; set; }
        public string? OBT_TOTAL { get; set; }
        public string? MAX_TOTAL { get; set; }
        public string? MIN_TOTAL { get; set; }
        public decimal? GP_TOTAL { get; set; }
        public decimal? SGPA_TOTAL { get; set; }
        public string? OBT_RG_MARKS { get; set; }
        public string? MIN_RG_MARKS { get; set; }
        public string? MAX_RG_MARKS { get; set; }
        public string? RESULT_RG { get; set; }
        public string? FINAL_RESULT { get; set; }
        public int ISDISPLAY { get; set; }
        public int? ISPUBLISHSED { get; set; }
        public DateTime? PUBLISH_DATE { get; set; }
        public int? ResultSeq { get; set; }
    }
}