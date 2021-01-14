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
    public class dalBlock
    {
        EFDBContext db = new EFDBContext();
        public IEnumerable<utblMstBlock> GetBlocks()
        {
            return db.utblMstBlocks.ToList();
        }
        public IEnumerable<MstBlockView> GetBlocksPaged(int PageNo, int PageSize, string sTerm, out int Total)
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

            IEnumerable<MstBlockView> model = db.Database.SqlQuery<MstBlockView>("udspMstBlockSelect @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveBlock(utblMstBlock model)
        {
            var parBlockID = new SqlParameter("@BlockID", model.BlockID);
            var parDistCode = new SqlParameter("@DistCode", model.DistCode);
            var parBlockName = new SqlParameter("@BlockName", model.BlockName);
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspBlockSave @BlockID,@DistCode, @BlockName, @TransDate, @UserID",
                parBlockID,  parDistCode, parBlockName, parTransDate, parUserID).FirstOrDefault();
        }
        public utblMstBlock GetBlockByID(long id)
        {
            return db.utblMstBlocks.Where(x => x.BlockID == id).FirstOrDefault();
        }
        public IEnumerable<utblMstBlock> GetBlockByDistrict(string code)
        {
            return db.utblMstBlocks.Where(x => x.DistCode == code).ToList();
        }
        public string DeleteBlock(long id)
        {
            try
            {
                utblMstBlock model = db.utblMstBlocks.Find(id);
                db.utblMstBlocks.Remove(model);
                db.SaveChanges();
                return "Block Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
        public List<utblMstBlock> GetBlockList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblMstBlocks.ToList();
            }
            else
            {
                return db.utblMstBlocks.Where(x => x.BlockName.Contains(sTerm)).ToList();
            }
        }
    }
}
