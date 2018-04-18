using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VKGame.WebAPI.Areas.publicAPI.Controllers
{
    [Produces("application/json")]
    [Area("publicAPI")]
    public class BattleController : Controller
    {
        public IActionResult start(int id, int token)
        {
            var data = new Models.RootResponse<string>
            {
                result = true,
                data = $"Вы передали: ID = {id} TOKEN = {token}",
            };
            return Json(data);
        }
    }
}
