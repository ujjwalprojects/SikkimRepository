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
    public class dalCompCategory
    {
        private EFDBContext db = new EFDBContext();
        public List<utblMstCompCategorie> GetCategoryList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblMstCompCategories.ToList();
            }
            else
            {
                return db.utblMstCompCategories.Where(x => x.CompCategoryName.Contains(sTerm)).ToList();
            }
        }
        public IEnumerable<utblMstCompCategorie> GetCategoryPaged(int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<utblMstCompCategorie> model = db.Database.SqlQuery<utblMstCompCategorie>("udspMstCompCategorySelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveCategory(utblMstCompCategorie model)
        {
            var parCategoryID = new SqlParameter("@CategoryID", model.CompCategoryID);
            var parCategoryName = new SqlParameter("@CategoryName", model.CompCategoryName);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspCompCategorySaveUpdate @CategoryID, @CategoryName,  @UserID",
            parCategoryID, parCategoryName, parUserID).FirstOrDefault();
            //else
            //{
            //    var parCategoryID = new SqlParameter("@CategoryID", model.CompCategoryID);
            //    return db.Database.SqlQuery<string>("udspCategoryUpdate @CategoryID, @CategoryName,  @UserID",
            //    parCategoryID, parCategoryName,  parUserID).FirstOrDefault();
            //}

        }
        public utblMstCompCategorie GetCategoryByID(long id)
        {
            return db.utblMstCompCategories.Where(x => x.CompCategoryID == id).FirstOrDefault();
        }
        public string DeleteCategory(long id)
        {
            try
            {
                utblMstCompCategorie model = db.utblMstCompCategories.Find(id);
                db.utblMstCompCategories.Remove(model);
                db.SaveChanges();
                return "Category Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }

        public IEnumerable<CategoryDD> GetCategoryDD()
        {
            return db.utblMstCompCategories.Select(x => new CategoryDD()
            {
                CompCategoryID = x.CompCategoryID,
                CompCategoryName = x.CompCategoryName
            }).ToList();
        }

    }
}
