using Microsoft.EntityFrameworkCore;
using MjultiTenancy.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IProductService, ProductService>();  
// Add services to the container.b
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
TenantSettings optionts = new ();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(optionts) ;
var defualtDbProvider = optionts.Defaults.DbProvider;
if(defualtDbProvider .ToLower()=="mssql")
{
    builder.Services.AddDbContext<ApplicationDbContext>((m => m.UseSqlServer()));
}
foreach(var tenant in optionts.Tenants)
{
    var connectionString =tenant.ConnectionString??optionts.Defaults.ConnectionString;
   using  var scope =builder.Services.BuildServiceProvider().CreateScope();
    var dbContext=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.SetConnectionString(connectionString);   
    if(dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }

}
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
