using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Settings;
using Services;
using Services.DataLayer;
using Swashbuckle.AspNetCore.Swagger;
using UsersAPI.Exceptions;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;

namespace UsersAPI
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
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder =>
					{
						builder
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
					});
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Users API", Version = "v1" });
			});

			services.AddDefaultIdentity<IdentityUser>()
					.AddDefaultUI(UIFramework.Bootstrap4)
					.AddEntityFrameworkStores<UsersDbContext>();

			services.AddDataProtection();

			//App Settings Injection
			services.Configure<JwtAuth>(Configuration.GetSection("JwtAuth"));
			services.Configure<FacebookAuth>(Configuration.GetSection("FacebookAuth"));
			services.Configure<Cryptography>(Configuration.GetSection("Cryptography"));

			//Inject DB Context
			services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			//Dependency Injection
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ICryptoEngineService, CryptoEngineService>();

			services.AddTransient<IErrorLogService, ErrorLogService>();
			services.AddTransient<IOAuthProviderService, OAuthProviderService>();

			services.AddTransient<IUsersService, UsersService>();
			services.AddTransient<IAuthService, AuthService>();

			var sp = services.BuildServiceProvider();
			var settingsService = sp.GetService<ISettingsService>();

			services.AddAuthentication().AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = settingsService.GetFacebookAuth().AppId;
				facebookOptions.AppSecret = settingsService.GetFacebookAuth().AppSecret;
			});

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options =>
					{
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = "yourdomain.com",
							ValidAudience = "yourdomain.com",
							IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settingsService.GetJwtAuth().SecurityKey))
						};
					});


			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors("AllowAll");
			app.UseStaticFiles();
			var swaggerUrl = "/swagger/v1/swagger.json";

			app.UseAuthentication();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				swaggerUrl = "/UsersAPI/swagger/v1/swagger.json";
				//app.UseExceptionHandler("/Error");
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(swaggerUrl, "Users API");
				c.RoutePrefix = string.Empty;
			});

			app.UseMiddleware(typeof(ErrorHandlingMiddleware));

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
