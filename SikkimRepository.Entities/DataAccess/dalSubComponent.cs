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
    public class dalSubComponent
    {
        EFDBContext db = new EFDBContext();
        public List<utblMstSubComponent> GetSubComponentList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblMstSubComponents.ToList();
            }
            else
            {
                return db.utblMstSubComponents.Where(x => x.SubComponentName.Contains(sTerm)).ToList();
            }
        }
        public IEnumerable<utblMstSubComponent> GetSubComponent()
        {
            return db.utblMstSubComponents.ToList();
        }
        public IEnumerable<MstSubComponentView> GetSubComponentPaged(int PageNo, int PageSize, string sTerm, out int Total)
        {
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

            IEnumerable<MstSubComponentView> model = db.Database.SqlQuery<MstSubComponentView>("udspMstSubComponentSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveSubComponent(utblMstSubComponent model)
        {
            var parSubComponentID = new SqlParameter("@SubComponentID", model.SubComponentID);
            var parSubComponentName = new SqlParameter("@SubComponentName", model.SubComponentName);
            var parComponentID = new SqlParameter("@ComponentID", model.ComponentID);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspSubComponentSave @SubComponentID,@SubComponentName, @ComponentID, @TransDate, @UserID",
               parSubComponentID , parSubComponentName, parComponentID, parTransDate, parUserID).FirstOrDefault();
        }
        public utblMstSubComponent GetSubComponentByID(long id)
        {
            return db.utblMstSubComponents.Where(x => x.SubComponentID == id).FirstOrDefault();
        }
        public List<utblMstSubComponent> GetSubCompByCompID(long CompID)
        {
            List<utblMstSubComponent> model = db.utblMstSubComponents.Where(x => x.ComponentID == CompID).ToList();
            return model;
        }
        public string DeleteSubComponent(long id)
        {
            try
            {
                utblMstSubComponent model = db.utblMstSubComponents.Find(id);
                db.utblMstSubComponents.Remove(model);
                db.SaveChanges();
                return "SubComponent Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
    }
}
