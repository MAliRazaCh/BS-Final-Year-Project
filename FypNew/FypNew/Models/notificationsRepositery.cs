using FypNew.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FypNew.Models
{
    public class notificationsRepositry
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["notificationConnection"].ConnectionString;
        DatabaseEntities db = new DatabaseEntities();
        public List<teamMembersTable> GetAllMessages()
        {
            var data = new List<teamMembersTable>();

            var query = db.teamMembersTables as DbQuery<teamMembersTable>;
            string cmdText = query.ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Notification = null;

            var dependency = new SqlDependency(cmd);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                teamMembersTable obj = new teamMembersTable();
                obj.project_id = Convert.ToInt32(reader[1]);
                obj.user_id = Convert.ToInt32(reader[2]);
                obj.request = Convert.ToBoolean(reader[3]);
                data.Add(obj);
            }
            return data;
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                notificationHub.SendMessages();
            }
        }
    }
}