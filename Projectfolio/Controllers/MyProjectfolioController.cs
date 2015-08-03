using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projectfolio.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Projectfolio.Services;

namespace Projectfolio.Controllers
{
    [Authorize]
    public class MyProjectfolioController : Controller
    {
        private IMyProjectfolioService _service;

        public MyProjectfolioController()
        {
            _service = new MyProjectfolioService();
        }

        public MyProjectfolioController(IMyProjectfolioService service)
        {
            _service = service;
        }

        // GET: myProjectfolio
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var projects = _service.GetUserProjects(userId);
            ViewBag.UserName = User.Identity.GetUserName();
            return View(projects.ToList());
        }

        // GET: myProjectfolio/Details/5
        public ActionResult ProjectDetails(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Project project = _service.GetSingleProject(id.Value, User.Identity.GetUserId());
            if (project == null) return HttpNotFound();

            return View(project);
        }

        // GET: Projectfolio/Create
        public ActionResult Create()
        {
            ProjectViewModel projectVM = _service.GetEmptyProjectViewModel();
            return View(projectVM);
        }
        
        // POST: Projectfolio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectViewModel projectVM, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (!_service.CreateProject(projectVM, User.Identity.GetUserId(), file))
                {
                    ViewBag.NoFileMessage = "Please add a valid project file";
                    return View(projectVM);
                }

                return RedirectToAction("Index");
            }
            return View(projectVM);
        }

        // GET: Projectfolio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var projectVM = _service.GetProjectViewModel(id.Value, User.Identity.GetUserId());
            if (projectVM == null) return HttpNotFound();

            return View(projectVM);
        }

        // POST: Projectfolio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectViewModel projectVM, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateProjectEntry(projectVM, file, User.Identity.GetUserId());               
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = User.Identity.GetUserId();
            return View(projectVM);
        }

        // GET: Projectfolio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _service.GetSingleProject(id.Value, User.Identity.GetUserId());
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projectfolio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.DeleteProject(id, User.Identity.GetUserId());
            return RedirectToAction("Index");
        }

        public ActionResult ViewProfile()
        {
            var userDetails = _service.GetUserDetails(User.Identity.GetUserId());

            return View(userDetails);
        }

        // GET: Projectfolio/Edit/5
        public ActionResult EditUserDetails()
        {
            UserDetails userDetails = _service.GetUserDetails(User.Identity.GetUserId());
            if (userDetails == null)
            {
                userDetails = new UserDetails();
                userDetails.UserId = User.Identity.GetUserId();
            }
            return View(userDetails); 
        }

        // POST: Projectfolio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserDetails(UserDetails userDetails, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var ud = _service.GetUserDetails(userDetails.UserId);
                if (image != null && image.ContentLength > 0 && image.FileName.Contains("jpg"))
                {
                    string newFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    image.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Images"), newFileName));
                    userDetails.ImagePath = "/Images/" + newFileName;
                }
                _service.ChangeUserDetailsValues(ud, userDetails);
                _service.SaveChanges();
                
            }
            return View(userDetails);
        }
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
