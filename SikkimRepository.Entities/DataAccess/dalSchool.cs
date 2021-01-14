using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalSchool
    {
        EFDBContext db = new EFDBContext();
        public List<utblSchool> GetSchoolList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblSchools.ToList();
            }
            else
            {
                return db.utblSchools.Where(x => x.SchoolName.Contains(sTerm)).ToList();
            }
        }
        public IEnumerable<MstSchoolView> GetSchoolsPaged(int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<MstSchoolView> model = db.Database.SqlQuery<MstSchoolView>("udspMstSchoolSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveSchool(utblSchool model)
        {
            try
            {
                model.UDISECode = Regex.Replace(model.UDISECode.Trim(), @"\s+", " ");
                var parSchoolID = new SqlParameter("@SchoolID", model.SchoolID);
                var parDistCode = new SqlParameter("@DistCode", model.DistCode);
                var parBlockID = new SqlParameter("@BlockID", model.BlockID);
                var parUDISECode = new SqlParameter("@UDISECode", model.UDISECode);
                var parSchoolName = new SqlParameter("@SchoolName", model.SchoolName);
                var parSchoolAddress = new SqlParameter("@SchoolAddress", model.SchoolAddress);
                var parTransDate = new SqlParameter("@TransDate", model.TransDate);
                var parUserID = new SqlParameter("@UserID", model.UserID);

                return db.Database.SqlQuery<string>("udspSchoolSave @SchoolID, @DistCode, @BlockID,@UDISECode, @SchoolName, @SchoolAddress, @TransDate, @UserID",
                    parSchoolID, parDistCode, parBlockID, parUDISECode, parSchoolName, parSchoolAddress, parTransDate, parUserID).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public utblSchool GetSchoolByID(long id)
        {
            return db.utblSchools.Where(x => x.SchoolID == id).FirstOrDefault();
        }
        public IEnumerable<utblMstBlock> GetBlockByDistrict(string code)
        {
            return db.utblMstBlocks.Where(x => x.DistCode == code).ToList();
        }
        public string DeleteSchool(long id)
        {
            try
            {
                utblSchool model = db.utblSchools.Find(id);
                db.utblSchools.Remove(model);
                db.SaveChanges();
                return "School Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
    }
}
