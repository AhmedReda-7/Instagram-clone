using instgaram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace instgaram.Controllers
{
    public class CommentsController : ApiController
    {
        private Model model = new Model(); 
       public string post()
       {
            
            int iduser = Convert.ToInt32(HttpContext.Current.Request.Form["iduser"]);
            int idpost = Convert.ToInt32(HttpContext.Current.Request.Form["idpost"]);
            string comm = HttpContext.Current.Request.Form["comment"];

            comment comment = new comment();
            comment.commenttext = comm;
            comment.User = model.Users.Find(iduser);
            comment.Post = model.posts.Find(idpost);

            model.comments.Add(comment);
            model.SaveChanges();
            return "";
       }
    }
}
