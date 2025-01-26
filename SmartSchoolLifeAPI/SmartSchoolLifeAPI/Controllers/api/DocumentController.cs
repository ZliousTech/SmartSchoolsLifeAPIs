using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class DocumentController : ApiController
    {
        SmartSchoolsEntities2 db;

        public DocumentController()
        {
            db = new SmartSchoolsEntities2();

        }

        [HttpGet]
        public IEnumerable<CustomPropertyType> GetAllProperties()
        {
            return db.CustomPropertyTypes.ToList();
        }

        [HttpGet]
        public IEnumerable<DocumrntCustomizedProperty> GetDocumrntCustomizedProperties(int DocumentTypeID)
        {
            return db.DocumrntCustomizedProperties.Where(x => x.DocumentTypeID == DocumentTypeID);
        }

        [HttpPost]
        public int AddDocuemnt(Document doc)
        {
            db.Documents.Add(doc);
            db.SaveChanges();
            return doc.DocumentID;
        }

        [HttpPost]
        public void AddDocumentCustomProperties(DocumentCustomProperty doc, int DocumentID)
        {
            db.DocumentCustomProperties.Add(doc);
            db.SaveChanges();
        }
    }
}
