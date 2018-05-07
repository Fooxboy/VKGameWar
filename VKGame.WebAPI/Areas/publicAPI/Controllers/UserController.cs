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
    public class UserController : Controller
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


        public IActionResult delete(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.Delete(id);
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

        public IActionResult get(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.GetObject(id);
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

        public IActionResult money(long id, int value)
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

                if(value == 0)
                {
                    var result = Bot.PublicAPI.Yarik.Users.Money(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }else
                {
                    var result = Bot.PublicAPI.Yarik.Users.SetMoney(id, value);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
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

        public IActionResult unitLvlUp(long id, int type)
        {
            try
            {
                if (id == 0 || type == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.upLevelUnit(id, type);
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

        public IActionResult unitOpen(long id, int type)
        {
            try
            {
                if (id == 0 || type == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.openUnit(id, type);
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

        public IActionResult unitFromType(long id, int type)
        {
            try
            {
                if (id == 0 || type == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.unitForType(id, type);
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

        public IActionResult units(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.GetArmy(id);
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

        public IActionResult editUnitLvl(long id, int type, int lvl)
        {
            try
            {
                if (id == 0 || type == 0 || lvl == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.EditUnit(id, type, lvl);
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

        public IActionResult editUnitStatus(long id, int type, bool status)
        {
            try
            {
                if (id == 0 || type == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.EditStatusUnit(id, type, status);
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

        public IActionResult battle(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.GetBattleId(id);
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


        public IActionResult outMoney(long id, int count)
        {
            try
            {
                if (id == 0 || count == 0)
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

                var result = Bot.PublicAPI.Yarik.Users.OutMoney(id, count);
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


        public IActionResult protection(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.GetProtection(id);
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

        public IActionResult attempts(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.GetAttempts(id);
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

        public IActionResult check(long id)
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

                var result = Bot.PublicAPI.Yarik.Users.Check(id);
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