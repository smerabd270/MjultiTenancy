namespace MjultiTenancy.Contract
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; } 
    }
}
