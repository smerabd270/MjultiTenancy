using Microsoft.Extensions.Options;
using MjultiTenancy.Settings;

namespace MjultiTenancy.Services
{
    public class TenantService: ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private Tenant? _currentTenant;
        private HttpContext? _httpContext;
        public TenantService(IHttpContextAccessor  httpContextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = httpContextAccessor.HttpContext;
            if(_httpContext is not null)
            {
                if(_httpContext.Request.Headers.TryGetValue("tenant",out var tenantId))
                {
                    SetCurrentTenant(tenantId);
                }
                else
                {
                    throw new Exception("No Tenant Found");
                }
            }
        }
        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null ? _tenantSettings.Defaults.ConnectionString
                : _currentTenant.ConnectionString;
            return currentConnectionString;
        }

        public string? GetDataBaseProvider()
        {
            return _tenantSettings.Defaults.DbProvider;
        }

        public Tenant? GetTenant()
        {
            return _currentTenant;
        }
        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.Where(t => t.TId == tenantId).FirstOrDefault();
            if (_currentTenant is null)
            {
                throw new Exception("Invalid TenantId");
            }
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;

            }
        }

    }
}
