using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBUtils.PaginatedList.Core
{
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }
        public int TotalPages { get; private set; }
        public int FirstItemOnPage { get; private set; }
        public int LastItemOnPage { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            FirstItemOnPage = ((PageIndex - 1) * PageSize) + 1;
            LastItemOnPage = FirstItemOnPage + PageSize - 1 > TotalItems ? TotalItems : FirstItemOnPage + PageSize - 1;

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<IPaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public new IEnumerable<T> GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /*
        IEnumerable IEnumerable.GetEnumerator()
        {
            return GetEnumerable();
        }
        */
    }

}
