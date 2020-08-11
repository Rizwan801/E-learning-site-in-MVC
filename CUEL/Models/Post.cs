using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CUEL.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public List<PostComment> PostComments { get; set; } = new List<PostComment>();
        public List<PostLikeDislike> PostLikes { get; set; } = new List<PostLikeDislike>();
    }
    public enum LikeDislike
    {
        Like, Dislike
    }
    public class PostLikeDislike
    {
        [Key]
        public int PostLikeDislikeID { get; set; }
        public int PostID { get; set; }
        public Post Post { get; set; }
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public LikeDislike LikeDislike { get; set; }
    }
    public class PostComment
    {
        [Key]
        public int PostCommentID { get; set; }
        public string CommentBody { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int? PostID { get; set; }
        public Post Post { get; set; }
        public int? AppUserID { get; set; }
        public AppUser AppUser { get; set; }
    }

}