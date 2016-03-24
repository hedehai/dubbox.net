using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiRegister.Controllers
{
    [DubboxService(ServiceName = "Api2")]
    public class Api2Controller : ApiController
    {

        // GET: api/Api2

        [DubboxMethod(MethodName = "getValue")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // GET: api/Api2/5
        [DubboxMethod(MethodName = "getValue2")]
        public string Get(int id)
        {
            return "value";
        }



        // POST: api/Api2
        public void Post([FromBody]string value)
        {
        }



        // PUT: api/Api2/5
        public void Put(int id, [FromBody]string value)
        {
        }



        // DELETE: api/Api2/5
        public void Delete(int id)
        {
        }


    }
}
