using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ML_UAR.Class;
using ML_UAR.Interfaces;
using ML_UAR.Models;
using ML_UAR.Models.ApproveMaster;
using ML_UAR.Models.Settings;
using ML_UAR.Services;
using Newtonsoft.Json;

namespace ML_UAR.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IDatabaseService _databaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ApproveMasterController _approveMasterController;
        public SettingsController(IDatabaseService databaseService,
                                  IHttpContextAccessor httpContextAccessor
                                  //,ApproveMasterController approveMasterController
                                  )
        {
            _databaseService = databaseService;
            _httpContextAccessor = httpContextAccessor;
            //_approveMasterController = approveMasterController;
        }

        public IActionResult Employee()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            return View(_SESSION);
        }

        public async Task<IActionResult> ApproveMappingSystem()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            ViewBag.SystemMasterListSearch = await getSystemMasterSearch();
            ViewBag.SystemMasterList = await getSystemMaster();
            ViewBag.ApproveMasterList = await getApproveMasterList();
            //ViewBag.ApproveMasterList = await _approveMasterController.getApproveMasterList();
            return View(_SESSION);
        }
        public async Task<IActionResult> PositionMappingSystem()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            ViewBag.SectionMasterList = await getSectionMaster();

            return View(_SESSION);
        }
        public async Task<IActionResult> EditDetailPositionMappingSystem(string posCode)
        {


            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            EncodeAndDecode fnBase64 = new EncodeAndDecode();
            var position_code = fnBase64.Decode(posCode);

            ViewBag.User = _SESSION.personnel_code;

            var getPositionMapHead = await GetPositionMappingHead(position_code);

            ViewBag.SectionMasterTxt = getPositionMapHead.section_name;
            ViewBag.DepartMasterTxt = getPositionMapHead.depart_name;
            ViewBag.PositionMasterTxt = getPositionMapHead.position_name;
            ViewBag.PositionCode = getPositionMapHead.position_code;

            ViewBag.ReasonDelList = await getReasonMasterPositionMapping();


            return View(_SESSION);
        }

        public async Task<IActionResult> AddPositionMappingSystem()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            ViewBag.SectionMasterList = await getSectionMaster();

            return View(_SESSION);
        }

        public IActionResult SetPermission()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            return View(_SESSION);
        }

        public async Task<List<SelectListItem>> getPersonnelList(string role_id = "", string section_code = "")
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $" EXEC [dbo].[ST_Personnelddl_GET] '{role_id}','{section_code}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlPersonnelList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.personnel_code,
                        Text = x.ddlpersonnelcode
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> getPersonnelDetail(string section_code = "", string depart_code = "", string position_code = "", string personnel_code = "")
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $" EXEC [dbo].[ST_PersonnelDetail_GET] '{section_code}','{depart_code}','{position_code}','{personnel_code}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<PersonnelDetail>(query, null, unitOfWork.Transaction);

                    var detail = data.ToList().FirstOrDefault();



                    unitOfWork.Complete();
                    return new JsonResult(detail);

                }
            }
            catch (Exception)
            {
                return new JsonResult(false);
            }
        }
        public async Task<List<SelectListItem>> getReasonMasterApproveFlow()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $" EXEC [dbo].[ST_ReasonMaster_GET] '01'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlReasonList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.reason_code,
                        Text = x.reason_description
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SelectListItem>> getReasonMasterPositionMapping()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $" EXEC [dbo].[ST_ReasonMaster_GET] '02'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlReasonList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.reason_code,
                        Text = x.reason_description
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //ApproveMappingSystem
        public async Task<List<SelectListItem>> getSystemMasterSearch()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $" EXEC [dbo].[ST_SystemMaster_GET]'1'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlSystemList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.system_id,
                        Text = x.system_name
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SelectListItem>> getSystemMaster()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $" EXEC [dbo].[ST_SystemMaster_GET]'0'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlSystemList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.system_id,
                        Text = x.system_name
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SelectListItem>> getApproveMasterList()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ApproveMaster>(@$"
                         EXEC [dbo].[ST_ApproveMaster_GET]
                     ", null,
                    unitOfWork.Transaction);


                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.approve_master_id,
                        Text = x.approve_master_name
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetApproveMappingSystemTrans(string system_id = "", int approve_master_id = 0)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_ApproveMapping_GET] '{system_id}','{approve_master_id}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ApproveMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> FormAddApproveMappingSystem(FormApproveMappingSystem param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var action_by = _SESSION.personnel_code;

                    var result = new FormResult();

                    var query = $@"EXEC [dbo].[ST_ApproveMapping_ins] '{param.ddlSystem_id}','{param.ddlApprove_master_id}','{param.isstandard}',{action_by},'{param.isUpdate}'";
                    var data = await unitOfWork.Transaction.Connection.QueryAsync<FormApproveMappingSystem>(query, null, unitOfWork.Transaction);


                    unitOfWork.Complete();
                    result = new FormResult { code = "200", message = "บันทึกข้อมูลสำเร็จ" };
                    return new JsonResult(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApproveMappingSystem> GetApproveMappingSystemById(string system_id, string approve_master_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_ApproveMapping_GET] '{system_id}','{approve_master_id}'";

                    var getdata = await unitOfWork.Transaction.Connection.QueryAsync<ApproveMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    var data = getdata.FirstOrDefault();
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }




        //PositionMsppingSystem
        public async Task<List<SelectListItem>> getSectionMaster()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $"EXEC [dbo].[ST_SectionMaster_GET]";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlSectionMasterList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.section_code,
                        Text = x.section_name
                    })
                    .ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //chain ddl2
        public JsonResult getDepartmentMaster(string section_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_DepartmentMaster_GET]'{section_code}'";

                    var data = unitOfWork.Transaction.Connection.Query(query, null, unitOfWork.Transaction);

                    return new JsonResult(data.ToList());

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //chain ddl3
        public JsonResult getPositionMaster(string depart_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_PositionMaster_GET]'{depart_code}'";

                    var data = unitOfWork.Transaction.Connection.Query(query, null, unitOfWork.Transaction);

                    return new JsonResult(data.ToList());

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JsonResult> GetPositionMappingSystemTrans()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_PositionMappingHead_GET]";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<PositionMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetPositionMappingSystemTransSearch(string position_code, string section_code, string depart_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_PositionMappingHead_GET] '{position_code}','{section_code}','{depart_code}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<PositionMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //detail modal
        public async Task<PositionMappingSystem> GetPositionMappingHeadMdl(string position_code, string section_code, string depart_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_PositionMappingHead_GET]'{position_code}','{section_code}','{depart_code}'";

                    var getdata = await unitOfWork.Transaction.Connection.QueryAsync<PositionMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();
                    var data = getdata.FirstOrDefault();

                    //return new JsonResult(_databaseService.FormatOnce(data.ToList()));
                    return data;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<JsonResult> GetPositionMappingLineTbl(string position_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_PositionMappingLine_GET]'{position_code}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<PositionMappingLine>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //add page
        public async Task<List<ddlRoleMasterList>> getRoleMaster(string system_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_RoleMaster_GET]'{system_id}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ddlRoleMasterList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return data.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<JsonResult> GetSystemListMdl(string system_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $@"EXEC [dbo].[ST_SystemModal_GET]'{system_id}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<SystemList>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {
                throw;

            }
        }

        [HttpPost]
        public async Task<JsonResult> FormAddPositionMappingSystem(FormAddPostionMappingSystem param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var action_by = _SESSION.personnel_code;

                    var result = new FormResult();

                    if (param.txtsystem_id.Count() != 0)
                    {
                        //Head
                        var query_head = $@"EXEC [dbo].[ST_PositionMappingHead_ins] '{param.ddlposition_code}',
                                                                                            '{action_by}',
                                                                                            'E',
                                                                                            '0'";

                        var data_head = await unitOfWork.Transaction.Connection.QueryAsync<FormAddPostionMappingSystem>(query_head, null, unitOfWork.Transaction);
                        for (var i = 0; i <= param.txtsystem_id.Count() - 1; i++)
                        {


                            //Line
                            var query_line = $@"EXEC [dbo].[ST_PositionMappingLine_ins] '{param.ddlposition_code}',
                                                                            '{param.txtsystem_id[i]}',
                                                                            '{param.txtdescription[i]}',
                                                                            '{param.ddlrole_id[i]}',
                                                                            '{action_by}',
                                                                            '{param.txtstatus[i]}',
                                                                            '{param.txtisUpdate[i]}',
                                                                            '{param.txtreason_code[i]}',
                                                                            '{param.txtremark[i]}'
                                                                            ";

                            var data_line = await unitOfWork.Transaction.Connection.QueryAsync<FormAddPostionMappingSystem>(query_line, null, unitOfWork.Transaction);
                        }
                    }
                    else
                    {
                        result = new FormResult { code = "404", message = "ดำเนินการไม่สำเร็จ" };
                        return new JsonResult(result);
                    }

                    unitOfWork.Complete();
                    result = new FormResult { code = "200", message = "แก้ไขข้อมูลสำเร็จ" };
                    return new JsonResult(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //edit page
        public async Task<PositionMappingSystem> GetPositionMappingHead(string position_code)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_PositionMappingHead_GET] '{position_code}'";

                    var getdata = await unitOfWork.Transaction.Connection.QueryAsync<PositionMappingSystem>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    var data = getdata.FirstOrDefault();
                    return data;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<JsonResult> FormEditPositionMappingSystem(FormAddPostionMappingSystem param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var action_by = _SESSION.personnel_code;
                    //var status = "E";

                    var result = new FormResult();

                    if (param.txtsystem_id.Count() != 0)
                    {

                        for (var i = 0; i <= param.txtsystem_id.Count() - 1; i++)
                        {

                            if (param.txtisUpdate[i] == 1)
                            {
                                //Head
                                var query_head = $@"EXEC [dbo].[ST_PositionMappingHead_ins] '{param.ddlposition_code}',
                                                                                            '{action_by}',
                                                                                            '{param.txtstatus[i]}',
                                                                                            '{param.txtisUpdate[i]}'";

                                var data_head = await unitOfWork.Transaction.Connection.QueryAsync<FormAddPostionMappingSystem>(query_head, null, unitOfWork.Transaction);

                                var query_line = $@"EXEC [dbo].[ST_PositionMappingLine_ins] '{param.ddlposition_code}',
                                                                            '{param.txtsystem_id[i]}',
                                                                            '{param.txtdescription[i]}',
                                                                            '{param.ddlrole_id[i]}',
                                                                            '{action_by}',
                                                                            '{param.txtstatus[i]}',
                                                                            '{param.txtisUpdate[i]}',
                                                                            '{param.txtreason_code[i]}',
                                                                            '{param.txtremark[i]}'
                                                                            ";

                                var data_line = await unitOfWork.Transaction.Connection.QueryAsync<FormAddPostionMappingSystem>(query_line, null, unitOfWork.Transaction);
                            }
                            else if (param.txtisUpdate[i] == 0)
                            {
                                //Line add
                                var query_line = $@"EXEC [dbo].[ST_PositionMappingLine_ins] '{param.ddlposition_code}',
                                                                            '{param.txtsystem_id[i]}',
                                                                            '{param.txtdescription[i]}',
                                                                            '{param.ddlrole_id[i]}',
                                                                            '{action_by}',
                                                                            '{param.txtstatus[i]}',
                                                                            '{param.txtisUpdate[i]}',
                                                                            '{param.txtreason_code[i]}',
                                                                            '{param.txtremark[i]}'
                                                                            ";

                                var data_line = await unitOfWork.Transaction.Connection.QueryAsync<FormAddPostionMappingSystem>(query_line, null, unitOfWork.Transaction);
                            }

                        }
                    }
                    else
                    {
                        result = new FormResult { code = "404", message = "ดำเนินการไม่สำเร็จ" };
                        return new JsonResult(result);
                    }

                    unitOfWork.Complete();
                    result = new FormResult { code = "200", message = "แก้ไขข้อมูลสำเร็จ" };
                    return new JsonResult(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
