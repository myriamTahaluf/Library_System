using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Library_System.Model;
using Library_System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using Library_System.Repositories.Generic.Interface;
using Library_System.Repositories.Generic;
using Library_System.Services.AuthorizationService;
using Library_System.AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>());

builder.Services.Configure<CookiePolicyOptions>(options =>
{

    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SwaggerAnnotations.xml"), includeControllerXmlComments: true);
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
}
);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationUserService, AuthorizationUserService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddDbContext<LibraryContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LibraryConnection"), o => o.MigrationsAssembly("Model")));

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.SaveToken = true;

    });
    builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperConfiguration>();
}).CreateMapper());

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
              .Where(a => a.FullName.Contains($"{nameof(Library_System.Services)}") ||
                          a.FullName.Contains($"{nameof(Library_System.Repositories)}"));

var llist = assemblies.Select(e => e.FullName).ToList();
builder.Services.Scan(scan =>
                scan.FromAssemblies(assemblies)
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithScopedLifetime());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization(); 
app.UseEndpoints(endpoints =>
{ endpoints.MapControllers(); }
);
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
          ForwardedHeaders.XForwardedProto
});
app.UseStaticFiles();

app.Run();
