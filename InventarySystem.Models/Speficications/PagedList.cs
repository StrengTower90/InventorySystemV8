using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models.Speficications
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageZize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageZize,
                TotalPages = (int)Math.Ceiling(count / (double)pageZize) // for example 1.5 it becomes to 2
            };
            AddRange(items); //Add the collection's items to the end of the list
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> entity, int pageNumber, int pageSize)
        {
            var count = entity.Count();
            var items = entity.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
