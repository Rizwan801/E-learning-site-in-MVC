using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class Batch
    {
        [Key]
        public int BatchID { get; set; }
        [Required]
        public string BatchNo { get; set; }
        public virtual List<AppUser> AppUsers { get; set; } = new List<AppUser>();
    }
}