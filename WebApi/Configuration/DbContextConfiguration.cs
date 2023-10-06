using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Configuration;

public static class DbContextConfiguration {
	public static void AddDbContext(this IServiceCollection services, IConfiguration configuration,
					IHostEnvironment environment, AppSettings appSettings) {
		if (environment.IsDevelopment()) {
			services.AddDbContext<DataContext>(options =>
				options.UseSqlite(appSettings.DefaultConnection).EnableSensitiveDataLogging()
			);
		}
		else {
			services.AddDbContext<DataContext>(options => options.UseSqlite(appSettings.DefaultConnection));
		}
	}
}
