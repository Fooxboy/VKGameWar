using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VKGame.WebAPI.Areas.publicAPI.Models;
using VKGame.Bot.PublicAPI.Models;
using VKGame.Bot.PublicAPI;

namespace VKGame.WebAPI.Areas.publicAPI.Controllers
{
    [Produces("application/json")]
    [Area("publicAPI")]
    public class BarracksController : Controller
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

        public IActionResult count(long user, int army)
        {
            try
            {
                if (user == 0 || army == 0)
                {
                    return Json(new RootResponse<IError>()
                    {
                        result = false,
                        data = new Models.Error()
                        {
                            Code = 13,
                            Message = "Не указан обязательный параметр."
                        }
                    });
                }

                var result = Bot.PublicAPI.Yarik.Barracks.GetCount(user, army);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);
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

        public IActionResult setCount(long user, int army, int count)
        {
            try
            {
                if (user == 0 || army == 0 || count == 0)
                {
                    return Json(new RootResponse<IError>()
                    {
                        result = false,
                        data = new Models.Error()
                        {
                            Code = 13,
                            Message = "Не указан обязательный параметр."
                        }
                    });
                }

                var result = Bot.PublicAPI.Yarik.Barracks.SetCount(user, army, count);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);

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