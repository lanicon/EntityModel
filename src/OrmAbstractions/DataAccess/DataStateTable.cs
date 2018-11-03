// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Agebull.Common.Rpc;
using Gboxt.Common.DataModel;

namespace Agebull.Orm.Abstractions
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    /// <typeparam name="TSqliteDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public abstract class DataStateTable<TData, TSqliteDataBase> : DataTable<TData, TSqliteDataBase>
        where TData : EditDataObject, IStateData, IIdentityData, new()
        where TSqliteDataBase : OrmDataBase
    {
        /// <summary>
        ///     ɾ����SQL���
        /// </summary>
        protected sealed override string DeleteSqlCode => $@"UPDATE [{WriteTableName}] SET [{FieldDictionary["DataState"]}]=255";

        /// <summary>
        ///     ����״̬��SQL���
        /// </summary>
        protected virtual string ResetStateFileSqlCode => $@"[{FieldDictionary["DataState"]}]=0,[{FieldDictionary["IsFreeze"]}]=0";

        /// <summary>
        ///     �õ�����ȷƴ�ӵ�SQL������䣨������û�У�
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        protected override void ContitionSqlCode(List<string> conditions)
        {
            if (GlobalContext.Current.IsManageMode || GlobalContext.Current.IsSystemMode)
                return;
            conditions.Add($"[{FieldDictionary["DataState"]}] < 255");
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(long id)
        {
            //using (SqliteDataBaseScope.CreateScope(DataBase))
            {
                var sql = $@"UPDATE [{WriteTableName}]
SET {ResetStateFileSqlCode} 
WHERE {PrimaryKeyConditionSQL}";
                return OrmDataBase.Execute(sql, CreatePimaryKeyParameter(id)) == 1;
            }
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public virtual bool ResetState(Expression<Func<TData, bool>> lambda)
        {
            //using (SqliteDataBaseScope.CreateScope(DataBase))
            {
                var convert = Compile(lambda);
                var sql = $@"UPDATE [{WriteTableName}]
SET {ResetStateFileSqlCode} 
WHERE {convert.ConditionSql}";
                return OrmDataBase.Execute(sql, convert.Parameters) >= 1;
            }
        }
    }
}