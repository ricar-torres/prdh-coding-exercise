public abstract class BaseEntity<T> : BaseMeta where T : IEquatable<T>, IComparable<T> {
	public T Id { get; set; }
}

public abstract class BaseMeta {
	public int? CreatedUser { get; set; }
	public DateTime? CreatedDate { get; set; }
	public int? UpdatedUser { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public bool IsActive { get; set; }
}
