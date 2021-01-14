using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalBanner
    {
        EFDBContext db = new EFDBContext();
        public IEnumerable<utblBanner> GetBanners()
        {
            return db.utblBanners.ToList();
        }
        public string SaveBanner(utblBanner model)
        {
            var parBannerID = new SqlParameter("@BannerID", model.BannerID);
            var parBannerTitle = new SqlParameter("@BannerTitle", model.BannerTitle);
            var parDesc = new SqlParameter("@BannerDesc", model.BannerDescription);
            var parThumb = new SqlParameter("@BannerThumb", model.BannerThumb);
            var parPath = new SqlParameter("@BannerPath", model.BannerPath);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspBannerSave @BannerID, @BannerTitle,@BannerDesc, @BannerThumb, @BannerPath, @TransDate, @UserID",
                parBannerID, parBannerTitle,parDesc, parThumb, parPath, parTransDate, parUserID).FirstOrDefault();
        }
        public utblBanner GetBannerByID(long id)
        {
            return db.utblBanners.Where(x => x.BannerID == id).FirstOrDefault();
        }
        public string DeleteBanner(long id)
        {
            try
            {
                utblBanner model = db.utblBanners.Find(id);
                string bannerpath = model.BannerPath;
                db.utblBanners.Remove(model);
                db.SaveChanges();
                return bannerpath;
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
    }
}
