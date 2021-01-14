using SikkimRepository.Entities.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalUser
    {
        EFDBContext db = new EFDBContext();

        public IEnumerable<RegisteredUserView> GetUsersPaged(int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<RegisteredUserView> model = db.Database.SqlQuery<RegisteredUserView>("udspSchoolUserSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public MyProfile GetProfileDetails(string id)
        {
            var parUserID = new SqlParameter("@UserID", id);
            return db.Database.SqlQuery<MyProfile>("udspGetUserProfile @UserID", parUserID).FirstOrDefault();
        }
    }
}
