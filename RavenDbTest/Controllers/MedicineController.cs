using System;
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
        public ActionResult Index(int pageNumber=0,int pageSize=5)
        {
            var items = Session.Query<Item>().Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return Json(items);
        }

        public ActionResult AddNew()
        {
            ViewBag.IsAdd = true;
            return View();
        }

        [HttpPost]
        public ActionResult AddNew(Item item, bool isAdd)
        {
            if(AddItem(item,isAdd))
            return RedirectToAction("List");
            return Content("Item not found in database");
        }

        //public ActionResult TestJson()
        //{
        //    return View();
        //}

        [HttpPost]
        public JsonResult Add(Item item)
        {
            item.IsActive = false;
            return Json(new { Result = AddItem(item, true) });
        }

        public ActionResult Edit(string id)
        {
            ViewBag.IsAdd = false;
            id = id.Replace('_', '/');
            var item = Session.Load<Item>(id);
            if (item != null)
                return View("AddNew",item);
            return Content("Item not found in database");
        }

        public ActionResult List(int pageNumber=1,int pageSize=5)
        {
            var items = Session.Query<Item>().Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
            ViewBag.NumOfPages = Math.Ceiling(Session.Query<Item>().Count() / (double)pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            return View(items);
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

        public ActionResult Delete(string id)
        {
            id = id.Replace('_', '/');
            var item = Session.Load<Item>(id);
            if (item != null)
                Session.Delete(item);
            return RedirectToAction("List");
        }

        public ActionResult MakeActive(string id)
        {
            id = id.Replace('_', '/');
            var item = Session.Load<Item>(id);
            if (item != null)
                item.IsActive = true;
            return RedirectToAction("List");
        }

        private bool AddItem(Item item, bool isAdd)
        {
            if (isAdd)
                Session.Store(item);
            else
            {
                var dbItem = Session.Load<Item>(item.Id.Replace('_', '/'));
                if (dbItem != null)
                {
                    dbItem.Name = item.Name;
                    dbItem.Category = item.Category;
                    dbItem.Desc = item.Desc;
                    dbItem.TakingPeriod = item.TakingPeriod;
                    dbItem.ImageUrl = item.ImageUrl;
                    dbItem.IsActive = item.IsActive;
                }
                else return false;
            }
            return true;
        }
    }
}
