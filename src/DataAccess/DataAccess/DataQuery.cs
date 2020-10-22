// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZeroTeam.MessageMVC.ZeroApis;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     Sqlʵ�������
    /// </summary>
    /// <typeparam name="TEntity">ʵ��</typeparam>
    public partial class DataQuery<TEntity> : DataAccessBase<TEntity>
         where TEntity : class, new()
    {

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="provider"></param>
        public DataQuery(DataAccessProvider<TEntity> provider)
            : base(provider)
        {
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��������
        /// </summary>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstAsync()
        {
            return LoadFirstInnerAsync(null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<TEntity> FirstAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadFirstInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public Task<TEntity> FirstAsync(string condition, DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstOrDefaultAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadFirstInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadFirstInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> FirstOrDefaultAsync(string condition, DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }

        #endregion

        #region β��

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastAsync()
        {
            return LoadLastInnerAsync(null);
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadLastInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public Task<TEntity> LastAsync(string condition, DbParameter[] args)
        {
            return LoadLastInnerAsync(condition, args);
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastOrDefaultAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadLastInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadLastInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());//SQL
        }

        /// <summary>
        ///     ����β��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">����</param>
        /// <returns>���������β��,���򷵻ؿ�</returns>
        public Task<TEntity> LastOrDefaultAsync(string condition, DbParameter[] args)
        {
            return LoadLastInnerAsync(condition, args);
        }

        #endregion

        #region All

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public Task<List<TEntity>> AllAsync()
        {
            return LoadDataInnerAsync(null, null, null);
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public Task<List<TEntity>> AllAsync(string condition, params DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, null, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadPageDataAsync(1, -1, null, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> AllAsync<TField>(Expression<Func<TEntity, bool>> lambda, Expression<Func<TEntity, TField>> orderBy,
            bool desc)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadPageDataAsync(1, -1, GetPropertyName(orderBy), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>����</returns>
        public Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return LoadDataInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , null
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public Task<List<TEntity>> AllAsync(LambdaItem<TEntity> lambda, params string[] orderBys)
        {
            var convert = SqlBuilder.Compile(lambda);

            return LoadDataInnerAsync(convert.ConditionSql,
                orderBys.Length == 0 ? null : string.Join(",", orderBys.Select(p=>Option.FieldMap[p])),
                convert.Parameters);
        }

        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <param name="orderBys">����</param>
        /// <returns>����</returns>
        public Task<List<TEntity>> AllAsync(Expression<Func<TEntity, bool>> lambda, params string[] orderBys)
        {
            var convert = SqlBuilder.Compile(lambda);
            return LoadDataInnerAsync(convert.ConditionSql,
                orderBys.Length == 0 ? null : string.Join(",", orderBys.Select(p => Option.FieldMap[p])),
                convert.Parameters);
        }

        #endregion

        #region �ۺϺ���

        #region Collect

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field)
        {
            return CollectInnerAsync(fun, Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync<TValue>(string fun, Expression<Func<TEntity, TValue>> field)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync(fun, Option.FieldMap[expression.Member.Name], null, null);
        }

        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync(fun, Option.FieldMap[field], condition, args);
        }


        /// <summary>
        ///     ���ܷ���
        /// </summary>
        public Task<(bool hase, object value)> CollectAsync(string fun, string field, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return CollectInnerAsync(fun, field, convert.ConditionSql, convert.Parameters);
        }

        #endregion

        #region Exist

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public Task<bool> ExistAsync()
        {
            return ExistInnerAsync();
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        public Task<bool> ExistAsync(string condition, params DbParameter[] args)
        {
            return ExistInnerAsync(condition, args);
        }

        /// <summary>
        ///     �Ƿ���ڴ�����������
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>�Ƿ��������</returns>
        public Task<bool> ExistPrimaryKeyAsync<T>(T id)
        {
            return ExistInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryProperty, id, SqlBuilder.GetDbType(Option.PrimaryProperty)));
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync()
        {
            return await ExistInnerAsync();
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync(string condition, DbParameter[] args)
        {
            return await ExistInnerAsync(condition, args);
        }


        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await ExistInnerAsync(convert.ConditionSql, convert.Parameters);
        }


        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return await ExistInnerAsync($"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
        }
        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition = null, DbParameter args = null)
        {
            return await ExistInnerAsync(condition, args == null ? new DbParameter[0] : new[] { args });
        }

        /// <summary>
        ///     �Ƿ��������
        /// </summary>
        protected async Task<bool> ExistInnerAsync(string condition, DbParameter[] args)
        {
            var (hase, _) = await LoadValueAsync(Option.PrimaryProperty, condition, args);
            return hase;
        }

        #endregion

        #region Count

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<long> CountAsync()
        {
            return await CountInnerAsync();
        }

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<long> CountAsync(string condition, params DbParameter[] args)
        {
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryProperty, condition, args);
            return value;
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryProperty, convert.ConditionSql, convert.Parameters);
            return value;
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public async Task<long> CountAsync(LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryProperty, convert.ConditionSql, convert.Parameters);
            return value;
        }

        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> a, Expression<Func<TEntity, bool>> b)
        {
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryProperty,
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return value;
        }


        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ����ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<long> CountAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition, params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var (_, value) = await CollectInnerAsync<long>("Count", expression.Member.Name, condition, args);
            return value;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected async Task<long> CountInnerAsync(string condition = null, DbParameter args = null)
        {
            var (_, value) = await CollectInnerAsync<long>("Count", Option.PrimaryProperty, condition, args);
            return value;
        }

        #endregion

        #region Min

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, object max)> MinAsyn(string field)
        {
            return CollectInnerAsync("Min", Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(string field)
        {
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[field], null, null);
        }
        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, object max)> MinAsyn(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync("Min", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());

        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ����ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<(bool hase, TValue max)> MinAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync<TValue>("Min", Option.FieldMap[expression.Member.Name], condition, args);
        }

        #endregion

        #region Max

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, object max)> MaxAsyn(string field)
        {
            return CollectInnerAsync("Max", Option.FieldMap[field], null, null);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(string field)
        {
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[field], null, null);
        }
        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, object max)> MaxAsyn(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync("Max", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(string field, string condition, params DbParameter[] args)
        {
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[field], condition, args);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";

            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());

        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ����ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<(bool hase, TValue max)> MaxAsyn<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            return CollectInnerAsync<TValue>("Max", Option.FieldMap[expression.Member.Name], condition, args);
        }

        #endregion


        #region Sum

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<decimal> SumAsync(string field)
        {
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[field], null, null);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     ����
        /// </summary>
        public async Task<decimal> SumAsync(string field, string condition, params DbParameter[] args)
        {
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[field], condition, args);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lambda">��ѯ����ʽ</param>
        /// <param name="condition2">����2��Ĭ��Ϊ��</param>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> lambda,
            string condition2 = null)
        {
            var expression = (MemberExpression)field.Body;
            var convert = SqlBuilder.Compile(lambda);
            var condition = condition2 == null
                ? convert.ConditionSql
                : convert.ConditionSql == null
                    ? condition2
                    : $"({convert.ConditionSql}) AND ({condition2})";
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name], condition, convert.Parameters);
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="a">��ѯ����ʽ</param>
        /// <param name="b"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, Expression<Func<TEntity, bool>> a,
            Expression<Func<TEntity, bool>> b)
        {
            var expression = (MemberExpression)field.Body;
            var convert1 = SqlBuilder.Compile(a);
            var convert2 = SqlBuilder.Compile(b);
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name],
                $"({convert1.ConditionSql}) AND ({convert1.ConditionSql})"
                , convert1.Parameters.Union(convert2.Parameters).ToArray());
            return Convert.ToDecimal(value);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ����ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public async Task<decimal> SumAsync<TValue>(Expression<Func<TEntity, TValue>> field, string condition,
            params DbParameter[] args)
        {
            var expression = (MemberExpression)field.Body;
            var (_, value) = await CollectInnerAsync("Sum", Option.FieldMap[expression.Member.Name], condition, args);
            return Convert.ToDecimal(value);
        }

        #endregion

        #region ʵ��

        /// <summary>
        ///     ����
        /// </summary>
        protected async Task<(bool hase, object value)> CollectInnerAsync(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            return await DataBase.ExecuteScalarAsync(sql, args);
        }

        /// <summary>
        ///     �ϼ�
        /// </summary>
        /// <param name="field"></param>
        /// <param name="condition">��ѯ����ʽ</param>
        /// <param name="args"></param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        protected async Task<(bool hase, TValue value)> CollectInnerAsync<TValue>(string fun, string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateCollectSql(fun, field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var (hase, value) = await DataBase.ExecuteScalarAsync(sql, args);
            return hase ? (true, (TValue)value) : ((bool hase, TValue max))(false, default);
        }

        #endregion

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit)
        {
            return await PageDataAsync(page, limit, null, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await PageDataAsync(page, limit, null, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TEntity>> PageDataAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return await PageDataAsync(page, limit, null, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public async Task<List<TEntity>> LoadDataAsync(int page, int limit, string orderField, bool desc, string condition, params DbParameter[] args)
        {
            return await PageDataAsync(page, limit, orderField, desc, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public Task<List<TEntity>> PageDataAsync(int page, int limit, string orderField, bool desc, string condition, params DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;
            return LoadPageDataAsync(page, limit, orderField, desc, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync(int page, int limit, string orderField, string condition, params DbParameter[] args)
        {
            return PageDataAsync(page, limit, orderField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> PageDataAsync(int page, int limit, string orderField, string condition, params DbParameter[] args)
        {
            return PageDataAsync(page, limit, orderField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField,
            Expression<Func<TEntity, bool>> lambda)
        {
            return PageDataAsync(page, limit, orderField, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageDataAsync(page, limit, GetPropertyName(orderField), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<List<TEntity>> PageDataAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageDataAsync(page, limit, GetPropertyName(orderField), desc, convert.ConditionSql, convert.Parameters);
        }



        protected async Task<List<TEntity>> LoadPageDataAsync(int page, int limit, string orderField, bool desc, string condition, DbParameter[] args)
        {
            var results = new List<TEntity>();
            var sql = SqlBuilder.CreatePageSql(page, limit, orderField, desc, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                //����Commad���󣬷������Ӳ�������
                await using var cmd = connectionScope.CreateCommand(sql, args);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(await LoadEntityAsync(reader));
                }
            }
            foreach (var entity in results)
            {
                await DataOperator.AfterLoad(entity);
            }
            return results;
        }

        #endregion

        #region ��ҳ��ȡ

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit)
        {
            return PageAsync(page, limit, Option.PrimaryDbField, false, null, null);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, Option.PrimaryDbField, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, Option.PrimaryProperty, false, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageAsync(page, limit, Option.PrimaryProperty, false, condition, args);
        }


        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string orderField, string condition, params DbParameter[] args)
        {
            return PageAsync(page, limit, orderField, false, condition, args);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField,
            Expression<Func<TEntity, bool>> lambda)
        {
            return PageAsync(page, limit, orderField, false, lambda);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField, bool desc,
            Expression<Func<TEntity, bool>> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, GetPropertyName(orderField), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public Task<ApiPageData<TEntity>> PageAsync<TField>(int page, int limit, Expression<Func<TEntity, TField>> orderField, bool desc,
            LambdaItem<TEntity> lambda)
        {
            var convert = SqlBuilder.Compile(lambda);
            return PageAsync(page, limit, GetPropertyName(orderField), desc, convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string orderField, bool desc, LambdaItem<TEntity> lambda)
        {
            var item = SqlBuilder.Compile(lambda);
            return LoadPageAsync(page, limit, orderField, desc, item.ConditionSql, item.Parameters);
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        /// <param name="page">ҳ��(��1��ʼ)</param>
        /// <param name="limit">ÿҳ����(С�ڲ���ҳ��</param>
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="desc">�Ƿ���</param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="args">��ѯ����</param>
        /// <returns></returns>
        public Task<ApiPageData<TEntity>> PageAsync(int page, int limit, string orderField, bool desc, string condition,
            params DbParameter[] args)
        {
            return LoadPageAsync(page, limit, orderField, desc, condition, args);
        }


        protected async Task<ApiPageData<TEntity>> LoadPageAsync(int page, int limit, string orderField, bool desc, string condition,
            DbParameter[] args)
        {
            if (page <= 0)
                page = 1;
            if (limit == 0)
                limit = 20;
            else if (limit == 9999)
                limit = -1;
            else if (limit > 500)
                limit = 500;

            var count = (int)await CountAsync(condition, args);
            var data = await PageDataAsync(page, limit, orderField, desc, condition, args);
            return new ApiPageData<TEntity>
            {
                Total = count,
                Rows = data,
                Page = page,
                PageSize = limit,
                PageCount = count / limit + ((count % limit) > 0 ? 1 : 0)
            };
        }

        #endregion

        #region ���ж�ȡ

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public async Task<(bool hase, TField value)> LoadValueAsync<TField>(Expression<Func<TEntity, TField>> field, Expression<Func<TEntity, bool>> lambda)
        {
            var fn = GetPropertyName(field);
            var convert = SqlBuilder.Compile(lambda);
            var (hase, value) = await LoadValueAsync(fn, convert.ConditionSql, convert.Parameters);
            return !hase
                ? (false, default)
                : (true, value == null ? default : (TField)value);
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="field">�ֶ�</param>
        /// <param name="key">����</param>
        /// <returns>����</returns>
        public Task<(bool hase, TField value)> LoadValueAsync<TField, TKey>(Expression<Func<TEntity, TField>> field, TKey key)
        {
            return LoadValueAsync<TField>(GetPropertyName(field),
                SqlBuilder.Condition(Option.PrimaryProperty),
                ParameterCreater.CreateParameter(Option.PrimaryProperty, key));
        }

        /// <summary>
        ///     ��ȡ���ֵ
        /// </summary>
        public async Task<(bool hase, TField value)> LoadValueAsync<TField>(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var (hase, value) = await DataBase.ExecuteScalarAsync(sql, args);
            return !hase
                ? (false, default)
                : (true, value == null ? default : (TField)value);
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="lambda">����</param>
        /// <returns>����</returns>
        public async Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression,
            Expression<Func<TEntity, bool>> lambda)
        {
            var field = GetPropertyName(fieldExpression);
            var convert = SqlBuilder.Compile(lambda);

            var sql = SqlBuilder.CreateLoadValuesSql(field, convert);
            var values = new List<TField>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                await using var cmd = connectionScope.CreateCommand(sql, convert.Parameters);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (!await reader.IsDBNullAsync(0))
                        values.Add(await reader.GetFieldValueAsync<TField>(0));
                }
            }

            return values;
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <param name="fieldExpression">�ֶ�</param>
        /// <param name="condition">����</param>
        /// <returns>����</returns>
        public Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition)
        {
            return LoadValuesInnerAsync<TField>(GetPropertyName(fieldExpression), condition);
        }

        /// <summary>
        ///     ��ȡһ���ֶ�
        /// </summary>
        /// <returns>����</returns>
        public Task<List<TField>> LoadValuesAsync<TField>(Expression<Func<TEntity, TField>> fieldExpression, string condition, DbParameter[] args)
        {
            return LoadValuesInnerAsync<TField>(GetPropertyName(fieldExpression), condition, args);
        }

        /// <summary>
        ///     ��ȡֵ
        /// </summary>
        public async Task<(bool hase, object value)> LoadValueAsync(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            await using var connectionScope = await DataBase.CreateConnectionScope();
            return await DataBase.ExecuteScalarAsync(sql, args);
        }

        /// <summary>
        ///     ��ȡ���ֵ
        /// </summary>
        protected async Task<List<T>> LoadValuesInnerAsync<T>(string field, string condition, params DbParameter[] args)
        {
            var sql = SqlBuilder.CreateLoadValueSql(field, condition);
            var values = new List<T>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(sql, args);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (await reader.IsDBNullAsync(0))
                    values.Add(default);
                else
                    values.Add(reader.GetFieldValue<T>(0));
            }
            return values;
        }
        #endregion

        #region ���ݶ�ȡ

        /*// <summary>
        ///     ��������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns>�������������,���򷵻ؿ�</returns>
        public Task<TEntity> LoadDataAsync(object id)
        {
            return LoadByPrimaryKeyAsync(id);
        }


        /// <summary>
        ///     ȫ����ȡ
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync()
        {
            return LoadDataInnerAsync(null, null, null);
        }


        /// <summary>
        ///     ������ȡ
        /// </summary>
        public Task<List<TEntity>> LoadDataAsync(string condition, params DbParameter[] args)
        {
            return LoadDataInnerAsync(condition, null, args);
        }*/

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public Task<TEntity> LoadByPrimaryKeyAsync(object key)
        {
            return LoadFirstInnerAsync(SqlBuilder.PrimaryKeyCondition, ParameterCreater.CreateParameter(Option.PrimaryProperty, key, SqlBuilder.GetDbType(Option.PrimaryProperty)));
        }

        /// <summary>
        ///     ������ȡ
        /// </summary>
        public async Task<List<TEntity>> LoadByPrimaryKeiesAsync(IEnumerable keies)
        {
            var list = new List<TEntity>();
            var par = ParameterCreater.CreateParameter(Option.PrimaryProperty, SqlBuilder.GetDbType(Option.PrimaryProperty));
            foreach (var key in keies)
            {
                par.Value = key;
                list.Add(await LoadFirstInnerAsync(SqlBuilder.PrimaryKeyCondition, par));
            }

            return list;
        }


        /*// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string condition = null)
        {
            return LoadFirstInnerAsync(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string condition, params DbParameter[] args)
        {
            return LoadFirstInnerAsync(condition, args);
        }

        /// <summary>
        ///     ������ڵĻ���ȡ����
        /// </summary>
        public Task<TEntity> LoadFirstAsync(string foreignKey, object key)
        {
            return LoadFirstInnerAsync(SqlBuilder.FieldConditionSQL(foreignKey),
                ParameterCreater.CreateParameter(foreignKey, key, SqlBuilder.GetDbType(foreignKey)));
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public Task<TEntity> LoadLastAsync(string condition = null)
        {
            return LoadLastAsync(condition);
        }

        /// <summary>
        ///     ������ڵĻ���ȡβ��
        /// </summary>
        public Task<TEntity> LoadLastAsync(string foreignKey, object key)
        {
            return LoadLastAsync(
                SqlBuilder.FieldConditionSQL(foreignKey),
                ParameterCreater.CreateParameter(foreignKey, key, SqlBuilder.GetDbType(foreignKey)));
        }*/

        /// <summary>
        ///     ���
        /// </summary>
        public Task<List<TEntity>> LoadByForeignKeyAsync(string foreignKey, object key)
        {
            return LoadDataInnerAsync(SqlBuilder.Condition(foreignKey), null,
                ParameterCreater.CreateParameter(foreignKey, key));

        }

        /// <summary>
        ///     ���¶�ȡ
        /// </summary>
        public async Task<bool> ReLoadAsync(TEntity entity)
        {
            return await ReLoadInnerAsync(entity);
        }

        /// <summary>
        ///     ��������
        /// </summary>
        protected async Task<bool> ReLoadInnerAsync(TEntity entity)
        {
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var para = ParameterCreater.CreateParameter(Option.PrimaryProperty, Provider.EntityOperator.GetValue(entity, Option.PrimaryProperty), SqlBuilder.GetDbType(Option.PrimaryProperty));
            var sql = SqlBuilder.CreateLoadSql(SqlBuilder.PrimaryKeyCondition, null, null);
            await using var cmd = connectionScope.CreateCommand(sql, para);
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return false;
            await DataOperator.LoadEntity(reader, entity);
            await DataOperator.AfterLoad(entity);

            return true;
        }

        #endregion

        #region ��������


        /// <summary>
        ///     ��ȡ����
        /// </summary>
        protected async Task<TEntity> LoadFirstInnerAsync(string condition, params DbParameter[] args)
        {
            TEntity entity;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, null, "1");
            {
                await using var cmd = connectionScope.CreateCommand(sql, args);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                    return null;
                entity = await LoadEntityAsync(reader);
            }
            await DataOperator.AfterLoad(entity);
            return entity;
        }


        /// <summary>
        ///     ��ȡβ��
        /// </summary>
        protected async Task<TEntity> LoadLastInnerAsync(string condition, params DbParameter[] args)
        {
            TEntity entity;
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, null, "1");
            {
                await using var cmd = connectionScope.CreateCommand(sql, args);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                    return null;
                entity = await LoadEntityAsync(reader);
            }
            await DataOperator.AfterLoad(entity);
            return entity;
        }

        /// <summary>
        ///     ��ȡȫ��
        /// </summary>
        protected async Task<List<TEntity>> LoadDataInnerAsync(string condition, string orderBy, params DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            var sql = SqlBuilder.CreateLoadSql(condition, orderBy, null);
            {
                await using var cmd = connectionScope.CreateCommand(sql, args);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    results.Add(await LoadEntityAsync(reader));
            }
            foreach (var entity in results)
                await DataOperator.AfterLoad(entity);
            return results;
        }

        /// <summary>
        ///     ��ȡȫ��(SQL�������д��,�ֶ����ƺ�˳�������ʱ��ͬ)
        /// </summary>
        public async Task<List<TEntity>> LoadDataBySqlAsync(string sql, DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            {
                await using var cmd = connectionScope.CreateCommand(sql, args);
                DataBase.TraceSql(cmd);
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(await LoadEntityAsync(reader));
                }
            }
            foreach (var entity in results)
                await DataOperator.AfterLoad(entity);
            return results;
        }

        /// <summary>
        ///     ��ȡ�洢����
        /// </summary>
        public async Task<List<TEntity>> LoadDataByProcedureAsync(string procedure, DbParameter[] args)
        {
            var results = new List<TEntity>();
            await using var connectionScope = await DataBase.CreateConnectionScope();
            await using var cmd = connectionScope.CreateCommand(procedure, args);
            cmd.CommandType = CommandType.StoredProcedure;
            DataBase.TraceSql(cmd);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(await LoadEntityAsync(reader));
            }
            return results;
        }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
        /// <returns>��ȡ���ݵ�ʵ��</returns>
        protected async Task<TEntity> LoadEntityAsync(DbDataReader reader)
        {
            var entity = new TEntity();
            await DataOperator.LoadEntity(reader, entity);
            return entity;
        }


        #endregion

        #region ����У��֧��

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val, Expression<Func<TEntity, bool>> condition)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            var convert = SqlBuilder.Compile(condition);

            convert.AddAndCondition(SqlBuilder.Condition(fieldName, "=", "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName)));
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val, object key)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            Debug.Assert(Option.FieldMap.ContainsKey(fieldName));
            return !await ExistAsync($"({SqlBuilder.Condition(fieldName, "=", "c_vl_")} AND {SqlBuilder.Condition(Option.PrimaryProperty, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val)
                , ParameterCreater.CreateParameter(Option.PrimaryProperty, key, SqlBuilder.GetDbType(Option.PrimaryProperty)));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync<TValue>(Expression<Func<TEntity, TValue>> field, TValue val)
        {
            if (Equals(val, default(TValue)))
                return false;
            var fieldName = GetPropertyName(field);
            return !await ExistAsync(SqlBuilder.Condition(fieldName, "=", "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName)));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="condition"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string field, TValue val, Expression<Func<TEntity, bool>> condition)
        {
            var convert = SqlBuilder.Compile(condition);
            convert.AddAndCondition(SqlBuilder.Condition(field, "=", "c_vl_"),
                ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(field)));
            return !await ExistAsync(convert.ConditionSql, convert.Parameters);
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync(string field, string val, object key)
        {
            return !await ExistAsync($"({SqlBuilder.Condition(field, "=", "c_vl_")} AND {SqlBuilder.Condition(Option.PrimaryProperty, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val)
                , ParameterCreater.CreateParameter(Option.PrimaryProperty, key, SqlBuilder.GetDbType(Option.PrimaryProperty)));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public async Task<bool> IsUniqueAsync(string field, string val)
        {
            return !await ExistAsync(SqlBuilder.Condition(field, "=", "c_vl_"), ParameterCreater.CreateParameter("c_vl_", val));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string fieldName, object val, string key)
        {
            return !await ExistAsync($"({SqlBuilder.Condition(fieldName, "=", "c_vl_")} AND {SqlBuilder.Condition(Option.PrimaryProperty, "<>")}"
                , ParameterCreater.CreateParameter("c_vl_", val, SqlBuilder.GetDbType(fieldName))
                , ParameterCreater.CreateParameter(Option.PrimaryProperty, key));
        }

        /// <summary>
        ///     ���ֵ��Ψһ��
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <param name="val"></param>
        /// <param name="key"></param>
        public async Task<bool> IsUniqueAsync<TValue>(string fieldName, string val)
        {
            return !await ExistAsync(SqlBuilder.Condition(fieldName, "=", "c_vl_"), ParameterCreater.CreateParameter("c_vl_", val));
        }
        #endregion
    }
}