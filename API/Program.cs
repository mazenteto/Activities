using Application.Activities.Queries;
using Application.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaulltConnection"));
});
builder.Services.AddCors();
builder.Services.AddMediatR(x =>
                                x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>());

builder.Services.AddAutoMapper(cfg => { },typeof(MappingProfiles).Assembly);

var app = builder.Build();

app.UseCors(opt => opt.AllowAnyMethod().AllowAnyMethod().WithOrigins("http://localhost:3000","https://localhost:3000"));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDBContext>();
    await context.Database.MigrateAsync();
    await DBInitializer.SeedData(context);
}
catch (Exception ex)
{

    var loger = services.GetRequiredService<ILogger<Program>>();
    loger.LogError(ex, "An error occurred during Migration.");
}
app.Run();
