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
    public class MyProjectfolioRepository : IMyProjectfolioRepository
    {
        private ProjectfolioContext db = new ProjectfolioContext();

        public IEnumerable<Project> List()
        {
            var projects = db.Projects.Include(p => p.Author).Include(p => p.ProjectDetails)
                                        .Include(p => p.Categories);
            return projects;
        }

        public void Add(Project project)
        {
            db.Projects.Add(project);
        }

        public void Remove(Project project)
        {
            db.Projects.Remove(project);
        }

        public Project GetSingle(int id, string userId)
        {
            var project = db.Projects.Include(p => p.Author).Include(p => p.ProjectDetails)
                                        .Include(p => p.Categories).SingleOrDefault(p => p.Id == id &&
                                         p.AuthorId == userId);
            return project;
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.Categories.ToList();
        }

        public UserDetails GetUserDetails(string userId)
        {
            return db.UserDetails.Find(userId);
        }

        public void AddUserDetails(UserDetails userDetails)
        {
            db.UserDetails.Add(userDetails);
        }

        public void ChangeUserDetailsValues(UserDetails ud, UserDetails userDetails)
        {
            db.Entry(ud).CurrentValues.SetValues(userDetails);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}