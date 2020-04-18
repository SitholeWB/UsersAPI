using System;
using System.Text;
using Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Settings;
using Services;
using Services.DataLayer;
using UsersAPI.Exceptions;

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
						.AllowAnyHeader();
					});
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Users API",
					Version = "v1",
					Contact = new OpenApiContact
					{
						Name = "Welcome Sithole" ,
						Url = new Uri("https://github.com/SitholeWB")
					},
					Description = "Talk is cheap. Show me the code. - Torvalds, Linus (2000-08-25)."
				});
			});

			services.AddDataProtection();

			//App Settings Injection
			services.Configure<JwtAuth>(Configuration.GetSection("JwtAuth"));
			services.Configure<FacebookAuth>(Configuration.GetSection("FacebookAuth"));
			services.Configure<Cryptography>(Configuration.GetSection("Cryptography"));

			//Inject DB Context
			services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddScoped<IUsersDbContext, UsersDbContext>();
			//Dependency Injection
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ICryptoEngineService, CryptoEngineService>();

			services.AddTransient<IErrorLogService, ErrorLogService>();
			services.AddTransient<IOAuthProviderService, OAuthProviderService>();

			services.AddTransient<IUsersService, UsersService>();
			services.AddHttpClient<IAuthService, AuthService>();

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
							ValidIssuer = settingsService.GetJwtAuth().ValidIssuer,
							ValidAudience = settingsService.GetJwtAuth().ValidAudience,
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
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(swaggerUrl, "Users API - v1");
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
