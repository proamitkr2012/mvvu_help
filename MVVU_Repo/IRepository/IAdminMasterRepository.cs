using MVVU_Model.DTO;
using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using MVVU_Model;
using System.Threading.Tasks;
using MVVU_Model.DTO.HelpDesk;
using MVVU_Model.Enum;

namespace MVVU_Repo
{
    public interface IAdminMasteMVVU_Repository : IRepository<AdminMaster>
    {
        //admin-login
        //AdminMasterDTO AuthenticateAdmin(string userName, string password);
        Task<AdminMasterDTO> AuthenticateAdmin(string userName, string password);
        bool IsAdminEmailExists(string email);
        Task<List<CourseMasterDTO_AM>> GetCourseTypeList(string flag);
        Task<List<CourseMasterDTO_AM>> GetCourseList(string flag, string CourseType, bool? IsPG);
        Task<StudentMasterDTO> CheckStudentData(SearchDTO model);
        Task<StudentMasterDTO> ResultStudentData(SearchDTO model);
        Task<List<STUDENT_MARKS_AMDTO>> ResultMarksStudentData(SearchDTO model);
        Task<List<STUDENT_RESULT_AMDTO>> ResultDataStudentData(SearchDTO model);
        Task<List<ExamTyperDTO_AM>> GetExamTypeList(string flag, string CourseName);


        Task<StudentMasterDTO> GETDeLLServeMVVU_Data();
        Task<List<CSP_MASTER_AMDTO>> CSVDataStudentData(StudentMasterDTO model);
        Task<FormResponse> SaveTicket(TblHelpDesk_AM model);
        Task<FormResponse> SaveRemarkTicket(TblHelpDeskHistories_AM model);

        Task<List<TblHelpDesk_AM>> GetHelpDeskList(string status, int UserId, int CTypeID);

        Task<DashboardCount> GetHelpDeskCount(int AdminId);
        Task<TblHelpDeskDTO_AM> GetTickets(string Ticket);
        Task<FormResponse> CheckTicket(string Ticket,string Mobile,string Name);
        Task<List<TblHelpDeskComplaintType_AM>> GetComplaintType();
        Task<FormResponse> FlagStatus(TblHelpDesk_AM model);
        Task<List<AdminMasterDTO>> AdminMasterList();
        Task<AdminMasterDTO> AdminEdit(int AdminId);
        Task<FormResponse> Update_AdminMasters(AdminMasterDTO model);
        Task<FormResponse> TransferTicket(TblHelpDesk_AM model , int UserId);
        Task<FormResponse> Deletechat(TblHelpDeskHistories_AM model);
    }
}
