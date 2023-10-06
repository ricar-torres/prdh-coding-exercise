using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Shared;

public interface IFeatureDefinition {
	void DefineServices(IServiceCollection services);
	void DefineModels(ModelBuilder modelBuilder);
	void DefineEndpoints(IEndpointRouteBuilder endpoints);
}

public abstract class BaseFeatureDefinition : IFeatureDefinition {
	[NonAction]
	public virtual void DefineServices(IServiceCollection services) { }
	[NonAction]
	public virtual void DefineModels(ModelBuilder modelBuilder) { }
	[NonAction]
	public virtual void DefineEndpoints(IEndpointRouteBuilder endpoints) { }

	protected IResult BadRequestError(string message) {
		return Results.BadRequest(new { errorMessage = message });
	}

	public sealed class ApiBaseUrl {
		const string _webApiBaseString = "api";
		private readonly StringBuilder _urlBuilder;

		public ApiBaseUrl(string prefix) {
			_urlBuilder = new StringBuilder($"{_webApiBaseString}/{prefix}");
		}

		public ApiBaseUrl(string prefix, string route) {
			_urlBuilder = new StringBuilder($"{prefix}/{route}");
		}

		public static implicit operator string(ApiBaseUrl apiBaseString) {
			return apiBaseString.ToString();
		}

		public static ApiBaseUrl operator +(ApiBaseUrl apiBaseString, string route) {
			return new ApiBaseUrl($"{apiBaseString}", $"{route}");
		}

		public override string ToString() {
			return _urlBuilder.ToString();
		}
	}
}
