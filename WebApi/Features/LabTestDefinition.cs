using WebApi.Shared;
using WebApi.Features.LabTests.Services;

namespace WebApi.Features.LabTests.Definition;
public sealed class LabTestDefinition : BaseFeatureDefinition {

	public override void DefineServices(IServiceCollection services) {
		services.AddSingleton<ILabTestsService, LabTestsService>();
		services.AddHostedService<WorkerTestLab>();
	}
}
