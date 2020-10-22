﻿using System;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 注入范围控制
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class InjectionScopeEx
    {
        /// <summary>
        /// 个性选择字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IDisposable InjectionScope<TEntity>(this DataQuery<TEntity> query, InjectionLevel levle)
        where TEntity : class, new()
        {
            return new NoInjectionScope<TEntity>(query, levle);
        }

        /// <summary>
        /// 字段选择范围控制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        class NoInjectionScope<TEntity> : IDisposable
            where TEntity : class, new()
        {
            private readonly DataQuery<TEntity> dataQuery;
            private readonly InjectionLevel oldInjection;
            internal NoInjectionScope(DataQuery<TEntity> query, InjectionLevel levle)
            {
                dataQuery = query;
                oldInjection = query.Option.InjectionLevel;
                query.Option.InjectionLevel = levle;
            }

            void IDisposable.Dispose()
            {
                dataQuery.Option.InjectionLevel = oldInjection;
            }
        }
    }
    
}