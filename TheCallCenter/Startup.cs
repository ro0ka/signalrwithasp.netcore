﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheCallCenter.Data;
using TheCallCenter.Hubs;

namespace TheCallCenter
{
  public class Startup
  {
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
      _config = config;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<CallCenterContext>()
        .AddEntityFrameworkSqlServer();

      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseSignalR(routes =>
      {
        routes.MapHub<CallCenterHub>("/callcenter");
      });

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
