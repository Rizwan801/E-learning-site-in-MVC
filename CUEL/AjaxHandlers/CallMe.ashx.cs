using CUEL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace CUEL.AjaxHandlers
{
    /// <summary>
    /// Summary description for CallMe
    /// </summary>
    public class CallMe : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            AppDb db = new AppDb();
            var serializer = new JavaScriptSerializer();
            Response response = new Response();
            string Action = context.Request["action"];
            if (Action == "GetPosts")
            {
                if (context.Session["AppUser"] != null)
                {
                    //AppUser user = context.Session["AppUser"] as AppUser;
                    //var posts = db.Posts.Where(p => p.AppUser.DepartmentID == user.DepartmentID).Include(p => p.AppUser).Include(p => p.PostComments).Include(p => p.PostComments.Select(c => c.AppUser)).Include(p => p.PostLikes);
                    //posts = posts.OrderByDescending(p => p.Added);
                    //return View(posts.ToList());
                    response.Status = "Success";
                    //response.Message = posts.Count().ToString();
//                    response.Result = posts.ToList();
                }
                else
                {
                    response.Status = "Failed";
                    response.Message = "Un Authorized Request";
                    response.Result = null;
                }
            }
            //JsonSerializerSettings Settings = new JsonSerializerSettings
            //{
            //    //TypeNameHandling = TypeNameHandling.All,
            //    //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};
            //var json = JsonConvert.SerializeObject(response, Formatting.Indented, Settings);
            //context.Response.Write(json);

            context.Response.Write(serializer.Serialize(response));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class Response
    {
        public string Status { get; set; } = "Error";
        public string Message { get; set; } = "Not Found";
        public object Result { get; set; } = null;
    }
}