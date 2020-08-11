using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class Chat
    {
        [Key]
        public int ChatID { get; set; }
        public int AppUser1ID { get; set; }
        public int AppUser2ID { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        [NotMapped]
        public AppUser AppUser1 { get; set; }
        [NotMapped]
        public AppUser AppUser2 { get; set; }
    }
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public string MessageBody { get; set; }
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int? ChatID { get; set; }
        public Chat Chat { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}