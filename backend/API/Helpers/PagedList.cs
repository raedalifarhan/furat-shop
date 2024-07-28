using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> where T : class
    {
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageCount = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            Data = items;
        }

        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }
        public int PageCount { get; private set; }
        public List<T> Data { get; private set; }

        public static PagedList<T> Create(IList<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}