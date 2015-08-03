using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projectfolio.Models
{
    public class Project
    {
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
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual ProjectDetails ProjectDetails { get; set; }
    }

    public class ProjectDetails
    {
        [Key, ForeignKey("Project")]
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime DateLastEdit { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public float Size { get; set; }

        public virtual Project Project { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
 
}