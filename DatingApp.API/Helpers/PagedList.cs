using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T>: List<T> 
    {
        public int CurrentPage { get; set; }   
        public int TotalPages { get; set; }
        public int PazeSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pagenumber, int pazesize)
        {
            TotalCount = count;
            CurrentPage = pagenumber;
            PazeSize = pazesize;
            TotalPages = (int)Math.Ceiling(count / (double)pazesize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pazeSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pazeSize).Take(pazeSize).ToListAsync();
                return new PagedList<T>(items, count, pageNumber, pazeSize);
        }


    }
}