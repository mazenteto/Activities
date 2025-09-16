using API.Middleware;
using Application.Activities.Queries;
using Application.Activities.Validators;
using Application.Core;
using FluentValidation;
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
{
    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    x.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
builder.Services.AddTransient<ExceptionMiddleware>();
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(opt =>
        opt.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
        );
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
