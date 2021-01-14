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
    public class dalCaseStudy
    {
        EFDBContext db = new EFDBContext();

        public string SaveCaseStudy(utblCaseStudie model)
        {
            var parCaseStudyID = new SqlParameter("@CaseStudyID", model.CaseStudyID);
            var parCaseStudyTitle = new SqlParameter("@CaseStudyTitle", model.CaseStudyTitle);
            var parCaseStudyDesc = new SqlParameter("@CaseStudyDesc", model.CaseStudyDesc);
            var parSchoolID = new SqlParameter("@SchoolID", model.SchoolID);
            var parComponentID = new SqlParameter("@ComponentID", model.ComponentID);
            var parSubComponentID = new SqlParameter("@SubComponentID", model.SubComponentID);
            var parCaseStudyFilePath = new SqlParameter("@CaseStudyFilePath", model.CaseStudyFilePath);
            var parCaseStudyDate = new SqlParameter("@CaseStudyDate", model.CaseStudyDate);
            var parIsPublished = new SqlParameter("@IsPublished", model.IsPublished);
            var parPublishDate = new SqlParameter("@PublishedDate", DBNull.Value);
            if (model.PublishedDate != null)
            {
                parPublishDate = new SqlParameter("@PublishedDate", model.PublishedDate);
            }
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspCaseStudySave @CaseStudyID,@CaseStudyTitle,@CaseStudyDesc,@SchoolID,@ComponentID,@SubComponentID,@CaseStudyFilePath,@CaseStudyDate,@IsPublished,@PublishedDate, @TransDate, @UserID",
                parCaseStudyID, parCaseStudyTitle, parCaseStudyDesc, parSchoolID, parComponentID, parSubComponentID, parCaseStudyFilePath, parCaseStudyDate, parIsPublished, parPublishDate, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<CaseStudyView> GetCaseStudyPaged(int PageNo, int PageSize, long SchoolID, string sTerm, out int Total)
        {
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parSchoolID = new SqlParameter("@SchoolID", SchoolID);
            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            IEnumerable<CaseStudyView> model = db.Database.SqlQuery<CaseStudyView>("udspCaseStudySelect @Start, @PageSize,@SchoolID,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSchoolID, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblCaseStudie GetCaseStudyByID(long id)
        {
            return db.utblCaseStudies.Where(x => x.CaseStudyID == id).FirstOrDefault();
        }
        public IEnumerable<utblMstSubComponent> GetSubCompByComp(long id)
        {
            return db.utblMstSubComponents.Where(x => x.ComponentID == id).ToList();
        }
        public string DeleteCaseStudy(long id)
        {
            try
            {
                utblCaseStudie model = db.utblCaseStudies.Find(id);
                db.utblCaseStudies.Remove(model);
                db.SaveChanges();
                return "CaseStudy Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }

        #region general page
        public IEnumerable<GenCaseStudyView> GenCaseStudylist()
        {
            IEnumerable<GenCaseStudyView> _gmodel = db.Database.SqlQuery<GenCaseStudyView>("udspGenCaseStudiesSelect").ToList();
            return _gmodel;
        }
        public CaseStudyGenViewModel GenCaseStudySelectPaged(long SubComponentID, int PageNo, int PageSize, string sTerm)
        {
            CaseStudyGenViewModel model = new CaseStudyGenViewModel();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parSubCompID = new SqlParameter("@SubComponentID", DBNull.Value);
            if (SubComponentID != 0)
                parSubCompID = new SqlParameter("@SubComponentID", SubComponentID);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.CaseStudyList = db.Database.SqlQuery<GenCaseStudyView>("GenCaseStudiesSelectAll @Start, @PageSize,@SearchTerm,@SubComponentID, @TotalCount out",
                parStart, parEnd, parSearchTerm, parSubCompID, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        public IEnumerable<GenCaseStudyByDist> GetCaseStudyByDist(string DistCode)
        {
            var parDistCode = new SqlParameter("@DistCode", DistCode);
            IEnumerable<GenCaseStudyByDist> model = db.Database.SqlQuery<GenCaseStudyByDist>("udspGenCaseStudyByDistCode @DistCode", parDistCode).ToList();
            return model;
        }
        public GenViewListByDistrictCode GetCaseStudyByDistCode(string DistCode, int PageNo, int PageSize, string sTerm)
        {
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            var parStart = new SqlParameter("@Start", (PageNo - 1) * PageSize);
            var parEnd = new SqlParameter("@PageSize", PageSize);
            //return result;
            var parDistCode = new SqlParameter("@DistCode", DistCode);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sTerm == null || sTerm == ""))
                parSearchTerm.Value = sTerm;
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.CaseStudyListByDist = db.Database.SqlQuery<GenCaseStudyByDist>("udspGetCaseStudySelectByDist @Start, @PageSize,@SearchTerm,@DistCode, @TotalCount out",
                parStart, parEnd, parSearchTerm, parDistCode, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        #endregion
    }
}
