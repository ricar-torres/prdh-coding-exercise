using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Dtos;
public class PaginatedList<T> : List<T> {
	public int PageIndex { get; }
	public int TotalPages { get; }
	public bool HasPreviousPage => PageIndex > 1;
	public bool HasNextPage => PageIndex < TotalPages;

	public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
		if (items == null) {
			throw new ArgumentNullException(nameof(items));
		}

		if (pageIndex <= 0) {
			throw new ArgumentException("Page index must be greater than zero.", nameof(pageIndex));
		}

		if (pageSize <= 0) {
			throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
		}

		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);

		this.AddRange(items);
	}

	public static PaginatedList<T> Create(IList<T> source, int pageIndex, int pageSize) {
		if (source == null) {
			throw new ArgumentNullException(nameof(source));
		}

		var count = source.Count();
		var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}
