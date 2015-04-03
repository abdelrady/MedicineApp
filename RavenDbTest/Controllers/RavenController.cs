using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using RavenDbTest.Helpers;

namespace RavenDbTest.Controllers
{
    public class RavenDbController : Controller
    {
        public new static IDocumentSession DbSession { get; set; }
        private static IDocumentStore _documentStore;
        public static IDocumentStore DocumentStore
        {
            get
            {
                if (_documentStore != null)
                    return _documentStore;
                lock(typeof(DocumentStoreBase))
                {
                    if (_documentStore != null)
                        return _documentStore;
                    _documentStore = new EmbeddableDocumentStore
                                         {
                                             ConnectionStringName = "RavenDb",
                                             UseEmbeddedHttpServer = false
                                         }.Initialize();
                    IndexCreation.CreateIndexes(typeof(ItemByName).Assembly, _documentStore);
                    IndexCreation.CreateIndexes(typeof(ItemByDesc).Assembly, _documentStore);
                }
                return _documentStore;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DbSession =  DocumentStore.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(DbSession !=null && filterContext.Exception == null)
                DbSession.SaveChanges();
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}
