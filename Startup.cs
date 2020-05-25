using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace JsonTaggerApi
{
    public class Startup
    {
        private const string JSONTAGGER_DATA_PATH_ENV_VAR = "JSONTAGGER_DATA_PATH";
        private const string ENV_VAR_ERROR = "Could not find value for environment variable";
        private const string DB_FILE_NAME = "data.db";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Func<string> GetJsonTaggerPath = () => {
            string? path = Environment.GetEnvironmentVariable(JSONTAGGER_DATA_PATH_ENV_VAR);

            if (path == null)
                throw new ApplicationException(ENV_VAR_ERROR + $": {JSONTAGGER_DATA_PATH_ENV_VAR}");

            return path;
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<TaggerDbContext>(
                options => options.UseSqlite(
                    "Data Source=" + Path.Join(GetJsonTaggerPath(), DB_FILE_NAME)
                )
            );
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(GetJsonTaggerPath()),
                RequestPath = new PathString("/file"),
                EnableDirectoryBrowsing = false
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
