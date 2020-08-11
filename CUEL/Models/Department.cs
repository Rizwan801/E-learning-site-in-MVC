using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public virtual List<AppUser> AppUsers { get; set; } = new List<AppUser>();
    }
}