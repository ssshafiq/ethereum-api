using EthereumAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EthereumAPI.Controllers
{
    public class AccountsController : ApiController
    {

        // GET api/values
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(string senderAddress)
        {
            Gatekeeper obj = new Gatekeeper();
            var res = obj.GetBalance(senderAddress);
            return res;
        }


        [HttpPost]
        [Route("transfer")]
        // POST api/values
        public string Transfer(string receiverAddress, string amount)
        {

            Gatekeeper obj = new Gatekeeper();
            var res = obj.Transfer(receiverAddress, amount);
            return res;

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
