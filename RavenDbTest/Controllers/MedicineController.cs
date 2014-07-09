using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using RavenDbTest.Helpers;
using RavenDbTest.Models;

namespace RavenDbTest.Controllers
{
    public class MedicineController : RavenDbController
    {
        public ActionResult Index()
        {
            return List();
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


        public ActionResult Search(string @by="name", string criteria="")
        {
            List<Item> items;
            switch (@by.ToLower())
            {
                case "cat":
                    items = Session.Query<Item>().Where(x => x.Category == criteria).ToList();
                    break;
                case "desc":
                    items = Session.Query<Item, ItemByDesc>().Search(x => x.Desc, criteria).ToList();
                    break;
                default:
                    items = Session.Query<Item, ItemByName>().Search(x => x.Name, criteria)
                        .ToList();
                    break;
            }
            return Json(items);
        }
    }
}
