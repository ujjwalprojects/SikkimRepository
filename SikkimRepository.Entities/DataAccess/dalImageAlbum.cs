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
    public class dalImageAlbum
    {
        EFDBContext db = new EFDBContext();

        #region admin
        public string SaveImgAlbum(utblImageAlbum model)
        {
            var parImageAlbumID = new SqlParameter("@ImageAlbumID", model.ImageAlbumID);
            var parImageAlbumTitle = new SqlParameter("@ImageAlbumTitle", model.ImageAlbumTitle);
            var parImageAlbumDesc = new SqlParameter("@ImageAlbumDesc", model.ImageAlbumDesc);
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

            return db.Database.SqlQuery<string>("udspImageAlbumSave @ImageAlbumID,@ImageAlbumTitle, @ImageAlbumDesc, @ProgramDate,@ComponentID,@SubComponentID,@SchoolID, @IsPublished,@PublishDate, @TransDate, @UserID",
                parImageAlbumID, parImageAlbumTitle, parImageAlbumDesc, parProgramDate, parComponentID, parSubComponentID,parSchoolID, parIsPublished, parPublishDate, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<ImageAlbumView> GetImgAlbumPaged(int PageNo, int PageSize, long SchoolID, string sTerm, out int Total)
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

            IEnumerable<ImageAlbumView> model = db.Database.SqlQuery<ImageAlbumView>("udspImageAlbumSelect @Start, @PageSize,@SchoolID,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSchoolID, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblImageAlbum GetImgAlbumByID(long id)
        {
            return db.utblImageAlbums.Where(x => x.ImageAlbumID == id).FirstOrDefault();
        }
        public string DeleteImgAlbum(long id)
        {
            try
            {
                utblImageAlbum model = db.utblImageAlbums.Find(id);
                db.utblImageAlbums.Remove(model);
                db.SaveChanges();
                return "Image Album Removed";
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
        #endregion

        #region general page
        public IEnumerable<GenImageAlbumView> GenImageAlbumlist()
        {
            IEnumerable<GenImageAlbumView> _gmodel = db.Database.SqlQuery<GenImageAlbumView>("udspGenImageSelect").ToList();
            return _gmodel;
        }
        public ImageAlbumGenViewModel GenImageAlbumSelectPaged(long SubComponentID, int PageNo, int PageSize, string sTerm)
        {
            ImageAlbumGenViewModel model = new ImageAlbumGenViewModel();
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

            model.ImageAlbumList = db.Database.SqlQuery<GenImageAlbumView>("GenImageAlbumSelectAll @Start, @PageSize,@SearchTerm,@SubComponentID, @TotalCount out",
                parStart, parEnd, parSearchTerm,parSubCompID, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        public IEnumerable<GenImageAlbumByDist> GetImageAlbumByDist(string DistCode)
        {
            var parDistCode = new SqlParameter("@DistCode", DistCode);
            IEnumerable<GenImageAlbumByDist> model = db.Database.SqlQuery<GenImageAlbumByDist>("udspGenImageAlbumByDistCode @DistCode", parDistCode).ToList();
            return model;
        }

        public GenViewListByDistrictCode GetImageAlbumByDistCode(string DistCode, int PageNo, int PageSize, string sTerm)
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

            model.ImageAlbumListByDist = db.Database.SqlQuery<GenImageAlbumByDist>("GenImageAlbumSelectByDist @Start, @PageSize,@SearchTerm,@DistCode, @TotalCount out",
                parStart, parEnd, parSearchTerm, parDistCode, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        #endregion
    }
}
