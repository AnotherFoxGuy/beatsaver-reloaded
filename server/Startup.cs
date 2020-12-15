using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using server.Models;
using server.Services;
using WebApi.Helpers;

namespace server
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(name: MyAllowSpecificOrigins,
          builder => { builder.WithOrigins("localhost"); });
      });

      // requires using Microsoft.Extensions.Options
      services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
      services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

      services.AddSingleton<UserService>();
      services.AddSingleton<MapsService>();

      services.AddHealthChecks();
      services.AddControllers();
      services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "server", Version = "v1"}); });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "server v1"));
      }

      app.UseCors(MyAllowSpecificOrigins);

      //app.UseHttpsRedirection();

      app.UseRouting();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints => endpoints.MapControllers());
      app.UseEndpoints(endpoints => endpoints.MapHealthChecks("/health"));
    }
  }
}
