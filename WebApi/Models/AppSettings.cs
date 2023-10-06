using System;
namespace WebApi.Models;
public class AppSettings {
	public string? DefaultConnection { get; set; }
	public string? StatisticsUrl { get; set; }
	public string? OrderTestCovid19MinimalEndpoint { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public TimeSpan TimeSpan { get; set; }
}

