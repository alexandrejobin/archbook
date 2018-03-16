using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArchBook.Data;
using ArchBook.Services.Books;
using ArchBook.Services.Pilotage.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static ArchBook.Web2.Controllers.Pilotage.Books.BooksPilotageController;
using static ArchBook.Web2.ViewResultExtensions;

namespace ArchBook.Web2
{
    public class Startup
    {
        ILogger logger;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Application starting.");

            this.logger = logger;            
            Configuration = configuration;            

            // Set up data directory
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(env.ContentRootPath, "App_Data"));
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddViewOptions(setup =>
                {
                    setup.HtmlHelperOptions.ClientValidationEnabled = false;
                });

            services.AddRazorViewRenderer();

            services.AddScoped<BookDbContext>(_ => new BookDbContext(Configuration.GetConnectionString("BookConnectionString")));
            services.AddScoped<BookService, BookService>();
            services.AddScoped<BookPilotageService, BookPilotageService>();
            //services.AddTransient<RazorViewToStringRenderer>();
            //services.AddTransient<ViewRenderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
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
