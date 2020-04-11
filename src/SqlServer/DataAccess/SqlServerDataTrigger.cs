﻿using ZeroTeam.MessageMVC.Context;

using Agebull.EntityModel.Interfaces;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.SqlServer
{
    /// <summary>
    /// 版本数据更新触发器
    /// </summary>
    public class SqlServerDataTrigger : IDataTrigger
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.SqlServer;

        //void IDataUpdateTrigger.ContitionSqlCode<TEntity>(List<string> conditions)
        //{
        //    if (GlobalContext.Current.IsSystemMode || GlobalContext.Current.User.UserId == LoginUserInfo.SystemUserId)
        //        return;
        //    if (DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIOrganizationData))
        //    {
        //        conditions.Add($"[organization_id] = {GlobalContext.Current.Organizational.OrgId}");
        //    }
        //}

        //void IDataUpdateTrigger.OnPrepareSave(EditDataObject entity, DataOperatorType operatorType)
        //{
        //    if (!GlobalContext.Current.IsSystemMode &&
        //        GlobalContext.Current.User.UserId != LoginUserInfo.SystemUserId &&
        //        entity is IOrganizationData organizationData)
        //    {
        //        organizationData.OrganizationId = GlobalContext.Current.Organizational.OrgId;
        //    }
        //}


        void IDataUpdateTrigger.AfterUpdateSql<TEntity>(IDataTable<TEntity> table, string condition, StringBuilder code)
        {
            if (!DefaultDataUpdateTrigger.IsType<TEntity>(DefaultDataUpdateTrigger.TypeofIHistoryData))
                return;
            var name = GlobalContext.Current.User.NickName?.Replace('\'','’');
            code.Append($@"
UPDATE [{table.ContextWriteTable}]
SET [{table.FieldDictionary[nameof(IHistoryData.LastReviserId)]}] = {GlobalContext.Current.User.UserId},
    [{table.FieldDictionary[nameof(IHistoryData.LastReviser)]}] = '{name}',
    [{table.FieldDictionary[nameof(IHistoryData.LastModifyDate)]}] = GetDate()");
            if (!string.IsNullOrEmpty(condition))
            {
                code.Append($@"
WHERE {condition}");
            }

            code.AppendLine(";");
        }
    }
}
