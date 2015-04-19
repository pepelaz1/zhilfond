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
using System.Xml;
using System.IO;


namespace DAL.Repositories
{
    public class XmlRepository : IXmlRepository
    {
        public string Generate(int id_group)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    var declaration = new XDeclaration("1.0", "utf-8", null);
                    XElement root = new XElement("ImportData");
                    var doc = new XDocument(declaration, root);
                    doc.Add(new XComment(Guid.NewGuid().ToString()));

                    XElement elem = null;
                    List<HouseV> houses = (from h in ctx.HousesV
                                           join gh in ctx.GroupHouses on h.id equals gh.Id_house
                                           where gh.Id_group == id_group
                                           select h).ToList();

                    foreach (var rec in houses)
                    {                      
                        int curf = -1;
                        int? curp = -1;
                        foreach (var data in (from f in ctx.Forms
                                              join fl in ctx.Fields on f.Id equals fl.Id_form
                                              join d in ctx.Dicts on fl.Id_dict equals d.Id
                                              join v in ctx.DValues on
                                                 new { Id_form = fl.Id_form, Id_field = fl.Id, Id_house = rec.id }
                                                  equals
                                                  new { Id_form = v.Id_form, Id_field = v.Id_field, Id_house = v.Id_house } into outer
                                              from v0 in outer.DefaultIfEmpty()
                                              orderby f.Order, v0.Id_parent, fl.Order
                                              select new
                                              {
                                                  Id_form = f.Id,
                                                  Form = f.Title,
                                                  Field = fl.Title,
                                                  Type = d.Title,
                                                  Required = fl.Required,
                                                  Value = (v0.Value == null ? "" : v0.Value),
                                                  Id_parent = (v0.Id_parent == null ? 0 : v0.Id_parent)
                                              }))
                        {
                            if (curf != data.Id_form || curp != data.Id_parent)
                            {
                                curf = data.Id_form;
                                curp = data.Id_parent;
                                
                                if (elem != null)
                                    root.Add(elem);

                                elem = new XElement("Element");
                                elem.Add( new XElement("House",  new XAttribute("nasp", rec.nasp == null ? "" : rec.nasp),
                                    new XAttribute("district", rec.raion == null ? "" : rec.raion), new XAttribute("street", rec.street),
                                    new XAttribute("number", rec.number), new XAttribute("letter", rec.letter == null ? "" : rec.letter)));

                                //XElement form = new XElement("Form", new XAttribute("operation", "Insert"));
                                XElement form = new XElement("Form");
                                if ( data.Form != "Основные данные")
                                    form.Add(new XAttribute("parent",data.Value ));
                                form.Value = data.Form;
                                elem.Add(form);
                            }
                            XElement field = new XElement("Field", new XAttribute("name", data.Field), new XAttribute("type", data.Type), new XAttribute("required", data.Required)
                                );
                            field.Value = data.Value == null ? "" : data.Value;
                            elem.Add(field);
                        }

                        if (elem != null)
                            root.Add(elem);
                    }
                    using (var sw = new Utf8StringWriter())
                    {
                        using (var xw = XmlWriter.Create(sw))
                        {
                            doc.WriteTo(xw);
                            xw.Flush();
                            return sw.GetStringBuilder().ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
