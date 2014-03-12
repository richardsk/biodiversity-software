using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TaxonBlaster;

namespace TaxonBlasterRest.Controllers
{
    public class BlasterController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]string value)
        {
            //either url or gene sequences
            string rid = Blaster.SubmitTaxonMatches(new List<string>{value});
            var response = Request.CreateResponse(HttpStatusCode.OK, rid);
            return response;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        public string Poll(string rid)
        {
            return "done";
        }
    }
}