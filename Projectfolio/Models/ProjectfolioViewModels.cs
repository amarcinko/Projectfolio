using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projectfolio.Models
{
    public class ProjectViewModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Project name")]
        [Required(ErrorMessage = "Please add a project name.")]
        [StringLength(30)]
        public string ProjectName { get; set; }
        [DisplayName("Short description")]
        public string ShortDesc { get; set; }
        public bool isPublic { get; set; }
        public bool isFinished { get; set; }
        [DataType(DataType.Url)]
        public string URL { get; set; }
        public string Description { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }

        [DisplayName("Categories")]
        public List<AssignedCategory> AssignedCategories { get; set; }
    }

    public class AssignedCategory
    {
        public Category Category { get; set; }
        public Boolean Assigned { get; set; }
    }

    public class BrowseProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }

}