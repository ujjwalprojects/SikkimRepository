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
    public class dalComponent
    {
        EFDBContext db = new EFDBContext();
        public List<utblMstComponent> GetComponentList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblMstComponents.ToList();
            }
            else
            {
                return db.utblMstComponents.Where(x => x.ComponentName.Contains(sTerm)).ToList();
            }
        }
        public IEnumerable<ComponentView> GetComponentPaged(int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<ComponentView> model = db.Database.SqlQuery<ComponentView>("udspMstComponentSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveComponent(utblMstComponent model)
        {
            var parComponentID = new SqlParameter("@ComponentID", model.ComponentID);
            var parComponentName = new SqlParameter("@ComponentName", model.ComponentName);
            var parCategoryID = new SqlParameter("@CategoryID", model.CompCategoryID);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspComponentSave @ComponentID,@ComponentName, @CategoryID, @UserID",
                parComponentID, parComponentName, parCategoryID, parUserID).FirstOrDefault();
        }
        public utblMstComponent GetComponentByID(long id)
        {
            return db.utblMstComponents.Where(x => x.ComponentID == id).FirstOrDefault();
        }
        public string DeleteComponent(long id)
        {
            try
            {
                utblMstComponent model = db.utblMstComponents.Find(id);
                db.utblMstComponents.Remove(model);
                db.SaveChanges();
                return "Component Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }

        public IEnumerable<GenComponentModel> GetGenComponentList()
        {
            IEnumerable<GenComponentModel> model = db.Database.SqlQuery<GenComponentModel>("udspGetGenComponentList").ToList();
            return model;
        }
    }
}
