using MjultiTenancy.Settings;

namespace MjultiTenancy.Services
{
    public interface ITenantService

    {
        public string? GetDataBaseProvider();
        public string? GetConnectionString();
        public Tenant? GetTenant();



    }
}
