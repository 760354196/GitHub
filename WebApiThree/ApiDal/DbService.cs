using ApiCommon;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDal
{
   public static class DbService
    {

        /// <summary>
        /// 开启事务
        /// </summary>
        public static void BeginTransaction()
        {
            DbFactory.Instance.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public static void CommitTransaction()
        {
            DbFactory.Instance.CommitTransaction();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public static void RollbackTransaction(Exception ex)
        {
            DbFactory.Instance.RollbackTransaction(ex);
        }

        /// <summary>
        /// 执行sql语句返回影响行数
        /// </summary>
        public static int Execute(string sql, object paramValues = null)
        {
            return DbFactory.Instance.MyConnection.Execute(sql, paramValues);
        }

        public static SingleType GetSingle<SingleType>(string sql, object paramValues = null)
        {
            return DbFactory.Instance.MyConnection.QueryFirstOrDefault<SingleType>(sql, paramValues);
        }
        /// <summary>
        /// 根据Sql查询实体列表
        /// </summary>
        public static IEnumerable<T> GetList<T>(string sql, object paramValues = null) where T : class
        {
            return DbFactory.Instance.MyConnection.Query<T>(sql, paramValues);
        }

        /// <summary>
        /// 根据Sql查询实体分页列表
        /// </summary>
        public static PageList<T> GetPageList<T>(string sql, object paramValues = null, int pageIndex = 1, int pageSize = 10, bool needTotalCount = true) where T : class
        {
            if (pageIndex <= 0)
                pageIndex = 1;
            if (pageSize <= 0)
                pageSize = 10;

            var pageList = new PageList<T>() { PageIndex = pageIndex, PageSize = pageSize };
            //获取总数
            if (needTotalCount)
            {
                var sqlTotal = $"select count(0) from ({sql}) t_total";
                var count = DbFactory.Instance.MyConnection.QueryFirstOrDefault<int>(sqlTotal, paramValues);

                pageList.TotalCount = count;
            }
            //获取分页列表
            sql = $"{sql} limit {(pageIndex - 1) * pageSize},{pageSize}";
            var list = DbFactory.Instance.MyConnection.Query<T>(sql, paramValues);

            pageList.AddRange(list);
            return pageList;
        }



    }
}
