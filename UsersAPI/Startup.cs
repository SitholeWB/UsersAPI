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
using Models.Constants;
using Models.Settings;
using Services;
using Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
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
						Name = "Welcome Bonginhlahla Sithole",
						Url = new Uri("https://github.com/SitholeWB")
					},
					Description = "Talk is cheap. Show me the code. - Torvalds, Linus (2000-08-25)."
				});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header,
						},
						new List<string>()
					}
				});
			});

			services.AddHttpContextAccessor();

			//App Settings Injection
			IConfigurationSection jwtAuthConfig = Configuration.GetSection("JwtAuth");
			IConfigurationSection facebookAuthConfig = Configuration.GetSection("FacebookAuth");
			IConfigurationSection cryptographyConfig = Configuration.GetSection("Cryptography");

			services.Configure<JwtAuth>(jwtAuthConfig);
			services.Configure<FacebookAuth>(facebookAuthConfig);
			services.Configure<Cryptography>(cryptographyConfig);

			//Inject DB Context
			services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddScoped<IUsersDbContext, UsersDbContext>();

			services.AddScoped<IUserIdendityService, UserIdendityService>();

			//Dependency Injection
			var settingsService = new SettingsService(jwtAuthConfig.Get<JwtAuth>(), facebookAuthConfig.Get<FacebookAuth>(), cryptographyConfig.Get<Cryptography>());
			services.AddSingleton<ISettingsService>(settingsService);
			services.AddSingleton<ICryptoEngineService, CryptoEngineService>();

			services.AddTransient<IErrorLogService, ErrorLogService>();
			services.AddTransient<IOAuthProviderService, OAuthProviderService>();

			services.AddTransient<IUsersService, UsersService>();
			services.AddHttpClient<IAuthService, AuthService>();

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

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policy.ALL_ADMINS,
					 policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.ADMIN, UserRoles.SUPER_ADMIN));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policy.SUPER_ADMIN,
					 policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.SUPER_ADMIN));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policy.ADMIN,
					 policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.ADMIN));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policy.DEVELOPER,
					 policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.DEVELOPER));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(Policy.EVERYONE,
					 policy => policy.RequireClaim(ClaimTypes.Role, UserRoles.DEVELOPER, UserRoles.GENERAL, UserRoles.SUPER_ADMIN, UserRoles.ADMIN));
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