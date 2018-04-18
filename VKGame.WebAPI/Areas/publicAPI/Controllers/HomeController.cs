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
    public class HomeController : Controller
    {
        public string Index()
        {
            return "NoneAccesp";
        }
    }
}
