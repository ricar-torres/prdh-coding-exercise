using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Features.Cases.Models;
public class Case {
	public Guid CaseId { get; set; }
	public Guid PatientId { get; set; }
	public DateTime EarliestPositiveOrderTestSampleCollectedDate { get; set; }
	public string EarliestPositiveOrderTestType { get; set; }
	public int OrderTestCount { get; set; }
}

public class CaseEntityConfiguration : IEntityTypeConfiguration<Case> {
	public void Configure(EntityTypeBuilder<Case> builder) {
		builder.HasKey(e => e.CaseId);
		builder.Property(e => e.CaseId).ValueGeneratedOnAdd();
		builder.Property(e => e.PatientId).ValueGeneratedNever();
		builder.ToTable("Cases");
	}
}
