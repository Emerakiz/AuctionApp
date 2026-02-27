
using AuctionApp.Core.Interfaces;
using AuctionApp.Core.Services;
using AuctionApp.Data;
using AuctionApp.Data.Interfaces;
using AuctionApp.Data.Models;
using AuctionApp.Data.Profiles;
using AuctionApp.Data.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Threading.Tasks;

namespace AuctionApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => // Enable XML comments
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repositories and services
            builder.Services.AddScoped<IAuctionService, AuctionService>();
            builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();
            builder.Services.AddScoped<IBidRepo, BidRepo>();
            builder.Services.AddScoped<IBidService, BidService>();
           // builder.Services.AddScoped<IUserRepo, UserRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
