using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;

namespace UI.Controllers
{
    public class OperationsApiController : ApiController
    {
        static readonly IOperationsRepository _repository = new OperationsRepository();

        // GET api/operationsapi
        public IEnumerable<RuleOperation> Get()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET api/operationsapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/operationsapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/operationsapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/operationsapi/5
        public void Delete(int id)
        {
        }
    }
}
