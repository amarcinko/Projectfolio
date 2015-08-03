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

namespace Projectfolio.Services
{
    public class PfService : IPfService
    {
        private IPfRepository _repository;

        public PfService()
        {
            _repository = new PfRepository();
        }

        public PfService(IPfRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Project> GetUserPublicProjects(string userName)
        {
            var projects = _repository.GetUserPublicProjects(userName);
            return projects;
        }
        
        public Project GetSingleProject(int id)
        {
            Project project = _repository.GetSingleProject(id);
            return project;
        }
        
        public BrowseProjectsViewModel GetBrowseProjectsViewModel()
        {
            BrowseProjectsViewModel bpvm = new BrowseProjectsViewModel();
            bpvm.Users = _repository.GetAllUsers();
            bpvm.Projects = _repository.GetNewProjects();
            return bpvm;
        }

        public UserDetails GetUserDetails(string userName)
        {
            var userDetails = _repository.GetUserDetails(userName);
            return userDetails;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

    }
}