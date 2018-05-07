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
    public class ClanController : Controller
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


        public IActionResult get(string id)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                var result = Bot.PublicAPI.Yarik.Clans.GetObject(id);
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

        public IActionResult delete(string id)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                var result = Bot.PublicAPI.Yarik.Clans.Delete(id);
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

        public IActionResult endBattle(string id)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                var result = Bot.PublicAPI.Yarik.Clans.EndBattle(id);
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


        public IActionResult treasury(string id, long count)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                if (count == 0)
                {
                    var result = Bot.PublicAPI.Yarik.Clans.GetFound(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
                else
                {
                    var result = Bot.PublicAPI.Yarik.Clans.SetFound(id, count);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
            }catch(Exception e)
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

        public IActionResult battleId(string id, string battle = null)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                if (battle is null)
                {
                    var result = Bot.PublicAPI.Yarik.Clans.GetBattleId(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
                else
                {
                    var result = Bot.PublicAPI.Yarik.Clans.SetBattleId(id, battle);
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

        public IActionResult members(string id)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                var result = Bot.PublicAPI.Yarik.Clans.GetMembers(id);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);
            }
            catch(Exception e)
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

        public IActionResult addMember(string id, long user)
        {
            try
            {

                if (id is null || id == string.Empty || user == 0)
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

                var result = Bot.PublicAPI.Yarik.Clans.AddMember(id, user);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);
            }
            catch(Exception e)
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

        public IActionResult removeMember(string id, long user)
        {
            try
            {

                if (id is null || id == string.Empty || user == 0)
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

                var result = Bot.PublicAPI.Yarik.Clans.Remove(id, user);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);
            }
            catch(Exception e)
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

        public IActionResult check(string id)
        {
            try
            {
                if (id is null || id == string.Empty)
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

                var result = Bot.PublicAPI.Yarik.Clans.Check(id);
                var model = new RootResponse() { result = true };
                if (result is IError) model.result = false;
                model.data = result;
                return Json(model);
            }
            catch(Exception e)
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

        public IActionResult level(string id, int value = 0)
        {
            try
            {

                if (id is null || id == string.Empty)
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

                if (value ==0)
                {
                    var result = Bot.PublicAPI.Yarik.Clans.Level(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }else
                {
                    var result = Bot.PublicAPI.Yarik.Clans.SetLevel(id, value);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }
            }catch(Exception e)
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

        public IActionResult isSearchBattle(string id, bool status=false)
        {

            if (id is null || id == string.Empty)
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

            try
            {
                return Json(new RootResponse() { result = false, data = new Models.Error() { Code = 8, Message = "Этот сервесный метод временно недоступен." } });
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

        public IActionResult countMembers(string id, int value=0)
        {

            if (id is null || id == string.Empty)
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

            try
            {
                if(value == 0)
                {
                    var result = Bot.PublicAPI.Yarik.Clans.GetCountMembers(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }else
                {
                    var result = Bot.PublicAPI.Yarik.Clans.SetCountMembers(id, value);
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

        public IActionResult isStartBattle(string id, bool status= false)
        {
            try
            {
                return Json(new RootResponse() { result = false, data = new Models.Error() { Code = 8, Message = "Этот сервесный метод временно недоступен." } });

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

        public IActionResult name(string id, string value = null)
        {
            try
            {
                if (id is null || id == string.Empty)
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

                if (value is null)
                {
                    var result = Bot.PublicAPI.Yarik.Clans.GetName(id);
                    var model = new RootResponse() { result = true };
                    if (result is IError) model.result = false;
                    model.data = result;
                    return Json(model);
                }else
                {
                    var result = Bot.PublicAPI.Yarik.Clans.GetName(id);
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