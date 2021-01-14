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
    public class dalTestimonial
    {
        EFDBContext db = new EFDBContext();

        public string SaveTestimonial(utblTestimonial model)
        {
            var parTestimonialID = new SqlParameter("@TestimonialID", model.TestimonialsID);
            var parName = new SqlParameter("@Name", model.Name);
            var parAddress = new SqlParameter("@Address", model.Address);
            var parImagePath = new SqlParameter("@ImagePath", model.ImagePath);
            var parRemarks = new SqlParameter("@Remarks", model.Remarks);
            var parSchoolID = new SqlParameter("@SchoolID", model.SchoolID);
            var parTestimonialDate = new SqlParameter("@TestimonialDate", model.TestimonialDate);
            var parComponentID = new SqlParameter("@ComponentID", model.ComponentID);
            var parSubComponentID = new SqlParameter("@SubComponentID", model.SubComponentID);
            var parIsPublished = new SqlParameter("@IsPublished", model.IsPublished);
            var parPublishDate = new SqlParameter("@PublishedDate", DBNull.Value);
            if (model.PublishedDate != null)
            {
                parPublishDate = new SqlParameter("@PublishedDate", model.PublishedDate);
            }
            var parTransDate = new SqlParameter("@TransDate", model.TransDate);
            var parUserID = new SqlParameter("@UserID", model.UserID);

            return db.Database.SqlQuery<string>("udspTestimonialSave @TestimonialID,@Name,@Address,@ImagePath, @Remarks,@SchoolID, @TestimonialDate,@ComponentID,@SubComponentID,@IsPublished,@PublishedDate, @TransDate, @UserID",
                parTestimonialID, parName, parAddress, parImagePath, parRemarks, parSchoolID, parTestimonialDate, parComponentID, parSubComponentID, parIsPublished, parPublishDate, parTransDate, parUserID).FirstOrDefault();
        }
        public IEnumerable<TestimonialView> GetTestimonialPaged(int PageNo, int PageSize, long SchoolID, string sTerm, out int Total)
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

            IEnumerable<TestimonialView> model = db.Database.SqlQuery<TestimonialView>("udspTestimonialSelect @Start, @PageSize,@SchoolID,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSchoolID, parSearchTerm, spOutput).ToList();
            Total = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public utblTestimonial GetTestimonialByID(long id)
        {
            return db.utblTestimonials.Where(x => x.TestimonialsID == id).FirstOrDefault();
        }
        public string DeleteTestimonial(long id)
        {
            try
            {
                utblTestimonial model = db.utblTestimonials.Find(id);
                db.utblTestimonials.Remove(model);
                db.SaveChanges();
                return "Tetimonial Removed";
            }
            catch (Exception)
            {
                return "Error: Server error, try again later";
            }
        }
        public IEnumerable<utblMstSubComponent> GetSubCompByComp(long id)
        {
            return db.utblMstSubComponents.Where(x => x.ComponentID == id).ToList();
        }

        #region general page
        public IEnumerable<GenTestimonialsView> GenTestimonialslist()
        {
            IEnumerable<GenTestimonialsView> _gmodel = db.Database.SqlQuery<GenTestimonialsView>("udspGenTestimonialsSelect").ToList();
            return _gmodel;
        }
        public TestimonialGenViewModel GenTestimonialSelectPaged(long SubComponentID, int PageNo, int PageSize, string sTerm)
        {
            TestimonialGenViewModel model = new TestimonialGenViewModel();
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

            model.TestimonialList = db.Database.SqlQuery<GenTestimonialsView>("GenTestimonialSelectAll @Start, @PageSize,@SearchTerm,@SubComponentID, @TotalCount out",
                parStart, parEnd, parSearchTerm, parSubCompID, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        public IEnumerable<GenTestimonialsByDist> GetTestimonialsByDist(string DistCode)
        {
            var parDistCode = new SqlParameter("@DistCode", DistCode);
            IEnumerable<GenTestimonialsByDist> model = db.Database.SqlQuery<GenTestimonialsByDist>("udspGenTestimonialByDistCode @DistCode", parDistCode).ToList();
            return model;
        }

        public GenViewListByDistrictCode GetTestimonialByDistCode(string DistCode, int PageNo, int PageSize, string sTerm)
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

            model.TestimonialsListByDist = db.Database.SqlQuery<GenTestimonialsByDist>("udspGetTestimonialSelectByDist @Start, @PageSize,@SearchTerm,@DistCode, @TotalCount out",
                parStart, parEnd, parSearchTerm, parDistCode, spOutput).ToList();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());

            return model;
        }
        #endregion
    }
}
