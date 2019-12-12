namespace TierraCreative.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema; 
    using System.Linq;

    public partial class Model : DbContext
    {
        static Model()
        {
            Database.SetInitializer<Model>(null);
        }

        public Model()
            : base("name=TierraCreativeConn")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 120;
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Role>()
        //        .Property(e => e.RoleName)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.UserName)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Email)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Password)
        //        .IsUnicode(false);
        //}
    }
}
