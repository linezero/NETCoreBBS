using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreBBS.Entities
{
    public class Page<T>
        where T : class
    {
        public int Total { get; private set; }
        public int PageSize { get; private set; }
        public List<T> List { get; private set; }
        public Page(List<T> list, int pageSize, int total)
        {
            this.List = list;
            this.PageSize = pageSize;
            this.Total = total;
        }

        public int GetPageCount()
        {
            return (Total + PageSize - 1) / PageSize;
        }
    }
}
