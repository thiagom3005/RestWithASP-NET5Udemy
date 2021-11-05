using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using RestWithASPNETUdemy.HyperMedia.Enricher;
using RestWithASPNETUdemy.HyperMedia.Filters;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Generic;
using RestWithASPNETUdemy.Token.Configurations;
using RestWithASPNETUdemy.Token.Services;
using RestWithASPNETUdemy.Token.Services.Implementations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestWithASPNETUdemy
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;

      Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var tokenConfigurations = new TokenConfigurations();

      new ConfigureFromConfigurationOptions<TokenConfigurations>(
        Configuration.GetSection("TokenConigurations")
        ).Configure(tokenConfigurations);

      services.AddSingleton(tokenConfigurations);

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = tokenConfigurations.Issuer,
          ValidAudience = tokenConfigurations.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
        };
      });

      services.AddAuthorization(auth =>
      {
        auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
          .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
          .RequireAuthenticatedUser().Build());
      });

      services.AddCors(options => options.AddDefaultPolicy(builder =>
      {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
      }));

      services.AddControllers();

      var connection = Configuration["MySQLConnection:MySQLConnectionString"];
      services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

      if (Environment.IsDevelopment())
      {
        MigrateDatabase(connection);
      }

      services.AddMvc(options =>
      {
        options.RespectBrowserAcceptHeader = true;
        options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
        options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
      }).AddXmlSerializerFormatters();

      var filterOptions = new HyperMediaFilterOptions();
      filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
      filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

      services.AddSingleton(filterOptions);

      //Versioning API
      services.AddApiVersioning();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
          new OpenApiInfo
          {
            Title = "REST API's From 0 to Azure with ASP .NET Core 5 and Docker",
            Version = "v1",
            Description = "API RESTFull developed in course 'REST API's From 0 to Azure with ASP .NET Core 5 and Docker'",
            Contact = new OpenApiContact
            {
              Name = "Thiago Guimar�es",
              Url = new Uri("https://github.com/thiagom3005")
            }
          });
      });

      //Dependency Injection
      services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddScoped<IPersonBusiness, PersonBusiness>();
      services.AddScoped<IBookBusiness, BookBusiness>();
      services.AddScoped<ILoginBusiness, LoginBusiness>();
      services.AddScoped<IFileBusiness, FileBusiness>();

      services.AddTransient<ITokenService, TokenService>();

      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPersonRepository, PersonRepository>();
      services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors();

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
        "REST API's From 0 to Azure with ASP .NET Core 5 and Docker");
      });

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
      });
    }

    private void MigrateDatabase(string connection)
    {
      try
      {
        var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
        var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
        {
          Locations = new List<string> { "db/migrations", "db/dataset" },
          IsEraseDisabled = true
        };
        evolve.Migrate();
      }
      catch (Exception ex)
      {

        Log.Error("Database migration failed", ex);
        throw;
      }
    }
  }
}
