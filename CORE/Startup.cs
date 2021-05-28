using BeicipFranLabERP.BL.Repository;
using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.Controllers.Administrator;
using BeicipFranLabERP.Controllers.Maintenance.HealthCheckUp;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.DAL.Services;
using BeicipFranLabERP.DAL.Services.Administrator;
using BeicipFranLabERP.DAL.Services.AdminSetting;
using BeicipFranLabERP.DAL.Services.Maintenance;
using Core.DAL.Contexts;
using Core.DAL.Services;
using CORE.Encryption;
using CORE.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using School.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using COREContext = CORE.Models.COREContext;
using CORE.Models;

namespace CORE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Temporarily allow CORS from all origin
            services.AddCors(cors =>
            {
                cors.AddPolicy("AllowOrigin", opts => opts.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            //To use connection strings from appsettings and bind into Context
            services.AddDbContext<COREContext>(opt => opt.UseSqlServer
            (Configuration.GetConnectionString("CoreAPI")));
            
            //To Ignore JSON cycling
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //To Inject depedency for JWT Settings
            var jwtsection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtsection);

            //To Inject dependency for MD5 Salt
            var MD5Salt = Configuration.GetSection("MD5Salt");
            services.Configure<EncryptSalt>(MD5Salt);

            //For JWT Authentication
            //to validate the token which has been sent by clients
            var appSettings = jwtsection.Get<JWTSettings>();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x=> 
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false ,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HttpReplApi", Version = "v1" });
            });
            
            //var connectionString = Configuration["sqlserverConnection:coreDBconnectionString"];
            services.AddDbContext<CoreDBContext>(options => options.UseSqlServer((Configuration.GetConnectionString("CoreAPI"))));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IBulkExtractRepository, BulkExtractRepository>();
            services.AddControllers();
            
            // services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationOptions>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<CategoryAPI>();
            services.AddTransient<LocationAPI>();
            services.AddTransient<DepartmentAPI>();
            services.AddTransient<EventAPI>();
            services.AddTransient<StakeHolderAPI>();
            services.AddTransient<HealthBrokenLinkAPI>();
            services.AddTransient<HealthDuplcateDocAPI>();
            services.AddTransient<HealthSpecialCharacterAPI>();
            services.AddTransient<HealthToLongPathAPI>();
            services.AddTransient<HealthWithoutContentAPI>();
            services.AddTransient<WellApi>();
            services.AddTransient<ProjectTrackingAPI>();
            services.AddTransient<IndexAPI>();
            services.AddHttpClient();
            services.AddTransient<IGeneral, GeneralDAL>();
            services.AddTransient<ICategory, CategoryDAL>();
            services.AddTransient<IDepartment, DepartmentDAL>();
            services.AddTransient<IEvent, EventDAL>();
            services.AddTransient<ILocation, LocationDAL>();
            services.AddTransient<IStakeHolder, StakeHolderDAL>();
            services.AddTransient<IWell, WellDAL>();
           
            services.AddTransient<IHealth, HealthDAL>();
            services.AddTransient<IDepartment, DepartmentDAL>();
            services.AddTransient<IEvent, EventDAL>();
            services.AddTransient<ILocation, LocationDAL>();
            services.AddTransient<IStakeHolder, StakeHolderDAL>();

            services.AddTransient<ICoreSetting, CorePortalDAL>(); 
            services.AddTransient<IPortalSetting, PortalSettingDAL>();
            
            services.AddTransient<IEmailTemplate, EmailTemplateDAL>();

            services.AddTransient<IBackupManagement, BackupManagementDAL>();
            services.AddSingleton<IHostedService, ScheduledTask>();

            services.AddTransient<IProjectTracker, ProjectTrackingDAL>();

            services.AddTransient<IIndex, IndexDAL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true) 
            //if (env.IsDevelopment()) //TODO: to get the option to enable Swagger from config file
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HttpReplApi v1");
                });
            }
            else
            {
                //app.UseAuthentication();
                //app.UseAuthorization();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseAuthentication();
            app.UseRouting();
            //Temporarily allow CORS -start
            app.UseCors("AllowOrigin");
            //Temporarily allow CORS -End
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
