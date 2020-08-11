using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class AppDb : DbContext
    {
        public AppDb():base("CUELDb")
        {

        }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DiscussionForm> DiscussionForms { get; set; }
        public DbSet<FormPost> FormPosts { get; set; }
        public DbSet<UserDiscussion> UserDiscussions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLikeDislike> postLikeDislikes { get; set; }
        public DbSet<FormPostComment> FormPostComments { get; set; }
        public DbSet<FormPostLikeDislike> FormPostLikeDislikes { get; set; }

        public System.Data.Entity.DbSet<CUEL.Models.Notification> Notifications { get; set; }
    }
}