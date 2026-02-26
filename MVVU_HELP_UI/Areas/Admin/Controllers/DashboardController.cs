using MVVU_Data.Entities;
using MVVU_Model;
using MVVU_Model.DTO;
using MVVU_Model.Enum;
using MVVU_Repo;
using MVVU_Repo.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using MVVU_Model.DTO.HelpDesk;
using System.Buffers.Text;

namespace MVVU_HELP_UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class DashboardController : BaseController
    {
        int pageSize;
        private IWebHostEnvironment environment;
        public string ImgCloudPath = "";
        private IMailClient _mailClient;
        public DashboardController(IHttpContextAccessor _httpContextAccessor, IConfiguration _config,
             IWebHostEnvironment _environment, IUnitOfWork uow, IMailClient mailClient) : base(_httpContextAccessor, _config, uow)
        {
            environment = _environment;
            string _pageSize = "15";//Convert.ToString(WebConfigSetting.PageSize);
            int PS;
            bool result = Int32.TryParse(_pageSize, out PS);
            pageSize = (result == true) ? PS : 15;
            _mailClient = mailClient;
        }
        [Route("~/admin/dashboard")]
        public async Task<IActionResult> Index()
        {
            DashboardCount model = await UOF.IAdminMaster.GetHelpDeskCount((int)CurrentUser.UserId);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CountIndex()
        {
            DashboardCount model = await UOF.IAdminMaster.GetHelpDeskCount((int)CurrentUser.UserId);
            return PartialView("_CounterPage", model);
        }
        public async Task<IActionResult> NewTickectList(int ID=0)
        {
            List<TblHelpDesk_AM> s = await UOF.IAdminMaster.GetHelpDeskList(ENUM_HELP_STATUS.Pending.ToString(), (int)CurrentUser.UserId, ID);
            return View(s);
        }
        public async Task<IActionResult> OpenTickectList(int ID = 0)
        {
            List<TblHelpDesk_AM> s = await UOF.IAdminMaster.GetHelpDeskList(ENUM_HELP_STATUS.Open.ToString(), (int)CurrentUser.UserId, ID);
            return View(s);
        }
        public async Task<IActionResult> HoldTickectList(int ID = 0)
        {
            List<TblHelpDesk_AM> s = await UOF.IAdminMaster.GetHelpDeskList(ENUM_HELP_STATUS.OnHold.ToString(), (int)CurrentUser.UserId, ID);
            return View(s);
        }
        public async Task<IActionResult> ClosedTickectList(int ID = 0)
        {
            List<TblHelpDesk_AM> s = await UOF.IAdminMaster.GetHelpDeskList(ENUM_HELP_STATUS.Closed.ToString(), (int)CurrentUser.UserId, ID);
            return View(s);
        }
        public async Task<IActionResult> SpamTickectList(int ID = 0)
        {
            List<TblHelpDesk_AM> s = await UOF.IAdminMaster.GetHelpDeskList(ENUM_HELP_STATUS.Spam.ToString(), (int)CurrentUser.UserId, ID);
            return View(s);
        }
        
        [HttpGet]
        public async Task<IActionResult> Tickets(string Ticket,int t=0)
        {

            if (t == 1)
            {
                Ticket = AESEncription.Base64Decode(Ticket);
            }
            TblHelpDeskDTO_AM s = await UOF.IAdminMaster.GetTickets(Ticket);
            if (s != null)
            {
                // List<tb> s= await UOF.IAdminMaster.GetTickets(Ticket);
            }
            return View(s);
        }
        public async Task<string> Geoloc()
        {
            try
            {
                string _ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                if (_ipAddress.Contains("::1"))
                    _ipAddress = "127.0.0.1";

                return _ipAddress;
            }
            catch (Exception e)
            {

            }
            return string.Empty;
        }
        [HttpPost]
        public async Task<IActionResult> Tickets(string Ticket, string Message, string Status)
        {
            TblHelpDeskHistories_AM model = new TblHelpDeskHistories_AM();
            model.TicketID = Ticket;
            model.Status = Status;
            model.Remarks = Message;
            model.Updatedby = (int)CurrentUser.UserId;
            model.IpAddress = await Geoloc();
            if (Request.Form.Files.Count > 0)
            {
                List<TblHelpFile_AM> docfile = new List<TblHelpFile_AM>();
                int j = 1;
                foreach (IFormFile file in Request.Form.Files)
                {
                    TblHelpFile_AM d = new TblHelpFile_AM();
                    var FullPath = "";

                    if ((file != null) && (file.Length > 0) && !string.IsNullOrEmpty(file.FileName))
                    {

                        var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        var name = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .Name
                                        .Trim('"');
                        d.Name = filename;
                        var ext = filename.Substring(filename.LastIndexOf('.'));
                        string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string myfile = time+j.ToString() + ext;
                        var path = "D:/DOCUMENTS/MVVU_HELPDESK_FILE/";
                        FullPath = path + myfile;
                        var uploads = Path.Combine(path);

                        var filePath = Path.Combine(uploads, myfile);

                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }

                        if (System.IO.File.Exists(filePath) == true)
                        {
                            System.IO.File.Delete(filePath);
                        }

                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(fs);
                        }
                        d.FilePath = FullPath.Replace("D:", "");

                        //  size += file.Length;
                        j++;
                    }

                    docfile.Add(d);
                }
                model.FileList = docfile;
            }

            FormResponse adminData = await UOF.IAdminMaster.SaveRemarkTicket(model);
            //if (adminData.ResponseCode == 1 && adminData.ResponseMessageCode != "-1")
            //   {
            //       string t = AESEncription.Base64Encode(adminData.ResponseMessageCode);
            //       return RedirectToAction("Success", new { t=t });
            //       }
            return RedirectToAction("Tickets", new { Ticket = Ticket });
        }
        [HttpPost]
        public async Task<IActionResult> FlagStatus([FromBody] TblHelpDesk_AM data)
        {
            try
            {

                FormResponse stat = await UOF.IAdminMaster.FlagStatus(data);
                if (stat.ResponseCode == 1 && stat.ResponseMessageCode != "-1")
                {
                    return Json(new { data = "success" });
                }
               

            }
            catch (Exception e)
            {
            }
            return Json("fail");

        }
        [HttpPost]
        public async Task<IActionResult> TransferTicket([FromBody] TblHelpDesk_AM data)
        {
            try
            {

                FormResponse stat = await UOF.IAdminMaster.TransferTicket(data,(int)CurrentUser.UserId);
                if (stat.ResponseCode == 1 && stat.ResponseMessageCode != "-1")
                {
                    return Json(new { data = "success" });
                }


            }
            catch (Exception e)
            {
            }
            return Json("fail");

        }
        
        public async Task<IActionResult> AdminMasterList()
        {
            List<AdminMasterDTO> model = await UOF.IAdminMaster.AdminMasterList();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AdminEdit(int AdminId) {
            try
            {
                AdminMasterDTO model=await UOF.IAdminMaster.AdminEdit(AdminId);
                model.Password= AESEncription.Base64Decode(model.Password);
                List<SelectListItem> Select_List = new List<SelectListItem>();
                foreach (var r in model.RolesDataList)
                {
                    SelectListItem obj = new SelectListItem()
                    {
                        Value = r.RoleId.ToString(),
                        Text = r.Description,
                        Selected = model.RolesSelectedList.Where(me => me.RoleId == r.RoleId).Count() > 0 ? true : false
                    };

                    Select_List.Add(obj);
                }
                model.RoleMaster = Select_List;

                List<SelectListItem> Select_Listct = new List<SelectListItem>();
                foreach (var r in model.CtypeList)
                {
                    SelectListItem obj = new SelectListItem()
                    {
                        Value = r.CTypeID.ToString(),
                        Text = r.TypeName,
                        Selected = model.CtySelectedList.Where(me => me.CTypeID == r.CTypeID).Count() > 0 ? true : false
                    };

                    Select_Listct.Add(obj);
                }
                model.CtMaster = Select_Listct;

                return View(model);
            }
            catch (Exception e)
            {
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AdminEdit(AdminMasterDTO data)
        {
            try
            {
                data.Password = AESEncription.Base64Encode(data.Password);

                FormResponse model = await UOF.IAdminMaster.Update_AdminMasters(data);
                return RedirectToAction("AdminEdit", new { AdminId=data.AdminId });
            }
            catch (Exception e)
            {
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Deletechat([FromBody] TblHelpDeskHistories_AM model)
        {
            try
            {

                FormResponse stat = await UOF.IAdminMaster.Deletechat(model);
                if (stat.ResponseCode == 1 && stat.ResponseMessageCode != "-1")
                {
                    return Json(new { data = "success" });
                }


            }
            catch (Exception e)
            {
            }
            return Json("fail");

        }

        
    }
}
