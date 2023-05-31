using Dapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ML_UAR.Class;
using ML_UAR.Interfaces;
using ML_UAR.Models;
using ML_UAR.Models.ApproveMaster;
using ML_UAR.Services;
using Newtonsoft.Json;

namespace ML_UAR.Controllers
{
    public class ApproveMasterController : Controller
    {
        private readonly IDatabaseService _databaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SettingsController _settingsController;
        public ApproveMasterController(IDatabaseService databaseService,
                                  IHttpContextAccessor httpContextAccessor,
                                  SettingsController settingsController)
        {
            _databaseService = databaseService;
            _httpContextAccessor = httpContextAccessor;
            _settingsController = settingsController;
        }
        // Page
        public async Task<IActionResult> TransactionApproveMaster()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            ViewBag.ApproveMasterList = await getApproveMasterList();

            return View(_SESSION);
        }
        public async Task<IActionResult> AddApproveMaster()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.PersonnelList = await _settingsController.getPersonnelList();
            ViewBag.ApproveTypeList = await getApproveTypeList();

            return View(_SESSION);
        }
        public async Task<IActionResult> EditDetailApproveMaster(string aprvid)
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            EncodeAndDecode fnBase64 = new EncodeAndDecode();
            var approve_master_id = fnBase64.Decode(aprvid);

            ViewBag.approve_master_id = approve_master_id;

            var approvemaster_detail = await GetAppeoveMasterByID(approve_master_id);

            ViewBag.approve_master_name = approvemaster_detail.approve_master_name;

            ViewBag.status_enable = approvemaster_detail.status == "E" ? "checked" : "";
            ViewBag.status_disable = approvemaster_detail.status == "D" ? "checked" : "";

            ViewBag.PersonnelList = await _settingsController.getPersonnelList();
            ViewBag.ApproveTypeList = await getApproveTypeList();

            ViewBag.ReasonDelList = await _settingsController.getReasonMasterApproveFlow();

            return View(_SESSION);
        }
        public async Task<IActionResult> ViewDetailApproveMaster(string aprvid)
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            EncodeAndDecode fnBase64 = new EncodeAndDecode();
            var approve_master_id = fnBase64.Decode(aprvid);

            ViewBag.approve_master_id = approve_master_id;

            var approvemaster_detail = await GetAppeoveMasterByID(approve_master_id);

            ViewBag.approve_master_name = approvemaster_detail.approve_master_name;
            ViewBag.status_enable = approvemaster_detail.status == "E" ? "checked" : "";
            ViewBag.status_disable = approvemaster_detail.status == "D" ? "checked" : "";

            return View(_SESSION);
        }
        // ddlApproveMaster
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
        public async Task<List<SelectListItem>> getApproveTypeList()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ApproveType>(@$"
                         EXEC [dbo].[ST_ApproveType_GET] 
                     ", null,
                    unitOfWork.Transaction);


                    unitOfWork.Complete();

                    return data.Select(x => new SelectListItem
                    {
                        Value = x.aprvtype_id.ToString(),
                        Text = x.aprvtype_name_th
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
        public async Task<JsonResult> GetApproveMasterTrans(string approve_master_id = "")
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_ApproveMaster_GET] '{approve_master_id}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ApproveMaster>(query, null, unitOfWork.Transaction);

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
        public async Task<JsonResult> FormAddApproveMaster(FormAddApproveMaster param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var actionby = _SESSION.personnel_code;

                    var result = new FormResult();

                    //var PageBack = param.txtPageBack;

                    ////[BL_WL_Customer_Inst]

                    var query_approvemaster = @$"EXEC [dbo].[ST_ApproveMaster_ins] '{param.txtApproveName}','{actionby}'";


                    var ins_approvemaster = await unitOfWork.Transaction.Connection.QueryAsync<FormAddApproveMaster>(query_approvemaster, null, unitOfWork.Transaction);

                    if (ins_approvemaster.Count() != 0)
                    {
                        var approve_master_id = ins_approvemaster.ToList().FirstOrDefault().approve_master_id;

                        for (var i = 0; i <= param.txtPersonnelCode.Count() - 1; i++)
                        {

                            var query_approveFlow = await unitOfWork.Transaction.Connection.QueryAsync<FormAddApproveMaster>($@"
                            EXEC [dbo].[ST_ApproveFlow_ins] '{approve_master_id}',
                                '{param.txtApproveLevel[i]}',
                                '{param.txtPersonnelCode[i]}',
                                '{param.txtApproveTypeID[i]}',
                                '{param.txtBackUPEmail[i]}',
                                '{param.txtStatus[i]}',
                                '{param.txtisFinal[i]}',
                                '{actionby}'
                        ", null, unitOfWork.Transaction);
                        }


                    }
                    else
                    {
                        result = new FormResult { code = "404", message = "ดำเนินการไม่สำเร็จ" };
                        return new JsonResult(result);
                    }



                    unitOfWork.Complete();

                    result = new FormResult { code = "200", message = "เพิ่มข้อมูลสำเร็จ" };
                    return new JsonResult(result);


                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ApproveMaster> GetAppeoveMasterByID(string approve_master_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {

                    var query = $"EXEC [dbo].[ST_ApproveMaster_GET] '{approve_master_id}'";

                    var getdata = await unitOfWork.Transaction.Connection.QueryAsync<ApproveMaster>(query, null, unitOfWork.Transaction);

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
        public async Task<JsonResult> GetApproveFlowTrans(string approve_master_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_ApproveFlow_GET] '{approve_master_id}'";

                    var data = await unitOfWork.Transaction.Connection.QueryAsync<ApproveFlow>(query, null, unitOfWork.Transaction);

                    unitOfWork.Complete();

                    return new JsonResult(_databaseService.FormatOnce(data.ToList()));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApproveFlow> GetApproveFlowById(string approve_master_id, string approve_flow_id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var query = $@"EXEC [dbo].[ST_ApproveFlow_GET] '{approve_master_id}','{approve_flow_id}'";

                    var getdata = await unitOfWork.Transaction.Connection.QueryAsync<ApproveFlow>(query, null, unitOfWork.Transaction);

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

        //Pnan example
        [HttpPost]
        public async Task<JsonResult> FormEditApproveMaster(FormEditApproveMaster param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var actionby = _SESSION.personnel_code;

                    var result = new FormResult();

                    //var PageBack = param.txtPageBack;

                    ////[BL_WL_Customer_Inst]

                    var query_approvemaster = @$"EXEC [dbo].[ST_ApproveMaster_ins] '{param.txtApproveName}','{actionby}'";


                    var ins_approvemaster = await unitOfWork.Transaction.Connection.QueryAsync<FormAddApproveMaster>(query_approvemaster, null, unitOfWork.Transaction);

                    if (ins_approvemaster.Count() != 0)
                    {
                        var approve_master_id = ins_approvemaster.ToList().FirstOrDefault().approve_master_id;

                        for (var i = 0; i <= param.txtPersonnelCode.Count() - 1; i++)
                        {

                            var query_approveFlow = await unitOfWork.Transaction.Connection.QueryAsync<FormAddApproveMaster>($@"
                            EXEC [dbo].[ST_ApproveFlow_ins] '{approve_master_id}',
                                '{param.txtApproveLevel[i]}',
                                '{param.txtPersonnelCode[i]}',
                                '{param.txtApproveTypeID[i]}',
                                '{param.txtBackUPEmail[i]}',
                                '{param.txtStatus[i]}',
                                '{param.txtisFinal[i]}',
                                '{actionby}'
                        ", null, unitOfWork.Transaction);
                        }


                    }
                    else
                    {
                        result = new FormResult { code = "404", message = "ดำเนินการไม่สำเร็จ" };
                        return new JsonResult(result);
                    }



                    unitOfWork.Complete();

                    result = new FormResult { code = "200", message = "เพิ่มข้อมูลสำเร็จ" };
                    return new JsonResult(result);


                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //add new approveflow in page edit
        [HttpPost]
        public async Task<JsonResult> FormAddNewApproveFlow(FormEditApproveMaster param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var action_by = _SESSION.personnel_code;

                    var result = new FormResult();
                    var query_updApproveMaster = $@"EXEC [ST_ApproveMaster_upd] '{param.txtApproveMasterID}',
                                '{param.txtApproveName}',
                                '{param.raiApproveMasterStatus}',
                                '{action_by}'";
                    var upd_approvename = await unitOfWork.Transaction.Connection.QueryAsync<FormEditApproveMaster>(query_updApproveMaster, null, unitOfWork.Transaction);

                    if (param.txtPersonnelCode.Count() != 0)
                    {

                        for (var i = 0; i <= param.txtPersonnelCode.Count() - 1; i++)
                        {
                            //insert 
                            if (param.txtisNewFlow[i] == 1)
                            {
                                var query_ins = $@"EXEC [dbo].[ST_ApproveFlow_ins]
                                '{param.txtApproveMasterID}',
                                '{param.txtApproveLevel[i]}',
                                '{param.txtPersonnelCode[i]}',
                                '{param.txtApproveTypeID[i]}',
                                '{param.txtBackUPEmail[i]}',
                                '{param.txtStatus[i]}',
                                '{param.txtisFinal[i]}',
                                '{action_by}'";

                                var ins_approveflow = await unitOfWork.Transaction.Connection.QueryAsync<FormEditApproveMaster>(query_ins, null, unitOfWork.Transaction);
                            }
                        }
                    }
                    else
                    {
                        result = new FormResult { code = "404", message = "ดำเนินการไม่สำเร็จ" };
                        return new JsonResult(result);
                    }

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

        //update & delete approveflow in page edit
        [HttpPost]
        public async Task<JsonResult> FormEditApproveFlow(FormEditApproveMaster param)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(_databaseService.GetConnection()))
                {
                    var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));
                    var action_by = _SESSION.personnel_code;

                    var result = new FormResult();
                    var query_updApproveMaster = $@"EXEC [ST_ApproveMaster_upd] '{param.txtApproveMasterID}',
                                '{param.txtApproveName}',
                                '{param.raiApproveMasterStatus}',
                                '{action_by}'";
                    var upd_approvename = await unitOfWork.Transaction.Connection.QueryAsync<FormEditApproveMaster>(query_updApproveMaster, null, unitOfWork.Transaction);

                    if (param.txtPersonnelCode.Count() != 0)
                    {

                        for (var i = 0; i <= param.txtPersonnelCode.Count() - 1; i++)
                        {
                             if (param.txtisNewFlow[i] == 0)
                            { 
                                var query_upd = $@"EXEC [dbo].[ST_ApproveFlow_upd]
                                    '{param.txtApproveFlowID[i]}',
                                    '{param.txtisDelete[i]}',
                                    '{action_by}',
                                    '{param.txtApproveLevel[i]}',
                                    '{param.txtPersonnelCode[i]}',
                                    '{param.txtApproveTypeID[i]}',
                                    '{param.txtBackUPEmail[i]}',
                                    '{param.txtStatus[i]}',
                                    '{param.txtReasonCode[i]}',
                                    '{param.txtRemark[i]}',
                                    '{param.txtisFinal[i]}'";
                                var upd_approveflow = await unitOfWork.Transaction.Connection.QueryAsync<FormEditApproveMaster>(query_upd, null, unitOfWork.Transaction);
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
