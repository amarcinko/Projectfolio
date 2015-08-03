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
using System.Data.Entity.Infrastructure;
using System.Collections;

namespace Projectfolio.Repository
{
    public class PfRepository : IPfRepository
    {
        private ProjectfolioContext db = new ProjectfolioContext();

        public IEnumerable<Project> GetUserPublicProjects(string userName)
        {
            var projects = (from p in db.Projects
                            where p.Author.UserName == userName && p.isPublic == true
                            select p);
            return projects;
        }
        
        public Project GetSingleProject(int id)
        {
            Project project = db.Projects.Find(id);
            return project;
        }
        
        public ICollection<ApplicationUser> GetAllUsers()
        {
            var allUsers = db.Users.ToList();
            return allUsers;
        }

        public IEnumerable<Project> GetNewProjects()
        {
            var projects = (from p in db.Projects where p.isPublic orderby p.DateCreated select p).Take(10);
            return projects;
        }

        public UserDetails GetUserDetails(string userName)
        {
            var userDetails = db.UserDetails.SingleOrDefault(u => u.User.UserName == userName);
            return userDetails;
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}