using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VKGame.WebAPI.Areas.publicAPI.Models;

namespace VKGame.WebAPI.Areas.publicAPI
{
    public class Helpers
    {
        public IActionResult Send(object result)
        {
            var model = new RootResponse() { result = true };
            if (result is Bot.PublicAPI.Models.IError) model.result = false;
            model.data = result;
            //return Json(model);
            return null;
        }
    }
}
