using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace QuizManagementSystem.Controllers
{
    public class ExampleController : Controller
    {
        private QuizManagementSystemDbContext db = new QuizManagementSystemDbContext();

        // GET: Example
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Class).Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Example/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Example/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.Classes, "Id", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        // POST: Example/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,PasswordHash,NewPasswordHash,Email,DayOfBirth,Phone,DateOfParticipation,FullName,Sex,ClassID,Avatar,Status,RoleID,CreateBy,ModifiedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "Id", "Name", user.ClassID);
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name", user.RoleID);
            return View(user);
        }

        // GET: Example/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.Classes, "Id", "Name", user.ClassID);
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name", user.RoleID);
            return View(user);
        }

        // POST: Example/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,PasswordHash,NewPasswordHash,Email,DayOfBirth,Phone,DateOfParticipation,FullName,Sex,ClassID,Avatar,Status,RoleID,CreateBy,ModifiedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "Id", "Name", user.ClassID);
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name", user.RoleID);
            return View(user);
        }

        // GET: Example/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Example/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
