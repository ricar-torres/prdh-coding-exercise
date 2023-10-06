using Microsoft.EntityFrameworkCore;
using WebApi.Shared;

namespace WebApi.Configuration;

public static class FeatureConfigurationExtensions {
	private static readonly List<IFeatureDefinition> EndpointsList = new();

	public static void AddFeatureServices(this IServiceCollection services) {
		var endpointDefinitions = DiscoverDefinitions();
		foreach (var endpointDefinition in endpointDefinitions) {
			endpointDefinition.DefineServices(services);
			EndpointsList.Add(endpointDefinition);
		}

		services.AddSingleton(endpointDefinitions as IReadOnlyCollection<BaseFeatureDefinition> 
			?? throw new InvalidOperationException());
	}

	public static void AddFeatureEndpoints(this IEndpointRouteBuilder endpoints) {
		foreach(var endpointDefinition in EndpointsList) endpointDefinition.DefineEndpoints(endpoints);
	}

	public static void AddFeatureModels(this ModelBuilder modelBuilder) {
		var definitions = DiscoverDefinitions();
		foreach (var definition in definitions) definition.DefineModels(modelBuilder);
	}

	private static IEnumerable<BaseFeatureDefinition> DiscoverDefinitions() {
		return typeof(BaseFeatureDefinition).Assembly
				.GetTypes()
				.Where(t => typeof(BaseFeatureDefinition).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsInterface)
				.Select(Activator.CreateInstance)
				.Cast<BaseFeatureDefinition>()
				.ToList();
	}
}
