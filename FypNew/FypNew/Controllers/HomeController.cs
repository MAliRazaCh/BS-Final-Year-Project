using FypNew.Controllers;
using FypNew.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FypNew.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EditIdea(int id)
        {
            
            DatabaseEntities db = new DatabaseEntities();
            PostTable obj = db.PostTables.Find(id);
            ideaDetail data = new ideaDetail();
            data.postData = obj;
            data.userData = db.UserTables.Where(r => r.user_Id == obj.post_userID).FirstOrDefault();
            List<commentTable> comment = db.commentTables.Where(r => r.comment_postID == obj.post_Id).ToList();
            List<commentDetail> cmList = new List<commentDetail>();
            foreach (var a in comment)
            {
                commentDetail cmntdetail = new commentDetail();
                cmntdetail.cmnt = a;
                cmntdetail.user = db.UserTables.Find(a.comment_userID);
                cmList.Add(cmntdetail);
            }
            data.commentData = cmList;
            data.likeData = db.LikesTables.Where(r => r.like_postID == obj.post_Id).ToList();
            return View(data);
        }
        public ActionResult ideaDetail(int obj)
        {
            if (Session["UserID"] != null)
            {
                int sID = (int)Session["UserID"];
                DatabaseEntities db = new DatabaseEntities();
                PostTable pt = db.PostTables.Find(obj);
                if (pt.post_userID == sID)
                {
                    return RedirectToAction("EditIdea", "Home", new { id = obj });
                }
                else
                {

                    ideaDetail data = new ideaDetail();
                    data.postData = pt;
                    data.userData = db.UserTables.Where(r => r.user_Id == pt.post_userID).FirstOrDefault();
                    List<commentTable> comment = db.commentTables.Where(r => r.comment_postID == pt.post_Id).ToList();
                    List<commentDetail> cmList = new List<commentDetail>();
                    foreach (var a in comment)
                    {
                        commentDetail cmntdetail = new commentDetail();
                        cmntdetail.cmnt = a;
                        cmntdetail.user = db.UserTables.Find(a.comment_userID);
                        cmList.Add(cmntdetail);
                    }
                    data.commentData = cmList;
                    data.likeData = db.LikesTables.Where(r => r.like_postID == pt.post_Id).ToList();
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        public ActionResult postIdea()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult createprofile(UserTable userData)
        {

            HttpPostedFileBase pic = Request.Files["photo"];

            try
            {
                DatabaseEntities db = new DatabaseEntities();
                db.UserTables.Add(userData);
                int i = db.SaveChanges();

                DatabaseEntities db2 = new DatabaseEntities();
                if (i > 0)
                {
                    var user1 = db2.UserTables.Where(x => x.user_userName.Equals(userData.user_userName)).FirstOrDefault();
                }
                else
                {
                    TempData["message"] = "account can not created try again with different userName";
                    return RedirectToAction("SignUp", "Home");
                }
            }
            catch (Exception e)
            {

                TempData["message"] = "account can not created try again with different userName : "+ e.Message;
                return RedirectToAction("SignUp", "Home");
            }

            if (pic == null || pic.ContentLength <= 0)
            {


                Session["UserID"] = userData.user_Id;
                Session.Timeout = 5540;
                return RedirectToAction("homepage", "Home");

            }
            else
            {
                pic.SaveAs(Path.Combine(Server.MapPath("~/Content/profilePic"), userData.user_Id + ".jpg"));
                ViewBag.Message = "uploaded Successfully";

                Session["UserID"] = userData.user_Id;
                Session.Timeout = 5540;
                return RedirectToAction("homepage", "Home");
            }
        }
        [HttpPost]
        public int chkUserName(String Value)
        {
            DatabaseEntities db1 = new DatabaseEntities();
            UserTable i = db1.UserTables.Where(x => x.user_userName.Equals(Value)).FirstOrDefault();
            int data;
            if (i != null)
                data = 1;
            else
                data = 0;
            return data;
        }
        [HttpPost]
        public string userData(int Value)
        {
            DatabaseEntities db = new DatabaseEntities();
            UserTable tb = db.UserTables.Where(x => x.user_Id == Value).FirstOrDefault();
            return tb.user_userName;
        }
        public ActionResult search(string searchproject)
        {
            DatabaseEntities db1 = new DatabaseEntities();
            List<PostTable> obj2 = db1.PostTables.Where(x => x.post_title.Contains(searchproject) || x.post_tags.Contains(searchproject)).ToList();
            return View(obj2);
        }
        public ActionResult updateProfile(UserTable obj)
        {
            HttpPostedFileBase pic = Request.Files["imgName"];
            DatabaseEntities db1 = new DatabaseEntities();
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int a = (int)Session["UserID"];
                UserTable obj2 = db1.UserTables.Where(x => x.user_Id == a).FirstOrDefault();

                obj2.user_FullName = obj.user_FullName;
                obj2.user_address = obj.user_address;
                obj2.user_description = obj.user_description;
                obj2.user_emailid = obj.user_emailid;
                obj2.user_type = obj.user_type;
                obj2.user_DOB = obj.user_DOB;
                obj2.user_gender = obj.user_gender;
                obj2.user_phoneNo = obj.user_phoneNo;

                db1.SaveChanges();

                if (pic != null || pic.ContentLength > 0)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/profilePic"), obj.user_Id + ".jpg");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    pic.SaveAs(Path.Combine(Server.MapPath("~/Content/profilePic"), obj.user_Id + ".jpg"));
                    ViewBag.Message = "updated Successfully";
                }

                return RedirectToAction("profile", "Home", new { id = (int)Session["UserID"] });

            }
        }
        public ActionResult updateIdea(PostTable obj)
        {
            DatabaseEntities db1 = new DatabaseEntities();
            int a = obj.post_Id;
            PostTable obj1 = db1.PostTables.Where(x => x.post_Id == a).FirstOrDefault();
            obj1.post_data = obj.post_data;
            obj1.post_description = obj.post_description;
            obj1.post_title = obj.post_title;
            db1.SaveChanges();
            return RedirectToAction("homepage", "Home");

        }
        public ActionResult saveIdea(PostTable obj)
        {
            if (Session["UserID"] != null)
            {
                int a = (int)System.Web.HttpContext.Current.Session["UserID"];
                DatabaseEntities db = new DatabaseEntities();
                UserTable obj1 = new UserTable();
                obj1 = db.UserTables.Where(x => x.user_Id == a).FirstOrDefault();

                obj.post_userID = a;


                PostTable b = db.PostTables.Add(obj);
                db.SaveChanges();
                return View("postIdea");
            }
            else
                return View("error");
        }
        public ActionResult addNewMember(int pid)
        {
            DatabaseEntities db = new DatabaseEntities();
            List<teamMembersTable> preMembers =  db.teamMembersTables.Where(dr => dr.project_id == pid).ToList();
            List<UserTable> all = db.UserTables.ToList();
            membersList mlist = new membersList();
            mlist.projectID = pid;
            foreach (var a in preMembers)
            {
                var mem= all.Find(r => r.user_Id == a.user_id);
                all.Remove(mem);
            }
            mlist.users = all;
            return View(mlist);
        }
        public string addSelectedMembers(string list, int pid)
        {
            try
            {
                string data = list.Substring(1, list.Length - 2);
                string[] data1 = data.Split(',');
                for (int i = 0; i < data1.Length; i++)
                {
                    DatabaseEntities db2 = new DatabaseEntities();
                    teamMembersTable teamData = new teamMembersTable();
                    teamData.project_id = pid;
                    teamData.user_id = int.Parse(data1[i]);
                    db2.teamMembersTables.Add(teamData);
                    db2.SaveChanges();
                }
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string delete1Member(int uid , int pid)
        {
            try{
            DatabaseEntities db = new DatabaseEntities();
            db.teamMembersTables.Remove(db.teamMembersTables.Where(r => r.project_id == pid && r.user_id == uid).First());
            db.SaveChanges();
                return "ok";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
        public ActionResult profileView(int id)
        {
                DatabaseEntities db = new DatabaseEntities();
                UserTable obj = new UserTable();
                obj = db.UserTables.Where(x => x.user_Id == id).FirstOrDefault();
                return View(obj);
            
        }
        public ActionResult profile(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id != (int)Session["UserID"])
                {
                    return RedirectToAction("profileView", "Home", new { id = id });
                }
                else
                {
                    DatabaseEntities db = new DatabaseEntities();
                    UserTable obj = new UserTable();
                    obj = db.UserTables.Where(x => x.user_Id == id).FirstOrDefault();
                    return View(obj);
                }
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult checkData(string formUsername, string formPassword)
        {
            DatabaseEntities db = new DatabaseEntities();
            UserTable i = db.UserTables.Where(x => x.user_userName.Equals(formUsername) && x.user_userPassword.Equals(formPassword)).FirstOrDefault();
            if (i == null)
            {
                TempData["message"] = "UserName Or password is wrong please try again if you do not have an account the sign up";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Session["UserID"] = i.user_Id;
                Session.Timeout = 5440;
                TempData["message"] = i.user_userName;
                return RedirectToAction("homepage", "Home");
            }
        }
        public ActionResult homepage()
        {
            DatabaseEntities db = new DatabaseEntities();
            List<PostTable> l1 = db.PostTables.ToList();
            l1.Reverse();
            return View(l1);
        }
        public ActionResult myIdeas(int id)
        {
            DatabaseEntities db = new DatabaseEntities();
            return View(db.PostTables.Where(x => x.post_userID == id).ToList());
        }
        public ActionResult projectHome(int postId)
        {
            Session["SelectedPostId"] = postId;

            DatabaseEntities db = new DatabaseEntities();
            return View(db.UserTables.ToList());
        }
        [HttpPost]
        public string startProject(string list)
        {
            string data = list.Substring(1, list.Length - 2);
            string[] data1 = data.Split(',');
            DatabaseEntities db = new DatabaseEntities();


            int teamLead_id = (int)Session["UserID"];
            int post_id = (int)Session["SelectedPostId"];

            ProjectsTable pt = db.ProjectsTables.Where(r => r.teamLead_id == teamLead_id && r.post_id == post_id).FirstOrDefault();
            if (pt != null)
            {
                Session["projectID"] = pt.project_id;
                return "!! project is already started by you .....";
            }
            else
            {
                ProjectsTable obj = new ProjectsTable();
                obj.teamLead_id = teamLead_id;
                obj.post_id = post_id;
                obj.startDate = System.DateTime.Today;
                obj.endDate = System.DateTime.Today;
                ProjectsTable obj2 = db.ProjectsTables.Add(obj);
                db.SaveChanges();
                int projectId = obj.project_id;

                for (int i = 0; i < data1.Length; i++)
                {
                    DatabaseEntities db2 = new DatabaseEntities();
                    teamMembersTable teamData = new teamMembersTable();
                    teamData.project_id = projectId;
                    teamData.user_id = int.Parse(data1[i]);
                    db2.teamMembersTables.Add(teamData);
                    db2.SaveChanges();
                }
                Session["projectID"] = projectId;
                return "Successfully you project is initializad";
            }
        }
        public JsonResult delIdea(int post_id)
        {
            DatabaseEntities db = new DatabaseEntities();
            PostTable pt = db.PostTables.Where(r => r.post_Id == post_id).FirstOrDefault();
            if (pt == null)
            {
                try
                {
                    List<LikesTable> lt = db.LikesTables.Where(r => r.like_postID == post_id).ToList();
                    foreach (var v in lt)
                    {
                        db.LikesTables.Remove(v);
                    }
                    List<commentTable> ct = db.commentTables.Where(r => r.comment_postID == post_id).ToList();
                    foreach (var v in ct)
                    {
                        db.commentTables.Remove(v);
                    }
                    db.PostTables.Remove(db.PostTables.Find(post_id));
                    db.SaveChanges();
                    var json1 = new
                    {
                        success = "ok"
                    };
                    return Json(json1);
                }
                catch (Exception e)
                {
                    var json2 = new
                    {
                        success = e
                    };
                    return Json(json2);
                }
            }
            else
            {
                var json = new
                {
                    success = "not"
                };
                return Json(json);
            }
        }
        public string updateProject(string list)
        {
            string data = list.Substring(1, list.Length - 2);
            string[] data1 = data.Split(',');
            int pId = (int)Session["projectID"];

            DatabaseEntities db2 = new DatabaseEntities();
            for (int i = 0; i < data1.Length; i++)
            {
                teamMembersTable teamData = new teamMembersTable();
                teamData.project_id = pId;
                teamData.user_id = int.Parse(data1[i]);
                db2.teamMembersTables.Add(teamData);
                db2.SaveChanges();
            }
            return "Successfully you project is initializad";

        }
        public ActionResult updatemembers(int projectID)
        {
            Session["projectID"] = projectID;

            DatabaseEntities db = new DatabaseEntities();
            List<teamMembersTable> tb = db.teamMembersTables.Where(r => r.project_id == projectID).ToList();
            foreach (var v in tb)
            {
                db.teamMembersTables.Remove(v);
            }
            db.SaveChanges();

            return View(db.UserTables.ToList());

        }
        public ActionResult projectProgress(int id = 0)
        {
            if (Session["UserID"] != null)
            {
                if (id == 0)
                {
                    id = (int)Session["projectID"];
                }
                int userId = (int)Session["UserID"];
                DatabaseEntities db = new DatabaseEntities();
                if (db.ProjectsTables.Find(id).teamLead_id == userId)
                {
                    projectProgress2 data = new projectProgress2();
                    List<projectData> pdata = db.projectDatas.Where(r => r.projectID == id).OrderByDescending(x => x.date).ToList();
                    List<UserTable> user = db.teamMembersTables.Where(r => r.project_id == id).Select(r => r.UserTable).ToList();
                    List<GroupPost> grp = db.GroupPosts.Where(r=>r.projectID == id).ToList();
                    List<groupMsgData> msgDetail = new List<groupMsgData>();
                    foreach(var d in grp)
                    {
                        groupMsgData obj = new groupMsgData();
                        obj.sender=db.UserTables.Find(d.userID);
                        obj.msg = d;
                        msgDetail.Add(obj);
                    }
                    data.projectdata = pdata;
                    data.usesrData = user;
                    data.projectID = id;
                    data.loginUser = db.UserTables.Find(userId);
                    data.messagesDetail = msgDetail;
                    data.teamLead = db.ProjectsTables.Where(r => r.project_id ==id).Select(r => r.UserTable).FirstOrDefault();
                    return View(data);
                }
                else
                {
                    teamMembersTable tm = db.teamMembersTables.Where(r => r.project_id == id && r.user_id == userId).FirstOrDefault();
                    if (tm != null)
                    {
                        return RedirectToAction("teamMemberView", "Home", new { pid = id });
                    }
                    else
                    {
                        return RedirectToAction("userView", "Home", new { pid = id });
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult userView(int pid)
        {
            DatabaseEntities db = new DatabaseEntities();
            projectProgress2 data = new projectProgress2();
            List<projectData> pdata = db.projectDatas.Where(r => r.projectID == pid).OrderByDescending(x => x.date).ToList();
            List<UserTable> user = db.teamMembersTables.Where(r => r.project_id == pid).Select(r => r.UserTable).ToList();
            data.projectdata = pdata;
            data.usesrData = user;
            data.projectID = pid;
            data.teamLead = db.ProjectsTables.Where(r => r.project_id == pid).Select(r => r.UserTable).FirstOrDefault();
            return View(data);
        }
        public ActionResult teamMemberView(int pid)
        {
            DatabaseEntities db = new DatabaseEntities();
            projectProgress2 data = new projectProgress2();
            List<projectData> pdata = db.projectDatas.Where(r => r.projectID == pid).OrderByDescending(x => x.date).ToList();
            List<UserTable> user = db.teamMembersTables.Where(r => r.project_id == pid).Select(r => r.UserTable).ToList();
            List<GroupPost> grp = db.GroupPosts.Where(r => r.projectID == pid).ToList();
            List<groupMsgData> msgDetail = new List<groupMsgData>();
            foreach (var d in grp)
            {
                groupMsgData obj = new groupMsgData();
                obj.sender = db.UserTables.Find(d.userID);
                obj.msg = d;
                msgDetail.Add(obj);
            }
            data.projectdata = pdata;
            data.usesrData = user;
            data.loginUser = db.UserTables.Find((int)Session["UserID"]);
            data.projectID = pid;
            data.tMTable = db.teamMembersTables.Where(r => r.project_id == pid).ToList();
            data.messagesDetail = msgDetail;
            data.teamLead = db.ProjectsTables.Where(r => r.project_id == pid).Select(r => r.UserTable).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult myPartialController(int id = 0)
        {
            if(id != 0)
            {
                DatabaseEntities db = new DatabaseEntities();
                var res = db.projectDatas.ToList();
                return PartialView("_projectProgress", res);
            }
            return null;
        }
        [HttpPost]
        public string deleteImage(int sNo)
        {

            try
            {
                DatabaseEntities db = new DatabaseEntities();
                projectData data = db.projectDatas.Find(sNo);
                var path = Path.Combine(Server.MapPath("~/Content/projectsData"), data.link);
                System.IO.File.Delete(path);
                db.projectDatas.Remove(data);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return "ok";
                }
                else
                {
                    return "not";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        public ActionResult projectData(int pid)
        {
            HttpPostedFileBase file = Request.Files["img"];
            if (file != null)
            {
                DatabaseEntities db = new DatabaseEntities();
                projectData obj = new projectData();
                obj.projectID = pid;
                obj.date = System.DateTime.Now;
                string fileName = Path.GetFileName(file.FileName).ToString();
                obj.link = fileName;
                db.projectDatas.Add(obj);

                var path = Path.Combine(Server.MapPath("~/Content/projectsData"), fileName);
                file.SaveAs(path);
                db.SaveChanges();
                ViewBag.AlertMessage = "SuccessfullyUploaded";
            }
            else
            {
                ViewBag.AlertMessage = "files can not be uploaded";

            }
            return RedirectToAction("projectProgress", "Home", new { id = pid });
        }
        public ActionResult myProjects(int id)
        {
            DatabaseEntities oobj = new DatabaseEntities();

            List<ProjectPostModel> mpm = new List<ProjectPostModel>();
            List<ProjectsTable> myProjects = new List<ProjectsTable>();
            myProjects = (from tm in oobj.teamMembersTables
                          join pr in oobj.ProjectsTables on tm.project_id equals pr.project_id
                          where tm.user_id == id
                          select pr).ToList();
            List<ProjectsTable> myProjects2 = new List<ProjectsTable>();
            myProjects2 = oobj.ProjectsTables.Where(r => r.teamLead_id == id).ToList();
            myProjects.AddRange(myProjects2);
            foreach (var project in myProjects)
            {
                ProjectPostModel ppm = new ProjectPostModel();
                ppm.post_idea = oobj.PostTables.Find(project.post_id);
                ppm.project = project;
                mpm.Add(ppm);
            }

            return View(mpm);
        }
        public ActionResult allProjects()
        {
            DatabaseEntities obj = new DatabaseEntities();
            List<ProjectPostModel> mpm = new List<ProjectPostModel>();
            List<ProjectsTable> myProjects = new List<ProjectsTable>();
            myProjects = obj.ProjectsTables.ToList();
            foreach (var project in myProjects)
            {
                ProjectPostModel ppm = new ProjectPostModel();
                ppm.post_idea = obj.PostTables.Find(project.post_id);
                ppm.project = project;
                ppm.user = obj.UserTables.Find(project.teamLead_id);
                mpm.Add(ppm);
            }
            return View(mpm);
        }
        public JsonResult AddComment(int post_id, string comment)
        {
            if (Session["UserID"] != null)
            {
                try
                {
                    DatabaseEntities db = new DatabaseEntities();
                    commentTable cmnt = new commentTable();
                    cmnt.comment_data = comment;
                    cmnt.comment_postID = post_id;
                    cmnt.comment_userID = (int)Session["UserID"];
                    cmnt.comment_time = System.DateTime.Now;
                    db.commentTables.Add(cmnt);
                    db.SaveChanges();
                    var json = new
                    {
                        success = "ok"
                    };
                    return Json(json);
                }
                catch (Exception e)
                {
                    var json = new
                    {
                        success = e
                    };
                    return Json(json);
                }
            }
            else
            {
                var json = new
                {
                    success = "Sesson TimeOut"
                };
                return Json(json);
            }
        }
        public JsonResult addLike(int post_id)
        {
            if (Session["UserID"] != null)
            {
                int id = (int)Session["UserID"];
                DatabaseEntities db = new DatabaseEntities();
                LikesTable a = (from x in db.LikesTables
                                where x.like_userID == id &&
                                x.like_postID == post_id
                                select x).FirstOrDefault();
                if (a != null)
                {
                    var json = new
                    {
                        success = "already liked"
                    };
                    return Json(json);
                }
                else
                {
                    try
                    {
                        LikesTable data = new LikesTable();
                        data.like_postID = post_id;
                        data.like_time = System.DateTime.Now;
                        data.like_userID = id;
                        db.LikesTables.Add(data);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        var json = new
                        {
                            success = e
                        };
                        return Json(json);
                    }
                    var json2 = new
                    {
                        success = "ok"
                    };
                    return Json(json2);
                }
            }
            else
            {
                var json2 = new
                {
                    success = "Session TimeOut"
                };
                return Json(json2);
            }
        }
        public JsonResult deleteProject(int pid)
        {
            DatabaseEntities db = new DatabaseEntities();
            ProjectsTable pt = db.ProjectsTables.Where(r => r.project_id == pid).FirstOrDefault();
            if (pt != null)
            {
                try
                {
                    List<projectData> lt = db.projectDatas.Where(r => r.projectID == pid).ToList();
                    foreach (var v in lt)
                    {
                        db.projectDatas.Remove(v);
                    }
                    List<teamMembersTable> ct = db.teamMembersTables.Where(r => r.project_id == pid).ToList();
                    foreach (var v in ct)
                    {
                        db.teamMembersTables.Remove(v);
                    }
                    List<GroupPost> gp = db.GroupPosts.Where(r => r.projectID == pid).ToList();
                    foreach (var v in gp)
                    {
                        db.GroupPosts.Remove(v);
                    }
                    db.ProjectsTables.Remove(db.ProjectsTables.Find(pid));
                    db.SaveChanges();
                    var json1 = new
                    {
                        success = "ok"
                    };
                    return Json(json1);
                }
                catch (Exception e)
                {
                    var json2 = new
                    {
                        success = e
                    };
                    return Json(json2);
                }
            }
            else
            {
                var json = new
                {
                    success = "not"
                };
                return Json(json);
            }
        }
        [HttpPost]
        public int getNotificationsCount(int uid)
        {
            notificationsRepositry nr = new notificationsRepositry();
            List<teamMembersTable> data =  nr.GetAllMessages();
             int c = data.Where(r => r.user_id == uid && r.request==false).Count();
        return c;
        }
        public ActionResult notificationDetail(int uid)
        {
            DatabaseEntities db = new DatabaseEntities();
            List<teamMembersTable> post = db.teamMembersTables.Where(r => r.user_id == uid && r.request == false).ToList();
            List<notificationViewData> data = new List<notificationViewData>();
            foreach(var v in post)
            {
                notificationViewData obj = new notificationViewData();
                obj.userData = db.ProjectsTables.Where(r => r.project_id == v.project_id).Select(r => r.UserTable).FirstOrDefault();
                obj.postData = db.ProjectsTables.Where(r => r.project_id == v.project_id).Select(r => r.PostTable).FirstOrDefault();
                obj.membersData = v;
                data.Add(obj);
            }

            return View(data);
        }
        [HttpPost]
        public bool deleteRequest(int id)
        {
            try
            {
                DatabaseEntities db = new DatabaseEntities();
                teamMembersTable obj = db.teamMembersTables.Find(id);
                if (obj != null)
                {
                    db.teamMembersTables.Remove(obj);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        [HttpPost]
        public bool approveRequest(int id)
        {
            try
            {
                DatabaseEntities db = new DatabaseEntities();
                teamMembersTable obj = db.teamMembersTables.Find(id);
                if (obj != null)
                {
                    obj.request = true;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception e)
            {
                return false;
            }
        }
        public bool addGrpMessage(string msg)
        {
            try
            {
                DatabaseEntities db = new DatabaseEntities();
                GroupPost obj = JsonConvert.DeserializeObject<GroupPost>(msg);
                obj.time = DateTime.Now;
                db.GroupPosts.Add(obj);
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
        public ActionResult SignOut()
        {
            Session.Remove("UserID");
            return RedirectToAction("Login", "Home");
        }
    }
}
