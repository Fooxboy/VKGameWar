using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VKGame.Bot.PublicAPI.Models;
using VKGame.Bot.PublicAPI;
using VKGame.WebAPI.Areas.publicAPI.Models;


namespace VKGame.WebAPI.Areas.publicAPI.Controllers
{
    [Produces("application/json")]
    [Area("publicAPI")]
    public class DamageController : Controller
    {
        public IActionResult Soildery(long value)
        {
            try
            {
                if(value ==0)
                {
                    var result = Bot.PublicAPI.Yarik.Damage.Soildery;
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
                else
                {
                    Bot.PublicAPI.Yarik.Damage.Soildery = value;
                    var model = new RootResponse() { result = true };
                    model.data = true;
                    return Json(model);
                }
            }
            catch (Exception e)
            {
                var model = new RootResponse<Models.Error>()
                {
                    result = false,
                    data = new Models.Error()
                    {
                        Code = 10,
                        Message = $"Внутренняя ошибка сервера." +
                        $"\n {e.ToString()}"
                    }
                };
                return Json(model);
            }
        }

        public IActionResult SoilderyLevel(long value)
        {
            try
            {
                if (value == 0)
                {
                    var result = Bot.PublicAPI.Yarik.Damage.SoilderyLevel;
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
                else
                {
                    Bot.PublicAPI.Yarik.Damage.SoilderyLevel = value;
                    var model = new RootResponse() { result = true };
                    model.data = true;
                    return Json(model);
                }
            }
            catch (Exception e)
            {
                var model = new RootResponse<Models.Error>()
                {
                    result = false,
                    data = new Models.Error()
                    {
                        Code = 10,
                        Message = $"Внутренняя ошибка сервера." +
                        $"\n {e.ToString()}"
                    }
                };
                return Json(model);
            }
        }
    }
}