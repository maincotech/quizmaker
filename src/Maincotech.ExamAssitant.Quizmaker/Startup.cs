using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AntDesign.Pro.Layout;
using Maincotech.Quizmaker.Services;
using ElectronNET.API;
using Maincotech.Web.Components.Vditor;
using Maincotech.Storage;
using Maincotech.Storage.AzureBlob;
using Maincotech.ExamAssitant.Services;

namespace Maincotech.Quizmaker
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();
			services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(sp.GetService<NavigationManager>().BaseUri)
            });
            services.Configure<ProSettings>(Configuration.GetSection("ProSettings"));
            services.Configure<AzSettings>(Configuration.GetSection("AzSettings"));
            services.Configure<FirestoreSettings>(Configuration.GetSection("FirestoreSettings"));

            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();

            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IExamService, ExamService>();

            services.AddSingleton<IBlobStorage, AzureBlobStorage>(sp =>
            {
                return new AzureBlobStorage(Configuration.GetConnectionString("azureStorage"));
            });
            services.AddScoped<IVditorService, VditorService>(sp =>
            {
                var blobStorage = sp.GetRequiredService<IBlobStorage>();
                return new VditorService(blobStorage, "https://samaincotechportal.blob.core.windows.net/images/");
            });
            AppRuntimeContext.Current.ConfigureServices(services, Configuration);
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllerRoute("api", "api/{controller}/{action}/{id?}");
                endpoints.MapFallbackToPage("/_Host");
            });


            // Open the Electron-Window here
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());

        }
    }
}
