using Projectfolio.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Projectfolio.Repository
{
    public interface IMyProjectfolioRepository
    {
        IEnumerable<Project> List();
        void Add(Project project);
        void Remove(Project project);
        Project GetSingle(int id, string userId);
        void AddUserDetails(UserDetails userDetails);
        IEnumerable<Category> GetCategories();
        UserDetails GetUserDetails(string userId);
        void ChangeUserDetailsValues(UserDetails ud, UserDetails userDetails);
        void SaveChanges();
        void Dispose();
    }
}