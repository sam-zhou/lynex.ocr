using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lynex.BillMaster.Service.Interface;

namespace Lynex.BillMaster.Api.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly IUserService _userService;

        public ValuesController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        // GET api/values/5
        public string Get(string id)
        {
            return _userService.GetUser(id).FirstName;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }


    }
}
