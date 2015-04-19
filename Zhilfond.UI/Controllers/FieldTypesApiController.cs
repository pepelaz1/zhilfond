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
    public class FieldTypesApiController : ApiController
    {
        static readonly IFieldTypesRepository _repository = new FieldTypesRepository();

        // GET api/fieldtypesapi
        public IEnumerable<FieldType> Get()
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


        // GET api/fieldtypesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/fieldtypesapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/fieldtypesapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/fieldtypesapi/5
        public void Delete(int id)
        {
        }
    }
}
