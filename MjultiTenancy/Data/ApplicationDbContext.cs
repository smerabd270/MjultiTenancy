using Microsoft.EntityFrameworkCore;

namespace MjultiTenancy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()!?.TId;
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder)
        {
            var tenatConnectionString = _tenantService.GetConnectionString();
            if(!string.IsNullOrEmpty(tenatConnectionString))
            {
                var dbProvider=_tenantService.GetDataBaseProvider();
                if(dbProvider?.ToLower()=="mssql")
                {
                    optionsBuilder.UseSqlServer(tenatConnectionString);
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == TenantId);
            base.OnModelCreating(modelBuilder);

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {

                entry.Entity.TenantId = TenantId;
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
