using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalVideoAlbum
    {
        EFDBContext db = new EFDBContext();


        public string SaveVideoAlbum(utblVideoAlbum model)
        {
            var parVideoAlbumID = new SqlParameter("@VideoAlbumID", model.VideoAlbumID);
            var parVideoAlbumTitle = new SqlParameter("@VideoAlbumTitle", model.VideoAlbumTitle);
            var parVideoAlbumDesc = new SqlParameter("@VideoAlbumDesc", model.VideoAlbumDesc);
            var parProgramDate = new SqlParameter("@ProgramDate", model.ProgramDate);
            var parComponentID = new SqlParameter("@ComponentID", model.ComponentID);
            var parSubComponentID = new SqlParameter("@SubComponentID", model.SubComponentID);
            var parSchoolID = new SqlParameter("@SchoolID", model.SchoolID);
            var parIsPublished = new SqlParameter("@IsPublished", model.IsPublished);
            var parPublishDate = new SqlParameter("@PublishDate", DBNull.Value);
            if (model.PublishDate != null)
            {
                parPublishDate = new SqlParameter("@PublishDate", model.PublishDate);
            }
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspVideoAlbumSave @VideoAlbumID,@VideoAlbumTitle, @VideoAlbumDesc, @ProgramDate,@ComponentID,@SubComponentID,@SchoolID,@IsPublished,@PublishDate, @TransDate, @UserID",
                parVideoAlbumID, parVideoAlbumTitle, parVideoAlbumDesc, parProgramDate, parComponentID, parSubComponentID, parSchoolID, parIsPublished, parPublishDate, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<VideoAlbumView> GetVideoAlbumPaged(int PageNo, int PageSize, long SchoolID, string sTerm, out int Total)
        {
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parSchoolID = new SqlParameter("@SchoolID", SchoolID);
            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            IEnumerable<VideoAlbumView> model = db.Database.SqlQuery<VideoAlbumView>("udspVideoAlbumSelect @Start, @PageSize,@SchoolID,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSchoolID, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblVideoAlbum GetVideoAlbumByID(long id)
        {
            return db.utblVideoAlbums.Where(x => x.VideoAlbumID == id).FirstOrDefault();
        }
        public string DeleteVideoAlbum(long id)
        {
            try
            {
                utblVideoAlbum model = db.utblVideoAlbums.Find(id);
                db.utblVideoAlbums.Remove(model);
                db.SaveChanges();
                return "Video Album Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
        public IEnumerable<utblMstSubComponent> GetSubCompByComp(long id)
        {
            return db.utblMstSubComponents.Where(x => x.ComponentID == id).ToList();
        }

        #region general page
        public IEnumerable<GenVideoAlbumView> GenVideoAlbumlist()
        {
            IEnumerable<GenVideoAlbumView> _gmodel = db.Database.SqlQuery<GenVideoAlbumView>("udspGenVideoSelect").ToList();
            return _gmodel;
        }
        public VideoAlbumGenViewModel GenVideoAlbumSelectPaged(long SubComponentID, int PageNo, int PageSize, string sTerm)
        {
            VideoAlbumGenViewModel model = new VideoAlbumGenViewModel();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parSubCompID = new SqlParameter("@SubComponentID", DBNull.Value);
            if (SubComponentID != 0)
                parSubCompID = new SqlParameter("@SubComponentID", SubComponentID);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.VideoAlbumList = db.Database.SqlQuery<GenVideoAlbumView>("GenVideoAlbumSelectAll @Start, @PageSize,@SearchTerm,@SubComponentID, @TotalCount out",
                parStart, parEnd, parSearchTerm, parSubCompID, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        public IEnumerable<GenVideoAlbumByDist> GetVideoAlbumByDist(string DistCode)
        {
            var parDistCode = new SqlParameter("@DistCode", DistCode);
            IEnumerable<GenVideoAlbumByDist> model = db.Database.SqlQuery<GenVideoAlbumByDist>("udspGenVideoAlbumByDistCode @DistCode", parDistCode).ToList();
            return model;
        }

        public GenViewListByDistrictCode GetVideoAlbumByDistCode(string DistCode, int PageNo, int PageSize, string sTerm)
        {
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parDistCode = new SqlParameter("@DistCode", DistCode);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };
            model.VideoAlbumListByDist = db.Database.SqlQuery<GenVideoAlbumByDist>("GenVideoAlbumSelectByDist @Start, @PageSize,@SearchTerm,@DistCode, @TotalCount out",
                parStart, parEnd, parSearchTerm, parDistCode, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        #endregion
    }
}
