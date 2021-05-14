using Maincotech.ExamAssistant.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Maincotech.ExamAssistant.MobileAPI
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Maincotech.ExamAssistant.MobileAPI", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<ISettingService, SettingService>();
            services.AddSingleton<IExamService>(provider =>
            {
                var settingService = AppRuntimeContext.Current.Resolve<ISettingService>();
                var context = provider.GetRequiredService<IHttpContextAccessor>();
                StringValues value = StringValues.Empty;
                if (context.HttpContext?.Request.Headers.TryGetValue("X-ExamAssistant-UserID", out value) == true)
                {
                    var firestoreSettings = settingService.GetFirebaseSetting(value.ToString()).GetAwaiter().GetResult();
                    return new ExamService(firestoreSettings.ProjectId, firestoreSettings.JsonCredentials);
                }
                throw new NotSupportedException();
            });
            AppDomain.CurrentDomain.Load(typeof(ExamService).Assembly.FullName);
            AppRuntimeContext.Current.ConfigureServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Maincotech.ExamAssistant.API v1"));
            // if (env.IsDevelopment())
            // {
            app.UseDeveloperExceptionPage();
            // app.UseSwagger();
            // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Maincotech.ExamAssistant.API v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}