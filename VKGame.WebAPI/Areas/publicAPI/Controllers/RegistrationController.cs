using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VKGame.WebAPI.Areas.publicAPI.Models;


namespace VKGame.WebAPI.Areas.publicAPI.Controllers
{
    [Produces("application/json")]
    [Area("publicAPI")]
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            var model = new Models.RootResponse<Models.Error>()
            {
                result = false,
                data = new Models.Error()
                {
                    Code = 404,
                    Message = "Не найдена запрошеная страница."
                }
            };
            return Json(model);
        }

        public IActionResult army(long id)
        {
            var reg = new Bot.PublicAPI.Yarik.Registration();
            var result = reg.Army(id);
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }
    }
}