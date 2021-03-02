namespace RBUtils.PaginatedList.Core
{
    ///<summary>
    /// Non-enumerable version of the PagedList class.
    ///</summary>    
    public class PaginatedListMetaData : IPaginatedList
	{
		/// <summary>
		/// Protected constructor that allows for instantiation without passing in a separate list.
		/// </summary>
		protected PaginatedListMetaData()
		{
		}

		///<summary>
		/// Non-enumerable version of the PagedList class.
		///</summary>
		///<param name="pagedList">A PagedList (likely enumerable) to copy metadata from.</param>
		public PaginatedListMetaData(IPaginatedList paginatedList)
		{
			TotalPages = paginatedList.TotalPages;
			TotalItems = paginatedList.TotalItems;
			PageIndex = paginatedList.PageIndex;
			PageSize = paginatedList.PageSize;
			HasPreviousPage = paginatedList.HasPreviousPage;
			HasNextPage = paginatedList.HasNextPage;
			//IsFirstPage = paginatedList.IsFirstPage;
			//IsLastPage = paginatedList.IsLastPage;
			FirstItemOnPage = paginatedList.FirstItemOnPage;
			LastItemOnPage = paginatedList.LastItemOnPage;
		}

		#region IPagedList Members

		/// <summary>
		/// 	Total number of subsets within the superset.
		/// </summary>
		/// <value>
		/// 	Total number of subsets within the superset.
		/// </value>
		public int TotalPages { get; protected set; }

		/// <summary>
		/// 	Total number of objects contained within the superset.
		/// </summary>
		/// <value>
		/// 	Total number of objects contained within the superset.
		/// </value>
		public int TotalItems { get; protected set; }

		/// <summary>
		/// 	One-based index of this subset within the superset, zero if the superset is empty.
		/// </summary>
		/// <value>
		/// 	One-based index of this subset within the superset, zero if the superset is empty.
		/// </value>
		public int PageIndex { get; protected set; }

		/// <summary>
		/// 	Maximum size any individual subset.
		/// </summary>
		/// <value>
		/// 	Maximum size any individual subset.
		/// </value>
		public int PageSize { get; protected set; }

		/// <summary>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the first subset within the superset.
		/// </value>
		public bool HasPreviousPage { get; protected set; }

		/// <summary>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the last subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is NOT the last subset within the superset.
		/// </value>
		public bool HasNextPage { get; protected set; }

		/// <summary>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the first subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the first subset within the superset.
		/// </value>
		//public bool IsFirstPage { get; protected set; }

		/// <summary>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the last subset within the superset.
		/// </summary>
		/// <value>
		/// 	Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this is the last subset within the superset.
		/// </value>
		//public bool IsLastPage { get; protected set; }

		/// <summary>
		/// 	One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
		/// </summary>
		/// <value>
		/// 	One-based index of the first item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
		/// </value>
		public int FirstItemOnPage { get; protected set; }

		/// <summary>
		/// 	One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
		/// </summary>
		/// <value>
		/// 	One-based index of the last item in the paged subset, zero if the superset is empty or PageNumber is greater than PageCount.
		/// </value>
		public int LastItemOnPage { get; protected set; }

		#endregion
	}
}