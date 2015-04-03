using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RavenDbTest.Models;

namespace RavenDbTest.Controllers
{
    public class NotesController : RavenDbController
    {
        public ActionResult AddTestUser()
        {
            return View();
        }
        public ActionResult LoginTest()
        {
            return View();
        }
        public ActionResult AddTestUserNote()
        {
            return View();
        }
        public ActionResult LogoutTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var result = false;

            email = email.ToLower();
            var user = DbSession.Query<NotesUser>()
                .FirstOrDefault(x => x.Email == email && x.Password == GetHash(password));
            if (user != null)
            {
                Session["Id"] = user.Id;
                result = true;
            }

            return Json(new { Result = result });
        }


        public ActionResult Logout()
        {
            Session["Id"] = null;
            return Json(new { Result = true });
        }

        private string GetHash(string password)
        {
            return password;
        }

        public ActionResult Index()
        {
            var userId = Session["Id"];
            if (userId != null && !string.IsNullOrEmpty(userId.ToString()))
                return Json(DbSession.Query<Note>().Where(x => x.UserId == userId.ToString()));
            return Json(new List<Note>());
        }

        private NotesUser GetUser()
        {
            var userId = Session["Id"];
            if (userId != null && !string.IsNullOrEmpty(userId.ToString()))
                return DbSession.Load<NotesUser>(userId.ToString());
            return null;
        }

        [HttpPost]
        public JsonResult AddNewUser(NotesUser user)
        {
            return Json(new { Result = AddUser(user, true) });
        }


        [HttpPost]
        public JsonResult AddUserNote(Note note)
        {
            var result = AddNote(note);
            return Json(new { Result = result });
        }


        [HttpPost]
        public JsonResult RemoveUserNote(string noteId)
        {
            var result = RemoveNote(noteId);
            return Json(new { Result = result });
        }

        [HttpPost]
        public JsonResult UpdateUserNote(Note note)
        {
            var result = RemoveNote(note.Id);
            result = AddNote(note);
            return Json(new { Result = result });
        }

        private bool AddNote(Note note)
        {
            var result = false;
            var user = GetUser();
            if (user != null)
            {
                note.UserId = user.Id;
                DbSession.Store(note);
                result = true;
            }
            return result;
        }

        private bool RemoveNote(string noteId)
        {
            var result = false;
            var user = GetUser();
            var dbNote = DbSession.Load<Note>(noteId);

            if (user != null && dbNote != null && dbNote.UserId == user.Id)
            {
                DbSession.Delete(dbNote);
                DbSession.SaveChanges();
                result = true;
            }
            return result;
        }


        private bool AddUser(NotesUser user, bool isAdd)
        {
            user.Email = user.Email.ToLower();
            if (isAdd)
                DbSession.Store(user);
            return true;
        }
    }
}
