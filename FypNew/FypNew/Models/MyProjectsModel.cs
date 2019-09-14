using FypNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypNew.Models
{
    public class ProjectPostModel
    {
        public ProjectsTable project { get; set; }
        public PostTable post_idea { get; set; }
        public UserTable user { get; set; }
    }
    public class projectProgress2
    {
        public List<projectData> projectdata{ get; set; }
        public List<UserTable> usesrData { get; set; }
        public List<teamMembersTable> tMTable { get; set; }
        public int projectID { get; set; }
        public UserTable loginUser { get; set; }
        public List<groupMsgData> messagesDetail { get; set; }
        public UserTable teamLead { get; set; }
    }
    public class groupMsgData
    {
        public UserTable sender { get; set; }
        public GroupPost msg { get; set; }
    }
    public class ideaDetail
    {
        public List<LikesTable> likeData { get; set; }
        public UserTable userData { get; set; }
        public List<commentDetail> commentData { get; set; }
        public PostTable postData { get; set; }
    }
    public class commentDetail
    {
        public commentTable cmnt { get; set; }
        public UserTable user { get; set; }
    }
    public class notificationViewData
    {
        public UserTable userData { get; set; }
        public PostTable postData { get; set; }
        public teamMembersTable membersData { get; set; }
    }
    public class membersList
    {
        public int projectID { get; set; }
        public List<UserTable> users { get; set; }
    }

}