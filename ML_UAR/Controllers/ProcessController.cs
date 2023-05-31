using Microsoft.AspNetCore.Mvc;
using ML_UAR.Models;
using Newtonsoft.Json;

namespace ML_UAR.Controllers
{
    public class ProcessController : Controller
    {
        public IActionResult RequestList()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            return View(_SESSION);
        }
        public IActionResult Todolist()
        {
            var _SESSION = JsonConvert.DeserializeObject<SESSION>(HttpContext.Session.GetString("_SESSION"));

            ViewBag.User = _SESSION.personnel_code;

            return View(_SESSION);
        }
    }
}
