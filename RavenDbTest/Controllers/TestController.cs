using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using RavenDbTest.Helpers;
using RavenDbTest.Models;

namespace RavenDbTest.Controllers
{
    public class TestController : RavenDbController
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            var item = Session.Query<Item>().First();
            return Json(item);
        }

        public ActionResult AddNew()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNew(Item item)
        {
            Session.Store(item);
            return Json(true);
        }

        public ActionResult List(int pageNumber=0,int pageSize=5)
        {
            var items = Session.Query<Item>().Skip(pageNumber*pageSize).Take(pageSize).ToList();
            return Json(items);
        }


        public ActionResult GetItems(string searchBy="Name", string searchCriteria="")
        {
            List<Item> items;
            switch (searchBy)
            {
                case "Category":
                    items = Session.Query<Item>().Where(x => x.Category == searchCriteria).ToList();
                    break;
                case "Desc":
                    items = Session.Query<Item, ItemByDesc>().Search(x => x.Desc, searchCriteria).ToList();
                    break;
                default:
                    items = Session.Query<Item, ItemByName>().Search(x => x.Name, searchCriteria)
                        .ToList();
                    break;
            }
            return Json(items);
        }
    }
}
