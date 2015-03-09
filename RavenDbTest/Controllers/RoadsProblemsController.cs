using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using RavenDbTest.Helpers;
using RavenDbTest.Models;
using System.IO;

namespace RavenDbTest.Controllers
{
    public class RoadsProblemsController : RavenDbController
    {

        public ActionResult TestJson(string id)
        {
            ViewBag.CatName = id;
            return View();
        }

        public ActionResult GetCategories()
        {
            return Json(GetCategoryList());
        }

        [HttpPost]
        public JsonResult AddCategory(string catName)
        {
            var result = false;
            try
            {
                Session.Store(new Category { CatName = catName });
                result = true;
            }
            catch
            { }

            return Json(new { Result = result });
        }

        [HttpPost]
        public ActionResult DeleteCategory(string catName)
        {
            var result = false;
            var problem = Session.Query<Category>().ToList().Where(x => x.CatName.ToLower() == catName.ToLower()).FirstOrDefault();
            if (problem != null)
            {
                Session.Delete(problem);
                result = true;
            }
            return Json(new { Result = result });
        }


        public ActionResult Index(int pageNumber = 0, int pageSize = 5)
        {
            var problems = Session.Query<Problem>().Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return Json(problems.Select(x => new
            {
                x.Id,
                x.Category,
                x.Description,
                x.IsActive,
                x.Latitude,
                x.Longitude,
                x.Rating,
                x.Severity,
                x.UserId
            }));
        }

        public ActionResult AddNew()
        {
            ViewBag.IsAdd = true;
            InitializeViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Problem problem, bool isAdd)
        {
            string imageData = null;
            var imageFile = Request.Files[0];
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(imageFile.InputStream))
                    fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                imageData = Convert.ToBase64String(fileData, 0, fileData.Length);
            }

            if (AddProblem(problem, isAdd, imageData))
                return RedirectToAction("List");
            else return Content("Item not found in database");
        }

        [HttpPost]
        public JsonResult Add(Problem problem)
        {
            return Json(new { Result = AddProblem(problem, true, problem.Image) });
        }

        public ActionResult Edit(string id)
        {
            ViewBag.IsAdd = false;
            InitializeViewBags();
            id = id.Replace('_', '/');
            var problem = Session.Load<Problem>(id);
            if (problem != null)
                return View("AddNew", problem);
            return Content("problem not found in database");
        }

        public ActionResult List(int pageNumber = 0, int pageSize = 5)
        {
            var problemItems = Session.Query<Problem>().Skip(pageNumber * pageSize).Take(pageSize).ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            return View(problemItems);
        }


        public ActionResult Search(string @by = "name", string criteria = "")
        {
            List<Problem> problems;
            switch (@by.ToLower())
            {
                case "cat":
                    problems = Session.Query<Problem>().Where(x => x.Category == criteria).ToList();
                    break;
                default:
                    problems = Session.Query<Problem, ProblemByDesc>().Search(x => x.Description, criteria)
                        .ToList();
                    break;
            }
            return Json(problems);
        }

        public ActionResult GetProblemImages(string id)
        {
            id = id.Replace('_', '/');
            var problem = Session.Load<Problem>(id);
            return Json(new { problem.Image });
        }

        public ActionResult GetAroundProblems(double @long, double lat, double range)
        {
            List<Problem> problems;
            problems = Session.Query<Problem>().Where(x => DistanceFinder.GetDistanceBetweenPoints(lat, @long, x.Latitude, x.Longitude) <= range).ToList();
            return Json(problems);
        }

        public ActionResult Delete(string id)
        {
            id = id.Replace('_', '/');
            var problem = Session.Load<Problem>(id);
            if (problem != null)
                Session.Delete(problem);
            return RedirectToAction("List");
        }

        private bool AddProblem(Problem problem, bool isAdd, string imageData)
        {
            if (isAdd)
            {
                problem.Image = imageData;
                Session.Store(problem);
            }
            else
            {
                var dbItem = Session.Load<Problem>(problem.Id.Replace('_', '/'));
                if (dbItem != null)
                {
                    dbItem.Description = problem.Description;
                    dbItem.Category = problem.Category;
                    dbItem.Severity = problem.Severity;
                    dbItem.Rating = problem.Rating;
                    dbItem.UserId = problem.UserId;
                    dbItem.Longitude = problem.Longitude;
                    dbItem.Latitude = problem.Latitude;
                    if (!string.IsNullOrEmpty(imageData)) dbItem.Image = imageData;
                }
                else return false;
            }
            return true;
        }

        #region Private Methods

        private void InitializeViewBags()
        {
            ViewBag.CategoryList = new SelectList(GetCategoryList(), "CatName", "CatName");
            ViewBag.SeverityList = new List<SelectListItem>{
                                                          new SelectListItem
                                                          {
                                                              Text = "Fatal",
                                                              Value = "Fatal"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "Dangrous",
                                                              Value = "Dangrous"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "UpNormal",
                                                              Value = "UpNormal"
                                                          }
            };

            ViewBag.RatingList = new List<SelectListItem>{
                                                          new SelectListItem
                                                          {
                                                              Text = "1",
                                                              Value = "1"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "2",
                                                              Value = "2"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "3",
                                                              Value = "3"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "4",
                                                              Value = "4"
                                                          },
                                                          new SelectListItem
                                                          {
                                                              Text = "5",
                                                              Value = "5"
                                                          }
            };

        }

        private List<Category> GetCategoryList()
        {
            return Session.Query<Category>().ToList();
        }

        #endregion
    }
}