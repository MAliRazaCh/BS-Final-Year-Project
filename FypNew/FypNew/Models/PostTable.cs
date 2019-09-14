//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FypNew.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostTable
    {
        public PostTable()
        {
            this.commentTables = new HashSet<commentTable>();
            this.LikesTables = new HashSet<LikesTable>();
            this.ProjectsTables = new HashSet<ProjectsTable>();
        }
    
        public int post_Id { get; set; }
        public string post_data { get; set; }
        public int post_userID { get; set; }
        public Nullable<System.DateTime> post_time { get; set; }
        public string post_title { get; set; }
        public string post_description { get; set; }
        public string post_tags { get; set; }
    
        public virtual ICollection<commentTable> commentTables { get; set; }
        public virtual ICollection<LikesTable> LikesTables { get; set; }
        public virtual ICollection<ProjectsTable> ProjectsTables { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}