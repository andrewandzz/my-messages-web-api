using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyMessages.Api.Models;
using MyMessages.Api.Validation;
using MyMessages.Data;
using MyMessages.Data.Interfaces;
using MyMessages.Data.Repositories;
using MyMessages.Logics.Interfaces;
using MyMessages.Logics.Services;
using System;
using System.Text;

namespace MyMessages.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Configuration.GetValue<string>("SecurityKey")
                        )
                    ),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile(typeof(Logics.Mapping.MapperProfile));
                config.AddProfile(typeof(Api.Mapping.MapperProfile));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IStickerService, StickerService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IStickerRepository, StickerRepository>();

            services.AddTransient<IValidator<NewMessageModel>, NewMessageModelValidator>();
            services.AddTransient<IValidator<EditMessageModel>, EditMessageModelValidator>();
            services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();
            services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();

            services.AddSpaStaticFiles(options => options.RootPath = "ClientApp/dist");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(builder =>
            {
                builder.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    builder.UseAngularCliServer("start");
                }
            });
        }
    }
}
