using ElectronNET.API;
using Maincotech.ExamAssistant.Maker.Services;
using Maincotech.Storage;
using Maincotech.Storage.AzureBlob;
using Maincotech.Web.Components.JQuery;
using Maincotech.Web.Components.Vditor;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Maker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
            });

            // Configuration to sign-in users with Azure AD B2C
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration, "AzureAdB2C");

            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            //Configuring appsettings section AzureAdB2C, into IOptions
            services.AddOptions();
            services.Configure<OpenIdConnectOptions>(Configuration.GetSection("AzureAdB2C"));

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(sp.GetService<NavigationManager>().BaseUri)
            });
            //   services.Configure<ProSettings>(Configuration.GetSection("ProSettings"));
            //services.Configure<AzSettings>(Configuration.GetSection("AzSettings"));
            //services.Configure<FirestoreSettings>(Configuration.GetSection("FirestoreSettings"));

            services.AddScoped<IUserService, UserService>();

            // services.AddScoped(sp =>
            //{
            //    using var scope = sp.CreateScope();
            //    var userSerive = scope.ServiceProvider.GetRequiredService<IUserService>();
            //    var user = userSerive.GetCurrentUserAsync().GetAwaiter().GetResult();
            //    return new ExamAssistantClient("https://localhost:44369/", new HttpClient()) { CurrentUser = user };
            //});

            services.AddScoped<ExamAssistantClient>(sp =>
            {
                return new ExamAssistantClient(Configuration.GetValue<string>("APIBaseUrl"), new HttpClient());
            });

            services.AddSingleton<IBlobStorage, AzureBlobStorage>(sp =>
            {
                return new AzureBlobStorage(Configuration.GetConnectionString("azureStorage"));
            });
            services.AddScoped<IVditorService, VditorService>(sp =>
            {
                var blobStorage = sp.GetRequiredService<IBlobStorage>();
                return new VditorService(blobStorage, "https://examassistant.blob.core.windows.net/assets/");
            });

            services.AddScoped<IJQueryService, JQueryService>(sp =>
             {
                 var blobStorage = sp.GetRequiredService<IBlobStorage>();
                 return new JQueryService(blobStorage, "https://examassistant.blob.core.windows.net/assets/");
             });

            AppRuntimeContext.Current.ConfigureServices(services, Configuration);
            //  services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //  name: "Account",
                //  pattern: "Account/{action}/{id?}");
                endpoints.MapControllerRoute("api", "api/{controller}/{action}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Open the Electron-Window here
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
        }
    }
}