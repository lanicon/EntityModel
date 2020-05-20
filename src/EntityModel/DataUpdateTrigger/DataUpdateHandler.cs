// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;
using Agebull.EntityModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Agebull.EntityModel.Events
{
    /// <summary>
    ///     数据更新处理器
    /// </summary>
    public static class DataUpdateHandler
    {
        #region MyRegion

        /// <summary>
        /// 类型接口实现的数字表示
        /// </summary>
        public static readonly Dictionary<Type, int> TypeInterfaces = new Dictionary<Type, int>();
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIAuthorData = 1;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIHistoryData = 2;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIOrganizationData = 4;
        /// <summary>
        /// 表示
        /// </summary>
        public const int TypeofIVersionData = 8;

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public static void InitType<TEntity>() where TEntity : class, new()
        {
            if (TypeInterfaces.ContainsKey(typeof(TEntity)))
                return;
            var entity = new TEntity();
            int type = 0;
            if (entity is IAuthorData)
            {
                type |= TypeofIAuthorData;
            }
            if (entity is IHistoryData)
            {
                type |= TypeofIHistoryData;
            }
            if (entity is IOrganizationData)
            {
                type |= TypeofIOrganizationData;
            }
            if (entity is IVersionData)
            {
                type |= TypeofIVersionData;
            }
            TypeInterfaces.Add(typeof(TEntity), type);
        }

        /// <summary>
        /// 是否指定类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsType<TEntity>(int type)
        {
            return TypeInterfaces.TryGetValue(typeof(TEntity), out var def) && (type & def) == type;
        }

        #endregion
        /// <summary>
        /// 事件代理
        /// </summary>
        public static IEntityEventProxy EventProxy
        {
            get;
        }

        static DataUpdateHandler()
        {
            EventProxy = DependencyHelper.GetService<IEntityEventProxy>();
        }

        /// <summary>
        /// 通用处理器
        /// </summary>
        private static readonly List<IDataTrigger> _generalTriggers = new List<IDataTrigger> { new DefaultDataUpdateTrigger() };



        /// <summary>
        ///     保存前处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnPrepareSave<TEntity>(TEntity data, DataOperatorType operatorType)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnPrepareSave(data, operatorType);
        }

        /// <summary>
        ///     保存完成后期处理
        /// </summary>
        /// <param name="data">保存的对象</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnDataSaved<TEntity>(TEntity data, DataOperatorType operatorType)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnDataSaved(data, operatorType);
        }


        /// <summary>
        ///     更新语句前处理(单个实体操作不引发)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnOperatorExecuting<TEntity>(IDataTable<TEntity> table, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
                     where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnOperatorExecuting(table, condition, args, operatorType);
        }

        /// <summary>
        ///     更新语句后处理(单个实体操作不引发)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">执行条件</param>
        /// <param name="args">参数值</param>
        /// <param name="operatorType">操作类型</param>
        public static void OnOperatorExecuted<TEntity>(IDataTable<TEntity> table, string condition, IEnumerable<DbParameter> args, DataOperatorType operatorType)
             where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.OnOperatorExecuted(table, condition, args, operatorType);
        }

        /// <summary>
        ///     得到可正确拼接的SQL条件语句（可能是没有）
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="conditions">附加的条件集合</param>
        /// <returns></returns>
        public static void ConditionSqlCode<TEntity>(IDataTable<TEntity> table, List<string> conditions) where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>())
                trigger.ConditionSqlCode(table, conditions);
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之前立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <returns></returns>
        public static void BeforeUpdateSql<TEntity>(IDataTable<TEntity> table, StringBuilder code, string condition)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>().Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
            {
                trigger.BeforeUpdateSql(table, condition, code);
            }
        }

        /// <summary>
        ///     与更新同时执行的SQL(更新之后立即执行)
        /// </summary>
        /// <param name="table">当前数据操作对象</param>
        /// <param name="condition">当前场景的执行条件</param>
        /// <param name="code">写入SQL的文本构造器</param>
        /// <returns></returns>
        public static void AfterUpdateSql<TEntity>(IDataTable<TEntity> table, StringBuilder code, string condition)
            where TEntity : EditDataObject, new()
        {
            foreach (var trigger in DependencyHelper.GetServices<IDataTrigger>().Where(p => p.DataBaseType.HasFlag(table.DataBaseType)))
            {
                trigger.AfterUpdateSql(table, condition, code);
            }
        }
    }
}