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
    public class dalVillage
    {
        EFDBContext db = new EFDBContext();
        public IEnumerable<MstVillageView> GetVillagesPaged(int PageNo, int PageSize, string sTerm, out int Total)
        {
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            IEnumerable<MstVillageView> model = db.Database.SqlQuery<MstVillageView>("udspMstVillageSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveVillage(utblMstVillage model)
        {
            var parVillageID = new SqlParameter("@VillageID", model.VillageID);
            var parVillageName = new SqlParameter("@VillageName", model.VillageName);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspVillageSave @VillageID, @VillageName, @TransDate, @UserID",
                parVillageID,  parVillageName, parTransDate, parUserID).FirstOrDefault();
        }
        public string DeleteVillage(long id)
        {
            try
            {
                utblMstVillage model = db.utblMstVillages.Find(id);
                db.utblMstVillages.Remove(model);
                db.SaveChanges();
                return "Village Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
        public utblMstVillage GetVillageByID(long id)
        {
            return db.utblMstVillages.Where(x => x.VillageID == id).FirstOrDefault();
        }
        public IEnumerable<utblMstVillage> GetAllVillages()
        {
            return db.utblMstVillages.ToList();
        }
    }
}
