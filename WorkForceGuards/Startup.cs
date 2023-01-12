using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Linq;
using WorkForceGuards.Repositories;
using WorkForceGuards.Repositories.Interfaces;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Identity;
using WorkForceManagementV0.Repositories;
using WorkForceManagementV0.Repositories.ActiveDirectory;
using WorkForceManagementV0.Repositories.Config;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0
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
            // services.AddSingleton(Configuration);
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        //.SetIsOriginAllowed(origin => origin.ToLower().StartsWith("http://awwfm140:5001")) // allow any origin
                        .SetIsOriginAllowed(origin => true) // allow any origin
                        .AllowCredentials();
            }));
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("SystemRole", "Admin", "SuperUser"));
                options.AddPolicy("Hos", policy => policy.RequireClaim("SystemRole", "Hos", "SuperUser"));
                options.AddPolicy("User", policy => policy.RequireClaim("SystemRole", "User", "SuperUser"));
                options.AddPolicy("SuperUser", policy => policy.RequireClaim("SystemRole", "SuperUser"));
                options.AddPolicy("AdminOrHos", policy => policy.RequireClaim("SystemRole", "SuperUser", "Admin", "Hos"));
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IClaimsTransformation, RoleClaimsTransformer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IActiveDirectory, ActiveDirectory>();
            services.AddScoped<IAppConfig, AppConfig>();

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IAssetTypeService, AssetTypeService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAssetTermService, AssetTermService>();
            services.AddScoped<IAssetTermValueServive, AssetTermValueServive>();
            services.AddScoped<ITransportationRouteService, TransportationRouteService>();
            services.AddScoped<IStaffTypeService, StaffTypeServicecs>();
            services.AddScoped<IHeadOfSectionService, HeadOfSectionService>();
            services.AddScoped<IStaffMemberService, StaffMemberService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IScheduleWithRuleService, ScheduleWithRuleService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IDayyOffWithBreakService, DayyOffWithBreakService>();
            services.AddScoped<IForecastService, ForecastService>();
            services.AddScoped<IDailyAttendanceService, DailyAttendanceService>();
            services.AddScoped<IReportAdheranceService, ReportAdheranceService>();


            //services.AddScoped<Helpers.IFinalSchedule, SchedulerHelper>();
            services.AddScoped<ISublocationService, SublocationService>();
            services.AddScoped<IFinalScheduleService, FinalScheduleService>();
            services.AddScoped<ISwapRequestService, SwapRequestService>();
            services.AddScoped<IAttendanceTypeService, AttendanceTypeService>();
            services.AddScoped<IAdherenceService, AdherenceService>();
            services.AddScoped<IAnalysisService, AnalysisService>();
            services.AddScoped<IDailyAttendancePatternService, DailyAttendancePatternService>();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("LocalIsoConnection")));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkForceManagementV0", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddMvc()
        .ConfigureApiBehaviorOptions(options =>
        {
            //options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errorMes = "";
                var modelState = actionContext.ModelState.Values;

                foreach (var s in modelState)
                {
                    foreach (var m in s.Errors)
                    {
                        errorMes = m.ErrorMessage;
                    }

                }
                return new BadRequestObjectResult(new { ErrorMessage = errorMes });

            };
        });





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "WorkForceManagementV0 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}
