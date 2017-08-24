using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModel
{
    public class BaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EntityState EntityState { get; set; }
    }

    /// <summary>
    /// 实体状态
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// 添加状态
        /// </summary>
        Add = 0,

        /// <summary>
        /// 修改状态
        /// </summary>
        Modify = 1,

        /// <summary>
        /// 删除状态
        /// </summary>
        Delete = 2
    }
}
