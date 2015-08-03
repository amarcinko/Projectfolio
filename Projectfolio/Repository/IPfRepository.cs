using Projectfolio.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Projectfolio.Repository
{
    public interface IPfRepository
    {
        IEnumerable<Project> GetUserPublicProjects(string userName);
        Project GetSingleProject(int id);
        ICollection<ApplicationUser> GetAllUsers();
        IEnumerable<Project> GetNewProjects();
        UserDetails GetUserDetails(string userName);
        void Dispose();
    }
}