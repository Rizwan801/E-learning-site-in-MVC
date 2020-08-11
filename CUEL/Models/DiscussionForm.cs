using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class DiscussionForm
    {
        [Key]
        public int DiscussionFormID { get; set; }
        public string Title { get; set; }
        public virtual List<FormPost> FormPosts { get; set; }
        public virtual List<UserDiscussion> UserDiscussions { get; set; } = new List<UserDiscussion>();
    }
    public class FormPost
    {
        [Key]
        public int FormPostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int? DiscussionFormID { get; set; }
        public DiscussionForm DiscussionForm { get; set; }
        public virtual List<FormPostComment> FormPostComments { get; set; } = new List<FormPostComment>();
        public virtual List<FormPostLikeDislike> FormPostLikes { get; set; } = new List<FormPostLikeDislike>();
    }
    public enum FormLikeDislike
    {
        Like, Dislike
    }
    public class FormPostLikeDislike
    {
        [Key]
        public int FormPostLikeDislikeID { get; set; }
        public int FormPostID { get; set; }
        public FormPost FormPost { get; set; }
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public FormLikeDislike LikeDislike { get; set; }
    }
    public class FormPostComment
    {
        [Key]
        public int FormPostCommentID { get; set; }
        public string CommentBody { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int? FormPostID { get; set; }
        public FormPost FormPost { get; set; }
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
    }
    public class UserDiscussion
    {
        [Key]
        public int UserDiscussionID { get; set; }
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int? DiscussionFormID { get; set; }
        public DiscussionForm DiscussionForm { get; set; }
    }
}