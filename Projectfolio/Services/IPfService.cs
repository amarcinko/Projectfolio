using Projectfolio.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Projectfolio.Services
{
    public interface IPfService
    {
        IEnumerable<Project> GetUserPublicProjects(string userName);
        Project GetSingleProject(int id);
        BrowseProjectsViewModel GetBrowseProjectsViewModel();
        UserDetails GetUserDetails(string userName);
        void Dispose();
    }
}