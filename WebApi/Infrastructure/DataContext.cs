using Microsoft.EntityFrameworkCore;
using WebApi.Configuration;
namespace WebApi.Infrastructure;
public class DataContext : DbContext {
	public DataContext(DbContextOptions<DataContext> options) : base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		modelBuilder.AddFeatureModels();
	}
}
