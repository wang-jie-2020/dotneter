using SomeProject.Core.Repository.Account.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SomeProject.WebApi.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            var repo = new UserRepository();
            var user = repo.Query(71);
            user.Email = "get";
            repo.Update(user, false);

            System.Threading.Thread.Sleep(10000);

            return Ok("结束");
        }

        public IHttpActionResult Post()
        {
            var repo = new UserRepository();
            var user = repo.Query(71);
            user.NickName = "post";
            repo.Update(user);

            return Ok("结束");
        }
    }
}
