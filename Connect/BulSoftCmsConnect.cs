using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CoreCnice.Domain;
using System.Configuration;

namespace CoreCnice.Connect
{
     public class BulSoftCmsConnectContext : DbContext
    {
        //public BulSoftCmsConnectContext(DbContextOptions<BulSoftCmsConnectContext> options)
        //: base(options)
        //{ }
        //protected BulSoftCmsConnectContext()
        //{
        //}

        public DbSet<ESHOP_CATEGORIES> ESHOP_CATEGORIES { get; set; }
        public DbSet<ESHOP_STORE> ESHOP_STORE { get; set; }
        public DbSet<ESHOP_NEWS> ESHOP_NEWS { get; set; }
        public DbSet<ESHOP_NEWS_CAT> ESHOP_NEWS_CAT { get; set; }
        public DbSet<ESHOP_NEWS_IMAGE> ESHOP_NEWS_IMAGE { get; set; }
        public DbSet<ESHOP_NEWS_COMMENT> ESHOP_NEWS_COMMENT { get; set; }
        public DbSet<ESHOP_NEWS_PROPERTIES> ESHOP_NEWS_PROPERTIES { get; set; }

        public DbSet<ESHOP_CUSTOMER_PROPERTIES> ESHOP_CUSTOMER_PROPERTIES { get; set; }
        public DbSet<ESHOP_UNITS> ESHOP_UNITS { get; set; }
        public DbSet<ESHOP_USER_GIFT> ESHOP_USER_GIFT { get; set; }
        public DbSet<ESHOP_NEWS_ATT> ESHOP_NEWS_ATT { get; set; }
        public DbSet<ESHOP_AD_ITEM> ESHOP_AD_ITEM { get; set; }
        public DbSet<ESHOP_AD_ITEM_CAT> ESHOP_AD_ITEM_CAT { get; set; }
        public DbSet<ESHOP_BASKET> ESHOP_BASKET { get; set; }
        public DbSet<ESHOP_CONFIG> ESHOP_CONFIG { get; set; }
        public DbSet<ESHOP_CONTACT> ESHOP_CONTACT { get; set; }
        public DbSet<ESHOP_EMAIL> ESHOP_EMAIL { get; set; }
        public DbSet<ESHOP_GIFT> ESHOP_GIFT { get; set; }
        public DbSet<ESHOP_GROUP> ESHOP_GROUP { get; set; }
        public DbSet<ESHOP_GROUP_CAT> ESHOP_GROUP_CAT { get; set; }
        public DbSet<ESHOP_ONLINE> ESHOP_ONLINE { get; set; }
        public DbSet<ESHOP_ORDER> ESHOP_ORDER { get; set; }
        public DbSet<ESHOP_ORDER_ITEM> ESHOP_ORDER_ITEM { get; set; }
        public DbSet<ESHOP_USER> ESHOP_USER { get; set; }
        public DbSet<ESHOP_CUSTOMER> ESHOP_CUSTOMER { get; set; }
        public DbSet<MODULE> MODULE { get; set; }
        public DbSet<ESHOP_COMPA> ESHOP_COMPA { get; set; }
        public DbSet<ESHOP_MONEY> ESHOP_MONEY { get; set; }
        public DbSet<ESHOP_PROPERTIES> ESHOP_PROPERTIES { get; set; }
        public DbSet<ESHOP_THEME> ESHOP_THEME { get; set; }
        public DbSet<ESHOP_WEBLINKS> ESHOP_WEBLINKS { get; set; }
        public DbSet<ESHOP_NEWS_NEWS> ESHOP_NEWS_NEWS { get; set; }
        public DbSet<ESHOP_COLOR> ESHOP_COLOR { get; set; }
        public DbSet<ESHOP_CAT_PRO> ESHOP_CAT_PRO { get; set; }

        public DbSet<PRO_DESC> PRO_DESC { get; set; }

        public DbSet<Customer_Request> Customer_Request { get; set; }

        public DbSet<NEWS_CUSTOMER_POST> NEWS_CUSTOMER_POST { get; set; }

        public DbSet<CustomerConfirm> CustomerConfirm { get; set; }


        //public DbSet<ESHOP_NEWS_CAT> ESHOP_NEWS_CATs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=.;Database=TEPO_SAVIHOUSE;user id=sa;password=1234567890;Trusted_Connection=True;ConnectRetryCount=0");
            //optionsBuilder.UseSqlServer(@"Server=112.78.15.236;Database=admin_monday_db;user id=sa;password=sA@89SIS77;");
            //optionsBuilder.UseSqlServer(@"Server=.;Database=admin_monday_db;user id=sa;password=sA@89SIS77;");
            optionsBuilder.UseSqlServer(@"Server=112.78.15.233;Database=admin_hcm_rental;user id=admin_hcm_rental;password=Luongngocanh@123;");
        }
    }
}
