using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Controller.Api
{
    public class Messages
    { 
        public static Models.Response<Models.NowMessages> GetNow(ulong ts)
        {
            var model = new Models.InData();
            model.Data = ts;
            model.Method = "Messages.GetHow";
            model.Token = "userID";
            var objresp = Connect.Request(model);
            var response = new Models.Response<Models.NowMessages>();
            response.Data = (Models.NowMessages)objresp.Data;
            response.Error = objresp.Error;
            response.Ok = objresp.Ok;

            return response;
        }

        public static Models.Response<ulong> SubscribeOnNewMessages()
        {
            var model = new Models.InData();
            model.Data = null;
            model.Method = "Messages.SubscribeOnNewMessages";
            model.Token = "userID";
            var objresp = Connect.Request(model);
            var response = new Models.Response<ulong>();
            response.Data = (ulong)objresp.Data;
            response.Error = objresp.Error;
            response.Ok = objresp.Ok;

            return response;
        }
    }
}
