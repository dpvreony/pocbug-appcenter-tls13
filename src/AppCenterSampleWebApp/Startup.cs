using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace AppCenterTls13.IntegrationTests
{
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures services.
        /// </summary>
        /// <param name="services">Services collection to modify.</param>
        public void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">web hosting environment details.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.MapWhen(_ => true, AppConfiguration);
        }

        private void AppConfiguration(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(Handler);
        }

        private async Task Handler(HttpContext context)
        {
            var response = context.Response;
            response.StatusCode = 200;
            response.ContentType = "text/plain";

            await response.WriteAsync("RESPONSE")
                .ConfigureAwait(false);

        }
    }
}
