using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCommon
{
   public  interface IPageList<T> : IList<T>
    {
        /// <summary>
        /// 总数量
        /// </summary>
        int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 第几页
        /// </summary>
        int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 一页显示几条数据
        /// </summary>
        int PageSize
        {
            get;
            set;
        }
    }
}
