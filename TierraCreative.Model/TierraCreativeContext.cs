using TierraCreative.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace TierraCreative
{
    public class TierraCreativeContext : DbContext
    {
        static TierraCreativeContext()
        {
            Database.SetInitializer<TierraCreativeContext>(null);
        }

        public TierraCreativeContext()
            : base("Name=TierraCreativeConn")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 120;
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<DRP> DRPs { get; set; }
        public virtual DbSet<AIL> AILs { get; set; }
        public virtual DbSet<SupplementaryDividend> SupplementaryDividends { get; set; }
        public virtual DbSet<CSNLookUp> CSNLookUps { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }
    }
}
