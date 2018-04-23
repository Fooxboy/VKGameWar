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
    public class ArmyController : Controller
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

        public IActionResult check(long id)
        {
            try
            {

                if (id== 0)
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

                var result = Bot.PublicAPI.Yarik.Army.Check(id);
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
                    data = new WebAPI.Areas.publicAPI.Models.Error()
                    {
                        Code = 10,
                        Message = $"Внутренняя ошибка сервера." +
                        $"\n {e.ToString()}"
                    }
                };
                return Json(model);
            }
        }

        public IActionResult count(long id)
        {
            try
            {

                if (id == 0)
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

                var result = Bot.PublicAPI.Yarik.Army.GetCount(id);
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
                    data = new WebAPI.Areas.publicAPI.Models.Error()
                    {
                        Code = 10,
                        Message = $"Внутренняя ошибка сервера." +
                        $"\n {e.ToString()}"
                    }
                };
                return Json(model);
            }
        }

        public IActionResult countEdit(long id, int type, int value)
        {
            try
            {
                if (type == 0 || value == 0)
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
               
                var result = Bot.PublicAPI.Yarik.Army.EditCount(id, type, value);
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
                    data = new WebAPI.Areas.publicAPI.Models.Error()
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