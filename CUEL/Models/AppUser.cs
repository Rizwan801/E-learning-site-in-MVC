using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public enum Gender
    {
        Male, Female
    }
    public enum UserType
    {
        Admin, Student, Teacher
    }
    public class AppUser
    {
        [Key]
        public int AppUserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only Alphabats are Allow.")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Only Alphabats are Allow.")]
        public string FatherName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        public int? DepartmentID { get; set; }
        public Department Department { get; set; }
        [StringLength(20)]
        [Display(Name = "Registration #")]
        public string RegNo { get; set; }
        public byte[] Picture { get; set; }
        public string Content { get; set; }
        public int? BatchID { get; set; }
        public Batch Batch { get; set; }
        public UserType UserType { get; set; }
        public bool Active { get; set; } = false;

        public virtual List<Post> Posts { get; set; } = new List<Post>();
        public virtual List<PostLikeDislike> PostLikes { get; set; } = new List<PostLikeDislike>();
        public virtual List<FormPost> FormPosts { get; set; } = new List<FormPost>();
        public virtual List<UserDiscussion> UserDiscussions { get; set; } = new List<UserDiscussion>();
        public virtual List<PostComment> PostComments { get; set; } = new List<PostComment>();
        public virtual List<Message> Messages { get; set; } = new List<Message>();
        [InverseProperty("AppUser")]
        public virtual List<Notification> AppUserNotifications { get; set; } = new List<Notification>();
        [InverseProperty("Driver")]
        public virtual List<Notification> DriverNotifications { get; set; } = new List<Notification>();
    }
}