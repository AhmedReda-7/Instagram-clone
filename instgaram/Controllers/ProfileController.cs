using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using instgaram.Models;
using System.Data.Entity;


namespace instgaram.Controllers
{
    public class ProfileController : Controller
    {
        private Model db = new Model();
        // GET: Porfile
        public ActionResult Index()
        {
            if(Session["Userid"] == "0" && Session["Userid"]==null)
            {
                return RedirectToAction("Index", "Home");
            }

            User u =db.Users.Find(Convert.ToInt32(Session["Userid"]));
            return View(u);
        }

        [HttpGet]
        public ActionResult addpost()
        {
            if (Session["Userid"] == "0" && Session["Userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult addpost(HttpPostedFileBase photo)
        {
            HttpPostedFileBase postedFile = Request.Files["photo"];
            string path = Server.MapPath("~/photopost/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            post post = new post();
            post.photo = "/photopost/" + Path.GetFileName(postedFile.FileName);
            post.user = db.Users.Find(Convert.ToInt32(Session["Userid"]));
            post.PostLike = 0;
            db.posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("index");
        }



        public ActionResult Edit()
        {
            int id = Convert.ToInt32(Session["Userid"]);
            if (id == null)
            {
                return RedirectToAction("index");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FName,LName,Username,Password,Mobile,Photo")] User user)
        {

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult allpersons()
        {
            int myId = Convert.ToInt32(Session["Userid"]);
            List<User> users = db.Users.Where(
                m => m.Id != myId
                ).ToList();
            
            List<friend_request> send_req = db.requests.
                Where(m => m.sender1_id == myId).ToList();
            
            foreach (var item in send_req)
            {
                users.Remove(item.receiver);
            }
            List<friend_request> receiver_req = db.requests.
                Where(m => m.receiver1_id == myId).ToList();

            foreach (var item in receiver_req)
            {
                users.Remove(item.sender);
            }
            List<friend> friendremove = db.friends.Where(m => m.my_id == myId).ToList();
            foreach (var item in friendremove)
            {
                users.Remove(item.Friend);
            }

            return View(users);
        }



        public ActionResult my_requests()
        {
            int myId = Convert.ToInt32(Session["Userid"]);
            List<friend_request> friend_Req =db.requests.Where(
                m => m.sender1_id == myId).ToList();
            return View(friend_Req);
        }


        public ActionResult addfriend(int? id)
        {
            friend_request friend_Req = new friend_request();

            int myId = Convert.ToInt32(Session["Userid"]);
            int userId = Convert.ToInt32(id);

            try
            {
               List<friend_request> remove =  /*check if this user is not sener or reciever  */
                    db.requests.Where(
                        m => m.sender1_id == myId && m.receiver1_id == userId
                        ).ToList();
                db.requests.RemoveRange(remove);
                db.SaveChanges();
            }
            catch
            {

            }

            friend_Req.sender1_id = myId;
            friend_Req.sender = db.Users.Find(myId);

            friend_Req.receiver1_id = userId;
            friend_Req.receiver = db.Users.Find(userId);
            
            db.requests.Add(friend_Req);
            db.SaveChanges();

            return RedirectToAction("my_requests");
        }

        public ActionResult cancel(int? id)
        {
            int myId = Convert.ToInt32(Session["Userid"]);
            int userId = Convert.ToInt32(id);
            
            try
            {
                List<friend_request> remove =
                   db.requests.Where(
                       m => m.sender1_id == myId && m.receiver1_id == userId
                       ).ToList();
                db.requests.RemoveRange(remove);
                db.SaveChanges();

            }
            catch
            {

            }
            return RedirectToAction("my_requests");
        }
        public ActionResult getpersons (FormCollection form)
        {
            int myId = Convert.ToInt32(Session["Userid"]);
            string name = form["name"].ToString();

            List<User> users = db.Users.Where(m => m.Id != myId && m.Username == name || m.Email == name).ToList();
            
            return View(users);
        }

        public ActionResult income_requests()
        {
            
            int myId = Convert.ToInt32(Session["Userid"]);
            List<friend_request> users =
                db.requests.Where(m => m.receiver1_id == myId).ToList();
            return View(users);
        }
        public ActionResult search()
        {
            return View();
        }
        public ActionResult acceptrequest(int? id)
        {
            friend_request friend_Req = new friend_request();
            friend accept_friend = new friend();

            int myId = Convert.ToInt32(Session["Userid"]);
            int userId = Convert.ToInt32(id);

            try
            {
                List<friend_request> remove =
                     db.requests.Where(
                         m => m.sender1_id == userId && m.receiver1_id == myId
                         ).ToList();
                db.requests.RemoveRange(remove);
                db.SaveChanges();
            }
            catch
            {

            }
            try
            {
                List<friend> remove =
                     db.friends.Where(
                         m => m.my_id == myId && m.friend1_id == userId
                         ).ToList();
                db.friends.RemoveRange(remove);
                db.SaveChanges();
            }
            catch
            {

            }

            accept_friend.my_id = myId;
            accept_friend.Me = db.Users.Find(myId);

            accept_friend.friend1_id = userId;
            accept_friend.Friend = db.Users.Find(userId);

            db.friends.Add(accept_friend);
            db.SaveChanges();


            accept_friend.my_id = userId;
            accept_friend.Me = db.Users.Find(userId);

            accept_friend.friend1_id = myId;
            accept_friend.Friend = db.Users.Find(myId);

            db.friends.Add(accept_friend);
            db.SaveChanges();

            return RedirectToAction("income_requests", "profile");
        }


        public ActionResult friends()
        {
            int myId = Convert.ToInt32(Session["Userid"]);
            List<friend> friend = db.friends.Where(m => m.my_id == myId).ToList();
            return View(friend);
        }


        public ActionResult reject(int? id)
        {

            int myId = Convert.ToInt32(Session["Userid"]);
            int userId = Convert.ToInt32(id);

            try
            {
                List<friend_request> remove =
                     db.requests.Where(
                         m => m.sender1_id == userId && m.receiver1_id == myId
                         ).ToList();
                db.requests.RemoveRange(remove);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("income_requests", "profile");
        }

        public ActionResult Like(int id)
        {
            post update = db.posts.ToList().Find(u => u.Id == id);
            update.PostLike += 1;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult DisLike(int id)
        {
            post update = db.posts.ToList().Find(u => u.Id == id);
            update.PostLike -= 1;
            if(update.PostLike < 0)
            {
                update.PostLike = 0;
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult ShowFriend(int? id)
        {
            return View(db.friends.Find(id));
        }
    }
}