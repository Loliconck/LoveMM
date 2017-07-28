using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveMM.Models
{
    public class PagingBase<T>
    {
        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int TotalCount { get; set; }

        public IEnumerable<T> PagingData { get; set; }
    }
}