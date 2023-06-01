using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllJobsRestAPI.DAL
{
    //public class RestAPIDBContext : DbContext
    //{

    //    //public RestAPIDBContext(DbContextOptions<RestAPIDBContext> options)
    //    //    : base(options)
    //    //{
    //    //}

    //    //// Define your DbSet properties for each entity in your database
    //    ////public DbSet<AllJobsRestApi> YourEntities { get; set; }

    //    public class order
    //    {
    //        public string order_id { get; set; }
    //    }

    //    public DbSet<order> Orders { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            optionsBuilder.UseSqlServer("Data Source=ALEX-PC;Initial Catalog=AllJobsDB;");
    //        }
    //    }
    //}

    //public class CatalogContext : DbContext
    //{
    //    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    //    {

    //    }

    //    public DbSet<CatalogItem> CatalogItems { get; set; }
    //    public DbSet<CatalogBrand> CatalogBrands { get; set; }
    //    public DbSet<CatalogType> CatalogTypes { get; set; }
    //}

}
