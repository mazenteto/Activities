using API.Middleware;
using API.SignalR;
using Application.Activities.Queries;
using Application.Activities.Validators;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddDbContext<AppDBContext>(opt =>
{
    //opt.UseSqlite(builder.Configuration.GetConnectionString("DefaulltConnection"));
    
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaulltConnection"));
});
builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>();
    x.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddScoped<IPhotoService,PhotoService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddIdentityApiEndpoints<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDBContext>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsActivityHost", policy =>
    {
        policy.Requirements.Add(new IsHostRequirement());
    });
});
builder.Services.AddTransient<IAuthorizationHandler, IsHostRequirementJandler>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration
    .GetSection("CloudinarySettings"));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(opt =>
        opt.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
        );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles(); 

app.MapControllers();
app.MapGroup("api").MapIdentityApi<User>();// api/login
app.MapHub<CommentHub>("/comments");
app.MapFallbackToController("Index","Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDBContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await DBInitializer.SeedData(context,userManager);
}
catch (Exception ex)
{

    var loger = services.GetRequiredService<ILogger<Program>>();
    loger.LogError(ex, "An error occurred during Migration.");
}
app.Run();
