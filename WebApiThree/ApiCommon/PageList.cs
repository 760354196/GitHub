
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCommon
{
    public class PageList<T> : List<T>, IPageList<T>
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount
        {
            get; set;
        }

        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 一页显示几条数据
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
    }
}
