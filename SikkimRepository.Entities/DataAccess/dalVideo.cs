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
    public class dalVideo
    {
        EFDBContext db = new EFDBContext();

        public string SaveVideo(utblVideo model)
        {
            var parVideoID = new SqlParameter("@VideoID", model.VideoID);
            var parVideoTitle = new SqlParameter("@VideoTitle", model.VideoTitle);
            var parVideoAlbumID = new SqlParameter("@VideoAlbumID", model.VideoAlbumID);
            var parVideoFilePathThumb = new SqlParameter("@VideoFilePathThumb",DBNull.Value);
            if(model.VideoFilePathDraft != null)
            {
                parVideoFilePathThumb = new SqlParameter("@VideoFilePathThumb", model.VideoFilePathDraft);
            }
            var parVideoFilePathNormal = new SqlParameter("@VideoFilePathNormal", DBNull.Value);
            if(model.VideoFilePathPublished != null)
            {
                parVideoFilePathNormal = new SqlParameter("@VideoFilePathNormal", model.VideoFilePathPublished);
            }
            var parIsAlbumCover = new SqlParameter("@IsAlbumCover", model.IsAlbumCover);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspVideoSave @VideoID,@VideoTitle, @VideoAlbumID, @VideoFilePathThumb,@VideoFilePathNormal,@IsAlbumCover,   @TransDate, @UserID",
                parVideoID, parVideoTitle, parVideoAlbumID, parVideoFilePathThumb, parVideoFilePathNormal, parIsAlbumCover, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<VideoView> GetVideoPaged(long AlbumID, int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<VideoView> model = db.Database.SqlQuery<VideoView>("udspVideoSelect @AlbumID, @Start, @PageSize,@SearchTerm, @TotalCount out",
               parAlbumID, parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblVideo GetVideoByID(long id)
        {
            return db.utblVideos.Where(x => x.VideoID == id).FirstOrDefault();
        }
        public string DeleteVideo(long id)
        {
            try
            {
                utblVideo model = db.utblVideos.Find(id);
                db.utblVideos.Remove(model);
                db.SaveChanges();
                return "Video Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }

        public IEnumerable<utblVideo> GetVideoByAlbumID(long id)
        {
            return db.utblVideos.Where(x => x.VideoAlbumID == id).ToList();
        }
    }
}
