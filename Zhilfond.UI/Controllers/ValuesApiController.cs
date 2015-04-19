using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class ValuesApiController : ApiController
    {
        static readonly IValuesRepository _repository = new ValuesRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IKeysRepository _keys_repository = new KeysRepository();

        // GET api/valuesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                string form = (from x in Request.GetQueryNameValuePairs()
                               where x.Key == "form"
                               select x.Value).FirstOrDefault();

                string id_house = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "id_house"
                                   select x.Value).FirstOrDefault();

                var dnames = _repository.GetDisplayNames(form, int.Parse(id_house));
               
                int n = 0;
                var res = new
                {
                    total = 1,
                    page = 1,
                    records = dnames.Count(),
                    rows = (
                        from dname in dnames
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                dname.Id.ToString(),
                                dname.Value
                            }
                        }).ToArray()
                };
                return res;
            }
            catch (Exception )
            {
                return new List<string>();
            }
        }

        // GET api/valuesapi
        public dynamic Get()
        {
            try
            {
                string form = (from x in Request.GetQueryNameValuePairs()
                                  where x.Key == "form"
                                  select x.Value).FirstOrDefault();

                string id_house = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "id_house"
                                  select x.Value).FirstOrDefault();

                string id_parent = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "id_parent"
                                   select x.Value).FirstOrDefault();

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();
                
                UserV u = _users_repository.GetVByToken(token);

                return _repository.Get(form, int.Parse(id_house), int.Parse(id_parent), u);
         
            }
            catch (Exception )
            {
                return new List<ZValueV>();
            }
        }

        // GET api/valuesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/valuesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string token = json.Token;
                string xml = json.Xml;
                string signature = json.Signature;
                string sourceType = json.SourceType;
                string id_source = json.Id_source;

                // Validate signature
                var key = _keys_repository.GetByToken(token);
            /*    string keyval = @"-----BEGIN CERTIFICATE-----
                                MIIBaDCCARKgAwIBAgIIVEOkVVDKXjMwDQYJKoZIhvcNAQELBQAwOTELMAkGA1UE
                                BhMCQVUxETAPBgNVBAoMCFpoaWxmb25kMRcwBwYDVQQLDAAwDAYDVQQHDAVUb21z
                                azAeFw0xNDAxMjcwMDAwMDBaFw0xNjAxMjcwMDAwMDBaMDkxCzAJBgNVBAYTAkFV
                                MREwDwYDVQQKDAhaaGlsZm9uZDEXMAcGA1UECwwAMAwGA1UEBwwFVG9tc2swXDAN
                                BgkqhkiG9w0BAQEFAANLADBIAkEAq3ANRvBFfYCa4/Wwi8IqnMDQi9dOycwDGERT
                                WIwbJhq2qgmp6Qn5JrlqYWVM2jvwISlp9J15c29GclguaHpcNwIDAQABMA0GCSqG
                                SIb3DQEBCwUAA0EAAFDmAcG2In0Csi1Kd2oFZ3j2ULvdInVIBDmVj/BWuUcQ5RWA
                                KIOyYVJMQNS4wE8/HnwUEEdWyU+VdpWOiCCaig==
                                -----END CERTIFICATE-----";*/

                User u = _users_repository.GetByToken(token);
                _repository.Process(u.Id, sourceType, id_source, xml, signature, key.KeyValue);
                
                var response = Request.CreateResponse<ZValue>(HttpStatusCode.OK, null);
                string uri = Url.Link("DefaultApi", new { id = -1});
                response.Headers.Location = new Uri(uri);

                return response;
            }
            catch (Exception ex)
            {
                string text = ex.Message;
                var response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, text);
                string uri = Url.Link("DefaultApi", new { id = -1 });
                response.Headers.Location = new Uri(uri);
                return response;
            }
        }


        // POST api/valuesapi
      /*  public HttpResponseMessage Put(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string token = json.Token;
                string xml = json.Xml;
                string signature = json.Signature;

                // Validate signature
                var pk = _keys_repository.Get(token);
                _repository.Update(xml, signature, pk.Data);

                var response = Request.CreateResponse<ZValue>(HttpStatusCode.OK, null);
                string uri = Url.Link("DefaultApi", new { id = -1 });
                response.Headers.Location = new Uri(uri);

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }*/
        // DELETE api/valuesapi/5
        public void Delete(int id)
        {
        }
    }
}