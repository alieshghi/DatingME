using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TodoApi.helper
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> item,int pageSize, int count, int pagenumber)
        {
            this.PageSize = pageSize;
            this.CurentPage= pagenumber;
            this.totalCountOfItems= count;
            this.TotalPage= (int) Math.Ceiling(totalCountOfItems/(double)pageSize);
            this.AddRange(item);
        }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }

        public int CurentPage { get; set; }
        public int totalCountOfItems { get; set; }
        public static async Task<PagedList<T>> CreatePagintion(IQueryable<T> source,int pageSize,
         int curentPage ){
            var count = await source.CountAsync();
            var items= await source.Skip((curentPage-1) * pageSize).Take(pageSize).ToListAsync();
            var result = new PagedList<T>( items,pageSize,count,curentPage);            
            return result;
        }
    }
}