using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class EFDBContext: DbContext
    {
        public DbSet<utblBanner> utblBanners { get; set; }
        public DbSet<utblCaseStudie> utblCaseStudies { get; set; }
        public DbSet<utblImage> utblImages { get; set; }
        public DbSet<utblImageAlbum> utblImageAlbums { get; set; }
        public DbSet<utblMstBlock> utblMstBlocks { get; set; }
        public DbSet<utblMstCluster> utblMstClusters { get; set; }
        public DbSet<utblMstComponent> utblMstComponents { get; set; }
        public DbSet<utblMstDistrict> utblMstDistricts { get; set; }
        public DbSet<utblMstSubComponent> utblMstSubComponents { get; set; }
        public DbSet<utblMstVillage> utblMstVillages { get; set; }
        public DbSet<utblSchool> utblSchools { get; set; }
        public DbSet<utblTestimonial> utblTestimonials { get; set; }
        public DbSet<utblVideo> utblVideos { get; set; }
        public DbSet<utblVideoAlbum> utblVideoAlbums { get; set; }
        public DbSet<utblMstCompCategorie> utblMstCompCategories { get; set; }

    }
}
