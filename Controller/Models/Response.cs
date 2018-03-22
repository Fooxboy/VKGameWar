using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Models
{
    public class Response<T>
    {
        public bool Ok { get; set; }
        public T Data { get; set; }
        public Error Error { get; set; }
    }
}
