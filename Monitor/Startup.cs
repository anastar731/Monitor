using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using StructureMap;
using Monitor.Models;
using Monitor.Reporting.Scheduler;
using Microsoft.Extensions.Hosting;

namespace Monitor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AgentDataContext>(opt =>
                opt.UseInMemoryDatabase("AgentDataList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            return ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP 
        //request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.For(typeof(IScheduledTask)).Add(typeof(CreateReportTask));
                config.For(typeof(IHostedService)).Add(typeof(HostedService)).Singleton();

                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();

        }

    }
}