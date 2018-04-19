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
    public class ArmyController : Controller
    {
        public IActionResult Index()
        {
            var model = new Models.RootResponse<Models.Error>()
            {
                result = false,
                data = new Error()
                {
                    Code = 404,
                    Message = "Не найдена запрошеная страница."
                }
            };
            return Json(model);
        }

        public IActionResult check(long id)
        {
            var result = Bot.PublicAPI.Yarik.Army.Check(id);

            var model = new RootResponse()
            {
                result = true,
                data = result
            };
            return Json(model);
        }

        public IActionResult edit_count(long id, int type, int count)
        {
            var result = Bot.PublicAPI.Yarik.Army.EditCount(id, type, count);
            var model = new RootResponse() { result = true};
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }

        public IActionResult edit_level(long id, int type, int lvl)
        {
            var result = Bot.PublicAPI.Yarik.Army.EditLevel(id, type, lvl);
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }

        public IActionResult current(long id)
        {
            var result = Bot.PublicAPI.Yarik.Army.Current(id);
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }

        public IActionResult levels(long id)
        {
            var result = Bot.PublicAPI.Yarik.Army.Levels(id);
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }

        public IActionResult count(long id)
        {
            var result = Bot.PublicAPI.Yarik.Army.Count(id);
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            return Json(model);
        }
    }
}