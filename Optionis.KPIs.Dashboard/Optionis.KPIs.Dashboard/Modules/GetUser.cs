﻿using Nancy;
using Optionis.KPIs.Dashboard.Application;
using Optionis.KPIs.Dashboard.Modules.Routes;
using Optionis.KPIs.DataAccess;

namespace Optionis.KPIs.Dashboard.Modules
{
    public class GetUser : NancyModule
    {
        public GetUser ()
        {
            Get [Routing.Users.GET] = _ => {
                Response response = null;
                new GetUserService (
                    new UserRetriever (),
                    () => response = HttpStatusCode.NotFound,
                    user => {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(Convert (user));
                        response.StatusCode = HttpStatusCode.OK;
                        response.ContentType = "application/json";
                    }
                ).Get (_.Id);
                return response;
            };
        }

        static dynamic Convert(GetUserService.User user)
        {
            return new {
                links = new {
                    self = Routing.Users.Get (user.Id)
                },
                user.UserName,
                user.Created
            };
        }
    }
}

