using Jingl.General.Model.User.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jingl.General.Model.Admin.Transaction;
using Jingl.General.Model.Admin.Master;
using Jingl.General.Model.Admin.UserManagement;

namespace Jingl.Web.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DeviceModel> Devices { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }



        public DbSet<Jingl.General.Model.Admin.Transaction.BookModel> BookModel { get; set; }



        public DbSet<Jingl.General.Model.Admin.Master.ParameterModel> ParameterModel { get; set; }



        public DbSet<Jingl.General.Model.Admin.Transaction.ClaimModel> ClaimModel { get; set; }



        public DbSet<Jingl.General.Model.Admin.Transaction.RefundModel> RefundModel { get; set; }



        public DbSet<Jingl.General.Model.Admin.Master.BannerModel> BannerModel { get; set; }


        //public DbSet<Jingl.General.Model.Admin.Master.VoucherModel> VoucherModel { get; set; }


        public DbSet<Jingl.General.Model.Admin.UserManagement.RoleModel> RoleModel { get; set; }
    }

    
}
