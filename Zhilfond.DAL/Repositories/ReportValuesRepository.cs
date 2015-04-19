using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Security.Cryptography;
using System.Xml.Linq;
using DAL.Classes;
using System.Dynamic;
using System.Data.Objects.SqlClient;


namespace DAL.Repositories
{
    public class ReportValuesRepository : IReportValuesRepository
    {
        public IEnumerable<ReportValue> Get(int id_group, int id_template)
        {
            try
            {
                using (var ctx = new UserContext())
                {

                    var result = new List<ReportValue>();
                    foreach (var r in (from gh in ctx.GroupHouses
                                       join h in ctx.HousesV on gh.Id_house equals h.id
                                       join rp in ctx.ReportParams on /*gh.Id_group*/id_template equals rp.Id_group
                                       join f in ctx.Forms on rp.Id_form equals f.Id
                                       join fl in ctx.Fields on new { Id_field = rp.Id_field, Id_form = rp.Id_form }
                                                         equals new { Id_field = fl.Id, Id_form = fl.Id_form }
                                       join v in ctx.DValues on new { Id_house = gh.Id_house, Id_form = rp.Id_form, Id_field = rp.Id_field }
                                                        equals new { Id_house = v.Id_house, Id_form = v.Id_form, Id_field = v.Id_field } into outer
                                       from v0 in outer.DefaultIfEmpty()                                       
                                       where rp.Chosen == true && gh.Id_group == id_group
                                       orderby  gh.Id_house, f.Order, fl.Order, v0.Id_parent
                                       select new ReportValue
                                       {
                                           Id_house = gh.Id_house,
                                           Id_form = rp.Id_form,
                                           Form = f.Title,
                                           Id_field = rp.Id_field,
                                           Field = fl.Title,
                                           Value = (v0.Value == null) ? "" : v0.Value,
                                           Id_parent = v0.Id_parent
                                       }))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
