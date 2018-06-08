using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ArchBook.Data;
using ArchBook.Services.Books;
using ArchBook.Services.Pilotage.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling.EntityFramework6;
using StackExchange.Profiling.Storage;

namespace ArchBook.Web2
{
    public class Startup
    {
        ILogger logger;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Application starting.");

            this.Configuration = configuration;
            this.logger = logger;

            // Set up data directory
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(env.ContentRootPath, "App_Data"));
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                options.PopupShowTrivial = true;
                options.PopupShowTimeWithChildren = true;
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddViewOptions(setup =>
                {
                    setup.HtmlHelperOptions.ClientValidationEnabled = false;
                });

            services.AddRouting(setup =>
            {
                setup.LowercaseUrls = true;
            });

            services.AddScoped<BookDbContext>(_ => new BookDbContext(Configuration.GetConnectionString("BookConnectionString")));
            services.AddScoped<BookService, BookService>();
            services.AddScoped<BookPilotageService, BookPilotageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            app.UseSerilogUserNameEnricher();

            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();
                MiniProfilerEF6.Initialize();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/errors/{0}/");
                app.UseExceptionHandler("/errors/error500");
            }

            app.UseStaticFiles();
            app.UseMvc();            

            applicationLifetime.ApplicationStarted.Register(ApplicationStarted);
            applicationLifetime.ApplicationStopped.Register(ApplicationStopped);
        }

        private void ApplicationStarted()
        {
            logger.LogInformation("Application started.");
        }

        private void ApplicationStopped()
        {
            logger.LogInformation("Application stopped.");
        }
    }
}
