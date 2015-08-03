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
using Projectfolio.Repository;
using System.IO;

namespace Projectfolio.Services
{
    public class MyProjectfolioService : IMyProjectfolioService
    {
        private IMyProjectfolioRepository _repository;

        public MyProjectfolioService()
        {
            _repository = new MyProjectfolioRepository();
        }

        public MyProjectfolioService(IMyProjectfolioRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Project> GetUserProjects(string userId)
        {
            var userProjects = _repository.List().Where(p => p.AuthorId == userId);
            return userProjects;
        }

        public Project GetSingleProject(int id, string userId)
        {
           var project = _repository.GetSingle(id, userId);
           return project;
        }

        public ICollection<Category> GetProjectCategories(int id, string userId)
        {
           ICollection<Category> projectCategories = _repository.GetSingle(id, userId).Categories;
           return projectCategories;
        }

        public ProjectViewModel GetEmptyProjectViewModel()
        {
            ProjectViewModel projectVM = new ProjectViewModel();
            var assignedCategories = new List<AssignedCategory>();
            foreach (var cat in _repository.GetCategories().ToList())
            {
                assignedCategories.Add(
                    new AssignedCategory
                    {
                        Category = cat,
                        Assigned = false
                    });
            }
            projectVM.AssignedCategories = assignedCategories;

            return projectVM;
        }

        public ProjectViewModel GetProjectViewModel(int id, string userId)
        {
            var project = _repository.GetSingle(id, userId);
            ProjectViewModel projectVM = new ProjectViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                isFinished = project.isFinished,
                isPublic = project.isPublic,
                ShortDesc = project.ShortDesc,
                URL = project.URL,
                Description = project.ProjectDetails.Description,
                FileName = project.ProjectDetails.FileName
            };

            var assignedCategories = new List<AssignedCategory>();
            foreach (var cat in _repository.GetCategories())
            {
                if (project.Categories.Contains(cat))
                {
                    assignedCategories.Add(new AssignedCategory
                    {
                        Category = cat,
                        Assigned = true
                    });
                }
                else
                {
                    assignedCategories.Add(new AssignedCategory
                    {
                        Category = cat,
                        Assigned = false
                    });
                }

            }

            projectVM.AssignedCategories = assignedCategories;
            return projectVM;
        }

        public UserDetails GetUserDetails(string userId)
        { 
            return _repository.GetUserDetails(userId);
        }

        public bool CreateProject(ProjectViewModel projectVM, string userId, HttpPostedFileBase file)
        {
            Project project = new Project
            {
                AuthorId = userId,
                ProjectName = projectVM.ProjectName,
                DateCreated = DateTime.Now,
                ProjectDetails = new ProjectDetails
                    {
                        DateLastEdit = DateTime.Now,
                        Description = projectVM.Description,
                        FileName = file.FileName
                    },
                ShortDesc = projectVM.ShortDesc,
                URL = projectVM.URL,
                isFinished = projectVM.isFinished,
                isPublic = projectVM.isPublic
            };

            //Pridruživanje kategorija odvojiti u zasebnu metodu?
            project.Categories = new List<Category>();
            var cat = _repository.GetCategories().ToList();
            foreach (var assignedCat in projectVM.AssignedCategories)
            {
                if (assignedCat.Assigned)
                    project.Categories.Add
                        (cat.Single(c => c.CategoryName == assignedCat.Category.CategoryName));
            }
            //Spremanje datoteke
            string extension = "";
            if (file != null) extension = System.IO.Path.GetExtension(file.FileName);
            if (file != null && file.ContentLength > 0 &&
                (extension == ".zip" || extension == ".rar"))
            {
                string newFileName = Guid.NewGuid().ToString();
                file.SaveAs(System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/ProjectFiles"), newFileName + extension));
                project.ProjectDetails.FilePath = "/Files?fh=" + Guid.NewGuid().ToString();
                project.ProjectDetails.FileName = newFileName + extension;
                project.ProjectDetails.Size = file.ContentLength;
            }
            else
            {
                return false;
            }

            _repository.Add(project);
            _repository.SaveChanges();
            return true;
        }

        public void UpdateProjectEntry(ProjectViewModel projectVM, HttpPostedFileBase file, string userId)
        {
            var project = _repository.GetSingle(projectVM.Id, userId);
            project.ProjectName = projectVM.ProjectName;
            project.ShortDesc = projectVM.ShortDesc;
            project.URL = projectVM.URL;
            project.isFinished = projectVM.isFinished;
            project.isPublic = projectVM.isPublic;
            project.ProjectDetails.Description = projectVM.Description;
            project.ProjectDetails.DateLastEdit = DateTime.Now;

            string extension = "";
            if (file != null) extension = System.IO.Path.GetExtension(file.FileName);
            if (file != null && file.ContentLength > 0 &&
                (extension == ".zip" || extension == ".rar"))
            {
                string fileName = projectVM.FileName;
                string newFileName = Guid.NewGuid().ToString() + extension;
                string fullPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/ProjectFiles"), fileName);
                string newFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/ProjectFiles"), newFileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                file.SaveAs(System.IO.Path.GetFullPath(newFilePath));
                project.ProjectDetails.FileName = newFileName;
                project.ProjectDetails.Size = file.ContentLength;
            }

            var cat = _repository.GetCategories();
            foreach (var assignedCat in projectVM.AssignedCategories)
            {
                if (assignedCat.Assigned &&
                    !project.Categories.Contains(cat.SingleOrDefault(c => c.Id == assignedCat.Category.Id)))
                {
                    project.Categories
                        .Add(cat.SingleOrDefault(c => c.Id == assignedCat.Category.Id));
                }
                else if (!assignedCat.Assigned &&
                    project.Categories.Contains(cat.SingleOrDefault(c => c.Id == assignedCat.Category.Id)))
                {
                    project.Categories
                        .Remove(cat.SingleOrDefault(c => c.Id == assignedCat.Category.Id));
                }
            }

            _repository.SaveChanges();
        }


        public void ChangeUserDetailsValues(UserDetails ud, UserDetails userDetails)
        {
             _repository.ChangeUserDetailsValues(ud, userDetails);
        }


        public void DeleteProject(int id, string userId)
        {
            Project project = _repository.GetSingle(id,userId);
            _repository.Remove(project);
            _repository.SaveChanges();
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}