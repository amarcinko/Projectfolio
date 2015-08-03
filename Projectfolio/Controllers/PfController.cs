using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projectfolio.Models;
using System.Threading.Tasks;
using Projectfolio.Repository;
using Projectfolio.Services;

namespace Projectfolio.Controllers
{
    public class PfController : Controller
    {
        private PfService _service;

        public PfController()
        {
            _service = new PfService();
        }

        public PfController(PfService service)
        {
            _service = service;
        }

        // GET: Pf
        public ActionResult Index(string userName)
        {
            if (userName != null)
            {
                if (userName == User.Identity.Name) return RedirectToAction("Index", "MyProjectfolio");

                var projects = _service.GetUserPublicProjects(userName);

                ViewBag.UserName = userName;
                return View(projects.ToList());
            }
            else
            {   
                return RedirectToAction("AllUsers");
            }
        }

        // GET: Pf/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = _service.GetSingleProject(id.Value);
            if (project == null && !project.isPublic)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Pf/AllUsers/
        public ActionResult BrowseProjects()
        {
            var bpvm = _service.GetBrowseProjectsViewModel();
            return View(bpvm);
        }

        public ActionResult ViewProfile(string userName)
        {
            if (userName != null)
            {
                if (userName == User.Identity.Name) return RedirectToAction("ViewProfile", "MyProjectfolio");

                var userDetails = _service.GetUserDetails(userName);

                if(userDetails != null) ViewBag.Image = userDetails.ImagePath;
                return View(userDetails);
            }
            else
            {
                return RedirectToAction("AllUsers");
            }
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
