using System.Collections.Generic;

namespace RBUtils.PaginatedList.Core
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <remarks>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </remarks>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IEnumerable{T}"/>
    public interface IPaginatedList<out T> : IPaginatedList, IEnumerable<T>
	{
		///<summary>
		/// Gets the element at the specified index.
		///</summary>
		///<param name="index">The zero-based index of the element to get.</param>
		T this[int index] { get; }

		///<summary>
		/// Gets the number of elements contained on this page.
		///</summary>
		int Count { get; }

		///<summary>
		/// Gets a non-enumerable copy of this paged list.
		///</summary>
		///<returns>A non-enumerable copy of this paged list.</returns>
		//IPaginatedList GetMetaData();
	}

	/// <summary>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </summary>
	/// <remarks>
	/// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
	/// </remarks>
	public interface IPaginatedList
	{
		/// <summary>
		/// Total number of pages.
		/// </summary>
		/// <value>
		/// Total number of pages.
		/// </value>
		int TotalPages { get; }

		/// <summary>
		/// Total number of items within the dataset.
		/// </summary>
		/// <value>
		/// Total number of items within the dataset.
		/// </value>
		int TotalItems { get; }

		/// <summary>
		/// One-based index of the current page.
		/// </summary>
		/// <value>
		/// One-based index of the current page.
		/// </value>
		int PageIndex { get; }

		/// <summary>
		/// Maximum items per page.
		/// </summary>
		/// <value>
		/// Maximum items per page.
		/// </value>
		int PageSize { get; }

		/// <summary>
		/// Returns true if this is NOT the first subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is NOT the first subset within the superset.
		/// </value>
		bool HasPreviousPage { get; }

		/// <summary>
		/// Returns true if this is NOT the last subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is NOT the last subset within the superset.
		/// </value>
		bool HasNextPage { get; }

		/// <summary>
		/// Returns true if this is the first subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is the first subset within the superset.
		/// </value>
		//bool IsFirstPage { get; }

		/// <summary>
		/// Returns true if this is the last subset within the superset.
		/// </summary>
		/// <value>
		/// Returns true if this is the last subset within the superset.
		/// </value>
		//bool IsLastPage { get; }

		/// <summary>
		/// One-based index of the first item in the paged subset.
		/// </summary>
		/// <value>
		/// One-based index of the first item in the paged subset.
		/// </value>
		int FirstItemOnPage { get; }

		/// <summary>
		/// One-based index of the last item in the paged subset.
		/// </summary>
		/// <value>
		/// One-based index of the last item in the paged subset.
		/// </value>
		int LastItemOnPage { get; }
	}
}
