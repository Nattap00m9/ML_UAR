using Microsoft.AspNetCore.Mvc;
using ML_UAR.Class;
using ML_UAR.Interfaces;
using ML_UAR.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ML_UAR.Controllers
{
    public class HomeController : Controller
    {

        private IDatabaseService _databaseService;
        private SessionController _sessionController;

        public HomeController(
                    IDatabaseService databaseService, 
                    SessionController sessionController
                )
        {
            _databaseService = databaseService;
            _sessionController = sessionController;
        }
        public async Task<IActionResult> Permission(string user = "MTkwMDAx")
        {
            EncodeAndDecode encodeAndDecode = new EncodeAndDecode();
            string user_decode = encodeAndDecode.Decode(user);

            var getDetail = await _sessionController.GetPersonnalRole(user_decode);

            if (getDetail.Count() == 0)
            {
                //Set Session
                var personnelDeatail = new SESSION()
                {
                    personnel_code = ""
                };

                HttpContext.Session.SetString("_SESSION", JsonConvert.SerializeObject(personnelDeatail));

                return RedirectToAction("Index");

            }
            else
            {
                //var detail = new SESSION();
                var detail = getDetail.FirstOrDefault();

                //Set Session
                var personnelDeatail = new SESSION()
                {
                    system_id = detail.system_id,
                    role_id = detail.role_id,
                    personnel_code = detail.personnel_code,
                    thname = detail.thname,
                    personnel_name_th = detail.personnel_name_th,
                    personnel_last_th = detail.personnel_last_th,
                    enname = detail.enname,
                    personnel_name_en = detail.personnel_name_en,
                    personnel_last_en = detail.personnel_last_en,
                    email = detail.email,
                    branch_code = detail.branch_code,
                    branch_name = detail.branch_name,
                    position_code = detail.position_code,
                    position_name = detail.position_name,
                    section_code = detail.section_code,
                    section = detail.section
                };
                HttpContext.Session.SetString("_SESSION", JsonConvert.SerializeObject(personnelDeatail));

                return RedirectToAction("Index");
            }


        }
        public IActionResult Index()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            return View(_SESSION);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}