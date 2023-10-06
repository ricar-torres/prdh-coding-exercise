using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;
using WebApi.Helpers;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi;
public class Startup {
	public IConfiguration Configuration { get; }
	readonly IHostEnvironment _environment;

	public Startup(IConfiguration configuration, IHostEnvironment environment) {
		Configuration = configuration;
		_environment = environment;
	}

	public void ConfigureServices(IServiceCollection services) {
		var appSettingsSection = Configuration.GetSection("AppSettings");
		services.Configure<AppSettings>(appSettingsSection);
		var appSettings = appSettingsSection.Get<AppSettings>();

		services.AddDbContext(Configuration, _environment, appSettings);

		services.AddHttpClient();

		services.AddCors(options => {
			options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
		});

		services.AddMemoryCache();
		services.AddResponseCompression(options => { options.EnableForHttps = true; });

		services.AddHsts(options => {
			options.Preload = true;
			options.MaxAge = TimeSpan.FromDays(30);
		});

		services.AddControllers(options => {
			options.MaxIAsyncEnumerableBufferLimit = 20000;
		});


		services.AddHttpsRedirection(options => options.HttpsPort = 443);

		services.AddHttpContextAccessor();

		services.AddFeatureServices();

		services.AddEndpointsApiExplorer();

		services.AddResponseCaching();

	}

	public void Configure(IApplicationBuilder app) {
		if (_environment.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
			app.UseCors("CORSPolicy");
		}
		else {
			app.UseHsts();
			app.UseCors();
		}

		app.UseResponseCompression();

		app.ConfigureExceptionHandler();

		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseResponseCaching();

		app.UseEndpoints(endpoints => {
			endpoints.AddFeatureEndpoints();
			endpoints.MapControllers().RequireAuthorization();
		});


		using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();

		if (dataContext != null && !dataContext.AllMigrationsApplied()) {
			dataContext.Database.Migrate();
		}

	}
}
