﻿// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-12
// // 修改:2016-06-16
// // *****************************************************/

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

namespace Agebull.EntityModel.Common
{
    /// <summary>
    /// 表示Sql生成器
    /// </summary>
    public interface ISqlBuilder : IParameterCreater
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DataBaseType DataBaseType { get; }

        /// <summary>
        /// 数据访问配置
        /// </summary>
        DataAccessOption Option { get; set; }

        #region Option对象支持

        /// <summary>
        /// 读取的字段
        /// </summary>
        /// <returns></returns>
        string BuilderLoadFields();

        /// <summary>
        /// 全量更新的字段
        /// </summary>
        /// <returns></returns>
        string BuilderUpdateFields();

        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="valueExpression">更新表达式(SQL)</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
        string BuilderUpdateCode(string valueExpression, string condition);

        /// <summary>
        /// 插入的代码
        /// </summary>
        /// <returns></returns>
        string BuilderInsertSqlCode();

        /// <summary>
        /// 删除的代码
        /// </summary>
        /// <returns></returns>
        string BuilderDeleteSqlCode();

        #endregion

        #region SQL片断

        /// <summary>
        ///     主键的条件部分SQL
        /// </summary>
        string PrimaryKeyCondition { get; }

        /// <summary>
        ///     得到字段的MySqlDbType类型
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>参数</returns>
        int GetDbType(string field);

        /// <summary>
        /// 条件SQL
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="paraName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        string Condition(string fieldName, string expression="=", string paraName=null);

        /// <summary>
        /// 构建排序SQL片断
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        string OrderCode(bool desc, string field);

        /// <summary>
        /// 字段更新
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string FileUpdateSetCode(string field, object value, IList<DbParameter> parameters);

        #endregion

        #region 完整SQL生成

        /// <summary>
        /// 汇总SQL
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="field"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateCollectSql(string fun, string field, string condition);

        /// <summary>
        /// 读取SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="orderSql">排序片断</param>
        /// <param name="limit">行数</param>
        /// <returns></returns>
        string CreateLoadSql(string condition = null, string orderSql = null, string limit = null);

        /// <summary>
        /// 单值读取SQL
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateLoadValueSql(string field, string condition);

        /// <summary>
        /// 单值读取SQL
        /// </summary>
        /// <param name="field"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        string CreateLoadValuesSql(string field, ConditionItem convert);

        /// <summary>
        /// 分页SQL
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderField"></param>
        /// <param name="desc"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreatePageSql(int page, int pageSize, string orderField, bool desc, string condition);

        /// <summary>
        /// 删除SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateDeleteSql(string condition);

        /// <summary>
        ///     物理删除数据
        /// </summary>
        string PhysicalDeleteSqlCode(string condition);

        /// <summary>
        /// 更新SQL
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        string CreateUpdateSqlCode(string valueExpression, string condition);

        #endregion

    }

    /// <summary>
    /// 表示Sql生成器
    /// </summary>
    public interface ISqlBuilder<TEntity> : ISqlBuilder, IDataAccessTool<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        ///     生成插入的SQL
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>更新的SQL</returns>
        public string CreateInsertSqlCode(TEntity entity);

        /// <summary>
        ///     生成更新的SQL
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="condition">更新条件</param>
        /// <returns>更新的SQL</returns>
        public string CreateUpdateSqlCode(TEntity entity, string condition);

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">条件</param>
        ConditionItem Compile(Expression<Func<TEntity, bool>> lambda);

        /// <summary>
        ///     编译查询条件
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        ConditionItem Compile(LambdaItem<TEntity> lambda);

    }
}