using Projectfolio.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Projectfolio.Services
{
    public interface IMyProjectfolioService
    {
        IEnumerable<Project> GetUserProjects(string userId);
        Project GetSingleProject(int id, string userId);
        void UpdateProjectEntry(ProjectViewModel projectVM, HttpPostedFileBase file, string userId);
        ICollection<Category> GetProjectCategories(int id, string userId);
        ProjectViewModel GetEmptyProjectViewModel();
        ProjectViewModel GetProjectViewModel(int id, string userId);
        bool CreateProject(ProjectViewModel projectVM, string userId, HttpPostedFileBase file);
        UserDetails GetUserDetails(string userId);
        void ChangeUserDetailsValues(UserDetails ud, UserDetails userDetails);
        void DeleteProject(int id, string userId);
        void SaveChanges();
        void Dispose();
    }
}