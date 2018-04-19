using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKGame.WebAPI.Areas.publicAPI.Models
{
    public class RootResponse<T>
    {
        public bool result { get; set; }
        public T data { get; set; }
    }

    public class RootResponse
    {
        public bool result { get; set; }
        public object data { get; set; }
    }
}
