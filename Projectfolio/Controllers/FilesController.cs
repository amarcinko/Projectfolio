using Projectfolio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projectfolio.Controllers
{
    public class FilesController : Controller
    {
        ProjectfolioContext db = new ProjectfolioContext();
        // GET: ProjectFiles
        public ActionResult Index(string fh)
        {
            if (fh == null) return RedirectToAction("NotFound");
            if (User.Identity.IsAuthenticated)
            {
                ProjectDetails pd = db.ProjectDetails.SingleOrDefault(p => p.FilePath == "/Files?fh=" + fh);
                string fileName = pd.FileName;
                string fullName = Path.Combine(Server.MapPath("~/App_Data/ProjectFiles"), fileName);

                return File(fullName, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else return RedirectToAction("NotFound");
        }
    }
}