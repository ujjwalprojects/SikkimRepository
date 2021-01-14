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
    public class dalImage
    {
        EFDBContext db = new EFDBContext();

        public string SaveImage(utblImage model)
        {
            var parImageID = new SqlParameter("@ImageID", model.ImageID);
            var parImageTitle = new SqlParameter("@ImageTitle", model.ImageTitle);
            var parImageAlbumID = new SqlParameter("@ImageAlbumID", model.ImageAlbumID);
            var parImageFilePathThumb = new SqlParameter("@ImageFilePathThumb", model.ImageFilePathThumb);
            var parImageFilePathNormal = new SqlParameter("@ImageFilePathNormal", model.ImageFilePathNormal);
            var parIsAlbumCover = new SqlParameter("@IsAlbumCover", model.IsAlbumCover);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspImageSave @ImageID,@ImageTitle, @ImageAlbumID, @ImageFilePathThumb,@ImageFilePathNormal,@IsAlbumCover,   @TransDate, @UserID",
                parImageID, parImageTitle, parImageAlbumID, parImageFilePathThumb, parImageFilePathNormal, parIsAlbumCover, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<ImageView> GetImagePaged(long AlbumID, int PageNo, int PageSize, string sTerm, out int Total)
        {
            var parAlbumID = new SqlParameter("@AlbumID", AlbumID);
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            IEnumerable<ImageView> model = db.Database.SqlQuery<ImageView>("udspImageSelect @AlbumID, @Start, @PageSize,@SearchTerm, @TotalCount out",
               parAlbumID, parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblImage GetImageByID(long id)
        {
            return db.utblImages.Where(x => x.ImageID == id).FirstOrDefault();
        }
        public string DeleteImage(long id)
        {
            try
            {
                utblImage model = db.utblImages.Find(id);
                db.utblImages.Remove(model);
                db.SaveChanges();
                return "Image Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }

        public IEnumerable<utblImage> GetImageByAlbumID(long id)
        {
            return db.utblImages.Where(x => x.ImageAlbumID == id).ToList();
        }

    }
}
