using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Jingl.Web.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Jingl
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
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.AllowAnyOrigin());
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
               // options.AddPolicy("ApiCorsPolicy",
                
                builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                        //.WithOrigins(new string[] { "https://www.fameoapp.com/", "https://fameoapp-reactjs.azurewebsites.net", "https://fameo-staging-fe.azurewebsites.net" });
                        //.WithOrigins("http");
                    });
            });

            //services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            //{
            //    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            //}));

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddCookieManager();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // this lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });



            //services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddAuthentication()
            //.AddCookie("Application")
            //.AddCookie("External").
            .AddGoogle(googleOptions =>
            {
                //googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                //googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                googleOptions.ClientId = Configuration.GetSection("Google:ClientId").Value;
                googleOptions.ClientSecret = Configuration.GetSection("Google:ClientSecret").Value;
                googleOptions.Events.OnRemoteFailure = (context) =>
                {
                    context.Response.Redirect("/home/index");
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };

            }).AddFacebook(facebookOptions =>
            {
                //facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                //facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.AppId = Configuration.GetSection("Facebook:AppId").Value;
                facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
                facebookOptions.Events.OnRemoteFailure = (context) =>
                {
                    context.Response.Redirect("/home/index");
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };
            });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerDocument();
            services.Configure<FormOptions>(x =>
            {
                //x.ValueLengthLimit = int.MaxValue;
                //x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                //x.ValueCountLimit = 10; //default 1024
                //x.ValueLengthLimit = int.MaxValue; //not recommended value
                //x.MultipartBodyLengthLimit = long.MaxValue; //not recommended value

                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;

            });
            //services.AddMvc();
            services.AddProgressiveWebApp();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-ID");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());

            // app.UseCors("ApiCorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
              //  app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            // app.UseCookiePolicy();
            app.UseSession();


            //app.UseGoogleAuthentication(new GoogleOptions
            //{
            //    AuthenticationScheme = "Google",
            //    ClientId = Configuration["Authentication:Google:ClientId"],
            //    ClientSecret = "YOUR_CLIENT_SECRET",
            //    CallbackPath = new PathString("/signin-google"),
            //    SignInScheme = "MainCookie"
            //});



            //app.UseMvc(routes =>
            //{

            //    routes.MapRoute(
            //        name: "default",
            //    // Untuk Admin Panel

            //    //template: "{controller=AdmAccount}/{action=Login}/{id?}");

            //    // Untuk Login
            //    //template: "{controller=Account}/{action=OnBoarding}/{id?}");
            //    //template: "{controller=Home}/{action=welcome}/{id?}");
            //      template: "{controller=Home}/{action=Welcome}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Home}/{action=Index}/{id?}");
                    template: "{controller=AdmAccounts}/{action=Login}/{id?}");
            });
        }
    }
}
