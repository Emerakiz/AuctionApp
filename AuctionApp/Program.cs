using AuctionApp.Core.Interfaces;
using AuctionApp.Core.Services;
using AuctionApp.Data;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Profiles;
using AuctionApp.Data.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace AuctionApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            

            builder.Services
                 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                         ValidAudience = builder.Configuration["JwtSettings:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                     };
                 });

            builder.Services.AddAuthorization();

            // Swagger configuration
            builder.Services.AddSwaggerGen(c => 
            {
                // Enable XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                
            });

            // Connection string and DbContext configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories and services
            builder.Services.AddScoped<IAuctionService, AuctionService>();
            builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();
            builder.Services.AddScoped<IBidRepo, BidRepo>();
            builder.Services.AddScoped<IBidService, BidService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                {
                    policy.WithOrigins("http://localhost:5103")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();

            app.UseCors("AllowReact");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();




            app.Run();
        }
    }
}
