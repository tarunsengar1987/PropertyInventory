using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PropertyInventory.Data;
using PropertyInventory.Interfaces.Repository;
using PropertyInventory.Interfaces.Services;
using PropertyInventory.Repository;
using PropertyInventory.Services;
using PropertyInventory.Utils.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<PropertyInventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories (data layer)
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Services (business layer)
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Property Inventory System API",
        Version = "v1",
        Description = "REST API for managing Properties, Contacts, Ownership history and Price history."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowAnyOrigin());
});

var app = builder.Build();

// Global exception handling (must be early in pipeline)
app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true);
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
