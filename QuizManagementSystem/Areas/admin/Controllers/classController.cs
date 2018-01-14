using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;

namespace QuizManagementSystem.Areas.admin.Controllers
{
    public class classController : baseController
    {
        private QuizManagementSystemDbContext db = new QuizManagementSystemDbContext();

        // GET: admin/class
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var _classDao = new ClassDAO();
            var _class = _classDao.GetAllClassPageList(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(_class);
        }

        // GET: admin/class/Details/5
        public ActionResult Details(string searchString, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _classDao = new ClassDAO();
            Class _class = _classDao.GetClassById(id);
            if (_class == null)
            {
                return HttpNotFound();
            }
            var _userDao = new UserDAO();
            ViewBag.UserClass = _userDao.GetAllUserByClass(searchString, _class);
            ViewBag.SearchString = searchString;
            return View(_class);
        }

        // GET: admin/class/Create
        public ActionResult Create()
        {
            ViewBag.SchoolYearID = new SelectList(db.SchoolYears, "Id", "NameOfSchoolYear");
            return View();
        }

        // POST: admin/class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,SchoolYearID,Status")] Class @class)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Classes.Add(@class);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.SchoolYearID = new SelectList(db.SchoolYears, "Id", "NameOfSchoolYear", @class.SchoolYearID);
            return View(@class);
        }

        // GET: admin/class/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SchoolYearID = new SelectList(db.SchoolYears, "Id", "NameOfSchoolYear", @class.SchoolYearID);
            return View(@class);
        }

        // POST: admin/class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SchoolYearID,Status")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.SchoolYearID = new SelectList(db.SchoolYears, "Id", "NameOfSchoolYear", @class.SchoolYearID);
            return View(@class);
        }

        // GET: admin/class/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: admin/class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
