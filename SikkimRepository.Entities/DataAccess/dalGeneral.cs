using SikkimRepository.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalGeneral
    {
        EFDBContext db = new EFDBContext();

        public GenContentCount GenContenCount()
        {
            GenContentCount _gmodel = db.Database.SqlQuery<GenContentCount>("udspGenContentsCount").FirstOrDefault();
            return _gmodel;
        }
        public string UpdatePublishedContent(long Id, bool IsPublished, string Type, string UserID)
        {
            try
            {
                var parID = new SqlParameter("@Id", Id);
                var parIsPublished = new SqlParameter("@IsPublished", IsPublished);
                var parType = new SqlParameter("@Type", Type);
                var parTransDate = new SqlParameter("@TransDate", DateTime.Now);
                var parUserID = new SqlParameter("@UserID", UserID);

                return db.Database.SqlQuery<string>("udspPublishContent @Id,@IsPublished, @Type, @TransDate, @UserID",
                  parID, parIsPublished, parType, parTransDate, parUserID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public List<DashCountModel> DashboardContentCount(long SchoolID)
        {
            var parSID = new SqlParameter("@SchoolID", SchoolID);
            List<DashCountModel> model = db.Database.SqlQuery<DashCountModel>("udspDashboardCount @SchoolId", parSID).ToList();
            return model;
        }
        public List<DashCountModel> DistWiseImgContentCount()
        {
            List<DashCountModel> model = db.Database.SqlQuery<DashCountModel>("udspDistContentCount").ToList();
            return model;
        }
        public List<DashCountModel> DistWiseVidContentCount()
        {
            List<DashCountModel> model = db.Database.SqlQuery<DashCountModel>("udspDistVidContentCount").ToList();
            return model;
        }
        public List<DashCountModel> DistWiseTstmContentCount()
        {
            List<DashCountModel> model = db.Database.SqlQuery<DashCountModel>("udspDistTesmContentCount").ToList();
            return model;
        }
        public List<DashCountModel> DistWiseCSContentCount()
        {
            List<DashCountModel> model = db.Database.SqlQuery<DashCountModel>("udspDistCaseStdyContentCount").ToList();
            return model;
        }

    }
}
