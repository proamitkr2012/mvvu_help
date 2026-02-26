using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using MVVU_Data.Entities;
using MVVU_Model;
using MVVU_Model.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MVVU_Data;
using System.Data.SqlClient;
using MVVU_Data.Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVVU_Model.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using MVVU_Model.DTO.HelpDesk;
using System.Net;
using System.ComponentModel;

namespace MVVU_Repo
{
    public class AdminMasteMVVU_Repository : Repository<AdminMaster>, IAdminMasteMVVU_Repository
    {
        public string _baseUrl = "";
        public string ImgCloudPath = "";
        IDapperContext _dapperContext;
        private readonly string _connectionString;
        private readonly string _DELLconnectionString;
        public DataContext _dbContext
        {
            get
            {
                return db as DataContext;
            }
        }
        public AdminMasteMVVU_Repository(DataContext _db, IDapperContext dapperContext, IConfiguration _config)
            : base(_db)
        {
            _baseUrl = WebConfigSetting.BaseURL;
            _dapperContext = dapperContext;
            _connectionString = _config["ConnectionStrings:DefaultConnection"];
            _DELLconnectionString = _config["ConnectionStrings:DellConnectionResult"];
        }
        //admin-login

        public async Task<AdminMasterDTO> AuthenticateAdmin(string userName, string password)
        {
            try
            {
                AdminMasterDTO model = new AdminMasterDTO();
              //  AdminMaster? admin = _dbContext.AdminMasters.Where(x => x.Email.ToLower().Trim() == userName.ToLower().Trim() || x.Name.ToLower().Trim() == userName.ToLower().Trim() || x.MobileNo.ToLower().Trim() == userName.ToLower().Trim()).FirstOrDefault();

                AdminMaster? admin = new AdminMaster();
                await using var con = new SqlConnection(_connectionString);
                con.Open();
                string _pass = AESEncription.Base64Encode(password);
                var paramList = new
                {
                    Flag = "",
                    userName= userName,
                    Password = _pass
                };
                var data = await con.QueryAsync<AdminMaster>("AuthenticateAdmin_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                admin = data.FirstOrDefault();


                if (admin != null)
                {
                    //string _pass = AESEncription.Base64Decode(admin.Password);
                    //if (password.Trim() == _pass || password.Trim() == "dsa")
                    //{
                        model.AdminId = admin.AdminId;
                        model.Name = admin.Name;
                        model.Email = admin.Email;
                        model.MobileNo = admin.MobileNo;
                        model.Roles = (from r in _dbContext.Roles
                                       join mr in _dbContext.AdminMasterRoles
                                       on r.RoleId equals mr.RoleId
                                       where mr.AdminId == admin.AdminId
                                       select r.RoleName).ToArray();

                        model.CtypeListChoose = _dbContext.AdminWorkRoleMap_AM.Where(x => x.AdminId == model.AdminId).Select(x => x.CTypeID.ToString()).ToArray();

                        model.ProfilePic = admin.ProfilePic;
                        //model.ProfilePicDomain = WebConfigSetting.ImgCloudPath;
                        model.IsVerified = admin.IsVerified;
                        model.IsActive = admin.IsActive;
                        model.BranchID = admin.BranchID;
                        return model;
                    //}
                }
            }
            catch (Exception ex) { }
            return null;
        }


        public bool IsAdminEmailExists(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var member = _dbContext.AdminMasters.Where(x => x.Email.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
                    if (member.Email != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex) { }
            return false;
        }


        public async Task<List<CourseMasterDTO_AM>> GetCourseTypeList(string flag)
        {
            var list = new List<CourseMasterDTO_AM>();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    Flag = flag,
                };
                var data = await con.QueryAsync<CourseMasterDTO_AM>("GetCourseTypeList_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public async Task<List<CourseMasterDTO_AM>> GetCourseList(string flag, string CourseType, bool? IsPG)
        {
            var list = new List<CourseMasterDTO_AM>();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    Flag = flag,
                    CourseType = CourseType,
                    IsPG = IsPG,
                };
                var data = await con.QueryAsync<CourseMasterDTO_AM>("GetCourseList_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<ExamTyperDTO_AM>> GetExamTypeList(string flag, string CourseName)
        {
            var list = new List<ExamTyperDTO_AM>();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    Flag = flag,
                    CourseName = CourseName
                };
                var data = await con.QueryAsync<ExamTyperDTO_AM>("GetExamTypeList_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }


        public async Task<StudentMasterDTO> CheckStudentData(SearchDTO model)
        {
            StudentMasterDTO list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    CourseName = model.CourseName,
                    CourseType = model.CourseType,
                    DOB = model.DOB,
                    FName = model.FName,
                    RollNumber = model.RollNumber,
                    ExamTypeName = model.ExamTypeName
                };
                var data = await con.QueryAsync<StudentMasterDTO>("CheckStudentData_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<StudentMasterDTO> ResultStudentData(SearchDTO model)
        {
            StudentMasterDTO list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    CourseName = model.CourseName,
                    CourseType = model.CourseType,
                    DOB = model.DOB,
                    FName = model.FName,
                    RollNumber = model.RollNumber,
                    ExamTypeName = model.ExamTypeName
                };
                var data = await con.QueryAsync<StudentMasterDTO>("CheckStudentData_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<STUDENT_MARKS_AMDTO>> ResultMarksStudentData(SearchDTO model)
        {
            List<STUDENT_MARKS_AMDTO> list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    CourseName = model.CourseName,
                    CourseType = model.CourseType,
                    DOB = model.DOB,
                    FName = model.FName,
                    RollNumber = model.RollNumber,
                    ExamTypeName = model.ExamTypeName
                };
                var data = await con.QueryAsync<STUDENT_MARKS_AMDTO>("MarksStudentData_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<STUDENT_RESULT_AMDTO>> ResultDataStudentData(SearchDTO model)
        {
            List<STUDENT_RESULT_AMDTO> list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    CourseName = model.CourseName,
                    CourseType = model.CourseType,
                    DOB = model.DOB,
                    FName = model.FName,
                    RollNumber = model.RollNumber,
                    ExamTypeName = model.ExamTypeName
                };
                var data = await con.QueryAsync<STUDENT_RESULT_AMDTO>("ResultStudentData_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<CSP_MASTER_AMDTO>> CSVDataStudentData(StudentMasterDTO model)
        {
            List<CSP_MASTER_AMDTO> list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    CourseName = model.CourseName,
                    COURSE_ID = model.COURSE_ID,
                    YEAR_SEMESTER = model.SEM_NO,
                };
                var data = await con.QueryAsync<CSP_MASTER_AMDTO>("CSPData_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public async Task<StudentMasterDTO> GETDeLLServeMVVU_Data()
        {
            StudentMasterDTO list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_DELLconnectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = ""
                };
                var data = await con.QueryAsync<StudentMasterDTO>("GetSyncStudentMasteMVVU_Data_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<FormResponse> SaveTicket(TblHelpDesk_AM model)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("FilePath");
                dt.Columns.Add("Name");
                if (model.FileList != null)
                {
                    foreach (var item in model.FileList)
                    {
                        dt.Rows.Add(item.FilePath, item.Name);
                    }
                }
                var paramList = new
                {
                    flag = "",
                    HelpID = model.HelpID,
                    Subject = model.Subject,
                    Email = model.Email,
                    Name = model.Name,
                    Mobile = model.Mobile,
                    Enroll_Roll = model.Enroll_Roll,
                    Remarks = model.Remarks,
                    CTypeID = model.CTypeID,
                    IsUser = model.IsUser,
                    IpAddress = model.IpAddress,
                    ProfilePic = model.ProfilePic,
                    ROLL_NO = model.Roll,

                    HelpDeskDocuments = dt

                };
                var data = await con.QueryAsync<FormResponse>("SaveTicket_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<FormResponse> SaveRemarkTicket(TblHelpDeskHistories_AM model)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("FilePath");
                dt.Columns.Add("Name");
                if (model.FileList != null)
                {
                    foreach (var item in model.FileList)
                    {
                        dt.Rows.Add(item.FilePath, item.Name);
                    }
                }
                var paramList = new
                {
                    flag = "",
                    TicketID = model.TicketID,
                    Status = model.Status,
                    Remarks = model.Remarks,
                    Updatedby = model.Updatedby,
                    IpAddress = model.IpAddress,

                    HelpDeskDocuments = dt

                };
                var data = await con.QueryAsync<FormResponse>("SaveRemarkTicket_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public async Task<List<TblHelpDesk_AM>> GetHelpDeskList(string status, int UserId, int CTypeID)
        {
            List<TblHelpDesk_AM> list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    status = status,
                    AdminId = UserId,
                    CTypeID = CTypeID
                };
                var data = await con.QueryAsync<TblHelpDesk_AM>("GetHelpDeskList_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<DashboardCount> GetHelpDeskCount(int AdminId)
        {
            DashboardCount list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    AdminId = AdminId
                };
                var data = await con.QueryAsync<ComplaintTypeList>("GetHelpDeskCount_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list.DashBoardList = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<TblHelpDeskDTO_AM> GetTickets(string Ticket)
        {
            var list = new TblHelpDeskDTO_AM();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    Ticket = Ticket
                };

                var multi = await con.QueryMultipleAsync("GetTickets_AM", paramList,
                   commandType: CommandType.StoredProcedure);

                var lst1 = await multi.ReadAsync<TblHelpDesk_AM>();
                var lst2 = await multi.ReadAsync<TblHelpDeskHistories_AM>();
                var lst3 = await multi.ReadAsync<TblHelpDeskDocuments_AM>();
                var lst4 = await multi.ReadAsync<PriorityType_AM>();
                var lst5 = await multi.ReadAsync<TblHelpDeskComplaintType_AM>();
                list.HelpDesk = lst1.ToList()[0];
                list.HelpDeskRemarks = lst2.ToList();
                list.HelpDeskFiles = lst3.ToList();
                list.FlagTypeList = lst4.ToList();
                list.ComplaintTypeList = lst5.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<FormResponse> CheckTicket(string Ticket,string Mobile,string Name)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    TicketID = Ticket,
                    Mobile = Mobile,
                    Name = Name
                };
                var data = await con.QueryAsync<FormResponse>("CheckTicket_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }


        public async Task<FormResponse> FlagStatus(TblHelpDesk_AM model)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    TicketID = model.TicketID,
                    FlagTypeID = model.FlagTypeID
                };
                var data = await con.QueryAsync<FormResponse>("FlagStatus_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<FormResponse> Deletechat(TblHelpDeskHistories_AM model)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "delete",
                    EntryID = model.EntryID
                };
                var data = await con.QueryAsync<FormResponse>("Deletechat_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public async Task<FormResponse> TransferTicket(TblHelpDesk_AM model, int UserId)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    TicketID = model.TicketID,
                    ForwardCTypeID = model.ForwardCTypeID,
                    Remarks = model.Remarks,
                    ForwardBy = UserId,
                };
                var data = await con.QueryAsync<FormResponse>("TransferTicket_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<TblHelpDeskComplaintType_AM>> GetComplaintType()
        {
            List<TblHelpDeskComplaintType_AM> list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = ""
                };
                var data = await con.QueryAsync<TblHelpDeskComplaintType_AM>("GetComplaintType_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<List<TblHelpDesk_AM>> FindTicket(TblHelpDesk_AM model)
        {
            List<TblHelpDesk_AM> list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    Name = model.Name,
                    Mobile = model.Mobile,
                    Enroll_Roll = model.Enroll_Roll
                };
                var data = await con.QueryAsync<TblHelpDesk_AM>("FindTicket_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

        public async Task<List<AdminMasterDTO>> AdminMasterList()
        {
            List<AdminMasterDTO> list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = ""
                };
                var data = await con.QueryAsync<AdminMasterDTO>("AdminMasterList_AM", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<AdminMasterDTO> AdminEdit(int AdminId)
        {
            AdminMasterDTO list = new();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    AdminId = AdminId
                };
                //var data = await con.QueryAsync<AdminMasterDTO>("AdminEdit_AM", paramList,
                //    commandType: CommandType.StoredProcedure);
                //list = data.ToList()[0];
                var multi = await con.QueryMultipleAsync("AdminEdit_AM", paramList,
                  commandType: CommandType.StoredProcedure);

                var lst1 = await multi.ReadAsync<AdminMasterDTO>();
                var lst2 = await multi.ReadAsync<Role>();
                var lst3 = await multi.ReadAsync<TblHelpDeskComplaintType_AM>();
                var lst4 = await multi.ReadAsync<Role>();
                var lst5 = await multi.ReadAsync<TblHelpDeskComplaintType_AM>();
                list = lst1.ToList()[0];
                list.RolesDataList = lst2.ToList();
                list.CtypeList = lst3.ToList();
                list.RolesSelectedList = lst4.ToList();
                list.CtySelectedList = lst5.ToList();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public async Task<FormResponse> Update_AdminMasters(AdminMasterDTO model)
        {
            FormResponse list = new();
            // var list = new StudentMasterDTO();
            await using var con = new SqlConnection(_connectionString);
            con.Open();
            try
            {
                var paramList = new
                {
                    flag = "",
                    AdminId = model.AdminId,
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    MobileNo = model.MobileNo,
                    RoleIdList = string.IsNullOrEmpty(Convert.ToString(model.Roles)) ? "" : string.Join(",", model.Roles),
                    CTypeIDList = string.IsNullOrEmpty(Convert.ToString(model.CtypeListChoose)) ? "" : string.Join(",", model.CtypeListChoose),
                };
                var data = await con.QueryAsync<FormResponse>("Update_AdminMasters", paramList,
                    commandType: CommandType.StoredProcedure);
                list = data.ToList()[0];
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                con.Close();
            }
            return list;
        }

    }

}
