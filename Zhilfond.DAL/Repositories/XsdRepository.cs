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
using System.Xml.Schema;


namespace DAL.Repositories
{
    class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }

    public class XsdRepository : IXsdRepository
    {
        public string Generate()
        {
            try
            {
                using (var ctx = new UserContext())
                {

                    // <ImportData>
                    XmlSchemaElement importDataElement = new XmlSchemaElement();
                    importDataElement.Name = "ImportData";

                    //<xs:complexType>
                    XmlSchemaComplexType importDataComplexType = new XmlSchemaComplexType();
                    importDataElement.SchemaType = importDataComplexType;

                    //<xs:choice minOccurs="0" maxOccurs="unbounded">
                    XmlSchemaChoice choice = new XmlSchemaChoice();
                    choice.MaxOccursString = "unbounded";
                    choice.MinOccurs = 0;

                    //<xs:element name="Element">
                    XmlSchemaElement elementElement = new XmlSchemaElement();
                    elementElement.Name = "Element";
                    choice.Items.Add(elementElement);

                    //<xs:complexType>
                    XmlSchemaComplexType elementComplexType = new XmlSchemaComplexType();
                    elementElement.SchemaType = elementComplexType;

                    //<xs:sequence>
                    XmlSchemaSequence sequence = new XmlSchemaSequence();

                    //<xs:element name="House" minOccurs="0" maxOccurs="unbounded">
                    XmlSchemaElement houseElement = new XmlSchemaElement();
                    houseElement.Name = "House";
                    houseElement.MinOccurs = 1;
                    houseElement.MaxOccurs = 1;

                    //<xs:complexType>
                    XmlSchemaComplexType houseComplexType = new XmlSchemaComplexType();
                    houseElement.SchemaType = houseComplexType;

                    // Create House's attribute
                    XmlSchemaAttribute naspAttribute = new XmlSchemaAttribute();
                    naspAttribute.Name = "nasp";
                    naspAttribute.Use = XmlSchemaUse.Required;
                    naspAttribute.SchemaTypeName = new XmlQualifiedName("NaspType", "http://www.w3.org/2001/XMLSchema");
                    houseComplexType.Attributes.Add(naspAttribute);

                    // Create the Dictionary type for the nasp attribute.
                    XmlSchemaSimpleType naspType = new XmlSchemaSimpleType();
                    naspType.Name = "NaspType";
                    XmlSchemaSimpleTypeRestriction naspRestriction = new XmlSchemaSimpleTypeRestriction();
                    naspRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in ( from d in ctx.Dicts
                                             join dv in ctx.DictsValues on d.Id equals dv.Id_dict
                                             where d.Title == "Населенные пункты"
                                             select dv))
                    {
                        XmlSchemaEnumerationFacet enumerationNasp = new XmlSchemaEnumerationFacet();
                        enumerationNasp.Value = rec.Value;
                        naspRestriction.Facets.Add(enumerationNasp);
                    }
                    naspType.Content = naspRestriction;

                    XmlSchemaAttribute districtAttribute = new XmlSchemaAttribute();
                    districtAttribute.Name = "district";
                    districtAttribute.Use = XmlSchemaUse.Required;
                    districtAttribute.SchemaTypeName = new XmlQualifiedName("DistrictType", "http://www.w3.org/2001/XMLSchema");
                    houseComplexType.Attributes.Add(districtAttribute);

                    // Create the Dictionary type for the district attribute.
                    XmlSchemaSimpleType districtType = new XmlSchemaSimpleType();
                    districtType.Name = "DistrictType";
                    XmlSchemaSimpleTypeRestriction districtTypeRestriction = new XmlSchemaSimpleTypeRestriction();
                    districtTypeRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in (from d in ctx.Dicts
                                         join dv in ctx.DictsValues on d.Id equals dv.Id_dict
                                         where d.Title == "Районы"
                                         select dv))
                    {
                        XmlSchemaEnumerationFacet enumerationDistrict = new XmlSchemaEnumerationFacet();
                        enumerationDistrict.Value = rec.Value;
                        districtTypeRestriction.Facets.Add(enumerationDistrict);
                    }
                    districtType.Content = districtTypeRestriction;

                    XmlSchemaAttribute streetAttribute = new XmlSchemaAttribute();
                    streetAttribute.Name = "street";
                    streetAttribute.Use = XmlSchemaUse.Required;
                    streetAttribute.SchemaTypeName = new XmlQualifiedName("StreetType", "http://www.w3.org/2001/XMLSchema");
                    houseComplexType.Attributes.Add(streetAttribute);

                    // Create the Dictionary type for the street attribute.
                    XmlSchemaSimpleType streetType = new XmlSchemaSimpleType();
                    streetType.Name = "StreetType";
                    XmlSchemaSimpleTypeRestriction streetRestriction = new XmlSchemaSimpleTypeRestriction();
                    streetRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in (from d in ctx.Dicts
                                         join dv in ctx.DictsValues on d.Id equals dv.Id_dict
                                         where d.Title == "Улицы"
                                         select dv))
                    {
                        XmlSchemaEnumerationFacet enumerationStreet = new XmlSchemaEnumerationFacet();
                        enumerationStreet.Value = rec.Value;
                        streetRestriction.Facets.Add(enumerationStreet);
                    }
                    streetType.Content = streetRestriction;

                    XmlSchemaAttribute numberAttribute = new XmlSchemaAttribute();
                    numberAttribute.Name = "number";
                    numberAttribute.Use = XmlSchemaUse.Required;
                    numberAttribute.SchemaTypeName = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
                    houseComplexType.Attributes.Add(numberAttribute);

                    XmlSchemaAttribute letterAttribute = new XmlSchemaAttribute();
                    letterAttribute.Name = "letter";
                    letterAttribute.Use = XmlSchemaUse.Optional;
                    letterAttribute.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    houseComplexType.Attributes.Add(letterAttribute);

                    //<xs:element name="Form" nillable="true" minOccurs="0" maxOccurs="unbounded">
                    XmlSchemaElement formElement = new XmlSchemaElement();
                    formElement.Name = "Form";
                    formElement.SchemaTypeName = new XmlQualifiedName("FormType", "http://www.w3.org/2001/XMLSchema");
                    formElement.MinOccurs = 1;
                    formElement.MaxOccurs = 1;

                    // Create the Dictionary type for the nasp attribute.
                    XmlSchemaSimpleType formType = new XmlSchemaSimpleType();
                    formType.Name = "FormType";
                    XmlSchemaSimpleTypeRestriction formRestriction = new XmlSchemaSimpleTypeRestriction();
                    formRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in (from f in ctx.Forms
                                         select f))
                    {
                        XmlSchemaEnumerationFacet enumerationForm = new XmlSchemaEnumerationFacet();
                        enumerationForm.Value = rec.Title;
                        formRestriction.Facets.Add(enumerationForm);
                    }
                    formType.Content = formRestriction;

                    //<xs:complexType>
                    XmlSchemaComplexType formComplexType = new XmlSchemaComplexType();
                    formElement.SchemaType = formComplexType;

                    //<xs:simpleContent>
                    XmlSchemaSimpleContent formSimpleContent = new XmlSchemaSimpleContent();

                    //<xs:extension base="xs:decimal">
                    XmlSchemaSimpleContentExtension formSimpleContent_extension = new XmlSchemaSimpleContentExtension();
                    formSimpleContent_extension.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    formSimpleContent.Content = formSimpleContent_extension;

                    // Create Form's attribute
                    XmlSchemaAttribute parentAttribute = new XmlSchemaAttribute();
                    parentAttribute.Name = "parent";
                    parentAttribute.Use = XmlSchemaUse.Optional;
                    parentAttribute.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    formSimpleContent_extension.Attributes.Add(parentAttribute);
                    formComplexType.ContentModel = formSimpleContent;

                    //<xs:element name="Field" nillable="true" minOccurs="0" maxOccurs="unbounded">
                    XmlSchemaElement fieldElement = new XmlSchemaElement();
                    fieldElement.Name = "Field";
                    fieldElement.MinOccurs = 0;
                    fieldElement.MaxOccursString = "unbounded";

                    //<xs:complexType>
                    XmlSchemaComplexType fieldComplexType = new XmlSchemaComplexType();
                    fieldElement.SchemaType = fieldComplexType;

                    //<xs:simpleContent>
                    XmlSchemaSimpleContent fieldSimpleContent = new XmlSchemaSimpleContent();

                    //<xs:extension base="xs:decimal">
                    XmlSchemaSimpleContentExtension fieldSimpleContent_extension = new XmlSchemaSimpleContentExtension();
                    fieldSimpleContent_extension.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    fieldSimpleContent.Content = fieldSimpleContent_extension;

                    // Create Form's attribute
                    XmlSchemaAttribute nameAttribute = new XmlSchemaAttribute();
                    nameAttribute.Name = "name";
                    nameAttribute.Use = XmlSchemaUse.Required;
                    nameAttribute.SchemaTypeName = new XmlQualifiedName("FieldType", "http://www.w3.org/2001/XMLSchema");
                    fieldSimpleContent_extension.Attributes.Add(nameAttribute);
                    fieldComplexType.ContentModel = fieldSimpleContent;

                    // Create the Dictionary type for the name attribute.
                    XmlSchemaSimpleType fieldType = new XmlSchemaSimpleType();
                    fieldType.Name = "FieldType";
                    XmlSchemaSimpleTypeRestriction fieldRestriction = new XmlSchemaSimpleTypeRestriction();
                    fieldRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in (from f in ctx.Fields
                                         select f))
                    {
                        XmlSchemaEnumerationFacet enumerationField = new XmlSchemaEnumerationFacet();
                        enumerationField.Value = rec.Title;
                        fieldRestriction.Facets.Add(enumerationField);
                    }
                    fieldType.Content = fieldRestriction;

                    XmlSchemaAttribute typeAttribute = new XmlSchemaAttribute();
                    typeAttribute.Name = "type";
                    typeAttribute.Use = XmlSchemaUse.Required;
                    typeAttribute.SchemaTypeName = new XmlQualifiedName("FieldTypeType", "http://www.w3.org/2001/XMLSchema");
                    fieldSimpleContent_extension.Attributes.Add(typeAttribute);
                    fieldComplexType.ContentModel = fieldSimpleContent;

                    // Create the Dictionary type for the street attribute.
                    XmlSchemaSimpleType fieldTypeType = new XmlSchemaSimpleType();
                    fieldTypeType.Name = "FieldTypeType";
                    XmlSchemaSimpleTypeRestriction fieldTypeRestriction = new XmlSchemaSimpleTypeRestriction();
                    fieldTypeRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    foreach (var rec in (from f in ctx.Dicts
                                         select f))
                    {
                        XmlSchemaEnumerationFacet enumerationFieldType = new XmlSchemaEnumerationFacet();
                        enumerationFieldType.Value = rec.Title;
                        fieldTypeRestriction.Facets.Add(enumerationFieldType);
                    }
                    fieldTypeType.Content = fieldTypeRestriction;

                    XmlSchemaAttribute requiredAttribute = new XmlSchemaAttribute();
                    requiredAttribute.Name = "required";
                    requiredAttribute.Use = XmlSchemaUse.Optional;
                    requiredAttribute.SchemaTypeName = new XmlQualifiedName("RequiredType", "http://www.w3.org/2001/XMLSchema");
                    fieldSimpleContent_extension.Attributes.Add(requiredAttribute);
                    fieldComplexType.ContentModel = fieldSimpleContent;

                    // Create the Dictionary type for the street attribute.
                    XmlSchemaSimpleType requiredType = new XmlSchemaSimpleType();
                    requiredType.Name = "RequiredType";
                    XmlSchemaSimpleTypeRestriction requiredRestriction = new XmlSchemaSimpleTypeRestriction();
                    requiredRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                    // <xs:enumeration value="Small"/>
                    XmlSchemaEnumerationFacet enumerationRequiredTrue = new XmlSchemaEnumerationFacet();
                    enumerationRequiredTrue.Value = "true";
                    requiredRestriction.Facets.Add(enumerationRequiredTrue);

                    // <xs:enumeration value="Medium"/>
                    XmlSchemaEnumerationFacet enumerationRequiredFalse = new XmlSchemaEnumerationFacet();
                    enumerationRequiredFalse.Value = "false";
                    requiredRestriction.Facets.Add(enumerationRequiredFalse);
                    requiredType.Content = requiredRestriction;

                    sequence.Items.Add(houseElement);
                    sequence.Items.Add(formElement);
                    sequence.Items.Add(fieldElement);

                    importDataComplexType.Particle = choice;
                    elementComplexType.Particle = sequence;

                    // Create an empty schema.
                    XmlSchema houseInfoSchema = new XmlSchema();
                    houseInfoSchema.Id = "ImportData";

                    // Add all top-level element and types to the schema
                    houseInfoSchema.Items.Add(importDataElement);
                    houseInfoSchema.Items.Add(naspType);
                    houseInfoSchema.Items.Add(districtType);
                    houseInfoSchema.Items.Add(streetType);
                    houseInfoSchema.Items.Add(formType);
                    houseInfoSchema.Items.Add(fieldType);
                    houseInfoSchema.Items.Add(fieldTypeType);
                    houseInfoSchema.Items.Add(requiredType);

                    // Add guid-comment to the schema 
                    XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
                    XmlSchemaDocumentation documentation = new XmlSchemaDocumentation();
                    XmlDocument helperDocument = new XmlDocument();
                    XmlComment comment = helperDocument.CreateComment(Guid.NewGuid().ToString());
                    documentation.Markup = new XmlNode[1] { comment };
                    annotation.Items.Add(documentation);
                    houseInfoSchema.Items.Add(annotation);

                    // Create an XmlSchemaSet to compile the customer schema.
                    XmlSchemaSet schemaSet = new XmlSchemaSet();
                    schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
                    schemaSet.Add(houseInfoSchema);
                    schemaSet.Compile();

                    foreach (XmlSchema schema in schemaSet.Schemas())
                    {
                        houseInfoSchema = schema;
                    }

                    var sw = new Utf8StringWriter();
                    var xw = XmlWriter.Create(sw);
                    // Write the complete schema to the Console.
                    houseInfoSchema.Write(sw);
                    xw.Flush();
                    string result = sw.GetStringBuilder().ToString();
                    xw.Close();
                    sw.Close();
       
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }

    }
}
