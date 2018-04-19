using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VKGame.WebAPI.Areas.privateAPI.Controllers
{
    [Produces("application/json")]
    [Area("privateAPI")]
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return Json(new Models.ResultStatus() { Status = true });
        }
    }
}
