using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public string Text { get; set; }
        [DataType(DataType.Url)]
        public string Link { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [ForeignKey("AppUser")]
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey("Driver")]
        public int? DriverID { get; set; }
        public AppUser Driver { get; set; }
    }
}