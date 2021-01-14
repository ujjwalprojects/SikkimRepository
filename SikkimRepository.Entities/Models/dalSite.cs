using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class dalSite
    {
        EFDBContext db = new EFDBContext();

        public void AddError(ErrorLogModel model)
        {
            var parDate = new SqlParameter("@ErrorDateTime", model.ErrorDateTime);
            var parURL = new SqlParameter("@URL", model.URL ?? "");
            var parSource = new SqlParameter("@Source", model.Source ?? "");
            var parType = new SqlParameter("@Type", model.Type ?? "");
            var parMessage = new SqlParameter("@Message", model.Message ?? "");
            var parIP = new SqlParameter("@IPAddress", model.IPAddress ?? "");
            var parUser = new SqlParameter("@LogUser", model.User ?? "");

            db.Database.SqlQuery<int>("udspErrorLogAdd @ErrorDateTime, @URL, @Source, @Type, @Message, @IPAddress, @LogUser",
                parDate, parURL, parSource, parType, parMessage, parIP, parUser).FirstOrDefault();
        }
        public void AddAuditLog(string tablename, string action, string ip, string user, long id = 0)
        {
            var parDate = new SqlParameter("@TimeStamp", DateTime.Now);
            var parTable = new SqlParameter("@TableName", tablename ?? "");
            var parAction = new SqlParameter("@Action", action ?? "");
            var parID = new SqlParameter("@ContentID", id);
            var parIP = new SqlParameter("@IPAddress", ip ?? "");
            var parUser = new SqlParameter("@ActionTakenBy", user ?? "");

            db.Database.SqlQuery<int>("udspAuditLogAdd @TimeStamp, @TableName, @Action, @ContentID, @IPAddress, @ActionTakenBy",
               parDate, parTable, parAction, parID, parIP, parUser).FirstOrDefault();
        }
        public List<AuditLogModel> GetLogs(DateTime sdate, DateTime edate)
        {
            var parSdate = new SqlParameter("@StartDate", sdate);
            var parEdate = new SqlParameter("@EndDate", edate);

            return db.Database.SqlQuery<AuditLogModel>("select * from dbo.udfGetAuditLogs(@StartDate, @EndDate)",
                parSdate, parEdate).ToList();
        }
    }
}
